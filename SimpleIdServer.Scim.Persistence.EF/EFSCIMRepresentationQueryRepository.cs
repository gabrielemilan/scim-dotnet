// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using Microsoft.EntityFrameworkCore;
using SimpleIdServer.Scim.Domain;
using SimpleIdServer.Scim.Extensions;
using SimpleIdServer.Scim.Persistence.EF.Extensions;
using SimpleIdServer.Scim.Persistence.EF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdServer.Scim.Persistence.EF
{
    public class EFSCIMRepresentationQueryRepository : ISCIMRepresentationQueryRepository
    {
        private readonly SCIMDbContext _scimDbContext;

        public EFSCIMRepresentationQueryRepository(SCIMDbContext scimDbContext)
        {
            _scimDbContext = scimDbContext;
        }

        public async Task<SCIMRepresentation> FindSCIMRepresentationByAttribute(string schemaAttributeId, string value, string endpoint = null)
        {
            var record = await IncludeRepresentationAttributeNavigationProperties(_scimDbContext.SCIMRepresentationAttributeLst)
                .Where(a => (endpoint == null || endpoint == a.Representation.ResourceType) && a.SchemaAttributeId == schemaAttributeId && a.Values.Any(v => v.ValueString != null && v.ValueString == value))
                .Select(a => a.Representation)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (record == null)
            {
                return null;
            }

            var result = record.ToDomain();
            Detach(record);
            return result;
        }

        public async Task<SCIMRepresentation> FindSCIMRepresentationByAttribute(string schemaAttributeId, int value, string endpoint = null)
        {
            var record = await IncludeRepresentationAttributeNavigationProperties(_scimDbContext.SCIMRepresentationAttributeLst)
                .Where(a => (endpoint == null || endpoint == a.Representation.ResourceType) && a.SchemaAttributeId == schemaAttributeId && a.Values.Any(v => v.ValueInteger != null && v.ValueInteger.Value == value))
                .Select(a => a.Representation)
                .AsNoTracking()
                .FirstOrDefaultAsync(); 
            if (record == null)
            {
                return null;
            }

            var result = record.ToDomain();
            Detach(record);
            return result;
        }

        public async Task<IEnumerable<SCIMRepresentation>> FindSCIMRepresentationByAttributes(string schemaAttributeId, IEnumerable<string> values, string endpoint = null)
        {
            var records = await _scimDbContext.SCIMRepresentationAttributeValueLst
                .Where(a => values.Contains(a.ValueString) && a.RepresentationAttribute.SchemaAttributeId == schemaAttributeId && a.RepresentationAttribute.Representation.ResourceType == endpoint)
                .Select(_ => new
                {
                    RepresentationId = _.RepresentationAttribute.Representation.Id,
                    RepresentationDisplayName = _.RepresentationAttribute.Representation.DisplayName,
                    SchemaAttributeId = _.RepresentationAttribute.SchemaAttributeId,
                    ValueString = _.ValueString
                })
                .AsNoTracking()
                .ToListAsync();
            var result = records.Select(r => r.RepresentationId).Distinct().Count();

            return records.GroupBy(r => r.RepresentationId).Select(grp => new SCIMRepresentation
            {
                Attributes = new List<SCIMRepresentationAttribute>
                {
                    new SCIMRepresentationAttribute
                    {
                        SchemaAttribute = new SCIMSchemaAttribute(grp.First().SchemaAttributeId),
                        ValuesString = grp.Select(v => v.ValueString).ToList()
                    }
                },
                DisplayName = grp.First().RepresentationDisplayName,
                Id = grp.First().RepresentationId
            });
        }

        public async Task<SCIMRepresentation> FindSCIMRepresentationById(string representationId)
        {
            var record = await IncludeRepresentationNavigationProperties(_scimDbContext.SCIMRepresentationLst).FirstOrDefaultAsync(r => r.Id == representationId);
            if (record == null)
            {
                return null;
            }

            var result = record.ToDomain();
            Detach(record);
            return result;
        }

        public async Task<SCIMRepresentation> FindSCIMRepresentationById(string representationId, string resourceType)
        {
            var record = await IncludeRepresentationNavigationProperties(_scimDbContext.SCIMRepresentationLst).FirstOrDefaultAsync(r => r.Id == representationId && r.ResourceType == resourceType);
            if (record == null)
            {
                return null;
            }

            var result = record.ToDomain();
            Detach(record);
            return result;
        }

        public Task<SearchSCIMRepresentationsResponse> FindSCIMRepresentations(SearchSCIMRepresentationsParameter parameter)
        {
            IQueryable<SCIMRepresentationModel> queryableRepresentations = IncludeRepresentationNavigationProperties(_scimDbContext.SCIMRepresentationLst)
                .Where(s => s.ResourceType == parameter.ResourceType)
                .OrderBy(s => s.Id);
            if (parameter.Filter != null)
            {
                var evaluatedExpression = parameter.Filter.Evaluate(queryableRepresentations);
                queryableRepresentations = (IQueryable<SCIMRepresentationModel>)evaluatedExpression.Compile().DynamicInvoke(queryableRepresentations);
            }

            int totalResults = queryableRepresentations.Count();
            IEnumerable<SCIMRepresentation> result = new List<SCIMRepresentation>();
            if (parameter.Count > 0)
            {
                result = queryableRepresentations.Skip(parameter.StartIndex).Take(parameter.Count).ToList().Select(s => s.ToDomain());
            }

            return Task.FromResult(new SearchSCIMRepresentationsResponse(totalResults, result));
        }

        private void Detach(SCIMRepresentationModel representation)
        {
            _scimDbContext.Entry(representation).State = EntityState.Detached;
            var attributes = _scimDbContext.ChangeTracker.Entries().Where(et =>
            {
                var att = et.Entity as SCIMRepresentationAttributeModel;
                if (att == null)
                {
                    return false;
                }

                return att.RepresentationId == representation.Id;
            });
            var ids = attributes.Select(_ => ((SCIMRepresentationAttributeModel)_.Entity).Id).ToList();
            foreach(var attribute in attributes)
            {
                _scimDbContext.Entry(attribute.Entity).State= EntityState.Detached;
            }

            var attributeValues = _scimDbContext.ChangeTracker.Entries().Where(et =>
            {
                var att = et.Entity as SCIMRepresentationAttributeValueModel;
                if (att == null)
                {
                    return false;
                }

                return ids.Contains(att.SCIMRepresentationAttributeId);
            });

            foreach(var attribute in attributeValues)
            {
                _scimDbContext.Entry(attribute.Entity).State = EntityState.Detached;
            }
        }

        private static IQueryable<SCIMRepresentationAttributeModel> IncludeRepresentationAttributeNavigationProperties(IQueryable<SCIMRepresentationAttributeModel> attributes)
        {
            return attributes.Include(a => a.Values)
                .Include(a => a.Representation).ThenInclude(s => s.Attributes).ThenInclude(s => s.SchemaAttribute)
                .Include(a => a.Representation).ThenInclude(s => s.Attributes).ThenInclude(s => s.Values)
                .Include(a => a.Representation).ThenInclude(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.SchemaAttribute)
                .Include(a => a.Representation).ThenInclude(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.Values)
                .Include(a => a.Representation).ThenInclude(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.Children).ThenInclude(s => s.SchemaAttribute)
                .Include(a => a.Representation).ThenInclude(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.Children).ThenInclude(s => s.Values)
                .Include(a => a.Representation).ThenInclude(s => s.Schemas).ThenInclude(s => s.Schema).ThenInclude(s => s.Attributes)
                .Include(a => a.Representation).ThenInclude(s => s.Schemas).ThenInclude(s => s.Schema).ThenInclude(s => s.SchemaExtensions);
        }

        private static IQueryable<SCIMRepresentationModel> IncludeRepresentationNavigationProperties(IQueryable<SCIMRepresentationModel> representations)
        {
            return representations.Include(s => s.Attributes).ThenInclude(s => s.SchemaAttribute)
                .Include(s => s.Attributes).ThenInclude(s => s.Values)
                .Include(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.SchemaAttribute)
                .Include(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.Values)
                .Include(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.Children).ThenInclude(s => s.SchemaAttribute)
                .Include(s => s.Attributes).ThenInclude(s => s.Children).ThenInclude(s => s.Children).ThenInclude(s => s.Values)
                .Include(s => s.Schemas).ThenInclude(s => s.Schema).ThenInclude(s => s.Attributes)
                .Include(s => s.Schemas).ThenInclude(s => s.Schema).ThenInclude(s => s.SchemaExtensions);
        }
    }
}

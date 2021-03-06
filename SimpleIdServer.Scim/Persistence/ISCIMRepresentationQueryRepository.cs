// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using SimpleIdServer.Scim.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdServer.Scim.Persistence
{
    public interface ISCIMRepresentationQueryRepository
    {
        Task<SearchSCIMRepresentationsResponse> FindSCIMRepresentations(SearchSCIMRepresentationsParameter parameter);
        Task<SCIMRepresentation> FindSCIMRepresentationById(string representationId);
        Task<SCIMRepresentation> FindSCIMRepresentationById(string representationId, string resourceType);
        Task<SCIMRepresentation> FindSCIMRepresentationByAttribute(string attributeId, string value, string endpoint = null);
        Task<SCIMRepresentation> FindSCIMRepresentationByAttribute(string attributeId, int value, string endpoint = null);
        Task<IEnumerable<SCIMRepresentation>> FindSCIMRepresentationByAttributes(string attributeId, IEnumerable<string> values, string endpoint = null);
    }
}
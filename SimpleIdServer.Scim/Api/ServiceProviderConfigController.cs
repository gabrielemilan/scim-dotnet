﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleIdServer.Scim.Builder;
using SimpleIdServer.Scim.Domain;
using SimpleIdServer.Scim.Extensions;
using System.Collections.Generic;
using System.Net;

namespace SimpleIdServer.Scim.Api
{
    [Route(SCIMConstants.SCIMEndpoints.ServiceProviderConfig)]
    public class ServiceProviderConfigController : Controller
    {
        private readonly SCIMHostOptions _options;

        public ServiceProviderConfigController(IOptionsMonitor<SCIMHostOptions> options)
        {
            _options = options.CurrentValue;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var schema = SCIMConstants.StandardSchemas.ServiceProvideConfigSchemas;
            var representation = SCIMRepresentationBuilder.Create(new List<SCIMSchema> { schema })
                .AddComplexAttribute("patch", schema.Id, c =>
                {
                    c.AddBooleanAttribute("supported", new List<bool> { true });
                })
                .AddComplexAttribute("bulk", schema.Id, c =>
                 {
                     c.AddBooleanAttribute("supported", new List<bool> { true });
                     c.AddIntegerAttribute("maxOperations", new List<int> { _options.MaxOperations });
                     c.AddIntegerAttribute("maxPayloadSize", new List<int> { _options.MaxPayloadSize });
                 })
                .AddComplexAttribute("filter", schema.Id, c =>
                 {
                     c.AddBooleanAttribute("supported", new List<bool> { true });
                     c.AddIntegerAttribute("maxResults", new List<int> { _options.MaxResults });
                 })
                .AddComplexAttribute("changePassword", schema.Id, c =>
                 {
                     c.AddBooleanAttribute("supported", new List<bool> { false });
                 })
                .AddComplexAttribute("sort", schema.Id, c =>
                {
                    c.AddBooleanAttribute("supported", new List<bool> { false });
                })
                .AddComplexAttribute("etag", schema.Id, c =>
                {
                    c.AddBooleanAttribute("supported", new List<bool> { false });
                })
                .AddComplexAttribute("authenticationSchemes", schema.Id, c =>
                {
                    c.AddStringAttribute("name", new List<string> { "OAuth Bearer Token" });
                    c.AddStringAttribute("description", new List<string> { "Authentication scheme using the OAuth Bearer Token Standard" });
                    c.AddStringAttribute("specUri", new List<string> { "http://www.rfc-editor.org/info/rfc6750" });
                    c.AddStringAttribute("type", new List<string> { "oauthbearertoken" });
                    c.AddBooleanAttribute("primary", new List<bool> { true });
                }).Build();
            var location = $"{Request.GetAbsoluteUriWithVirtualPath()}/{SCIMConstants.SCIMEndpoints.ServiceProviderConfig}";
            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = representation.ToResponse(location, true).ToString(),
                ContentType = "application/scim+json"
            };
        }
    }
}

// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using SimpleIdServer.Scim.Serialization;

namespace SimpleIdServer.Scim.DTOs
{
    [SCIMSchema("urn:ietf:params:scim:api:messages:2.0:Error")]
    public class SCIMErrorRepresentation
    {
        public SCIMErrorRepresentation(string status, string detail, string scimType)
        {
            Status = status;
            Detail = detail;
            ScimType = scimType;
        }

        [SCIMSchemaProperty("status")]
        public string Status { get; set; }
        [SCIMSchemaProperty("scimType")]
        public string ScimType { get; set; }
        [SCIMSchemaProperty("detail")]
        public string Detail { get; set; }
    }
}
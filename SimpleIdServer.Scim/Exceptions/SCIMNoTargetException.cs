// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System;

namespace SimpleIdServer.Scim.Exceptions
{
    public class SCIMNoTargetException : Exception
    {
        public SCIMNoTargetException(string message) : base(message)
        {

        }
    }
}

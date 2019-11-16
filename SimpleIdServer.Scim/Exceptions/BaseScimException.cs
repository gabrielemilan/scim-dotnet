﻿// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System;

namespace SimpleIdServer.Scim.Exceptions
{
    public class BaseScimException : Exception
    {
        public BaseScimException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string Code { get; }
    }
}

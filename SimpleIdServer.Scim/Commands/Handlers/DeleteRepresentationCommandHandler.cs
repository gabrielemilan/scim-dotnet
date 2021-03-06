// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using SimpleIdServer.Scim.Exceptions;
using SimpleIdServer.Scim.Persistence;
using SimpleIdServer.Scim.Resources;
using System.Threading.Tasks;

namespace SimpleIdServer.Scim.Commands.Handlers
{
    public class DeleteRepresentationCommandHandler : IDeleteRepresentationCommandHandler
    {
        private readonly ISCIMRepresentationCommandRepository _scimRepresentationCommandRepository;
        private readonly ISCIMRepresentationQueryRepository _scimRepresentationQueryRepository;

        public DeleteRepresentationCommandHandler(ISCIMRepresentationCommandRepository scimRepresentationCommandRepository,
            ISCIMRepresentationQueryRepository scimRepresentationQueryRepository)
        {
            _scimRepresentationCommandRepository = scimRepresentationCommandRepository;
            _scimRepresentationQueryRepository = scimRepresentationQueryRepository;
        }

        public async Task<bool> Handle(DeleteRepresentationCommand request)
        {
            var representation = await _scimRepresentationQueryRepository.FindSCIMRepresentationById(request.Id, request.ResourceType);
            if (representation == null)
            {
                throw new SCIMNotFoundException(string.Format(Global.ResourceNotFound, request.Id));
            }

            using (var transaction = await _scimRepresentationCommandRepository.StartTransaction())
            {
                await _scimRepresentationCommandRepository.Delete(representation);
                await transaction.Commit();
            }

            return true;
        }
    }
}
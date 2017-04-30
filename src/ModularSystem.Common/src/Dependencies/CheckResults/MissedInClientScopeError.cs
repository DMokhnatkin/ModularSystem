﻿using ModularSystem.Common.Modules;

namespace ModularSystem.Common.Dependencies
{
    public class MissedInClientScopeError : MissedModuleError
    {
        /// <inheritdoc />
        public MissedInClientScopeError(IModule sourceModule, ModuleIdentity requiredModule, string userId, string clientId) : base(sourceModule, requiredModule, ModuleType.Client)
        {
            UserId = userId;
            ClientId = clientId;
        }

        public string UserId { get; }

        public string ClientId { get; }

        /// <inheritdoc />
        public override string GetMessage()
        {
            return $"Module {RequiredModule} was required by {SourceModule.ModuleIdentity} on {RequiredModuleType} side but was missed in client scope (UserId = {UserId} ClientId = {ClientId})";
        }
    }
}
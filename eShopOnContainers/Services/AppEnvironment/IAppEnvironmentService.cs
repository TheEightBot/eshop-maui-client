using System;
namespace eShopOnContainers.Services.AppEnvironment
{
    public interface IAppEnvironmentService
    {
        bool UseMockService { get; }

        void UpdateDependencies(bool useMockServices);
}
}


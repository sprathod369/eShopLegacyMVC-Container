using Autofac; 
using eShopLegacyMVC_Core.Models; 
using eShopLegacyMVC_Core.Models.Infrastructure; 
using eShopLegacyMVC_Core.Services; 
using Microsoft.Extensions.Logging; 
using Microsoft.Extensions.Configuration; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity; 
namespace eShopLegacyMVC_Core.Modules 
{ 
    public class ApplicationModule : Module 
    { 
        private readonly bool _useMockData; 
        private readonly bool _useCustomizationData; 
        private readonly ILoggerFactory _loggerFactory; 
        private readonly string _connectionString; 
        public ApplicationModule(IConfiguration configuration, ILoggerFactory loggerFactory) 
        { 
            _useMockData = configuration.GetValue<bool>("UseMockData"); 
            _useCustomizationData = configuration.GetValue<bool>("UseCustomizationData"); 
            _connectionString = configuration.GetConnectionString("CatalogDBContext"); 
            _loggerFactory = loggerFactory; 
        } 
        protected override void Load(ContainerBuilder builder) 
        { 
            if (_useMockData) 
            { 
                builder.RegisterType<CatalogServiceMock>() 
                    .As<ICatalogService>() 
                    .SingleInstance(); 
            } 
            else 
            { 
                builder.RegisterType<CatalogService>() 
                    .As<ICatalogService>() 
                    .InstancePerLifetimeScope(); 
            } 
            builder.RegisterType<CatalogDBContext>() 
                .WithParameter("options", new DbContextOptionsBuilder<CatalogDBContext>() 
                    .UseSqlServer(_connectionString) 
                    .Options) 
                .WithParameter("logger", _loggerFactory.CreateLogger<CatalogDBContext>()) 
                .InstancePerLifetimeScope(); 
            builder.RegisterType<CatalogDBInitializer>() 
                .WithParameter("useCustomizationData", _useCustomizationData) 
                .WithParameter("logger", _loggerFactory.CreateLogger<CatalogDBInitializer>()) 
                .InstancePerLifetimeScope(); 
            builder.RegisterType<CatalogItemHiLoGenerator>() 
                .WithParameter("logger", _loggerFactory.CreateLogger<CatalogItemHiLoGenerator>()) 
                .SingleInstance(); 
            // Register Identity services 
            builder.RegisterType<UserManager<IdentityUser>>() 
                .AsSelf() 
                .InstancePerLifetimeScope(); 
            builder.RegisterType<SignInManager<IdentityUser>>() 
                .AsSelf() 
                .InstancePerLifetimeScope(); 
        } 
    } 
}
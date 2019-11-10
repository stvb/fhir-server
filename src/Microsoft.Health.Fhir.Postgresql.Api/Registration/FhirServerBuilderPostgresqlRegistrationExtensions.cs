// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Health.Extensions.DependencyInjection;
using Microsoft.Health.Fhir.Core.Registration;
using Microsoft.Health.Fhir.Postgresql.Configs;
using Microsoft.Health.Fhir.Postgresql.Features.Health;
using Microsoft.Health.Fhir.Postgresql.Features.Schema;
using Microsoft.Health.Fhir.Postgresql.Features.Search;
using Microsoft.Health.Fhir.Postgresql.Features.Storage;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FhirServerBuilderPostgresqlRegistrationExtensions
    {
        public static IFhirServerBuilder AddExperimentalPostgresql(this IFhirServerBuilder fhirServerBuilder, Action<PostgresqlDataStoreConfiguration> configureAction = null)
        {
            EnsureArg.IsNotNull(fhirServerBuilder, nameof(fhirServerBuilder));
            IServiceCollection services = fhirServerBuilder.Services;

            services.Add(provider =>
                {
                    var config = new PostgresqlDataStoreConfiguration();
                    provider.GetService<IConfiguration>().GetSection("Postgresql").Bind(config);
                    configureAction?.Invoke(config);
                    return config;
                })
                .Singleton()
                .AsSelf();

            services.Add<PostgresqlFhirDatastoreContext>()
                .Transient()
                .AsSelf();

            services.Add<SchemaInitializer>()
                .Singleton()
                .AsService<IStartable>();

            services.Add<PostgresqlFhirDataStore>()
                .Scoped()
                .AsSelf()
                .AsImplementedInterfaces();

            services.Add<PostgresqlFhirOperationDataStore>()
                .Scoped()
                .AsSelf()
                .AsImplementedInterfaces();

            services.Add<PostgresqlSearchService>()
                .Scoped()
                .AsSelf()
                .AsImplementedInterfaces();

            services
                .AddHealthChecks()
                .AddCheck<PostgresqlHealthCheck>(nameof(PostgresqlHealthCheck));

            // This is only needed while adding in the ConfigureServices call in the E2E TestServer scenario
            // During normal usage, the controller should be automatically discovered.
            // services.AddMvc().AddApplicationPart(typeof(SchemaController).Assembly);

            return fhirServerBuilder;
        }
    }
}

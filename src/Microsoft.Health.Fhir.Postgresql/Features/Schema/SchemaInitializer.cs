// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Health.Extensions.DependencyInjection;
using Microsoft.Health.Fhir.Postgresql.Configs;
using Microsoft.Health.Fhir.Postgresql.Features.Schema.Entities;
using Microsoft.Health.Fhir.Postgresql.Features.Storage;

namespace Microsoft.Health.Fhir.Postgresql.Features.Schema
{
    public class SchemaInitializer : IStartable
    {
        private ILogger<SchemaInitializer> _logger;
        private PostgresqlDataStoreConfiguration _configuration;

        public SchemaInitializer(PostgresqlDataStoreConfiguration configuration, ILogger<SchemaInitializer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Start()
        {
            using (var context = new PostgresqlFhirDatastoreContext(_configuration))
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
                var tmp = context.Database.CanConnect();
                context.Resources.Add(new PgResource());
                context.SaveChanges();
            }
        }
    }
}

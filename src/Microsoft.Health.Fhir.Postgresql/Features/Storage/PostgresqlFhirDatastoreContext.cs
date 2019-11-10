// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Health.Fhir.Postgresql.Configs;
using Microsoft.Health.Fhir.Postgresql.Features.Schema.Entities;

namespace Microsoft.Health.Fhir.Postgresql.Features.Storage
{
    public class PostgresqlFhirDatastoreContext : DbContext
    {
        private PostgresqlDataStoreConfiguration _configuration;

        public PostgresqlFhirDatastoreContext(PostgresqlDataStoreConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PostgresqlFhirDatastoreContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<PgResource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(
                    _configuration.ConnectionString,
                    x => x.MigrationsAssembly("Microsoft.Health.Fhir.Postgresql"));
            }
        }
    }
}

// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Health.Fhir.Postgresql.Configs;
using Microsoft.Health.Fhir.Postgresql.Features.Storage;

namespace Microsoft.Health.Fhir.Postgresql.Features.Schema
{
    public class PostgresqlDbContextFactory : IDesignTimeDbContextFactory<PostgresqlFhirDatastoreContext>
    {
        public PostgresqlFhirDatastoreContext CreateDbContext(string[] args)
        {
            var conf = new PostgresqlDataStoreConfiguration();
            conf.ConnectionString = Environment.GetEnvironmentVariable("Postgresql:ConnectionString");
            return new PostgresqlFhirDatastoreContext(conf);
        }
    }
}

// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Health.Fhir.Postgresql.Features.Storage;

namespace Microsoft.Health.Fhir.Postgresql.Features.Health
{
    public class PostgresqlHealthCheck : IHealthCheck
    {
        private readonly ILogger<PostgresqlHealthCheck> _logger;
        private PostgresqlFhirDatastoreContext _context;

        public PostgresqlHealthCheck(PostgresqlFhirDatastoreContext context, ILogger<PostgresqlHealthCheck> logger)
        {
            EnsureArg.IsNotNull(context, nameof(context));
            EnsureArg.IsNotNull(logger, nameof(logger));

            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            try
            {
                var validQuery = await _context.Resources
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
                return HealthCheckResult.Healthy("Successfully connected to the data store.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to connect to the data store.");
                return HealthCheckResult.Unhealthy("Failed to connect to the data store.");
            }
        }
    }
}

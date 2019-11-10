// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Hl7.FhirPath.Sprache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Health.Fhir.Core.Features.Conformance;
using Microsoft.Health.Fhir.Core.Features.Persistence;
using Microsoft.Health.Fhir.Core.Models;
using Microsoft.Health.Fhir.Postgresql.Configs;
using Microsoft.Health.Fhir.Postgresql.Features.Schema;
using Microsoft.Health.Fhir.Postgresql.Features.Schema.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using Polly;
using Remotion.Linq.Parsing;
using static System.TimeSpan;

namespace Microsoft.Health.Fhir.Postgresql.Features.Storage
{
    public class PostgresqlFhirDataStore : IFhirDataStore, IProvideCapability
    {
        private PostgresqlDataStoreConfiguration _configuration;
        private ILogger<SchemaInitializer> _logger;

        public PostgresqlFhirDataStore(
            PostgresqlDataStoreConfiguration configuration,
            ILogger<SchemaInitializer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public ITransactionScope BeginTransaction()
        {
            throw new System.NotImplementedException();
        }

        public Task<UpsertOutcome> UpsertAsync(
            ResourceWrapper resource,
            WeakETag weakETag,
            bool allowCreate,
            bool keepHistory,
            CancellationToken cancellationToken)
        {
            var retryPolicy = Policy
                .Handle<NpgsqlException>()
                .WaitAndRetryAsync(1, i => FromSeconds(1));
            return retryPolicy.ExecuteAsync(async () =>
            {
                using (var context = new PostgresqlFhirDatastoreContext(_configuration))
                {
                    // check if there is an existing resource
                    bool existing = await context.Resources.AnyAsync(x => x.ResourceId == resource.ResourceId, cancellationToken: cancellationToken);
                    var version = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

                    var newEntry = new PgResource
                    {
                        IsDeleted = resource.IsDeleted,
                        Resource = resource.RawResource.Data,
                        ResourceId = resource.ResourceId,
                        ResourceType = resource.ResourceTypeName,
                        LastModified = resource.LastModified.UtcDateTime,
                        LastModifiedClaims = resource.LastModifiedClaims.Select(x => $"{x.Key}:{x.Value}").ToList(),
                        Version = version,
                    };
                    context.Add(newEntry);
                    await context.SaveChangesAsync(cancellationToken);
                    resource.Version = version.ToString(CultureInfo.InvariantCulture);
                    return new UpsertOutcome(
                            wrapper: resource,
                            outcomeType: existing ? SaveOutcomeType.Updated : SaveOutcomeType.Created);
                }
            });
        }

        public Task<ResourceWrapper> GetAsync(ResourceKey key, CancellationToken cancellationToken)
        {
            using (var context = new PostgresqlFhirDatastoreContext(_configuration))
            {
                int? requestedVersion = null;
                if (!string.IsNullOrEmpty(key.VersionId))
                {
                    if (!int.TryParse(key.VersionId, out var parsedVersion))
                    {
                        return null;
                    }

                    requestedVersion = parsedVersion;
                }

                var q = context.Resources
                    .Where(x => x.ResourceType == key.ResourceType)
                    .Where(x => x.ResourceId == key.Id)
                    .AsQueryable();
                if (requestedVersion.HasValue)
                {
                    q = q.Where(x => x.Version == requestedVersion.Value);
                }

                var result = q.FirstOrDefaultAsync(cancellationToken);

                return Task.FromResult(new ResourceWrapper(
                    resourceId: key.Id,
                    versionId: result.Result.Version.ToString(CultureInfo.InvariantCulture),
                    resourceTypeName: key.ResourceType,
                    rawResource: new RawResource(result.Result.Resource, FhirResourceFormat.Json),
                    request: null,
                    lastModified: result.Result.LastModified,
                    deleted: result.Result.IsDeleted,
                    searchIndices: null,
                    compartmentIndices: null,
                    lastModifiedClaims: null));
            }
        }

        public Task HardDeleteAsync(ResourceKey key, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Build(ICapabilityStatementBuilder builder)
        {
            EnsureArg.IsNotNull(builder, nameof(builder));
            builder.AddDefaultResourceInteractions()
                .AddDefaultSearchParameters();

            // if (_coreFeatures.SupportsBatch)
            //            {
            //                // Batch supported added in listedCapability
            //                builder.AddRestInteraction(SystemRestfulInteraction.Batch);
            //            }
            //
            //            if (_coreFeatures.SupportsTransaction)
            //            {
            //                // Transaction supported added in listedCapability
            //                builder.AddRestInteraction(SystemRestfulInteraction.Transaction);
            //            }
        }
    }
}

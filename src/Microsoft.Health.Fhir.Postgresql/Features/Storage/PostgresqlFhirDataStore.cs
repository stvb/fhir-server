// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.Health.Fhir.Core.Features.Conformance;
using Microsoft.Health.Fhir.Core.Features.Persistence;

namespace Microsoft.Health.Fhir.Postgresql.Features.Storage
{
    public class PostgresqlFhirDataStore : IFhirDataStore, IProvideCapability
    {
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
            int etag = 0;
            if (weakETag != null && !int.TryParse(weakETag.VersionId, out etag))
            {
                // Set the etag to a sentinel value to enable expected failure paths when updating with both existing and nonexistent resources.
                etag = -1;
            }

            throw new System.NotImplementedException();
        }

        public Task<ResourceWrapper> GetAsync(ResourceKey key, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
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

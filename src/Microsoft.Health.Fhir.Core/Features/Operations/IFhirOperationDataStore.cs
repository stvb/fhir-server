﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Health.Fhir.Core.Features.Operations.Export.Models;
using Microsoft.Health.Fhir.Core.Features.Operations.Import.Models;
using Microsoft.Health.Fhir.Core.Features.Persistence;

namespace Microsoft.Health.Fhir.Core.Features.Operations
{
    public interface IFhirOperationDataStore
    {
        Task<ImportJobOutcome> CreateImportJobAsync(ImportJobRecord jobRecord, CancellationToken cancellationToken);

        Task<ImportJobOutcome> GetImportJobAsync(string jobId, CancellationToken cancellationToken);

        Task<ImportJobOutcome> UpdateImportJobAsync(ImportJobRecord jobRecord, WeakETag eTag, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<ImportJobOutcome>> AcquireImportJobsAsync(ushort maximumNumberOfConcurrentJobsAllowed, TimeSpan jobHeartbeatTimeoutThreshold, CancellationToken cancellationToken);

        Task<ExportJobOutcome> CreateExportJobAsync(ExportJobRecord jobRecord, CancellationToken cancellationToken);

        Task<ExportJobOutcome> GetExportJobAsync(string jobId, CancellationToken cancellationToken);

        Task<ExportJobOutcome> UpdateExportJobAsync(ExportJobRecord jobRecord, WeakETag eTag, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<ExportJobOutcome>> AcquireExportJobsAsync(ushort maximumNumberOfConcurrentJobsAllowed, TimeSpan jobHeartbeatTimeoutThreshold, CancellationToken cancellationToken);
    }
}

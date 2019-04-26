﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using MediatR;
using Microsoft.Health.Fhir.Core.Features.Operations.Import.Models;
using Microsoft.Health.Fhir.Core.Messages.Import;

namespace Microsoft.Health.Fhir.Core.Features.Operations.Import
{
    public class GetImportRequestHandler : IRequestHandler<GetImportRequest, GetImportResponse>
    {
        private IFhirOperationDataStore _fhirOperationDataStore;

        public GetImportRequestHandler(IFhirOperationDataStore fhirOperationDataStore)
        {
            EnsureArg.IsNotNull(fhirOperationDataStore, nameof(fhirOperationDataStore));

            _fhirOperationDataStore = fhirOperationDataStore;
        }

        public async Task<GetImportResponse> Handle(GetImportRequest request, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            ImportJobOutcome outcome = await _fhirOperationDataStore.GetImportJobAsync(request.JobId, cancellationToken);

            // We have an existing job. We will determine the response based on the status of the export operation.
            GetImportResponse exportResponse;
            if (outcome.JobRecord.Status == OperationStatus.Completed)
            {
                var jobResult = new ImportJobResult(
                    outcome.JobRecord.QueuedTime,
                    outcome.JobRecord.RequestUri,
                    requiresAccessToken: false,
                    null,
                    null);

                exportResponse = new GetImportResponse(HttpStatusCode.OK, jobResult);
            }
            else
            {
                exportResponse = new GetImportResponse(HttpStatusCode.Accepted);
            }

            return exportResponse;
        }
    }
}

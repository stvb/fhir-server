// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

namespace Microsoft.Health.Fhir.Core.Features.Search
{
    /// <summary>
    /// Indicates if the number of matching search results should be accurately calculated, estimated, or excluded.
    /// </summary>
    public enum CountType
    {
        // There is no need to populate the total count; the client will not use it.
        None = 0,

        // The client requests that the server provide an exact total of the number of matching resources.
        Accurate = 1,

        // A rough estimate of the number of matching resources is sufficient.
        Estimate = 2,
    }
}

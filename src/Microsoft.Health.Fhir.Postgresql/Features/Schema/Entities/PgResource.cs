// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Health.Fhir.Postgresql.Features.Schema.Entities
{
    public class PgResource
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "jsonb")]
        public string Resource { get; set; }

        [Timestamp]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1819:Properties should not return arrays", Justification = "Default EF behavior.")]
        public byte[] Timestamp { get; set; }
    }
}

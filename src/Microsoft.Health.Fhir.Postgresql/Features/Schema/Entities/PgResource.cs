// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Health.Fhir.Postgresql.Features.Schema.Entities
{
    public class PgResource
    {
        public PgResource()
        {
            LastModifiedClaims = new List<string>();
        }

        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "jsonb")]
        public string Resource { get; set; }

        public string ResourceId { get; set; }

        public bool IsDeleted { get; set; }

        public string ResourceType { get; set; }

        public DateTime LastModified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA2227", Justification = "No need for private setter.")]
        public List<string> LastModifiedClaims { get; set; }

        public double Version { get; set; }
    }
}

/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using System.Collections.Generic;
using FluentValidation.Results;

namespace Simusharp.FomGen.Core.Models
{
    public interface IFomModule
    {
        string Name { get; set; }

        ModelIdentificationSection ModelIdentificationSection { get; set; }

        ObjectClassSection ObjectClassSection { get; set; }

        InteractionClassSection InteractionClassSection { get; set; }

        DimensionSection DimensionSection { get; set; }

        TransportationSection TransportationSection { get; set; }

        DataTypeSection DataTypeSection { get; set; }

        SynchronizationSection SynchronizationSection { get; set; }

        TagSection TagSection { get; set; }

        TimeSection TimeSection { get; set; }

        UpdateRatesSection UpdateRatesSection { get; set; }

        SwitchSection SwitchSection { get; set; }

        IList<ValidationFailure> Validate();
    }
}



using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NanoSurvey.Webapi.Models
{
    public class ValidationError : Dictionary<string, IEnumerable<string>>
    {
        public ValidationError(ModelStateDictionary states)
        {
            foreach (var state in states)
            {
                if (state.Value.ValidationState == ModelValidationState.Invalid)
                {
                    this.Add(state.Key, state.Value.Errors.Select(e => e.ErrorMessage));
                }
            }
        }
    }
}
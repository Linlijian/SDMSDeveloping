using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC011P001Validator))]
    [Serializable]
    public class SEC011P001Model : StandardModel
    {

    }
    
    public class SEC011P001Validator : AbstractValidator<SEC011P001Model>
    {
        public SEC011P001Validator()
        {
            RuleSet("Add", () =>
            {
                Valid();
            });

            RuleSet("Edit", () =>
            {
                Valid();
            });
        }

        private void Valid()
        {
           
        }
    }
}
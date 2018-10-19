using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC014P001Validator))]
    [Serializable]
    public class SEC014P001Model : StandardModel
    {
        [Display(Name = "SQL_COMMAND", ResourceType = typeof(Translation.SEC.SEC014P001))]
        public string SQL_COMMAND { get; set; }

        public string DYNAMIC_COL { get; set; }

        public int? RECORD_COUNT { get; set; }
    }

    public class SEC014P001Validator : AbstractValidator<SEC014P001Model>
    {
        public SEC014P001Validator()
        {
            RuleSet("Index", () =>
            {
                RuleFor(m => m.SQL_COMMAND).NotEmpty();
            });
        }

        private void Valid()
        {

        }
    }
}
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC002P001Validator))]
    [Serializable]
    public class SEC002P001Model : StandardModel
    {
        public decimal? TITLE_ID { get; set; }
        [Display(Name = "TITLE_NAME_TH", ResourceType = typeof(Translation.SEC.SEC002P001))]
        public string TITLE_NAME_TH { get; set; }
        [Display(Name = "TITLE_NAME_EN", ResourceType = typeof(Translation.SEC.SEC002P001))]
        public string TITLE_NAME_EN { get; set; }
        [Display(Name = "TITLE_NAME", ResourceType = typeof(Translation.SEC.SEC002P001))]
        public string TITLE_NAME { get; set; }
    }

    public class SEC002P001Validator : AbstractValidator<SEC002P001Model>
    {
        public SEC002P001Validator()
        {
            //If Not Empty Set Start 1,...
            RuleSet("Add", () =>
            {
                RuleFor(m => m.TITLE_NAME_TH).NotEmpty().Store("CD_TITLE_001");
            });

            RuleSet("Edit", () =>
            {
                RuleFor(m => m.TITLE_NAME_TH).NotEmpty().Store("CD_TITLE_002", m => m.TITLE_ID);
            });
        }
    }
}
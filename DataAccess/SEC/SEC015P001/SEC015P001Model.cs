using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC015P001Validator))]
    [Serializable]
    public class SEC015P001Model : StandardModel
    {
        public decimal ID { get; set; }
        [Display(Name = "NAME", ResourceType = typeof(Translation.SEC.SEC015P001))]
        public string NAME { get; set; }
        public string NAME_Old { get; set; }
        [Display(Name = "SEQUENCE", ResourceType = typeof(Translation.SEC.SEC015P001))]
        public Nullable<int> SEQUENCE { get; set; }
        public string SYS_CODE { get; set; }
        public string STR_VALUE { get; set; }
        public Nullable<int> INT_VALUE { get; set; }
        public Nullable<decimal> DEC_VALUE { get; set; }
        public string IMG { get; set; }
        public string IMG_COLOR { get; set; }

        public List<SEC015P001_SystemModel> SystemModels { get; set; }
    }
    public class SEC015P001_SystemModel : StandardModel
    {
        public string SYS_CODE { get; set; }
        public int? ROW_NO { get; set; }
        public Nullable<decimal> SYS_SEQ { get; set; }
        public string SYS_NAME_TH { get; set; }
        public string SYS_NAME_EN { get; set; }
        public string SYS_STATUS { get; set; }
        public bool SELECTED { get; set; }
    }

    public class SEC015P001Validator : AbstractValidator<SEC015P001Model>
    {
        public SEC015P001Validator()
        {
            //If Not Empty Set Start 1,...
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
            RuleFor(m => m.NAME).NotEmpty();
        }
    }
}
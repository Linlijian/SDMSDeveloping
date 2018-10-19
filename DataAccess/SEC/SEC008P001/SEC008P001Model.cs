using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC008P001Validator))]
    [Serializable]
    public class SEC008P001Model : StandardModel
    {

        public string COM_NAME { get; set; }
        public string COM_NAME_TH { get; set; }
        public string COM_NAME_EN { get; set; }
        [Display(Name = "PRG_CODE", ResourceType = typeof(Translation.SEC.SEC008P001))]
        public string PRG_CODE { get; set; }
        public string PRG_NAME { get; set; }
        [Display(Name = "PRG_NAME_TH", ResourceType = typeof(Translation.SEC.SEC008P001))]
        public string PRG_NAME_TH { get; set; }
        [Display(Name = "PRG_NAME_EN", ResourceType = typeof(Translation.SEC.SEC008P001))]
        public string PRG_NAME_EN { get; set; }
        public Nullable<int> PRG_SEQ { get; set; }
        [Display(Name = "SYS_NAME", ResourceType = typeof(Translation.SEC.SEC008P001))]
        public string SYS_CODE { get; set; }
        public string SYS_NAME_TH { get; set; }
        public string SYS_NAME_EN { get; set; }
        public IEnumerable<DDLCenterModel> SYS_CODE_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> COM_CODE_MODEL { get; set; }
        public IEnumerable<int> _DT_RowIndex { get; set; }

    }

    public class SEC008P001Validator : AbstractValidator<SEC008P001Model>
    {
        public SEC008P001Validator()
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
            RuleFor(m => m.PRG_CODE).NotEmpty();
        }
    }
}

using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC001P001Validator))]
    [Serializable]
    public class SEC001P001Model : StandardModel
    {
        [Display(Name = "COM_CODE", ResourceType = typeof(Translation.SEC.SEC001P001))]
        public string COM_CODE { get; set; }

        [Display(Name = "TBL_NAME", ResourceType = typeof(Translation.SEC.SEC001P001))]
        public string TBL_NAME { get; set; }

        [Display(Name = "TBL_TYPE", ResourceType = typeof(Translation.SEC.SEC001P001))]
        public string TBL_TYPE { get; set; }

        [Display(Name = "LOG_STATUS", ResourceType = typeof(Translation.SEC.SEC001P001))]
        public string LOG_STATUS { get; set; }
     
        public string CRET_BY { get; set; }
        public Nullable<System.DateTime> CRET_DATE { get; set; }
        public string MNT_BY { get; set; }
        public Nullable<System.DateTime> MNT_DATE { get; set; }


        //------DDL----//
        public IEnumerable<DDLCenterModel> LOG_STATUS_MODEL { get; set; }

    }

    public class SEC001P001Validator : AbstractValidator<SEC001P001Model>
    {
        public SEC001P001Validator()
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
            RuleFor(m => m.TBL_NAME).NotEmpty();
           // RuleFor(m => m.LOG_STATUS).NotEmpty();
        }
    }
}
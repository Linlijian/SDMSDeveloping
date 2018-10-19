using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UtilityLib;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC003P001Validator))]
    [Serializable]
    public class SEC003P001Model : StandardModel
    {
        public decimal? DEPT_ID { get; set; }

        [Display(Name = "DEPT_NAME_TH", ResourceType = typeof(Translation.SEC.SEC003P001))]
        public string DEPT_NAME_TH { get; set; }

        [Display(Name = "DEPT_NAME_EN", ResourceType = typeof(Translation.SEC.SEC003P001))]
        public string DEPT_NAME_EN { get; set; }

        public List<FileUpload> FileAttached { get; set; }
    }

    public class SEC003P001Validator : AbstractValidator<SEC003P001Model>
    {
        public SEC003P001Validator()
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
            //RuleFor(m => m.TITLE_NAME_TH).NotEmpty();
        }
    }
}
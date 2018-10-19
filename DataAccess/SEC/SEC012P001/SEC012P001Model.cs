using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC012P001Validator))]
    [Serializable]
    public class SEC012P001Model : StandardModel
    {
        public string NAME { get; set; }
        public string STR_VALUE { get; set; }
        public int? INT_VALUE { get; set; }
        public decimal? DEC_VALUE { get; set; }
        [Display(Name = "FTP_SERVER", ResourceType = typeof(Translation.SEC.SEC012P001))]
        public string FTP_SERVER { get; set; }
        [Display(Name = "FTP_PRT", ResourceType = typeof(Translation.SEC.SEC012P001))]
        public string FTP_PRT { get; set; }
        [Display(Name = "FTP_FOLDER", ResourceType = typeof(Translation.SEC.SEC012P001))]
        public string FTP_FOLDER { get; set; }
        [Display(Name = "USER_NAME", ResourceType = typeof(Translation.SEC.SEC012P001))]
        public string USER_NAME { get; set; }
        [Display(Name = "PASSWORD", ResourceType = typeof(Translation.SEC.SEC012P001))]
        public string PASSWORD { get; set; }
        public List<SEC012P001ModelDetail> SEC012P001S { get; set; }
    }

    public class SEC012P001ModelDetail : StandardModel
    {
        public string NAME { get; set; }
        public string STR_VALUE { get; set; }
    }

    public class SEC012P001Validator : AbstractValidator<SEC012P001Model>
    {
        public SEC012P001Validator()
        {
            RuleSet("Index", () =>
            {
                Valid();
            });
        }

        private void Valid()
        {
            RuleFor(m => m.FTP_SERVER).NotEmpty().Length(1, 400);
            RuleFor(m => m.FTP_PRT).NotEmpty().Length(1, 400);
            RuleFor(m => m.USER_NAME).NotEmpty().Length(1, 400);
            RuleFor(m => m.PASSWORD).NotEmpty().Length(1, 400);
            RuleFor(m => m.FTP_FOLDER).Length(1, 400);
        }
    }
}

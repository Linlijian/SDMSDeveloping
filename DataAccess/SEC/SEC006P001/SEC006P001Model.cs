using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SEC006P001Validator))]
    [Serializable]
    public class SEC006P001Model : StandardModel
    {
        [Display(Name = "USER_ID", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string USER_ID { get; set; }

        [Display(Name = "USER_FNAME_TH", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string USER_FNAME_TH { get; set; }

        [Display(Name = "USER_LNAME_TH", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string USER_LNAME_TH { get; set; }

        [Display(Name = "USER_FNAME_EN", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string USER_FNAME_EN { get; set; }

        [Display(Name = "USER_LNAME_EN", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string USER_LNAME_EN { get; set; }

        [Display(Name = "TITLE_ID", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public decimal? TITLE_ID { get; set; }
        public string TITLE_NAME_TH { get; set; }
        public IEnumerable<DDLCenterModel> TITLE_ID_MODEL { get; set; }

        public string DEPT_NAME_TH { get; set; }

        [Display(Name = "DEPT_ID", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public decimal? DEPT_ID { get; set; }
        public IEnumerable<DDLCenterModel> DEPT_ID_MODEL { get; set; }

        public string USG_NAME_TH { get; set; }

        [Display(Name = "USG_ID", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public decimal? USG_ID { get; set; }
        public IEnumerable<DDLCenterModel> USG_ID_MODEL { get; set; }

        public string USG_LEVEL { get; set; }

        [Display(Name = "USER_STATUS", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string USER_STATUS { get; set; }
        public IEnumerable<DDLCenterModel> USER_STATUS_MODEL { get; set; }

        [Display(Name = "IS_DISABLED", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string IS_DISABLED { get; set; }
        public IEnumerable<DDLCenterModel> IS_DISABLED_MODEL { get; set; }

        [Display(Name = "USER_SPEC_ID", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string USER_SPEC_ID { get; set; }

        public string USER_PWD { get; set; }
        public DateTime? USER_EFF_DATE { get; set; }
        public DateTime? USER_EXP_DATE { get; set; }
        public DateTime? PWD_EXP_DATE { get; set; }
        public DateTime? WNING_USER_DATE { get; set; }
        public DateTime? WNING_PWD_DATE { get; set; }
        public DateTime? END_ACT_DATE { get; set; }

        [Display(Name = "TELEPHONE", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string TELEPHONE { get; set; }

        [Display(Name = "EMAIL", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string EMAIL { get; set; }

        public string IS_FCP { get; set; }
        public string IS_NCE { get; set; }
        public DateTime? LAST_LOGIN_DATE { get; set; }

        [Display(Name = "ComUserModel", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public List<SEC006P001_CompanyForUserModel> ComUserModel { get; set; }
    }

    public class SEC006P001_CompanyForUserModel 
    {
        [Display(Name = "COM_CODE", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string COM_CODE { get; set; }

        [Display(Name = "COM_NAME_E", ResourceType = typeof(Translation.SEC.SEC006P001))]
        public string COM_NAME_E { get; set; }

        public string USER_ID { get; set; }

        [Display(Name = "CRET_BY", ResourceType = typeof(Translation.CenterLang.Center))]
        public string CRET_BY { get; set; }

        [Display(Name = "CRET_DATE", ResourceType = typeof(Translation.CenterLang.Center))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> CRET_DATE { get; set; }

        [Display(Name = "MNT_BY", ResourceType = typeof(Translation.CenterLang.Center))]
        public string MNT_BY { get; set; }

        [Display(Name = "MNT_DATE", ResourceType = typeof(Translation.CenterLang.Center))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> MNT_DATE { get; set; }
    }

    public class SEC006P001Validator : AbstractValidator<SEC006P001Model>
    {
        public SEC006P001Validator()
        {
            RuleSet("Add", () =>
            {
                Valid();
                RuleFor(m => m.USER_ID).NotEmpty();
            });

            RuleSet("Edit", () =>
            {
                Valid();
            });
        }

        private void Valid()
        {
            RuleFor(m => m.USER_FNAME_TH).NotEmpty();
            RuleFor(m => m.USER_FNAME_EN).NotEmpty();
            RuleFor(m => m.TITLE_ID).NotEmpty();
            RuleFor(m => m.USG_ID).NotEmpty();
            RuleFor(m => m.DEPT_ID).NotEmpty();
            RuleFor(m => m.USER_STATUS).NotEmpty();
        }
    }
}
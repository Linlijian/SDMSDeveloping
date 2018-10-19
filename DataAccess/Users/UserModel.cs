using DataAccess.SEC;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;

namespace DataAccess.Users
{
    [Serializable]
    public partial class UserModel : StandardModel
    {
        public string USER_ID { get; set; }
        public string USER_FNAME_TH { get; set; }
        public string USER_LNAME_TH { get; set; }
        public string USER_FNAME_EN { get; set; }
        public string USER_LNAME_EN { get; set; }
        public string USER_NAME_EN { get; set; }
        public string USER_NAME_TH { get; set; }
        public Nullable<decimal> USG_ID { get; set; }
        public string SYS_GROUP_NAME { get; set; }
        public string COM_NAME_T { get; set; }
        public string COM_NAME_E { get; set; }
        public string USG_LEVEL { get; set; }
    }

}

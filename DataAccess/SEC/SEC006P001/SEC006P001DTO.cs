
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC006P001DTO : BaseDTO
    {
        public SEC006P001DTO()
        {
            Model = new SEC006P001Model();
        }

        public SEC006P001Model Model { get; set; }
        public List<SEC006P001Model> Models { get; set; }
    }

    public class SEC006P001ExecuteType : DTOExecuteType
    {
        public const string GetQuerySearchAll = "GetQuerySearchAll";
        public const string GetQueryCheckUserAdmin = "GetQueryCheckUserAdmin";
        public const string GetUserCOM = "GetUserCOM";
    }
}
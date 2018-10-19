
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC002P001DTO : BaseDTO
    {
        public SEC002P001DTO()
        {
            Model = new SEC002P001Model();
        }

        public SEC002P001Model Model { get; set; }
        public VSMS_TITLE ModelEF { get; set; }
        public List<SEC002P001Model> Models { get; set; }
    }

    public class SEC002P001ExecuteType : DTOExecuteType
    {
    }
}
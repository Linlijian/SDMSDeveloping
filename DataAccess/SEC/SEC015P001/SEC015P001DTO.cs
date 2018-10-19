
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC015P001DTO : BaseDTO
    {
        public SEC015P001DTO()
        {
            Model = new SEC015P001Model();
        }

        public SEC015P001Model Model { get; set; }
        public List<SEC015P001Model> Models { get; set; }
        public List<SEC015P001_SystemModel> SystemModels { get; set; }
    }

    public class SEC015P001ExecuteType : DTOExecuteType
    {
        public const string GetSystemDetail = "GetSystemDetail";
    }
}
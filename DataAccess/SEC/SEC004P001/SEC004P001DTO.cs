
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC004P001DTO : BaseDTO
    {
        public SEC004P001DTO()
        {
            Model = new SEC004P001Model();
        }

        public SEC004P001Model Model { get; set; }
        public List<SEC004P001Model> Models { get; set; }
    }

    public class SEC004P001ExecuteType : DTOExecuteType
    {
        public const string GetSeqMax = "GetSeqMax"; 
    }
}
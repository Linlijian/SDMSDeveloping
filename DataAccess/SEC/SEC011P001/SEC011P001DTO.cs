
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC011P001DTO : BaseDTO
    {
        public SEC011P001DTO()
        {
            Model = new SEC011P001Model();
        }

        public SEC011P001Model Model { get; set; }
        public List<SEC011P001Model> Models { get; set; }
    }

    public class SEC011P001ExecuteType : DTOExecuteType
    {

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC014P001DTO : BaseDTO
    {
        public SEC014P001DTO()
        {
            Model = new SEC014P001Model();
        }

        public SEC014P001Model Model { get; set; }
        public List<SEC014P001Model> Models { get; set; }
    }

    public class SEC014P001ExecuteType : DTOExecuteType
    {

    }
}
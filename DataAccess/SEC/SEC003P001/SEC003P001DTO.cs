
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC003P001DTO : BaseDTO
    {
        public SEC003P001DTO()
        {
            Model = new SEC003P001Model();
        }

        public SEC003P001Model Model { get; set; }
        public List<SEC003P001Model> Models { get; set; }
    }

    public class SEC003P001ExecuteType : DTOExecuteType
    {
    }
}
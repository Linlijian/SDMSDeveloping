
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC001P001DTO : BaseDTO
    {
        public SEC001P001DTO()
        {
            Model = new SEC001P001Model();   // new โมเดล 
        }

        public SEC001P001Model Model { get; set; }   //model
        public List<SEC001P001Model> Models { get; set; }  //list 
    }

    public class SEC001P001ExecuteType : DTOExecuteType
    {
    }
}
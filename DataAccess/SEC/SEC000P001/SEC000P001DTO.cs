
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC000P001DTO : BaseDTO
    {
        public SEC000P001DTO()
        {
            Model = new SEC000P001Model();   // new โมเดล 
        }

        public SEC000P001Model Model { get; set; }   //model
        public List<SEC000P001Model> Models { get; set; }  //list 
    }

    public class SEC000P001ExecuteType : DTOExecuteType
    {
        public const string GetComLicenseID = "GetComLicenseID";
    }
}
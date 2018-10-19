using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC012P001DTO : BaseDTO
    {
        public SEC012P001DTO()
        {
            Model = new SEC012P001Model();
        }

        public SEC012P001Model Model { get; set; }
        public List<SEC012P001Model> Models { get; set; }
    }

    public class SEC012P001ExecuteType : DTOExecuteType
    {

    }
}

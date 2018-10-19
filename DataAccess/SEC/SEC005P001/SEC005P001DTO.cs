using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC005P001DTO : BaseDTO
    {
        public SEC005P001DTO()
        {
            Model = new SEC005P001Model();
        }

        public SEC005P001Model Model { get; set; }
        public List<SEC005P001Model> Models { get; set; }
    }

    public class SEC005P001ExecuteType : DTOExecuteType
    {
        public const string CHECK_DB_SERVER_NAME = "CHECK_DB_SERVER_NAME";
    }
}

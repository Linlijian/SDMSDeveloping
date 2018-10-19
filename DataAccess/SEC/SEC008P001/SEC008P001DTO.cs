using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC008P001DTO : BaseDTO
    {
        public SEC008P001DTO()
        {
            Model = new SEC008P001Model();
        }

        public SEC008P001Model Model { get; set; }
        public List<SEC008P001Model> Models { get; set; }
    }

    public class SEC008P001ExecuteType : DTOExecuteType
    {
        public const string GetProgram = "GetProgram";
        public const string GetSysPrg = "GetSysPrg";
    }
}

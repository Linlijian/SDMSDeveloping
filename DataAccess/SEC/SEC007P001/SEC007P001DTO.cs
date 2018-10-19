using System;
using System.Collections.Generic;

namespace DataAccess.SEC
{
    [Serializable]
    public class SEC007P001DTO : BaseDTO
    {
        public SEC007P001DTO()
        {
            Model = new SEC007P001Model();
            Models = new List<SEC007P001Model>();
        }
        public SEC007P001Model Model { get; set; }
        public List<SEC007P001Model> Models { get; set; }
    }

    public class SEC007P001ExecuteType : DTOExecuteType
    {
        public const string GetUsgPriv = "GetUsgPriv";
        public const string GetSysSeq = "GetSysSeq";
        public const string GetPrgSeq = "GetPrgSeq"; 
        public const string InsertData = "InsertData";
        public const string UpdateData = "UpdateData";
        public const string UpdateUsgPriv = "UpdateUsgPriv";
        public const string UpdateSysSeq = "UpdateSysSeq";
        public const string UpdatePrgSeq = "UpdatePrgSeq";
        public const string DeleteData = "DeleteData";
    }
}

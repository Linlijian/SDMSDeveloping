using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC014P001DA : BaseDA
    {
        public SEC014P001DTO DTO { get; set; }

        #region ====Property========
        public SEC014P001DA()
        {
            DTO = new SEC014P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SEC014P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC014P001ExecuteType.GetAll: return GetAll(dto);
            }
            return dto;
        }

        private SEC014P001DTO GetAll(SEC014P001DTO dto)
        {
            if (!string.IsNullOrEmpty(dto.Model.SQL_COMMAND))
            {
                if (dto.Model.SQL_COMMAND.ToUpper().IndexOf("INSERT") > -1 ||
                        dto.Model.SQL_COMMAND.ToUpper().IndexOf("UPDATE") > -1 ||
                        dto.Model.SQL_COMMAND.ToUpper().IndexOf("DELETE") > -1 ||
                        dto.Model.SQL_COMMAND.ToUpper().IndexOf("ALTER") > -1 ||
                        dto.Model.SQL_COMMAND.ToUpper().IndexOf("CREATE") > -1)
                {

                    var parameters = CreateParameter();
                    var result = _DBMangerNoEF.ExecuteNonQuery(dto.Model.SQL_COMMAND, parameters, CommandType.Text);
                    if (result.Success(dto))
                    {
                        dto.Model.RECORD_COUNT = result.data.AsInt();
                    }

                    if((!dto.Result.IsResult) && dto.Result.ResultMsg.IsNullOrEmpty())
                    {
                        dto.Result.ActionResult = 0;
                        dto.Result.IsResult = true;
                        dto.Model.RECORD_COUNT = -1;
                    }
                }
                else
                {
                    var parameters = CreateParameter();
                    var result = _DBMangerNoEF.ExecuteDataSet(dto.Model.SQL_COMMAND, parameters, CommandType.Text);
                    if (result.Success(dto))
                    {
                        dto.Model.DYNAMIC_COL = ToDynamicColumns(result.OutputDataSet.Tables[0]);
                    }
                }
            }

            return dto;
        }
        #endregion

        public string ToDynamicColumns(DataTable table)
        {
            string jsonData = "\"COLUMNS\": [";

            for (int i = 0; i < table.Columns.Count; i++)
            {
                string column_name = table.Columns[i].ColumnName.AsString();

                jsonData += "{";
                jsonData += "\"title\": \"" + column_name + "\"";
                jsonData += "},";
            }

            jsonData += "],";
            jsonData += "\"DATA\": [";

            foreach (DataRow dr in table.Rows)
            {
                jsonData += "[";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var data = dr[table.Columns[i].ColumnName.AsString()].AsString();
                    jsonData += "\"" + data + "\",";
                }
                jsonData += "],";
            }

            jsonData += "]";

            return "[{" + jsonData + "}]";
        }
    }
}
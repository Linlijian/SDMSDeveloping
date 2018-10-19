using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC012P001DA : BaseDA
    {
        public SEC012P001DTO DTO { get; set; }

        #region Property

        public SEC012P001DA()
        {
            DTO = new SEC012P001DTO();
        }

        #endregion

        #region Select

        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SEC012P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC012P001ExecuteType.GetAll: return GetAll(dto);
            }
            return dto;
        }

        private SEC012P001DTO GetAll(SEC012P001DTO dto)
        {
            var strList = new string[] { "FTP_SERVER", "FTP_PORT", "FTP_FOLDER", "FTP_USER_NAME", "FTP_PASSWORD" };
            dto.Models = _DBManger.VSMS_CONFIG_FTP.Where(m => strList.Contains(m.NAME))
                         .Select(m => new SEC012P001Model
                         {
                             NAME = m.NAME,
                             STR_VALUE = m.STR_VALUE
                         }).ToList();

            foreach (var item in dto.Models)
            {
                if (item.NAME == "FTP_SERVER")
                {
                    dto.Model.FTP_SERVER = item.STR_VALUE;
                }
                else if (item.NAME == "FTP_PORT")
                {
                    dto.Model.FTP_PRT = item.STR_VALUE;
                }
                else if (item.NAME == "FTP_FOLDER")
                {
                    dto.Model.FTP_FOLDER = item.STR_VALUE;
                }
                else if (item.NAME == "FTP_USER_NAME")
                {
                    dto.Model.USER_NAME = item.STR_VALUE;
                }
                else if (item.NAME == "FTP_PASSWORD")
                {
                    dto.Model.PASSWORD = item.STR_VALUE;
                }
            }

            return dto;
        }

        #endregion

        #region Insert

        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SEC012P001DTO)baseDTO;
            //Delete Before Insert
            DELETE(dto);

            if (dto.Result.ActionResult >= 0)
            {
                foreach (var item in dto.Model.SEC012P001S)
                {
                    var model = item.ToNewObject(new VSMS_CONFIG_FTP());
                    _DBManger.VSMS_CONFIG_FTP.Add(model);
                }
            }
            return dto;
        }

        #endregion

        #region Delete

        private SEC012P001DTO DELETE(BaseDTO baseDTO)
        {
            var dto = (SEC012P001DTO)baseDTO;
            foreach (var item in dto.Model.SEC012P001S)
            {
                var items = _DBManger.VSMS_CONFIG_FTP.Where(m => 1 == 1);
                _DBManger.VSMS_CONFIG_FTP.RemoveRange(items);
            }
            return dto;
        }

        #endregion
    }
}

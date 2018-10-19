using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC001P001DA : BaseDA
    {
        public SEC001P001DTO DTO { get; set; }

        #region ====Property========
        public SEC001P001DA()
        {
            DTO = new  SEC001P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SEC001P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC001P001ExecuteType.GetAll: return GetAll(dto);
                case SEC001P001ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }

        private SEC001P001DTO GetAll(SEC001P001DTO dto)
        {
            dto.Models = _DBManger.VSMS_CTRLLOG
                .Where((m => ((dto.Model.TBL_NAME == null || dto.Model.TBL_NAME == string.Empty) || m.TBL_NAME.Contains(dto.Model.TBL_NAME))
                && ((dto.Model.TBL_TYPE == null || dto.Model.TBL_TYPE == string.Empty) || m.TBL_TYPE.Contains(dto.Model.TBL_TYPE))
                && ((dto.Model.LOG_STATUS == null || dto.Model.LOG_STATUS == string.Empty) || m.LOG_STATUS.Contains(dto.Model.LOG_STATUS))

                ))
                //.OrderBy(m => new { m.COM_CODE, m.COM_CODE })
                .Select(m => new SEC001P001Model
                {
                    COM_CODE = m.COM_CODE,
                    TBL_NAME = m.TBL_NAME,
                    TBL_TYPE = m.TBL_TYPE,
                    LOG_STATUS = m.LOG_STATUS

                }).ToList();

            return dto;
        }
        

        private SEC001P001DTO GetByID(SEC001P001DTO dto)
        {
            dto.Model = _DBManger.VSMS_CTRLLOG
                .Where(m => m.TBL_NAME == dto.Model.TBL_NAME)
                .FirstOrDefault().ToNewObject(new SEC001P001Model());
          
            dto.Model.TBL_NAME = dto.Model.TBL_NAME;
            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SEC001P001DTO)baseDTO;
            //var COM_CODE = _DBManger.VSMS_CTRLLOG.Max(m => m.COM_CODE).AsString() + 1;
            //dto.Model.COM_CODE = COM_CODE;

            var model = dto.Model.ToNewObject(new VSMS_CTRLLOG());
            var TBL_NAME  = model.TBL_NAME;

            model.TBL_NAME = TBL_NAME;

            _DBManger.VSMS_CTRLLOG.Add(model);
            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SEC001P001DTO)baseDTO;
            var TBL_NAME = dto.Model.TBL_NAME;  //เอา  COM_CODE มาหาในเทเบิล เพื่อ อัพเดท
            //dto.Model.COM_BRANCH.TrimEnd();
            var model = _DBManger.VSMS_CTRLLOG.First(m => m.TBL_NAME == TBL_NAME);
          
            model.MergeObject(dto.Model);
            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SEC001P001DTO)baseDTO;
            foreach (var item in dto.Models)
            {
               //var TBL_NAME = item.TBL_NAME.AsString();

                var items = _DBManger.VSMS_CTRLLOG.Where(m => m.TBL_NAME == item.TBL_NAME);
                _DBManger.VSMS_CTRLLOG.RemoveRange(items);
            }
            return dto;
        }
        #endregion
    }
}
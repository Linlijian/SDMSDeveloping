using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC003P001DA : BaseDA
    {
        public SEC003P001DTO DTO { get; set; }

        #region ====Property========
        public SEC003P001DA()
        {
            DTO = new SEC003P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SEC003P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC003P001ExecuteType.GetAll: return GetAll(dto);
                case SEC003P001ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }

        private SEC003P001DTO GetAll(SEC003P001DTO dto)
        {
            dto.Models = _DBManger.VSMS_DEPARTMENT
                .Where(m => ((dto.Model.DEPT_NAME_TH == null) || (m.DEPT_NAME_TH.Contains(dto.Model.DEPT_NAME_TH) || m.DEPT_NAME_EN.Contains(dto.Model.DEPT_NAME_TH))))
                .OrderBy(m => new { m.DEPT_NAME_TH })
                .Select(m => new SEC003P001Model
                {
                    DEPT_ID = m.DEPT_ID,
                    DEPT_NAME_TH = m.DEPT_NAME_TH,
                    DEPT_NAME_EN = m.DEPT_NAME_EN,
                }).ToList();
            return dto;
        }
        private SEC003P001DTO GetByID(SEC003P001DTO dto)
        {
            dto.Model = _DBManger.VSMS_DEPARTMENT
                .Where(m => m.DEPT_ID == dto.Model.DEPT_ID)
                .FirstOrDefault().ToNewObject(new SEC003P001Model());
            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SEC003P001DTO)baseDTO;
            //var TITLE_ID = _DBManger.VSMS_TITLE.Max(m => m.TITLE_ID).AsDecimal() + 1;
            //var model = dto.Model.ToNewObject(new DataModel.VSMS_TITLE());
            //model.TITLE_ID = TITLE_ID;
            //_DBManger.VSMS_TITLE.Add(model);
            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SEC003P001DTO)baseDTO;
            var DEPT_ID = dto.Model.DEPT_ID.AsDecimal();
            var model = _DBManger.VSMS_DEPARTMENT.First(m => m.DEPT_ID == DEPT_ID);
            model.MergeObject(dto.Model);
            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SEC003P001DTO)baseDTO;
            foreach (var item in dto.Models)
            {
                var DEPT_ID = item.DEPT_ID.AsDecimal();
                var items = _DBManger.VSMS_DEPARTMENT.Where(m => m.DEPT_ID == DEPT_ID);
                _DBManger.VSMS_DEPARTMENT.RemoveRange(items);
            }
            return dto;
        }
        #endregion
    }
}
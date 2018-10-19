using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC015P001DA : BaseDA
    {
        public SEC015P001DTO DTO { get; set; }

        #region ====Property========
        public SEC015P001DA()
        {
            DTO = new SEC015P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SEC015P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC015P001ExecuteType.GetAll: return GetAll(dto);
                case SEC015P001ExecuteType.GetSystemDetail: return GetSystemDetail(dto);
                case SEC015P001ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }
        private SEC015P001DTO GetAll(SEC015P001DTO dto)
        {
            dto.Models = _DBManger.VSMS_CONFIG_GENERAL
                  .Where(m => ((dto.Model.NAME == null || dto.Model.NAME == string.Empty) || m.NAME.Contains(dto.Model.NAME)))
                  .OrderBy(m => new { m.NAME })
                  .Select(m => new
                  {
                      NAME = m.NAME,
                      SEQUENCE = m.SEQUENCE
                  }).Distinct().Select(m => new SEC015P001Model { NAME = m.NAME, SEQUENCE = m.SEQUENCE }).ToList();
            return dto;
        }
        private SEC015P001DTO GetSystemDetail(SEC015P001DTO dto)
        {
            var cmd = @"select t1.SYS_CODE,
                               t1.SYS_NAME_TH,
                               t1.SYS_NAME_EN,
                               case
                                 when t2.SYS_CODE is not null then
                                  1
                                 else
                                  0
                               end SELECTED,
	                           ROW_NUMBER() OVER(order by SYS_NAME_EN,SYS_NAME_TH) ROW_NO
                          from VSMS_SYSTEM t1
                          left join (select * from VSMS_CONFIG_GENERAL where NAME = @NAME) t2
                            on t1.COM_CODE = t2.COM_CODE
                           and t1.SYS_CODE = t2.SYS_CODE
                           order by SYS_NAME_EN,SYS_NAME_TH
                        ";
            var parameters = CreateParameter();
            parameters.AddParameter("NAME", dto.Model.NAME);
            var result = _DBMangerNoEF.ExecuteDataSet(cmd, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model.SystemModels = result.OutputDataSet.Tables[0].ToList<SEC015P001_SystemModel>();
            }
            return dto;
        }

        private SEC015P001DTO GetByID(SEC015P001DTO dto)
        {
            dto.Model = _DBManger.VSMS_CONFIG_GENERAL
                .Where(m => m.NAME == dto.Model.NAME)
                .FirstOrDefault().ToNewObject(new SEC015P001Model());
            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SEC015P001DTO)baseDTO;

            if (dto.Model.SystemModels.Count() > 0)
            {
                foreach (var item in dto.Model.SystemModels)
                {
                    var model = dto.Model.ToNewObject(new VSMS_CONFIG_GENERAL());
                    model.SYS_CODE = item.SYS_CODE;
                    _DBManger.VSMS_CONFIG_GENERAL.Add(model);
                }
            }
            else
            {
                var ID = _DBManger.VSMS_CONFIG_GENERAL.Max(m => m.ID).AsDecimalNull() + 1;
                dto.Model.ID = ID.AsDecimal();

                var model = dto.Model.ToNewObject(new VSMS_CONFIG_GENERAL());
                _DBManger.VSMS_CONFIG_GENERAL.Add(model);
            }

            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            //Delete
            var dto = (SEC015P001DTO)baseDTO;
            var group_name = dto.Model.NAME_Old;

            var items = _DBManger.VSMS_CONFIG_GENERAL.Where(m => m.NAME == group_name);
            _DBManger.VSMS_CONFIG_GENERAL.RemoveRange(items);

            //Add
            if (dto.Model.SystemModels.Count() > 0)
            {
                foreach (var item in dto.Model.SystemModels)
                {
                    var ID = _DBManger.VSMS_CONFIG_GENERAL.Max(m => m.ID).AsDecimalNull() + 1;

                    var data = new SEC015P001Model();
                    data = dto.Model;
                    data.ID = ID.AsDecimal();
                    data.SYS_CODE = item.SYS_CODE.Trim();

                    var model = data.ToNewObject(new VSMS_CONFIG_GENERAL());
                    _DBManger.VSMS_CONFIG_GENERAL.Add(model);
                }
            }
            else
            {
                var ID = _DBManger.VSMS_CONFIG_GENERAL.Max(m => m.ID).AsDecimalNull() + 1;
                dto.Model.ID = ID.AsDecimal();

                var model = dto.Model.ToNewObject(new VSMS_CONFIG_GENERAL());
                _DBManger.VSMS_CONFIG_GENERAL.Add(model);
            }

            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SEC015P001DTO)baseDTO;
            foreach (var item in dto.Models)
            {
                var group_name = item.NAME;
                var items = _DBManger.VSMS_CONFIG_GENERAL.Where(m => m.NAME == group_name);
                _DBManger.VSMS_CONFIG_GENERAL.RemoveRange(items);
            }
            return dto;
        }
        #endregion
    }
}
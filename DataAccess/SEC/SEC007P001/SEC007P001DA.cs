using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC007P001DA : BaseDA
    {
        public SEC007P001DTO DTO
        {
            get;
            set;
        }

        public SEC007P001DA()
        {
            DTO = new SEC007P001DTO();
        }

        #region Select
        protected override BaseDTO DoSelect(BaseDTO DTO)
        {
            var dto = (SEC007P001DTO)DTO;

            switch (dto.Execute.ExecuteType)
            {
                case SEC007P001ExecuteType.GetAll:
                    return GetAll(dto);
                case SEC007P001ExecuteType.GetByID:
                    return GetByID(dto);
                case SEC007P001ExecuteType.GetUsgPriv:
                    return GetUsgPriv(dto);
                case SEC007P001ExecuteType.GetSysSeq:
                    return GetSysSeq(dto);
                case SEC007P001ExecuteType.GetPrgSeq:
                    return GetPrgSeq(dto); 
            }

            return dto;
        }

        private SEC007P001DTO GetAll(SEC007P001DTO dto)
        {
            dto.Models = (from t1 in _DBManger.VSMS_USRGROUP.AsEnumerable()
                          where (dto.Model.USG_NAME_TH.IsNullOrEmpty() || t1.USG_NAME_TH.Contains(dto.Model.USG_NAME_TH)) &&
                          (dto.Model.USG_NAME_EN.IsNullOrEmpty() || t1.USG_NAME_EN.Contains(dto.Model.USG_NAME_EN)) &&
                          (dto.Model.USG_CODE.IsNullOrEmpty() || t1.USG_NAME_TH.Contains(dto.Model.USG_CODE)) &&
                          (dto.Model.USG_STATUS.IsNullOrEmpty() || t1.USG_STATUS == dto.Model.USG_STATUS)
                          select new SEC007P001Model
                          {
                              COM_CODE = t1.COM_CODE,
                              USG_ID = t1.USG_ID,
                              USG_CODE = t1.USG_CODE,
                              USG_NAME_TH = t1.USG_NAME_TH,
                              USG_NAME_EN = t1.USG_NAME_EN,
                              USG_STATUS = t1.USG_STATUS
                          }).ToList();


            return dto;
        }
        private SEC007P001DTO GetByID(SEC007P001DTO dto)
        {
            dto.Model = _DBManger.VSMS_USRGROUP
                .Where(m => m.COM_CODE == dto.Model.COM_CODE && m.USG_ID == dto.Model.USG_ID)
                .FirstOrDefault().ToNewObject(new SEC007P001Model());
            return dto;
        }

        private SEC007P001DTO GetUsgPriv(SEC007P001DTO dto)
        {
            var cmd = @"
                        select t1.SYS_CODE,
                               t1.PRG_CODE,
                               t1.PRG_SEQ,
                               t4.SYS_SEQ,
                               t2.PRG_NAME_EN,
                               t2.PRG_NAME_TH,
                               t2.PRG_TYPE,
                               ISNULL(t3.ROLE_SEARCH,'F') ROLE_SEARCH,
                               ISNULL(t3.ROLE_ADD,'F') ROLE_ADD,
                               ISNULL(t3.ROLE_EDIT,'F') ROLE_EDIT,
                               ISNULL(t3.ROLE_DEL,'F') ROLE_DEL,
                               ISNULL(t3.ROLE_PRINT,'F') ROLE_PRINT,
                               t3.USRGRPPRIV_ID
                          from (select *
                                  from VSMS_SYS_PGC
                                 where COM_CODE = @COM_CODE
                                   and SYS_CODE = @SYS_CODE) t1
                         inner join VSMS_PROGRAM t2
                            on t1.PRG_CODE = t2.PRG_CODE
                          left join (select * from VSMS_USRGRPPRIV where USG_ID = @USG_ID) t3
                            on t1.COM_CODE = t3.COM_CODE
                           and t1.SYS_CODE = t3.SYS_CODE
                           and t1.PRG_CODE = t3.PRG_CODE
						  inner join VSMS_SYSTEM t4
						    on t1.COM_CODE = t4.COM_CODE
						   and t1.SYS_CODE = t4.SYS_CODE
                           order by t1.PRG_SEQ
                        ";
            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("SYS_CODE", dto.Model.SYS_CODE);
            parameters.AddParameter("USG_ID", dto.Model.USG_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(cmd, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model.PRIV_MODEL = result.OutputDataSet.Tables[0].ToList<SEC007P00101Model>();
            }
            return dto;
        }

        private SEC007P001DTO GetSysSeq(SEC007P001DTO dto)
        {
            var cmd = @"
                    select t1.COM_CODE, t1.SYS_CODE, t1.SYS_SEQ, t2.SYS_NAME_TH, t2.SYS_NAME_EN
                      from (select distinct t1.COM_CODE, t1.SYS_CODE, t1.SYS_SEQ
                              from VSMS_USRGRPPRIV t1
                             inner join VSMS_CONFIG_GENERAL t2
                                on t1.COM_CODE = t2.COM_CODE
                               and t1.SYS_CODE = t2.SYS_CODE
                             where t1.COM_CODE = @COM_CODE
                               and t1.USG_ID = @USG_ID
                               and t2.NAME = @SYS_GROUP_NAME) t1
                     inner join VSMS_SYSTEM t2
                        on t1.COM_CODE = t2.COM_CODE
                       and t1.SYS_CODE = t2.SYS_CODE
                     order by SYS_SEQ
                    ";
            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("USG_ID", dto.Model.USG_ID);
            parameters.AddParameter("SYS_GROUP_NAME", dto.Model.SYS_GROUP_NAME);

            var result = _DBMangerNoEF.ExecuteDataSet(cmd, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model.PRIV_MODEL = result.OutputDataSet.Tables[0].ToList<SEC007P00101Model>();
            }
            return dto;
        }
        private SEC007P001DTO GetPrgSeq(SEC007P001DTO dto)
        {
            var cmd = @"
                    select t1.COM_CODE, t1.PRG_CODE, t1.PRG_SEQ, t2.PRG_NAME_TH, t2.PRG_NAME_EN
                      from (select t1.COM_CODE, t1.PRG_CODE, t1.PRG_SEQ
                              from VSMS_USRGRPPRIV t1
                             inner join VSMS_CONFIG_GENERAL t2
                                on t1.COM_CODE = t2.COM_CODE
                               and t1.SYS_CODE = t2.SYS_CODE
                             where t1.COM_CODE = @COM_CODE
                               and t1.USG_ID = @USG_ID
                               and t1.SYS_CODE = @SYS_CODE
                               and t2.NAME = @SYS_GROUP_NAME) t1
                     inner join VSMS_PROGRAM t2
                        on t1.COM_CODE = t2.COM_CODE
                       and t1.PRG_CODE = t2.PRG_CODE
                     order by PRG_SEQ
                    ";
            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("USG_ID", dto.Model.USG_ID);
            parameters.AddParameter("SYS_CODE", dto.Model.SYS_CODE);
            parameters.AddParameter("SYS_GROUP_NAME", dto.Model.SYS_GROUP_NAME);

            var result = _DBMangerNoEF.ExecuteDataSet(cmd, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model.PRIV_MODEL = result.OutputDataSet.Tables[0].ToList<SEC007P00101Model>();
            }
            return dto;
        }
        #endregion

        #region Insert
        protected override BaseDTO DoInsert(BaseDTO DTO)
        {
            var dto = (SEC007P001DTO)DTO;

            switch (dto.Execute.ExecuteType)
            {
                case SEC007P001ExecuteType.InsertData:
                    return InsertData(dto);
            }

            return dto;
        }

        private SEC007P001DTO InsertData(SEC007P001DTO dto)
        {
            var model = dto.Model.ToNewObject(new VSMS_USRGROUP());
            _DBManger.VSMS_USRGROUP.Add(model);
            return dto;
        }
        #endregion

        #region Update
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SEC007P001DTO)DTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC007P001ExecuteType.UpdateData:
                    return UpdateData(dto);
                case SEC007P001ExecuteType.UpdateUsgPriv:
                    return UpdateUsgPriv(dto);
                case SEC007P001ExecuteType.UpdateSysSeq:
                    return UpdateSysSeq(dto);
                case SEC007P001ExecuteType.UpdatePrgSeq:
                    return UpdatePrgSeq(dto);
            }

            return dto;
        }

        private SEC007P001DTO UpdateData(SEC007P001DTO dto)
        {
            var model = _DBManger.VSMS_USRGROUP.Where(m => m.COM_CODE == dto.Model.COM_CODE && m.USG_ID == dto.Model.USG_ID).FirstOrDefault();
            model = model.MergeObject(dto.Model);
            return dto;
        }
        private SEC007P001DTO UpdateUsgPriv(SEC007P001DTO dto)
        {
            var dels = dto.Model.PRIV_MODEL.Where(m => m.ROLE_SEARCH != "T" && !m.USRGRPPRIV_ID.IsNullOrEmpty()).Select(m => m.USRGRPPRIV_ID);
            if (dels != null && dels.Count() > 0)
            {
                var del = _DBManger.VSMS_USRGRPPRIV.Where(m =>
                                            m.COM_CODE == dto.Model.COM_CODE &&
                                            m.USG_ID == dto.Model.USG_ID &&
                                            m.SYS_CODE == dto.Model.SYS_CODE &&
                                            dels.Contains(m.USRGRPPRIV_ID));
                _DBManger.VSMS_USRGRPPRIV.RemoveRange(del);
            }
            var update = dto.Model.PRIV_MODEL.Where(m => m.ROLE_SEARCH == "T");
            if (update != null && update.Count() > 0)
            {
                foreach (var item in update)
                {
                    var model = _DBManger.VSMS_USRGRPPRIV.Where(m => m.COM_CODE == dto.Model.COM_CODE && m.USG_ID == dto.Model.USG_ID && m.SYS_CODE == dto.Model.SYS_CODE && m.USRGRPPRIV_ID == item.USRGRPPRIV_ID).FirstOrDefault();
                    if (model != null)
                    {
                        model = model.MergeObject(item);
                        model = model.MergeObject(dto.Model);
                    }
                    else
                    {
                        var newModel = item.ToNewObject(new VSMS_USRGRPPRIV());
                        newModel = newModel.MergeObject(dto.Model);
                        _DBManger.VSMS_USRGRPPRIV.Add(newModel);
                    }
                }
            }
            return dto;
        }
        private SEC007P001DTO UpdateSysSeq(SEC007P001DTO dto)
        {
            var update = from t1 in _DBManger.VSMS_USRGRPPRIV
                         join t2 in _DBManger.VSMS_CONFIG_GENERAL on new { t1.COM_CODE, t1.SYS_CODE } equals new { t2.COM_CODE, t2.SYS_CODE }
                         where t1.COM_CODE == dto.Model.COM_CODE && t1.USG_ID == dto.Model.USG_ID && t2.NAME == dto.Model.SYS_GROUP_NAME
                         select t1;
            if (update != null && update.Any())
            {
                var i = 0;
                foreach (var item in dto.Model.PRIV_MODEL)
                {
                    var models = update.Where(m => m.SYS_CODE == item.SYS_CODE);
                    if (models != null && models.Any())
                    {
                        models.ToList().ForEach(m => m.SYS_SEQ = i);
                    }
                    i++;
                }
            }
            return dto;
        }

        private SEC007P001DTO UpdatePrgSeq(SEC007P001DTO dto)
        {
            var update = from t1 in _DBManger.VSMS_USRGRPPRIV
                         join t2 in _DBManger.VSMS_CONFIG_GENERAL on new { t1.COM_CODE, t1.SYS_CODE } equals new { t2.COM_CODE, t2.SYS_CODE }
                         where t1.COM_CODE == dto.Model.COM_CODE && t1.USG_ID == dto.Model.USG_ID && t1.SYS_CODE == dto.Model.SYS_CODE && t2.NAME == dto.Model.SYS_GROUP_NAME
                         select t1;
            if (update != null && update.Any())
            {
                var i = 0;
                foreach (var item in dto.Model.PRIV_MODEL)
                {
                    var models = update.Where(m => m.PRG_CODE == item.PRG_CODE);
                    if (models != null && models.Any())
                    {
                        models.ToList().ForEach(m => m.PRG_SEQ = i);
                    }
                    i++;
                }
            }
            return dto;
        }
        #endregion

        #region Delete
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SEC007P001DTO)DTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC007P001ExecuteType.DeleteData:
                    return DeleteData(dto);
            }
            return dto;
        }
        private SEC007P001DTO DeleteData(SEC007P001DTO dto)
        {
            foreach (var item in dto.Models)
            {
                var model = _DBManger.VSMS_USRGROUP.Where(m => m.COM_CODE == item.COM_CODE && m.USG_ID == item.USG_ID);
                _DBManger.VSMS_USRGROUP.RemoveRange(model);
            }
            return dto;
        }
        #endregion
    }
}

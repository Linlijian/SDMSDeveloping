using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC006P001DA : BaseDA
    {
        public SEC006P001DTO DTO { get; set; }

        #region ====Property========
        public SEC006P001DA()
        {
            DTO = new SEC006P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SEC006P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC006P001ExecuteType.GetAll: return GetAll(dto);
                case SEC006P001ExecuteType.GetByID: return GetByID(dto);
                case SEC006P001ExecuteType.GetQuerySearchAll:
                    return GetQuerySearchAll(dto);
                case SEC006P001ExecuteType.GetQueryCheckUserAdmin:
                    return GetQueryCheckUserAdmin(dto);
                case SEC006P001ExecuteType.GetUserCOM:
                    return GetUserCOM(dto);
            }
            return dto;
        }

        private SEC006P001DTO GetAll(SEC006P001DTO dto)
        {
            dto.Models = _DBManger.VSMS_USER
                .Where(m => (m.USER_ID == dto.Model.USER_ID))
                .OrderBy(m => new { m.USER_ID })
                .Select(m => new SEC006P001Model
                {
                    COM_CODE = m.COM_CODE,
                    USER_ID = m.USER_ID,
                    USER_FNAME_TH = m.USER_FNAME_TH,
                    USER_LNAME_TH = m.USER_LNAME_TH,
                    USER_FNAME_EN = m.USER_FNAME_EN,
                    USER_LNAME_EN = m.USER_LNAME_EN,
                    TITLE_ID = m.TITLE_ID,
                    DEPT_ID = m.DEPT_ID,
                    USG_ID = m.USG_ID,
                    USER_SPEC_ID = m.USER_SPEC_ID,
                    USER_PWD = m.USER_PWD,
                    USER_EFF_DATE = m.USER_EFF_DATE,
                    USER_EXP_DATE = m.USER_EXP_DATE,
                    PWD_EXP_DATE = m.PWD_EXP_DATE,
                    WNING_USER_DATE = m.WNING_USER_DATE,
                    WNING_PWD_DATE = m.WNING_PWD_DATE,
                    END_ACT_DATE = m.END_ACT_DATE,
                    TELEPHONE = m.TELEPHONE,
                    EMAIL = m.EMAIL,
                    USER_STATUS = m.USER_STATUS,
                    IS_FCP = m.IS_FCP,
                    IS_NCE = m.IS_NCE,
                    CRET_BY = m.CRET_BY,
                    CRET_DATE = m.CRET_DATE,
                    MNT_BY = m.MNT_BY,
                    MNT_DATE = m.MNT_DATE,
                    IS_DISABLED = m.IS_DISABLED,
                    LAST_LOGIN_DATE = m.LAST_LOGIN_DATE,
                }).ToList();
            return dto;
        }
        private SEC006P001DTO GetByID(SEC006P001DTO dto)
        {
            dto.Model = _DBManger.VSMS_USER
                .Where(m => m.USER_ID == dto.Model.USER_ID)
                .FirstOrDefault().ToNewObject(new SEC006P001Model());
            return dto;
        }
        private SEC006P001DTO GetQuerySearchAll(SEC006P001DTO dto)
        {
            dto.Models = (
                        from a in _DBManger.VSMS_USER

                        join b in _DBManger.VSMS_DEPARTMENT on new { DEPT_ID = a.DEPT_ID.ToString(), COM_CODE = a.COM_CODE } equals new { DEPT_ID = b.DEPT_ID.ToString(), COM_CODE = b.COM_CODE }
                        into ab
                        from a_b in ab.DefaultIfEmpty()

                        join c in _DBManger.VSMS_USRGROUP on a.USG_ID equals c.USG_ID
                        into ac
                        from a_c in ac.DefaultIfEmpty()

                        where (dto.Model.USER_ID == null || a.USER_ID.Contains(dto.Model.USER_ID))
                                && (dto.Model.USER_FNAME_TH == null || a.USER_FNAME_TH.Contains(dto.Model.USER_FNAME_TH))
                                && (dto.Model.USER_FNAME_EN == null || a.USER_FNAME_EN.Contains(dto.Model.USER_FNAME_EN))
                                && (dto.Model.DEPT_ID == null || a.DEPT_ID == dto.Model.DEPT_ID)
                                && (dto.Model.USG_ID == null || a.USG_ID == dto.Model.USG_ID)
                                && (dto.Model.IS_DISABLED == null || ((a.IS_DISABLED == null ? "N" : a.IS_DISABLED) == dto.Model.IS_DISABLED))
                        orderby a.USER_ID
                        select new SEC006P001Model
                        {
                            USER_ID = a.USER_ID,
                            USER_FNAME_TH = a.USER_FNAME_TH,
                            USER_LNAME_TH = a.USER_LNAME_TH,
                            USER_FNAME_EN = a.USER_FNAME_TH,
                            USER_LNAME_EN = a.USER_LNAME_EN,
                            DEPT_NAME_TH = a_b.DEPT_NAME_TH,
                            USG_NAME_TH = a_c.USG_NAME_TH,
                            IS_DISABLED = a.IS_DISABLED,
                        }).ToList();

            //dto.Models = _DBManger.VSMS_USER
            //    .Where(m => (m.COM_CODE == dto.Model.COM_CODE))
            //    .OrderBy(m => new { m.USER_ID })
            //    .Select(m => new SEC006P001Model
            //    {
            //        COM_CODE = m.COM_CODE,
            //        USER_ID = m.USER_ID,
            //        USER_FNAME_TH = m.USER_FNAME_TH,
            //        USER_LNAME_TH = m.USER_LNAME_TH,
            //        USER_FNAME_EN = m.USER_FNAME_EN,
            //        USER_LNAME_EN = m.USER_LNAME_EN,
            //        TITLE_ID = m.TITLE_ID,
            //        DEPT_ID = m.DEPT_ID,
            //        USG_ID = m.USG_ID,
            //        USER_SPEC_ID = m.USER_SPEC_ID,
            //        USER_PWD = m.USER_PWD,
            //        USER_EFF_DATE = m.USER_EFF_DATE,
            //        USER_EXP_DATE = m.USER_EXP_DATE,
            //        PWD_EXP_DATE = m.PWD_EXP_DATE,
            //        WNING_USER_DATE = m.WNING_USER_DATE,
            //        WNING_PWD_DATE = m.WNING_PWD_DATE,
            //        END_ACT_DATE = m.END_ACT_DATE,
            //        TELEPHONE = m.TELEPHONE,
            //        EMAIL = m.EMAIL,
            //        USER_STATUS = m.USER_STATUS,
            //        IS_FCP = m.IS_FCP,
            //        IS_NCE = m.IS_NCE,
            //        CRET_BY = m.CRET_BY,
            //        CRET_DATE = m.CRET_DATE,
            //        MNT_BY = m.MNT_BY,
            //        MNT_DATE = m.MNT_DATE,
            //        IS_DISABLED = m.IS_DISABLED,
            //        LAST_LOGIN_DATE = m.LAST_LOGIN_DATE
            //    }).ToList();
            return dto;
        }
        private SEC006P001DTO GetQueryCheckUserAdmin(SEC006P001DTO dto)
        {
            string strSQL = @"  SELECT USG_LEVEL 
                                FROM VSMS_USRGROUP a inner join VSMS_USER b on a.COM_CODE = b.COM_CODE and a.USG_ID = b.USG_ID
                                WHERE USG_STATUS = 'E'  ";

            var parameters = CreateParameter();

            if (!dto.Model.COM_CODE.IsNullOrEmpty())
            {
                strSQL += " and b.com_code = @com_code ";
                parameters.AddParameter("com_code", dto.Model.COM_CODE);
            }

            if (!dto.Model.USER_ID.IsNullOrEmpty())
            {
                strSQL += " and b.USER_ID like '%' + @USER_ID + '%'";
                parameters.AddParameter("USER_ID", dto.Model.USER_ID);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<SEC006P001Model>();
            }

            return dto;
        }
        private SEC006P001DTO GetUserCOM(SEC006P001DTO dto)
        {
            string strSQL = @"  select VUC.*
		                                ,VC.COM_NAME_E
                                from VSMS_USERCOM VUC
	                                left join VSMS_COMPANY VC on VUC.COM_CODE = VC.COM_CODE
                                where (1=1) ";

            var parameters = CreateParameter();
          
            if (!dto.Model.USER_ID.IsNullOrEmpty())
            {
                strSQL += " and VUC.USER_ID = @USER_ID ";
                parameters.AddParameter("USER_ID", dto.Model.USER_ID);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model.ComUserModel = result.OutputDataSet.Tables[0].ToList<SEC006P001_CompanyForUserModel>();
            }

            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SEC006P001DTO)baseDTO;

            #region delete insert USERCOM //insert ไม่ต้อง
            //var USER_ID = dto.Model.USER_ID.AsString();
            //var COM_CODE = dto.Model.COM_CODE.AsString();
            //var items = _DBManger.VSMS_USERCOM.Where(m => (m.USER_ID.Trim() == USER_ID.Trim() && m.COM_CODE.Trim() == COM_CODE.Trim()));
            //_DBManger.VSMS_USERCOM.RemoveRange(items);

            //foreach (var item in dto.Model.ComUserModel)
            //{
            //    _DBManger.VSMS_USERCOM.Add(item.ToNewObject(new VSMS_USERCOM()));
            //}
            #endregion

            var USER_ID = dto.Model.USER_ID.AsString();
            var model = dto.Model.ToNewObject(new VSMS_USER());
            _DBManger.VSMS_USER.Add(model);

            if (dto.Result.IsResult)
            {
                foreach (var item in dto.Model.ComUserModel)
                {
                    item.USER_ID = USER_ID;
                    _DBManger.VSMS_USERCOM.Add(item.ToNewObject(new VSMS_USERCOM()));
                }
            }
            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SEC006P001DTO)baseDTO;
            var USER_ID = dto.Model.USER_ID.AsString();
            var COM_CODE = dto.Model.COM_CODE.AsString();

            #region delete insert USERCOM 
            var items = _DBManger.VSMS_USERCOM.Where(m => (m.USER_ID.Trim() == USER_ID.Trim()));
            _DBManger.VSMS_USERCOM.RemoveRange(items);

            foreach (var item in dto.Model.ComUserModel)
            {
                _DBManger.VSMS_USERCOM.Add(item.ToNewObject(new VSMS_USERCOM()));
            }
            #endregion

            var model = _DBManger.VSMS_USER.First(m => m.USER_ID == USER_ID && m.COM_CODE == COM_CODE);
            model.MergeObject(dto.Model);
            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SEC006P001DTO)baseDTO;

            foreach (var item in dto.Models)
            {
                var USER_ID = item.USER_ID.AsString();
                var COM_CODE = dto.Model.COM_CODE.AsString();

                var userCom = _DBManger.VSMS_USERCOM.Where(m => (m.USER_ID.Trim() == USER_ID.Trim()));
                _DBManger.VSMS_USERCOM.RemoveRange(userCom);

                var items = _DBManger.VSMS_USER.Where(m => m.USER_ID == USER_ID && m.COM_CODE == COM_CODE);
                _DBManger.VSMS_USER.RemoveRange(items);
            }
            return dto;
        }
        #endregion
    }
}
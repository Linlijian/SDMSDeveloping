﻿using DataAccess;
using DataAccess.SEC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SEC006P001Controller : SECBaseController
    {
        #region Property
        private SEC006P001Model localModel = new SEC006P001Model();
        private SEC006P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC006P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC006P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC006P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC006P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC006P001Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }

        #endregion

        #region View
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);

            //AddButton(StandButtonType.ButtonAjax, "report", "Report", cssClass: "std-btn-print", iconCssClass: FaIcons.FaPrint, url: Url.Action("ViewReport"));

            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch.CloneObject();
            }
            SetDefaultData(StandardActionName.Index);

            return View(StandardActionName.Index, localModel);
        }

        [RuleSetForClientSideMessages("Add")]
        public ActionResult Add()
        {
            SetDefaulButton(StandardButtonMode.Create);
            SetDefaultData(StandardActionName.Add);
            return View(StandardActionName.Add, localModel);
        }

        [RuleSetForClientSideMessages("Edit")]
        public ActionResult Edit(string USER_ID)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SEC006P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC006P001ExecuteType.GetByID;
            TempModel.USER_ID = da.DTO.Model.USER_ID = USER_ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }

            SetDefaultData(StandardActionName.Edit);

            return View(StandardActionName.Edit, localModel);
        }

        [HttpGet]
        public ActionResult Info(string USER_ID)
        {
            SetDefaulButton(StandardButtonMode.View);
            var da = new SEC006P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = DTOExecuteType.GetByID;
            TempModel.USER_ID = da.DTO.Model.USER_ID = USER_ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Info, localModel);
        }

        public ActionResult test()
        {
            SetDefaulButton(StandardButtonMode.Other);
            AddStandardButton(StandardButtonName.LoadFile);

            localModel.USER_EFF_DATE = DateTime.Now;
            return View(localModel);
        }
        #endregion

        #region Action 
        public ActionResult Search(SEC006P001Model model)
        {
            var da = new SEC006P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC006P001ExecuteType.GetQuerySearchAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;

            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;

            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }

        [HttpPost]
        public ActionResult DeleteSearch(List<SEC006P001Model> data)
        {
            var jsonResult = new JsonResult();
            if (data != null && data.Count > 0)
            {
                var result = SaveData(StandardActionName.Delete, data);
                jsonResult = Success(result, StandardActionName.Delete);
            }
            else
            {
                jsonResult = ValidateError(StandardActionName.Delete, new ValidationError("", Translation.CenterLang.Center.DataNotFound));
            }
            return jsonResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SEC006P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = SessionHelper.SYS_COM_CODE;

                var result = SaveData(StandardActionName.SaveCreate, model);
                jsonResult = Success(result, StandardActionName.SaveCreate, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveCreate);
            }
            return jsonResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SEC006P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.DEPT_ID = TempModel.DEPT_ID;
                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        public ActionResult CustomsExport(SEC006P001Model model)
        {
            var da = new SEC006P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC006P001ExecuteType.GetQuerySearchAll;

            da.Select(da.DTO);

            if (da.DTO.Models != null)
            {
                ExportHelper.ExportExcel(Response, da.DTO.Models);
            }

            return Content("ExportExcel clicked");
        }

        public ActionResult ViewReport(SEC006P001Model model)
        {
            string ReportName = "J03FWL007R01";
            return Content("http://" + AppConfigHelper.ReportServerName + "/ReportServer?/" + AppConfigHelper.ReportFolderName + "/" + ReportName + "&rs:Command=Render&rs:Format=HTML4.0&rc:Parameters=false" + "&company_name=AutoAlliance%20(Thailand)%20Co.,Ltd.&user_id=vsmadmin&DateFromHead=21+%e0%b8%98%e0%b8%b1%e0%b8%99%e0%b8%a7%e0%b8%b2%e0%b8%84%e0%b8%a1+2558&DateToHead=25+%e0%b8%98%e0%b8%b1%e0%b8%99%e0%b8%a7%e0%b8%b2%e0%b8%84%e0%b8%a1+2558&error_code=0&WARE_HOUSE_FT=AAT&PE_HID=25&PE_NAME=2015-DEC%20PERIOD%204%20TO%20AAT&PE_START_DATE=2015-12-21&PE_END_DATE=2015-12-25");
        }
        #endregion

        #region Event
        public JsonResult Bind_ComForUser()
        {
            var da = new SEC006P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC006P001ExecuteType.GetUserCOM;

            da.DTO.Model.USER_ID = TempModel.USER_ID;

            da.SelectNoEF(da.DTO);

            return JsonAllowGet(da.DTO.Model.ComUserModel);
        }
        #endregion

        #region ====Private Mehtod====
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC006P001DA();

            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
              da.DTO,
              model,
              GetSaveLogConfig("dbo", "VSMS_USER", "USER_ID"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC006P001Model)model;
                SetStandardField(da.DTO.Model.ComUserModel);
                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC006P001Model)model;
                SetStandardField(da.DTO.Model.ComUserModel);
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SEC006P001Model();
                SetStandardField(da.DTO.Model);
                da.DTO.Models = (List<SEC006P001Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        private void SetDefaultData(string mode = "")
        {
            if (mode == StandardActionName.Index)
            {
                localModel.IS_DISABLED_MODEL = BindIS_DISABLED_MODEL();
                localModel.DEPT_ID_MODEL = BindDEPT_ID_MODEL();
                localModel.USG_ID_MODEL = BindUSG_ID_MODEL();
            }
            else if (mode == StandardActionName.Add || mode == StandardActionName.Edit)
            {
                localModel.IS_DISABLED_MODEL = BindIS_DISABLED_MODEL();
                localModel.DEPT_ID_MODEL = BindDEPT_ID_MODEL();
                localModel.USG_ID_MODEL = BindUSG_ID_MODEL_ADD();
                localModel.TITLE_ID_MODEL = BindTITLE_ID_MODEL();
                localModel.USER_STATUS_MODEL = BindUSER_STATUS_MODEL();
            }
        }

        private List<DDLCenterModel> BindIS_DISABLED_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_71));
        }

        private List<DDLCenterModel> BindDEPT_ID_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_DEPARTMENT_001, new VSMParameter(SessionHelper.SYS_COM_CODE));
        }

        private List<DDLCenterModel> BindUSG_ID_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_001);
        }
        private List<DDLCenterModel> BindUSG_ID_MODEL_ADD()
        {
            var da = new SEC006P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC006P001ExecuteType.GetQueryCheckUserAdmin;
            da.DTO.Model.USER_ID = SessionHelper.SYS_USER_ID;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.SelectNoEF(da.DTO);

            if (da.DTO.Models[0].USG_LEVEL.AsString() == "S")
            {
                return GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_001);
            }
            else
            {//
                return GetDDLCenter(DDLCenterKey.DD_VSMS_USRGROUP_002);
            }
        }

        private List<DDLCenterModel> BindTITLE_ID_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_TITLE_001, new VSMParameter(SessionHelper.SYS_COM_CODE));
        }

        private List<DDLCenterModel> BindUSER_STATUS_MODEL()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_89));
        }
        #endregion
    }
}
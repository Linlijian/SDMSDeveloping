using DataAccess;
using DataAccess.SEC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SEC000P001Controller : SECBaseController
    {
        #region Property
        private SEC000P001Model localModel = new SEC000P001Model();
        private SEC000P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC000P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC000P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC000P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC000P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC000P001Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }

        #endregion

        #region Action 
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);
            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch.CloneObject();
            }
            return View(StandardActionName.Index, localModel);
        }

        public ActionResult Search(SEC000P001Model model)
        {
            var da = new SEC000P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC000P001ExecuteType.GetAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;
            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models, da.DTO.Result);
        }

        [HttpPost]
        public ActionResult DeleteSearch(List<SEC000P001Model> data)
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

        [RuleSetForClientSideMessages("Add")]
        public ActionResult Add()
        {
            SetDefaulButton(StandardButtonMode.Create);
            SetDefaultData();
            return View(StandardActionName.Add, localModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SEC000P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                TempModel.COM_CODE = model.COM_CODE;
                model.COM_BRANCH = model.COM_BRANCH;
                model.COM_POST_CODE = model.COM_POST_CODE_E;   //เฟิวที่เก็บชื่อไม่เหมือนกัน 
                model.COM_FAC_POST = model.COM_FAC_POST_E;


                var result = SaveData(StandardActionName.SaveCreate, model);
                jsonResult = Success(result, StandardActionName.SaveCreate, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveCreate);
            }

            return jsonResult;
        }

        [RuleSetForClientSideMessages("Edit")]
        public ActionResult Edit(string COM_CODE, string COM_BRANCH, SEC000P001Model model)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SEC000P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC000P001ExecuteType.GetByID;
            TempModel.COM_CODE = da.DTO.Model.COM_CODE = COM_CODE.ToString();
            TempModel.COM_BRANCH = da.DTO.Model.COM_BRANCH = COM_BRANCH.ToString();
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                model.COM_POST_CODE_E = da.DTO.Model.COM_CODE;   //เอาค่ารหัสไปษณีออกมาเเสดง 
                model.COM_FAC_POST_E = da.DTO.Model.COM_CODE;
                localModel = da.DTO.Model;
            }
            SetDefaultData();   //set ค่า DDL


            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SEC000P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.COM_CODE = TempModel.COM_CODE;
                model.COM_BRANCH = TempModel.COM_BRANCH;

                //รหัสไปสณี 
                model.COM_POST_CODE = model.COM_POST_CODE_E;
                model.COM_FAC_POST = model.COM_FAC_POST_E;

                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        [HttpGet]
        //public ActionResult Info(decimal? TITLE_ID)
        //{
        //    SetDefaulButton(StandardButtonMode.View);
        //    var da = new SEC000P001DA();
        //    SetStandardErrorLog(da.DTO);
        //    da.DTO.Execute.ExecuteType = DTOExecuteType.GetByID;
        //    TempModel.TITLE_ID = da.DTO.Model.TITLE_ID = TITLE_ID;
        //    da.Select(da.DTO);
        //    if (da.DTO.Model != null)
        //    {
        //        localModel = da.DTO.Model;
        //    }
        //    return View(StandardActionName.Info, localModel);
        //}

        #endregion

        #region Mehtod  
        //----------------------- DDL-----------------------
        private void SetDefaultData(string mode = "")
        {
            localModel.COM_USE_LANGUAGE_MODEL = BindLANGUAGE();
            localModel.COM_PROVINCE_T_MODEL = BindPROVINCE_T();
            localModel.COM_PROVINCE_E_MODEL = BindPROVINCE_E();
            localModel.COM_FAC_PRV_T_MODEL = BindFAC_PRV_T();
            localModel.COM_FAC_PRV_E_MODEL = BindFAC_PRV_E();
        }

        private List<DDLCenterModel> BindLANGUAGE()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_80));
        }
        private List<DDLCenterModel> BindPROVINCE_T()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_TH_001);
        }
        private List<DDLCenterModel> BindPROVINCE_E()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_EN_001);
        }
        private List<DDLCenterModel> BindFAC_PRV_T()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_TH_001);
        }

        private List<DDLCenterModel> BindFAC_PRV_E()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_PROVINCE_EN_001);
        }

        //----------------------------------------------//
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC000P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_COMPANY", "COM_CODE"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC000P001Model)model;
                da.DTO.Model.COM_CODE = TempModel.COM_CODE;

                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC000P001Model)model;

                da.DTO.Model.COM_CODE = TempModel.COM_CODE;
                //da.DTO.Model.COM_BRANCH = TempModel.COM_BRANCH.TrimEnd();
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                //da.DTO.Model = new SEC000P001Model();
                //SetStandardField(da.DTO.Model);
                da.DTO.Models = (List<SEC000P001Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}
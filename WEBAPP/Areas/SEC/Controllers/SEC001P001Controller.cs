﻿using DataAccess;
using DataAccess.SEC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SEC001P001Controller : SECBaseController
    {
        #region Property
        private SEC001P001Model localModel = new SEC001P001Model();
        private SEC001P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC001P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC001P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC001P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC001P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC001P001Model;
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

        public ActionResult Search(SEC001P001Model model)
        {
            var da = new SEC001P001DA();
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
        public ActionResult DeleteSearch(List<SEC001P001Model> data)
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
            return View(StandardActionName.Add, localModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SEC001P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                TempModel.TBL_NAME = model.TBL_NAME;
                model.TBL_TYPE = model.TBL_TYPE;
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
        public ActionResult Edit(string TBL_NAME)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SEC001P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC001P001ExecuteType.GetByID;
            TempModel.TBL_NAME = da.DTO.Model.TBL_NAME = TBL_NAME.ToString();
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }

            SetDefaultData();
            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SEC001P001Model model)  //แก้ไข ถ้ามี มี Readonly รับจาก temp
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.TBL_NAME = TempModel.TBL_NAME;
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
        //    var da = new SEC001P001DA();
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

        //----------------------- DDL-----------------------
        private void SetDefaultData(string mode = "")
        {
            localModel.LOG_STATUS_MODEL = BindLOG_STATUS();
        }

        private List<DDLCenterModel> BindLOG_STATUS()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_81));
        }

        #region Mehtod
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC001P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_CTRLLOG", "TBL_NAME"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC001P001Model)model;
                da.DTO.Model.TBL_NAME = TempModel.TBL_NAME;

                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC001P001Model)model;

                da.DTO.Model.TBL_NAME = TempModel.TBL_NAME;

                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                //da.DTO.Model = new SEC001P001Model();
                //SetStandardField(da.DTO.Model);
                da.DTO.Models = (List<SEC001P001Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}
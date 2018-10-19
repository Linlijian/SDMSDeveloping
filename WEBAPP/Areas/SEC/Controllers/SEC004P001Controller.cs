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
    public class SEC004P001Controller : SECBaseController
    {
        #region Property
        private SEC004P001Model localModel = new SEC004P001Model();
        private SEC004P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC004P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC004P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC004P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC004P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC004P001Model;
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
            SetDefaultData();
            return View(StandardActionName.Index, localModel);
        }

        public ActionResult Search(SEC004P001Model model)
        {
            var da = new SEC004P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC004P001ExecuteType.GetAll;
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
        public ActionResult DeleteSearch(List<SEC004P001Model> data)
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

            var da = new SEC004P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC004P001ExecuteType.GetSeqMax;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }

            SetDefaultData();
            return View(StandardActionName.Add, localModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SEC004P001Model model)
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

        [RuleSetForClientSideMessages("Edit")]
        public ActionResult Edit(string SYS_CODE)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SEC004P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC004P001ExecuteType.GetByID;
            TempModel.SYS_CODE = da.DTO.Model.SYS_CODE = SYS_CODE;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
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
        public ActionResult SaveModify(SEC004P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.SYS_CODE = TempModel.SYS_CODE;
                model.COM_CODE = SessionHelper.SYS_COM_CODE;
                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        #endregion

        #region Mehtod
        private void SetDefaultData(string mode = "")
        {
            localModel.SYS_STATUS_MODEL = BindStatus();
        }

        private List<DDLCenterModel> BindStatus()
        {
            return GetDDLCenter(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_002, new VSMParameter(FIXOptionID.FixOpt_71));
        }

        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC004P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_SYSTEM", "ID"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC004P001Model)model;
                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC004P001Model)model;
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SEC004P001Model();
                SetStandardField(da.DTO.Model);
                da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
                da.DTO.Models = (List<SEC004P001Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}
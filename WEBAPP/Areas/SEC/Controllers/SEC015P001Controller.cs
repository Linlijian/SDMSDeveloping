using DataAccess;
using DataAccess.SEC;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SEC015P001Controller : SECBaseController
    {
        #region Property
        private SEC015P001Model localModel = new SEC015P001Model();
        private SEC015P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC015P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC015P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC015P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC015P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC015P001Model;
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

        public ActionResult Search(SEC015P001Model model)
        {
            var da = new SEC015P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC015P001ExecuteType.GetAll;
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
        public ActionResult DeleteSearch(List<SEC015P001Model> data)
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

        public ActionResult BindSystem()
        {
            var da = new SEC015P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC015P001ExecuteType.GetSystemDetail;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Model.NAME = TempModel.NAME;
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Model.SystemModels, da.DTO.Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SEC015P001Model model)
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
        public ActionResult Edit(string NAME)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SEC015P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC015P001ExecuteType.GetByID;
            TempModel.NAME = da.DTO.Model.NAME = NAME;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SEC015P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.NAME_Old = TempModel.NAME;
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

        [HttpGet]
        public ActionResult Info(decimal ID)
        {
            SetDefaulButton(StandardButtonMode.View);
            var da = new SEC015P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = DTOExecuteType.GetByID;
            TempModel.ID = da.DTO.Model.ID = ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Info, localModel);
        }

        #endregion

        #region Mehtod
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC015P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_CONFIG_GENERAL", "ID"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC015P001Model)model;
                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC015P001Model)model;
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SEC015P001Model();
                SetStandardField(da.DTO.Model);
                da.DTO.Models = (List<SEC015P001Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}
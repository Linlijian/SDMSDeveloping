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
    public class SEC002P001Controller : SECBaseController
    {
        #region Property
        private SEC002P001Model localModel = new SEC002P001Model();
        private SEC002P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC002P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC002P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC002P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC002P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC002P001Model;
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

        public ActionResult Search(SEC002P001Model model)
        {
            var da = new SEC002P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC002P001ExecuteType.GetAll;
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
        public ActionResult DeleteSearch(List<SEC002P001Model> data)
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
        public ActionResult SaveCreate(SEC002P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
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
        public ActionResult Edit(decimal? TITLE_ID)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SEC002P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC002P001ExecuteType.GetByID;
            TempModel.TITLE_ID = da.DTO.Model.TITLE_ID = TITLE_ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SEC002P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.TITLE_ID = TempModel.TITLE_ID;
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
        public ActionResult Info(decimal? TITLE_ID)
        {
            SetDefaulButton(StandardButtonMode.View);
            var da = new SEC002P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC002P001ExecuteType.GetByID;
            TempModel.TITLE_ID = da.DTO.Model.TITLE_ID = TITLE_ID;
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
            var da = new SEC002P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_TITLE", "TITLE_ID"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC002P001Model)model;
                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC002P001Model)model;
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SEC002P001Model();
                SetStandardField(da.DTO.Model);
                da.DTO.Models = (List<SEC002P001Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}
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
    public class SEC003P001Controller : SECBaseController
    {
        #region Property
        private SEC003P001Model localModel = new SEC003P001Model();
        private SEC003P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC003P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC003P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC003P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC003P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC003P001Model;
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
            var ddl = new DataAccess.DDLCenterDA();
            ddl.DTO.Execute.ExecuteType = "GetAll";
            ddl.Select(ddl.DTO);
            //SessionHelper.FileModel = new FileModel
            //{
            //    Name = "FileAttached",
            //};

            //GetDDLCenter(DDLCenterKey.COMPANY);
            //GetDDLFixoption(DDLCenterKey.DD_VSMS_FIX_OPTIONDETAIL_001, FIXOptionID.FixOpt_10001));

            return View(StandardActionName.Index, localModel);
        }

        public ActionResult Search(SEC003P001Model model)
        {
            var da = new SEC003P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC003P001ExecuteType.GetAll;
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
        public ActionResult DeleteSearch(List<SEC003P001Model> data)
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
        public ActionResult SaveCreate(SEC003P001Model model)
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
        public ActionResult Edit(decimal? DEPT_ID)
        {
            SetDefaulButton(StandardButtonMode.Modify);

            var da = new SEC003P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC003P001ExecuteType.GetByID;
            TempModel.DEPT_ID = da.DTO.Model.DEPT_ID = DEPT_ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Edit, localModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(SEC003P001Model model)
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

        [HttpGet]
        public ActionResult Info(decimal? DEPT_ID)
        {
            SetDefaulButton(StandardButtonMode.View);
            var da = new SEC003P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = DTOExecuteType.GetByID;
            TempModel.DEPT_ID = da.DTO.Model.DEPT_ID = DEPT_ID;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Info, localModel);
        }

        #endregion

        #region ====Private Mehtod====
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC003P001DA();

            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
              da.DTO,
              model,
              GetSaveLogConfig("dbo", "VSMS_DEPARTMENT", "DEPT_ID"));


            if (mode == StandardActionName.SaveCreate)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC003P001Model)model;
                da.Insert(da.DTO);
            }
            else if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC003P001Model)model;
                da.Update(da.DTO);
            }
            else if (mode == StandardActionName.Delete)
            {
                da.DTO.Model = new SEC003P001Model();
                SetStandardField(da.DTO.Model);
                da.DTO.Models = (List<SEC003P001Model>)model;
                da.Delete(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}
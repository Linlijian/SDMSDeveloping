using System.Collections.Generic;
using System.Web.Mvc;
using DataAccess;
using DataAccess.SEC;
using WEBAPP.Helper;
using UtilityLib;

namespace WEBAPP.Areas.SEC.Controllers
{
    public class SEC008P001Controller : SECBaseController
    {
        #region Property
        private SEC008P001Model localModel = new SEC008P001Model();
        private SEC008P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC008P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC008P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC008P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC008P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC008P001Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }
        #endregion

        #region View
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Index);

            RemoveStandardButton(StandardButtonName.Add);
            RemoveStandardButton(StandardButtonName.DeleteSearch);

            if (TempSearch.IsDefaultSearch && !Request.GetRequest("page").IsNullOrEmpty())
            {
                localModel = TempSearch;
            }
            localModel.COM_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_COMPANY_001);
            localModel.SYS_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_001);
            return View(StandardActionName.Index, localModel);
        }

        [HttpGet]
        public ActionResult Edit(string COM_CODE, string SYS_CODE)
        {
            SetDefaulButton(StandardButtonMode.Modify);
            localModel = new SEC008P001Model();
            TempModel.COM_CODE = localModel.COM_CODE = COM_CODE;
            TempModel.SYS_CODE = localModel.SYS_CODE = SYS_CODE;
            localModel.COM_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_COMPANY_001);
            localModel.SYS_CODE_MODEL = GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_001, new VSMParameter(COM_CODE));
            return View(StandardActionName.Edit, localModel);
        }
        #endregion

        #region Action
        public ActionResult Search(SEC008P001Model model)
        {
            var da = new SEC008P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC008P001ExecuteType.GetAll;
            if (Request.GetRequest("page").IsNullOrEmpty())
            {
                model.IsDefaultSearch = true;
                TempSearch = model;
            }
            da.DTO.Model = TempSearch;
            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveModify(List<SEC008P001Model> SysPrg)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                var result = SaveData(StandardActionName.SaveModify, SysPrg);
                jsonResult = Success(result, StandardActionName.SaveModify, Url.Action(StandardActionName.Index, new { page = 1 }));
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }
        #endregion

        #region Event
        public ActionResult GetSystem(string COM_CODE)
        {
            return JsonAllowGet(GetDDLCenter(DDLCenterKey.DD_VSMS_SYSTEM_001, new VSMParameter(COM_CODE)));
        }
        public ActionResult GetProgram()
        {
            var da = new SEC008P001DA();
            da.DTO.Execute.ExecuteType = SEC008P001ExecuteType.GetProgram;
            da.DTO.Model = TempModel.CloneObject();
            da.SelectNoEF(da.DTO);
            return JsonAllowGet(da.DTO.Models);
        }
        public ActionResult GetSysPrg()
        {
            var da = new SEC008P001DA();
            da.DTO.Execute.ExecuteType = SEC008P001ExecuteType.GetSysPrg;
            da.DTO.Model = TempModel.CloneObject();
            da.Select(da.DTO);
            return JsonAllowGet(da.DTO.Models);
        }
        #endregion

        #region Method
        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC008P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo", "VSMS_SYS_PGC", "COM_CODE", "SYS_CODE"));

            if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Models = (List<SEC008P001Model>)model;
                da.DTO.Model.COM_CODE = TempModel.COM_CODE;
                da.DTO.Model.SYS_CODE = TempModel.SYS_CODE;
                da.Update(da.DTO);
            }
            return da.DTO.Result;
        }
        #endregion
    }
}
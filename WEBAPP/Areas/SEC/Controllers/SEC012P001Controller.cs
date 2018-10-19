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
    public class SEC012P001Controller : SECBaseController
    {
        #region Property

        private SEC012P001Model localModel = new SEC012P001Model();
        private SEC012P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC012P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC012P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }

        #endregion

        #region Action

        [RuleSetForClientSideMessages("Index")]
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Create);

            var da = new SEC012P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SEC012P001ExecuteType.GetAll;
            da.Select(da.DTO);
            if (da.DTO.Model != null)
            {
                da.DTO.Model.FTP_SERVER = Encryption.Decrypt(da.DTO.Model.FTP_SERVER.FromBase64String());
                da.DTO.Model.FTP_PRT = Encryption.Decrypt(da.DTO.Model.FTP_PRT.FromBase64String());
                if (!string.IsNullOrEmpty(da.DTO.Model.FTP_FOLDER))
                {
                    da.DTO.Model.FTP_FOLDER = Encryption.Decrypt(da.DTO.Model.FTP_FOLDER.FromBase64String());
                }
                da.DTO.Model.USER_NAME = Encryption.Decrypt(da.DTO.Model.USER_NAME.FromBase64String());
                da.DTO.Model.PASSWORD = Encryption.Decrypt(da.DTO.Model.PASSWORD.FromBase64String());

                localModel = da.DTO.Model;
            }
            return View(StandardActionName.Index, localModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCreate(SEC012P001Model model)
        {
            var jsonResult = new JsonResult();
            if (ModelState.IsValid)
            {
                model.FTP_SERVER = Encryption.Encrypt(model.FTP_SERVER).ToBase64String();
                model.FTP_PRT = Encryption.Encrypt(model.FTP_PRT).ToBase64String();
                if (!string.IsNullOrEmpty(model.FTP_FOLDER))
                {
                    model.FTP_FOLDER = Encryption.Encrypt(model.FTP_FOLDER).ToBase64String();
                }
                model.USER_NAME = Encryption.Encrypt(model.USER_NAME).ToBase64String();
                model.PASSWORD = Encryption.Encrypt(model.PASSWORD).ToBase64String();

                List<SEC012P001ModelDetail> res = new List<SEC012P001ModelDetail>()
                {
                    new SEC012P001ModelDetail() {NAME = "FTP_SERVER", STR_VALUE = model.FTP_SERVER },
                    new SEC012P001ModelDetail() {NAME = "FTP_PORT", STR_VALUE = model.FTP_PRT },
                    new SEC012P001ModelDetail() {NAME = "FTP_FOLDER", STR_VALUE = model.FTP_FOLDER },
                    new SEC012P001ModelDetail() {NAME = "FTP_USER_NAME", STR_VALUE = model.USER_NAME },
                    new SEC012P001ModelDetail() {NAME = "FTP_PASSWORD", STR_VALUE = model.PASSWORD },
                };
                model.SEC012P001S = res;

                var url = Url.Action("Index", "Default", new { Area = "Admin" });
                var result = SaveData(StandardActionName.SaveModify, model);
                jsonResult = Success(result, StandardActionName.SaveModify, url);
            }
            else
            {
                jsonResult = ValidateError(ModelState, StandardActionName.SaveModify);
            }
            return jsonResult;
        }

        #endregion

        #region Mehtod

        private DTOResult SaveData(string mode, object model)
        {
            var da = new SEC012P001DA();
            //ในกรณีที่มีการ SaveLog ให้ Include SetStandardLog ด้วย
            SetStandardLog(
               da.DTO,
               model,
               GetSaveLogConfig("dbo","VSMS_CONFIG_FTP", "NAME"));

            if (mode == StandardActionName.SaveModify)
            {
                SetStandardField(model);
                da.DTO.Model = (SEC012P001Model)model;
                da.Insert(da.DTO);
            }
            return da.DTO.Result;
        }

        #endregion
    }
}
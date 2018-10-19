using DataAccess;
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
    public class SEC014P001Controller : SECBaseController
    {
        #region Property
        private SEC014P001Model localModel = new SEC014P001Model();
        private SEC014P001Model TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new SEC014P001Model();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as SEC014P001Model;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private SEC014P001Model TempSearch
        {
            get
            {
                if (TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = new SEC014P001Model();
                }
                TempData.Keep(StandardActionName.Search + SessionHelper.SYS_CurrentAreaController);

                return TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] as SEC014P001Model;
            }
            set { TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = value; }
        }

        #endregion

        #region View
        [RuleSetForClientSideMessages("Index")]
        public ActionResult Index()
        {
            SetDefaulButton(StandardButtonMode.Other);
            AddButton(StandButtonType.ButtonAjax, "btnSearch", Translation.CenterLang.Center.Search, iconCssClass: FaIcons.FaSearch, url: Url.Action("Search"), isValidate: true);

            return View(StandardActionName.Index, localModel);
        }
        #endregion

        #region Action 
        public ActionResult Search(SEC014P001Model model)
        {
            if (ValidateCommand(model))
            {
                var da = new SEC014P001DA();
                SetStandardErrorLog(da.DTO);
                da.DTO.Execute.ExecuteType = SEC014P001ExecuteType.GetAll;
                da.DTO.Model = model;
                da.SelectNoEF(da.DTO);

                if (da.DTO.Result.ActionResult > -1)
                {
                    if (da.DTO.Model.RECORD_COUNT != null)
                    {
                        return Success(da.DTO.Result, new ResultOptions { Mode = "Query", SuccessMessage = "Sucess: " + da.DTO.Model.RECORD_COUNT + " row(s) affected" });
                    }
                    else
                    {
                        return JsonAllowGet(da.DTO.Model);
                    }
                }
                else
                {
                    return Success(da.DTO.Result, "Query");
                }
            }
            else
            {
                return Json(new WEBAPP.Models.AjaxResult("Query", false, AlertStyles.Error, "Can't execute this command : " + model.SQL_COMMAND));
            }

        }
        #endregion

        #region Event
        #endregion

        #region ====Private Mehtod====
        private bool ValidateCommand(SEC014P001Model model)
        {
            bool result = true;
            string[] strExp = { "INSERT", "UPDATE", "DELETE", "TRUNCATE" };
            char[] strSparater = { ' ' };
            string[] strComm = model.SQL_COMMAND.Split(strSparater);
            for (int i = 0; i < strComm.Length; i++)
            {
                for (int j = 0; j < strExp.Length; j++)
                {
                    if (strComm[i].Trim().ToUpper().Equals(strExp[j]))
                    {
                        result = false;
                    }
                    if (!result)
                    {
                        break;
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
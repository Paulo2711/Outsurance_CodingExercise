using DataProcessing.Engine;
using Outsurance.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Outsurance.Web.Controllers
{
    public class HomeController : Controller
    {
        private const string defaultControl = "file";
        private const string defaultView = "~/Views/Home/Index.cshtml";
        private const string defaultErrorMessage = "An unexpected error occurred !";

        public ActionResult Index()
        {
            var model = new FileModel();
            return View(model);
        }

        public void OpenOutPutFolder(string PathToOutPutFolder)
        {
            //This path would be a network path if this app was hosted on a server so it would still work
            if (!string.IsNullOrEmpty(PathToOutPutFolder))
                Process.Start(PathToOutPutFolder);
        }
        internal ActionResult HandleError(string controlId, string errorMessage, string view, FileModel model)
        {
            ModelState.AddModelError(controlId ?? defaultControl, errorMessage ?? defaultErrorMessage);
            ViewData["TempErrorMessage"] = errorMessage ?? defaultErrorMessage;
            return View(view ?? defaultView, model);
        }

        [HttpPost]
        public ActionResult ProcessFile(FileModel model)
        {
            if (!ModelState.IsValid)
                return HandleError("file", "Please upload a valid import file !", "~/Views/Home/Index.cshtml", model);

            try
            {

                //Now we can attempt to process the file
                ProcessFile processor = new ProcessFile();
                DataProcessResult DPR = processor.MainProcess(model.file.FileName);
                model.ProcessingComplete = DPR.ProcessSuccess;

                if (DPR.ProcessSuccess.Equals(false))
                {
                    return HandleError("file", $"Unexpected Error : {DPR.ErrorMessage}", "~/Views/Home/Index.cshtml", model);

                }

            }
            //Should the try catch(s) be removed, the system will redirect to a generic error page set in the global.asax file

            catch (Exception ex)
            {
                return HandleError("file", $"Unexpected Error : {ex.Message}", "~/Views/Home/Index.cshtml", model);
            }

            ViewData["TempSuccessMessage"] = "File processed successfully";
            return View("~/Views/Home/Index.cshtml", model);
        }

    }
}
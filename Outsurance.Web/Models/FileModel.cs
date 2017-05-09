using DataProcessing.Helpers;
using Outsurance.Web.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsurance.Web.Models
{
    public class FileModel
    {
        [ImportFileUploadValidation(ErrorMessage = "Please select a data import file")]
        public HttpPostedFileBase file { get; set; }

        public string PathToOutPutFolder { get; set; } = ConfigAppSettings.OutputFileLocation;
        public bool ProcessingComplete { get; set; } = false;

    }
}
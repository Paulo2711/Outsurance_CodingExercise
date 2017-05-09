using DataProcessing.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;


namespace Outsurance.Web.CustomAttributes
{
    public class ImportFileUploadValidationAttribute : RequiredAttribute
    {
        //This custom attribute will validate that the correct file (.xml) was uploaded
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            var ext = Path.GetExtension(file?.FileName ?? string.Empty);
            if (file == null || string.IsNullOrEmpty(ext) || ext.ToLower() != ConfigAppSettings.AllowedFileUploadExtension || file.ContentLength == 0)
                return false;
            return true;
        }
    }

}
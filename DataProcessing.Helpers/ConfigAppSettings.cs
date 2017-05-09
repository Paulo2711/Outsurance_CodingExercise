using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.Helpers
{
    public class ConfigAppSettings
    {

        public static readonly int ProcessRetryCountLimit = int.Parse(ConfigurationManager.AppSettings["ProcessRetryCountLimit"]?.ToString() ?? "0");
        public static readonly string AllowedFileUploadExtension = ConfigurationManager.AppSettings["AllowedFileUploadExtension"]?.ToString() ?? string.Empty;
        public static readonly string OutputFileLocation_Person = ConfigurationManager.AppSettings["OutputFileLocation_Person"]?.ToString() ?? @"C:\OutsuranceData\Output_PersonSorted.csv";
        public static readonly string OutputFileLocation_Address = ConfigurationManager.AppSettings["OutputFileLocation_Address"]?.ToString() ?? @"C:\OutsuranceData\Output_AddressSorted.csv";
    }

}

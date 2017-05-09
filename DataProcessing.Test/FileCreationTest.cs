using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataProcessing.Engine;
using TinyCsvParser;
using System.Linq;
using System.IO;
using DataProcessing.Helpers;
using static DataProcessing.Engine.ProcessFile;

namespace DataProcessing.Test
{
    /// <summary>
    /// Summary description for DataProcessing.Engine testing
    /// </summary>
    [TestClass]
    public class FileCreationTest
    {

        ProcessFile DPE;
        DataProcessResult DPR;

        public FileCreationTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }



        [TestMethod]
        public void MainProcess_if_file_is_empty()
        {
            string FilePath = string.Empty;

            DPR = new DataProcessResult { ProcessResponse = "Error Processing File" };
            DPE = new ProcessFile();

            DataProcessResult ActualOutput = DPE.MainProcess(FilePath);

            Assert.AreEqual(ActualOutput.ProcessResponse, DPR.ProcessResponse);

        }

        [TestMethod]
        public void MainProcess_Directory_not_found()
        {
            string FilePath = @"c:\SomeDirectory\file.csv";

            DPR = new DataProcessResult { ProcessResponse = "Directory Not Found" };
            DPE = new ProcessFile();

            DataProcessResult ActualOutput = DPE.MainProcess(FilePath);

            Assert.AreEqual(ActualOutput.ProcessResponse, DPR.ProcessResponse);

        }

        [TestMethod]
        public void MainProcess_if_file_invalid()
        {
            bool allowedFileExt = true;
            bool invalidfileExt = false;

            //Change file type to csv to test the config settings
            string FilePath = @"c:\file.xml";
            var ext = Path.GetExtension(FilePath);
            if (FilePath == null || string.IsNullOrEmpty(ext) || ext.ToLower() != ConfigAppSettings.AllowedFileUploadExtension)
                allowedFileExt = false;


            Assert.AreEqual(allowedFileExt, invalidfileExt);

        }

        //This test will pass if the default directory does not exist and no valid directory specified in web.config
        [TestMethod]
       public void WriteCSV_Person()
        {

            OutputProcessResult OPR = new OutputProcessResult();
            OPR.ProcessSuccess = true;

            List<SortedPerson> LSP = new List<SortedPerson>();
            SortedPerson SP = new SortedPerson
            {
                Firstname = "John",
                Frequency = 7
            };

            LSP.Add(SP);
            DPE = new ProcessFile();
            OutputProcessResult Res= DPE.WriteCSV_Person(LSP);

            Assert.AreEqual(Res.ProcessSuccess, OPR.ProcessSuccess);

        }

    }
}

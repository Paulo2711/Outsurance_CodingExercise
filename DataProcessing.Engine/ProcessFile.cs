using CsvHelper;
using DataProcessing.Engine.Interfaces;
using DataProcessing.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TinyCsvParser;

namespace DataProcessing.Engine
{
    public class ProcessFile : IProcessFile
    {
        private string _absolutePath { get; set; }

        public class SortedPerson
        {
            public string Firstname { get; set; }
            public int Frequency { get; set; }
        }

#region Const_Errors
        const string ProcessError = "Error Processing File";
        const string ProcessComplete = "Processing Successful";
        const string DirectoryMessage = "Directory Not Found";
        const string SortingError = "Unable to sort collection";
        const string OutputError = "Unable to save file";
#endregion


        //////<summary>
        ////// Entry point into the file processing engine
        //////</summary>
        ////// <paramref name="absolutePath">physical path of the csv file</paramref>
        ////// <returns>Data Process result object this will define process success or failures</returns>
        public DataProcessResult MainProcess(string absolutePath)
        {
            DataProcessResult DPR = new DataProcessResult();

            try
            {

                //Read the data from the file into a Person List
                _absolutePath = absolutePath;
                List<Person> P = new List<Person>(ReadMe());
                
                //Sort the Person list and write to a csv file
                List<SortedPerson> SP = SortFrequency(P);
                WriteCSV_Person(SP);

                //Sort the Address list and write to a csv file
                List<AddressValues> AV = AddressSort();
                WriteCSV_Address(AV);
            }
            catch (DirectoryNotFoundException e)
            {
                DPR.ProcessSuccess = false;
                DPR.ProcessResponse = DirectoryMessage;
                DPR.ErrorMessage = e.Message;

                return DPR;
            }

            catch (Exception ex)
            {
                return new DataProcessResult { ProcessSuccess = false, ProcessResponse = ProcessError, ErrorMessage = ex.Message };
            }

            DPR.ProcessSuccess = true;
            DPR.ProcessResponse = ProcessComplete;
            return DPR;

        }

        //////<summary>
        ////// Uses absolute path location to read the file
        //////</summary>
        ////// <returns>Person collection which is unsorted</returns>
        public List<Person> ReadMe()
        {

            StreamReader sr = new StreamReader(_absolutePath);
            try
            {

                CsvReader csvread = new CsvReader(sr);
                csvread.Configuration.HasHeaderRecord = true;

                List<Person> UnsortedPersons = new List<Person>();

                //csvread will fetch all record in one go to the IEnumerable object record
                IEnumerable<Person> record = csvread.GetRecords<Person>();

                foreach (var rec in record)
                {
                    UnsortedPersons.Add(new Person()
                    {
                        FirstName = rec.FirstName,
                        LastName = rec.LastName,
                        Address = rec.Address,
                        PhoneNumber = rec.PhoneNumber

                    });
                }

                return UnsortedPersons;

            }
            catch (IOException ex)
            {
                throw new Exception(string.Format("{0}:{1}", ProcessError, ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}:{1}", ProcessError, ex.Message));
            }
            finally
            {
                sr.Close();
            }

        }

        //////<summary>
        ////// Show the frequency of the first and last names ordered by frequency descending and then alphabetically ascending. 
        //////</summary>
        ////// <paramref name="List<Person>"> List of object Person</paramref>
        ////// <returns>Person collection which is sorted</returns>
        private List<SortedPerson> SortFrequency(List<Person> P)
        {


            StringBuilder s = new StringBuilder();
            List<SortedPerson> SP = new List<SortedPerson>();

            try
            {
                //Use LINQ - to sort through the names 
                var q = from x in P
                        group x by x.FirstName into g
                        let count = g.Count()
                        orderby count descending, g.First().FirstName ascending
                        select new { Firstname = g.Key, Count = count };

                foreach (var x in q)
                {
                    SP.Add(new SortedPerson()
                    {
                        Firstname = x.Firstname,
                        Frequency = x.Count
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}:{1}", SortingError, ex.Message));
            }

            return SP;
        }

        public class AddressValues
        {

            public string AddressNumber { get; set; }
            public string AddressStreet { get; set; }


            public static AddressValues FromCsv(string csvLine)
            {
                string[] values = csvLine.Split(',');
                AddressValues Address = new AddressValues();

                //Using Regular expressions to split number and street name
                var regex = Regex.Match(values[2], @"(\d+\s)([a-zA-Z]+\s[a-zA-Z]+)");

                Address.AddressNumber = regex.Groups[1].Value;
                Address.AddressStreet = regex.Groups[2].Value;

                return Address;
            }
        }


        //////<summary>
        ////// show the addresses sorted alphabetically by street name.
        //////</summary>
        ////// <returns>Address collection which is sorted</returns>
        public List<AddressValues> AddressSort()
        {
          try
            { 
            List<AddressValues> values = File.ReadAllLines(_absolutePath)
                             .Skip(1)
                             .Select(v => AddressValues.FromCsv(v))
                             .ToList();

            return values.OrderBy(x => x.AddressStreet).ToList();

            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("{0}:{1}", SortingError, ex.Message));
            }
        }

        //////<summary>
        ////// writes the sorted list of Person to file location specified 
        //////</summary>
        public OutputProcessResult WriteCSV_Person(List<SortedPerson> SP)
        {

            //use the CSVHelper librbary to output the file
            StreamWriter write = new StreamWriter(ConfigAppSettings.OutputFileLocation_Person);
            OutputProcessResult OPR = new OutputProcessResult();

            try
            {
                //Csv writer stream
                CsvWriter csw = new CsvWriter(write);

            
            foreach (var rec in SP) // Each record will be fetched 
            {
                //write each record to file
                csw.WriteRecord<SortedPerson>(rec);
            }


                OPR.ProcessSuccess = true;
                OPR.ProcessResponse = ProcessComplete;
                return OPR;

            }
            catch(Exception ex)
            {
                OPR.ProcessSuccess = false;
                OPR.ProcessResponse = OutputError;
                OPR.ErrorMessage = ex.Message;
                return OPR;
            }
            finally
            { 

            write.Close();
            }

        }

        //////<summary>
        ////// writes the sorted list of Address to file location specified 
        //////</summary>
        public OutputProcessResult WriteCSV_Address(List<AddressValues> AV)
        {
                        
            //use the CSVHelper librbary to output the file
            StreamWriter write = new StreamWriter(ConfigAppSettings.OutputFileLocation_Address);
            OutputProcessResult OPR = new OutputProcessResult();

            try
            {
                //Csv writer stream
                CsvWriter csw = new CsvWriter(write);

                foreach (var rec in AV) // Each record will be fetched 
                {
                    ///write each record to file
                    csw.WriteRecord<AddressValues>(rec);
                }

                OPR.ProcessSuccess = true;
                OPR.ProcessResponse = ProcessComplete;
                return OPR;

            }
            catch (Exception ex)
            {
                OPR.ProcessSuccess = false;
                OPR.ProcessResponse = OutputError;
                OPR.ErrorMessage = ex.Message;
                return OPR;
            }
            finally
            {
                write.Close();
            }


        }



    }
}
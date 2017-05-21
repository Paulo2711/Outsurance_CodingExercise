using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataProcessing.Engine;
using System.Collections.Generic;
using static DataProcessing.Engine.ProcessFile;
using System.Text.RegularExpressions;

namespace DataProcessing.Test
{
    [TestClass]
    public class BusinessLogicTest
    {
        private List<Person> ValidInputCollection;
        private List<Person> InvalidInputCollection;
        private List<AddressValues> ValidAddressCollection;
       

        private List<Person> CreateValidInputCollection()
        {
            //Create a list of Person with valid data
            ValidInputCollection = new List<Person>();
            ValidInputCollection.Add(new Person() { FirstName = "Jimmy", LastName = "Smith", Address = "12 Long Lane", PhoneNumber = "29384857" });
            ValidInputCollection.Add(new Person() { FirstName = "Clive", LastName = "Owen", Address = "23 Ambling Way", PhoneNumber = "31214788" });
            ValidInputCollection.Add(new Person() { FirstName = "James", LastName = "Brown", Address = "45 Stewart St", PhoneNumber = "32114566" });
            ValidInputCollection.Add(new Person() { FirstName = "James", LastName = "Green", Address = "66 Lawn St", PhoneNumber = "32156789" });

            return ValidInputCollection;
        }


        private List<Person> CreateInvalidInputCollection()
        {
            //Create a list of Person with invalid data
            InvalidInputCollection = new List<Person>();
            InvalidInputCollection.Add(new Person() { FirstName = "1234", LastName = "Smith", Address = "102LongLane", PhoneNumber = "29384857" });
            InvalidInputCollection.Add(new Person() { FirstName = "Clive", LastName = "", Address = "65 Ambling Way", PhoneNumber = "31214788" });
            InvalidInputCollection.Add(new Person() { FirstName = "James", LastName = "Brown", Address = "82 Stewart St", PhoneNumber = "32114566" });

            return InvalidInputCollection;
        }

        private List<AddressValues> CreateValidAddressCollection()
        {
            //Create a list of Person with invalid data
            ValidAddressCollection = new List<AddressValues>();
            ValidAddressCollection.Add(new AddressValues() { AddressNumber = "102", AddressStreet = "LongLane" });
            ValidAddressCollection.Add(new AddressValues() { AddressNumber = "65", AddressStreet = "Ambling Way" });
            ValidAddressCollection.Add(new AddressValues() { AddressNumber = "82", AddressStreet = "Stewart St" });

            return ValidAddressCollection;
        }

        [TestMethod]
        public void SortFrequency_for_frequency_count_if_input_valid()
        {
            //Verify the SortFrequency method in the Business Logic returns data frequency of the first and last names ordered by frequency descending and then alphabetically ascending
            List<Person> TestList = CreateValidInputCollection();
            int AssertIndex = 2;
            int TestFrequency=0;

            List <SortedPerson> SortedValidList= new ProcessFile().SortFrequency(TestList);

            //Alphabetically sorted, James will be first, select James' frequency using LINQ
            int index = SortedValidList.FindIndex(f => f.Firstname == "James");
            if(index >= 0 )
            {
                 TestFrequency = SortedValidList[index].Frequency;
            }

            Assert.AreEqual(TestFrequency, AssertIndex);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Validation on input failed - Input value can not be null or empty")]
        public void SortFrequency_for_frequency_count_if_input_invalid()
        {

            //Verify the SortFrequency method and its validation
            //using invalid data
            List<Person> TestList = CreateInvalidInputCollection();

            List<SortedPerson> SortedValidList = new ProcessFile().SortFrequency(TestList);
        }

        [TestMethod]
        public void SortFrequency_for_alphabetic_order_if_input_valid()
        {
            //Verify the SortFrequency method in the Business Logic returns data frequency of the first and last names ordered by frequency descending and then alphabetically ascending
            List<Person> TestList = CreateValidInputCollection();
           
            List<SortedPerson> SortedValidList = new ProcessFile().SortFrequency(TestList);

            //Index starts at 0, so ensure Clive is 2nd ie index of 1
            int AssertIndex = 1;
            int index = SortedValidList.FindIndex(f => f.Firstname == "Clive");
           
            Assert.AreEqual(AssertIndex, index);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Validation on input failed - Input value can not be null or empty")]
        public void SortFrequency_for_alphabetic_order_if_input_invalid()
        {

            //Verify the SortFrequency method and its validation
            //using invalid data
            List<Person> TestList = CreateInvalidInputCollection();

            List<SortedPerson> SortedValidList = new ProcessFile().SortFrequency(TestList);
        }


        [TestMethod]
        public void Address_Sorting_with_valid_input()
        {
            //Verify the address is sorted alphabetically by streetname
            List<AddressValues> TestList = CreateValidAddressCollection();

            List<AddressValues> AddressList=new ProcessFile().AddressSort(TestList);
         
            //new ProcessFile().AddressSort(AddressList);
            int index = AddressList.FindIndex(f => f.AddressStreet == "Ambling Way");

            //Ambling way should be first in the index
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void Address_split_from_person()
        {
            //Ensure the Regular Expression splits up the number and the street name
            List<Person> TestList = CreateValidInputCollection();
            List<AddressValues> AddressList = new List<AddressValues>();
            bool AddressMatch = false;
            Regex RegExpression;

            foreach (Person av in TestList)
            {
                AddressList.Add(AddressValues.FromCsv(string.Format("{0},{1},{2},{3}", av.FirstName, av.LastName, av.Address, av.PhoneNumber)));
            }

           //Test if street name contains only a-z 
            RegExpression = new Regex(@"^[a-zA-Z\s]*$");

            foreach(AddressValues av in AddressList)
            {
                if (RegExpression.IsMatch(av.AddressStreet))
                {
                    AddressMatch = true;
                }
                else
                {
                    AddressMatch = false;
                }
            }

            //Test if street number contains only numeric characters
            RegExpression = new Regex(@"^\d+$");

            foreach (AddressValues av in AddressList)
            {
                if (RegExpression.IsMatch(av.AddressNumber.Trim()))
                {
                    AddressMatch = true;
                }
                else
                {
                    AddressMatch = false;
                }
            }

            Assert.AreEqual(AddressMatch, true);

        }
    }
}

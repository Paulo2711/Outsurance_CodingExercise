using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataProcessing.Engine
{
    public static class Validation
    {
        public static bool ValidateStringInput(string InputString)
        {
            bool isMatch = false;

            //Check that the string contains only Alpha characters
            Regex RegExpression = new Regex("^[a-zA-Z]*$");

            if (string.IsNullOrEmpty(InputString))
            {
                throw new ArgumentException("Validation on input failed - Input value can not be null or empty");
            }

            if(RegExpression.IsMatch(InputString))
            {
                isMatch = true;
            }

            return isMatch;

        }

        //This is to demonstrate partial validation of alpha characters
        //Valid telephone numbers should also be matched against "^[0-9]*$" with max length of 10 characters
        public static bool ValidateCollection (List<Person> PersonList)
        {
             bool isMatch = false;

            foreach(Person p in PersonList)
            {
                isMatch = ValidateStringInput(p.FirstName);
                isMatch = ValidateStringInput(p.LastName);
            }

            return isMatch;

        }
    }
}

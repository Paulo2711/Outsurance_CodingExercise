using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessing.Engine;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            ProcessFile pf = new DataProcessing.Engine.ProcessFile();
            pf.MainProcess();

        }
    }
}

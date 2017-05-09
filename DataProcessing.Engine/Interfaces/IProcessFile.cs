using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.Engine.Interfaces
{
    public interface IProcessFile
    {
        DataProcessResult MainProcess(string absolutePath);

    }
}

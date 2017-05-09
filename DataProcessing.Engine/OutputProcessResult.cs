using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.Engine
{
    public class OutputProcessResult
    {
        public bool ProcessSuccess { get; set; }
        public string ProcessResponse { get; set; }

        public string ErrorMessage { get; set; }

    }
}

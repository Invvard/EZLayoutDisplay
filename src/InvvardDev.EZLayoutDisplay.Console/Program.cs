using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvvardDev.EZLayoutDisplay.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeProcess();
        }

        private static void InitializeProcess()
        {
            var process = new KeyDefinitionProcessor();
            process.StartProcess();
        }
    }
}

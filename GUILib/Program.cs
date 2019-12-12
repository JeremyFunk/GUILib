using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiLib
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineCore core = new EngineCore(1920, 1080, "Hallo Welt");

            core.Run(144, 144);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrySharpSnmp
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            Console.WriteLine("Hit ENTER to continue");
            Console.ReadLine();
        }

        static void Run()
        {
            SnmpWorker worker = new SnmpWorker();
            worker.IPAddr = "192.168.10.6";
            worker.PortNum = 161;
            worker.CommunityName = "public";
            worker.TimeoutMsec = 60000;

            worker.Get(".1.3.6.1.2.1.1.1.0");
        }
    }
}

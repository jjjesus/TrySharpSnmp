using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrySharpSnmp
{
    public class SnmpWorker
    {
        public string IPAddr { get; set; }
        public int PortNum { get; set; }
        public string CommunityName { get; set; }
        public int TimeoutMsec { get; set; }

        public void Get(string oid)
        {
            IList<Variable> results = Messenger.Get(
                VersionCode.V2,
                new IPEndPoint(IPAddress.Parse(this.IPAddr), this.PortNum),
                new OctetString(this.CommunityName),
                new List<Variable> {
                    new Variable(
                        new ObjectIdentifier(oid))
                },
                this.TimeoutMsec);
            DumpResults(results);
        }

        private void DumpResults(IList<Variable> results)
        {
            foreach (Variable result in results)
            {
                Console.WriteLine(result.Data.ToString());
            }
        }

    }
}

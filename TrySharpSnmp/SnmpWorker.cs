using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
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

        public void GetSensors()
        {
            List<string> sensorIndexRange = makeSensorIndexRange(19, 316);
            foreach (string sensorIndex in sensorIndexRange)
            {
                List<string> snmpOidList = makeSensorOidList(sensorIndex);
                this.Get(sensorIndex, snmpOidList);
            }
        }

        private List<string> makeSensorOidList(string sensorIndex)
        {
            List<string> oidList = new List<string>();
            string valueOID = ".1.3.6.1.2.1.99.1.1.1.4." + sensorIndex;
            string descOID = ".1.3.6.1.2.1.47.1.1.1.1.2." + sensorIndex;
            string containedInOID = ".1.3.6.1.2.1.47.1.1.1.1.4." + sensorIndex;
            string sensorTypeOID = ".1.3.6.1.2.1.99.1.1.1.1." + sensorIndex;
            oidList.Add(valueOID);
            oidList.Add(descOID);
            oidList.Add(containedInOID);
            oidList.Add(sensorTypeOID);
            return oidList;
        }

        private List<string> makeSensorIndexRange(int startIx, int endIx)
        {
            List<string> range = new List<string>();
            for (int ii = startIx; ii <= endIx; ii++)
            {
                range.Add(ii.ToString());
            }
            return range;
        }

        public void Get(string sensorIndex, string oid)
        {
            var varList = new List<Variable>();
            varList.Add(new Variable(new ObjectIdentifier(oid)));
            SnmpGet(sensorIndex, varList);
        }

        public void Get(string sensorIndex, List<string> oidList)
        {
            var varList = new List<Variable>();
            foreach (string oid in oidList)
            {
                varList.Add(new Variable( new ObjectIdentifier(oid)));
            }
            SnmpGet(sensorIndex, varList);
        }

        private void SnmpGet(string sensorIndex, List<Variable> varList)
        {
            try
            {
                IList<Variable> results = Messenger.Get(
                    VersionCode.V2,
                    new IPEndPoint(IPAddress.Parse(this.IPAddr), this.PortNum),
                    new OctetString(this.CommunityName),
                    varList,
                    this.TimeoutMsec);
                SaveResults(sensorIndex, results);
            }
            catch (Exception e)
            {
                DumpVarList(sensorIndex, varList);
                Console.WriteLine(e.Message);
            }
        }

        private void DumpVarList(string sensorIndex, List<Variable> varList)
        {
            Console.WriteLine("Problem getting {0}", sensorIndex);
            foreach (Variable snmpVar in varList)
            {
                Console.Write(snmpVar.Id.ToString() + ",");
            }
            Console.Write("\n");
        }

        private void SaveResults(string sensorIndex, IList<Variable> results)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(sensorIndex + ",");
            foreach (Variable result in results)
            {
                sb.Append(result.Data.ToString() + ",");
            }
            sb.Append("\n");

            File.AppendAllText("D:\\snmp-out.csv", sb.ToString());
        }

    }
}

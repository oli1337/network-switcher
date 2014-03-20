using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using ROOT.CIMV2.Win32;

namespace NetworkSwitcher
{
    public class Network
    {
        public static void SetActiveNetworkGroup(NetworkGroup group)
        {
            var wmiQuery = new SelectQuery("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionId != NULL");
            var objectSearcher = new ManagementObjectSearcher(wmiQuery);

            foreach (var item in objectSearcher.Get().Cast<ManagementObject>())
            {
                var adapter = new NetworkAdapter(item);

                var element = group.Interfaces.FirstOrDefault(p => p.Name == adapter.NetConnectionID);

                if (element == null)
                {
                    DisableNetworkInterface(adapter);
                }
                else
                {
                    EnableNetworkInterface(adapter, element.Metric);
                }
            }
        }

        private static void DisableNetworkInterface(NetworkAdapter adapter)
        {
            Console.WriteLine("disabling interface " + adapter.NetConnectionID);
            adapter.Disable();
        }

        private static void EnableNetworkInterface(NetworkAdapter adapter, uint? metric)
        {
            Console.WriteLine("enabling interface " + adapter.NetConnectionID);

            var command = string.Format("/C netsh interface ipv4 set interface \"{0}\" metric={1}", adapter.NetConnectionID, metric.HasValue ? metric.Value.ToString() : "");
            Debug.WriteLine(command);

            var processStartInfo = new ProcessStartInfo("cmd.exe", command) { WindowStyle = ProcessWindowStyle.Hidden };

            Process.Start(processStartInfo);

            adapter.Enable();
        }
    }
}
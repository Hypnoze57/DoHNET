using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoHNET
{
    class DNSListener
    {

        UdpClient srv;
        int port = 53;

        bool run = true;

        public DNSListener()
        {
            srv = new UdpClient(port);
        }

        public void Listen()
        {
            while (run)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, port);
                byte[] data = srv.Receive(ref remoteEP);
                
                // Console.WriteLine("Receive data from " + remoteEP.ToString());
                // Console.WriteLine(data);

                byte[] httpResponse = HTTProxy.ResolveDoH(data);
                
                // Console.WriteLine("HTTP Response received!");
                // Console.WriteLine("HTTP Response: {0}", Encoding.UTF8.GetString(httpResponse));

                // srv.Send(new byte[] { 1 }, 1, remoteEP);
                srv.Send(httpResponse, httpResponse.Length, remoteEP);
            }


        }

        public void Stop()
        {
            run = false;
        }
    }
}

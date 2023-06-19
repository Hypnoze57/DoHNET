using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoHNET
{
    class Program
    {
        static void Main(string[] args)
        {
            HTTProxy.Initialize();
            DNSListener dnsrv = new DNSListener();

            Console.WriteLine("Simple DNS over HTTPS server (RFC 8484 only, NO JSON API)");

            Console.WriteLine("\nUse DoHNET.exe [resolver] to modify the DoH resolver! (Default dns.google)");
            Console.WriteLine("Example: DoHNET.exe https://cloudflare-dns.com/");


            if(args.Length != 0)
            {
                HTTProxy.doh_resolver = args[0];
            }

            Console.WriteLine("\n[*] DNS server listening 127.0.0.1:53");
            Console.WriteLine("[*] Used DoH Provider: {0}", HTTProxy.doh_resolver);

            // Thread dnsThread = new Thread(new ThreadStart(dnsrv.Listen));
            // dnsThread.Start();

            dnsrv.Listen();

            /*
            Console.ReadLine();

            dnsrv.Stop(); // require to send one more packet to stop correctly
            //*/
        }
    }
}

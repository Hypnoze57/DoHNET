using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DoHNET
{

    static class HTTProxy
    {
        public static string doh_resolver = "https://dns.google/";
        // public static string doh_resolver = "https://cloudflare-dns.com/";

        private static HttpClient client;

        public static void Initialize()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; // Disable ssl verification
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            client = new HttpClient(handler);

            client.BaseAddress = new Uri(doh_resolver);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/dns-message")); // Accept
        }

        private static async Task<byte[]> doPOSTAsync(string data)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "dns-query");
            request.Content = new StringContent(data, Encoding.UTF8, "application/dns-message"); // Content-Type

            var response = await client.SendAsync(request);
            byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
            // string responseString = System.Text.Encoding.UTF8.GetString(responseBytes);
            // string responseString = await response.Content.ReadAsStringAsync();

            return responseBytes;
        }

        private static async Task<byte[]> doPOSTAsync(byte[] data)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "dns-query");
            request.Content = new ByteArrayContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/dns-message"); // Content-Type

            var response = await client.SendAsync(request);
            byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
            // string responseString = System.Text.Encoding.UTF8.GetString(responseBytes);
            // string responseString = await response.Content.ReadAsStringAsync();

            return responseBytes;
        }


        public static byte[] ResolveDoH(byte[] dnsData)
        {
            // string base64data = Convert.ToBase64String(dnsData);
            // var response = HTTProxy.doPOSTAsync(base64data);
            var response = HTTProxy.doPOSTAsync(dnsData);
            response.Wait();
            byte[] responseBytes = response.Result;
            return responseBytes;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotaLass.API
{
    public static class RequestHandler
    {
        const int MaxRetries = 3;
        public static string GET(string url)
        {
            return GET(url, 0);
        }

        private static string GET(string url, int retries)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                    if (((HttpWebResponse)ex.Response).StatusCode == (HttpStatusCode)429)
                    {
                        if (retries <= MaxRetries)
                        {
                            Thread.Sleep(1000 * (retries + 1));
                            return GET(url, retries + 1);
                        }
                    }

                return null;
            }
        }
    }
}

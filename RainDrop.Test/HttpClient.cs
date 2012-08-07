using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace RainDrop.Test
{
    public class HttpClient
    {
        public static string Get(string url)
        {
            WebRequest request = WebRequest.Create(url);

            request.Method = "GET";

            string result = string.Empty;
            using (var response = request.GetResponse())
            {
                if(response != null)
                {
                    using (var dataStream = response.GetResponseStream())
                    {
                        
                        byte[] res = new byte[response.ContentLength];
                        dataStream.Read(res, 0, res.Length);

                        result = Encoding.Default.GetString(res);
                    }
                }
            }
            return result;
        }
    }
}

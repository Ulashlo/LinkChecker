using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TrialProgram
{
    public static class LinkInfo
    {
        public static string GetHeaders(string url)
        {
            StringBuilder builder = new StringBuilder();
            WebHeaderCollection collection;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Head;
                request.AllowAutoRedirect = false;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    collection = response.Headers;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw new WebException(ex.Message);
                }
                collection = ((HttpWebResponse)ex.Response).Headers;
            }
            string[] keys = collection.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                builder.Append(keys[i]);
                builder.Append(" - ");
                builder.Append(collection.Get(i));
                builder.Append('\n');
            }
            return builder.ToString();
        }

        public static HttpStatusCode GetStatusCode(string url, bool autoRedirect = false)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Head;
                request.AllowAutoRedirect = autoRedirect;
                request.Accept = @"*/*";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    throw new WebException(ex.Message);
                }
                return ((HttpWebResponse)ex.Response).StatusCode;
            }
        }

        public static string GetLocation(string uri, bool autoRedirect = false)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp(new Uri(uri));
            webRequest.Method = WebRequestMethods.Http.Head;  
            webRequest.AllowAutoRedirect = autoRedirect;
            HttpWebResponse webResponse = null;
            WebHeaderCollection webHeader = new WebHeaderCollection();
            try
            {
                using (webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    webHeader = webResponse.Headers;
                }
            }
            catch (WebException ex)
            {
                if(ex.Response == null)
                {
                    throw new WebException("Illegal status!");
                }
                webHeader = ex.Response.Headers;
            }
            string[] location = webHeader.GetValues("Location");
            if(location == null)
            {
                location = webHeader.GetValues("location");
            }
            if (location == null)
            {
                throw new ArgumentException("Headers haven't location!");
            }
            return location[0];
        }

        public static List<LinkRedirect> GetRedirects(string uri, Uri parent)
        {
            List<LinkRedirect> mas = new List<LinkRedirect>();
            string location = GetFullUri(uri, parent).AbsoluteUri;
            LinkRedirect redirect = new LinkRedirect(LinkInfo.GetStatusCode(location), location);
            mas.Add(redirect);
            int s = (int)redirect.Status;
            while (LinkInfo.IsRedirect(s))
            {
                try
                {
                    location = LinkInfo.GetFullUri(LinkInfo.GetLocation(location), new Uri(location)).AbsoluteUri;
                }
                catch
                {
                    location = LinkInfo.GetFullUri(LinkInfo.GetLocation(location, true), new Uri(location)).AbsoluteUri;
                }
                if (mas.Exists(x => x.Locations.Equals(location)))
                {
                    break;
                }
                HttpStatusCode stat = LinkInfo.GetStatusCode(location);
                mas.Add(new LinkRedirect(stat, location));
                s = (int)stat;
            }
            return mas;
        }

        public static bool IsRedirect(int status) => status > 299 && status < 400;

        public static bool IsError(int status) => status > 399;

        public static string GetPageCode(Uri uri)
        {
            try
            {
                WebClient webClient = new WebClient();
                byte[] rawData = webClient.DownloadData(uri);
                Encoding encoding = GetEncoding(webClient.ResponseHeaders, Encoding.UTF8);
                return encoding.GetString(rawData);
            }
            catch (WebException)
            {
                return "";
            }
        }

        public static Uri GetFullUri(string line, Uri pageUri)
        {

            if (line[0] == '#')
            {
                return pageUri;
            }
            int indexSharp = line.IndexOf('#');
            if (indexSharp != -1)
            {
                line = line.Substring(0, indexSharp);
            }
            if (line.StartsWith("//"))
            {
                line = "https:" + line;
            }
            Uri uri;
            if (line.StartsWith("http"))
            {
                try
                {
                    uri = new Uri(line);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                if (pageUri == null)
                {
                    return pageUri;
                }
                uri = new Uri(pageUri, line);
            }
            return uri;
        }

        private static Encoding GetEncoding(NameValueCollection responseHeaders, Encoding defaultEncoding = null)
        {
            if (responseHeaders == null)
                throw new ArgumentNullException("responseHeaders");

            var contentType = responseHeaders["Content-Type"];
            if (contentType == null)
            {
                return defaultEncoding;
            }
            var contentTypeParts = contentType.Split(';');
            if (contentTypeParts.Length <= 1)
            {
                return defaultEncoding;
            }
            var charsetPart = contentTypeParts.Skip(1).FirstOrDefault(p => p.TrimStart().StartsWith("charset", StringComparison.InvariantCultureIgnoreCase));
            if (charsetPart == null)
            {
                return defaultEncoding;
            }
            var charsetPartParts = charsetPart.Split('=');
            if (charsetPartParts.Length != 2)
            {
                return defaultEncoding;
            }
            var charsetName = charsetPartParts[1].Trim();
            if (charsetName == "")
            {
                return defaultEncoding;
            }
            try
            {
                return Encoding.GetEncoding(charsetName);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("The server returned data in an unknown encoding: " + charsetName, ex);
            }
        }
    }
}

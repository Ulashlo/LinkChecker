using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrialProgram
{
    public struct LinkPosition
    {
        public string ParentUri { get; }
        public int Line { get; }

        public LinkPosition(string parentUri = "", int line = 0) : this()
        {
            this.ParentUri = parentUri;
            this.Line = line;
        }
    }

    public struct LinkRedirect
    {
        public string Locations { get; }
        public HttpStatusCode Status { get; }

        public LinkRedirect(HttpStatusCode status, string locations) : this()
        {
            Locations = locations;
            Status = status;
        }
    }

    public class AboutLink
    {
        public List<LinkPosition> Positions { get; }
        public List<LinkRedirect> Redirects { get; }

        public HttpStatusCode Status
        {
            get => Redirects[0].Status;
        }

        public string Uri
        {
            get => Redirects[0].Locations;
        }

        public int NestingLevel { get; }
        public string FirstUri { get; }
        public string ExceptionMessage { get; }

        private AboutLink()
        {
            Positions = new List<LinkPosition>();
            Redirects = new List<LinkRedirect>();
        }

        public AboutLink(string uri, string parentUri = "", int level = 0) : this()
        {
            string location = uri;
            LinkRedirect redirect = new LinkRedirect(LinkInfo.GetStatusCode(uri), location);
            Redirects.Add(redirect);
            int s = (int)redirect.Status;
            while (LinkInfo.IsRedirect(s))
            {
                try
                {
                    location = LinkInfo.GetFullUri(LinkInfo.GetLocation(location), new Uri(location)).AbsoluteUri;
                }
                catch
                {
                    break;
                }
                if (Redirects.Exists(x => x.Locations.Equals(location)))
                {
                    break;
                }
                HttpStatusCode stat = LinkInfo.GetStatusCode(location);
                Redirects.Add(new LinkRedirect(stat, location));
                s = (int)stat;
            }
            this.NestingLevel = level;
        }

        public AboutLink(string uri, string firstUri, string parentUri, int position, int level = 0) : this(uri, parentUri, level)
        {
            FirstUri = firstUri;
            AddInfo(parentUri, position);
        }

        public AboutLink(List<LinkPosition> positions, List<LinkRedirect> redirects, string firstUri, int recursionLevel, string exceptionMessage = "")
        {
            Positions = positions;
            Redirects = redirects;
            NestingLevel = recursionLevel;
            FirstUri = firstUri;
            ExceptionMessage = exceptionMessage;
        }

        public void AddInfo(string parentUri, int position)
        {
            Positions.Add(new LinkPosition(parentUri, position));
        }
    }
}

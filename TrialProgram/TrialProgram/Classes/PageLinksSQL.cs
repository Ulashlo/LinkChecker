using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using TrialProgram.DBContext.Model;

namespace TrialProgram
{
    public class PageLinksSQL
    {
        private int siteId;
        private int maxLevel;

        public Uri SiteUri { get; }
        public int MaxLevel
        {
            get => maxLevel;
            set
            {
                maxLevel = value;
                using (DataBaseContext db = new DataBaseContext())
                {
                    db.Sites.First(x => x.SiteId == siteId).NestingLevel = value;
                    db.SaveChanges();
                }
            }
        }
        public int InnerCount { get; private set; }
        public int OuterCount { get; private set; }
        public int Count
        {
            get => InnerCount + OuterCount;
        }

        public PageLinksSQL(string siteUri, int maxLevel = 1)
        {
            this.SiteUri = new Uri(siteUri);
            using (DataBaseContext db = new DataBaseContext())
            {
                if (db.Sites.Any(x => x.Uri.Equals(siteUri)))
                {
                    siteId = db.Sites.First(x => x.Uri.Equals(siteUri)).SiteId;
                }
                else
                {
                    Site site = new Site { Uri = siteUri, NestingLevel = maxLevel };
                    db.Sites.Add(site);
                    db.SaveChanges();
                    siteId = db.Sites.First(x => x.Uri.Equals(siteUri)).SiteId;
                }
            }
            if (maxLevel < 1)
            {
                maxLevel = 1;
            }
            this.maxLevel = maxLevel;
            InnerCount = 0;
            OuterCount = 0;
        }

        public void ReLevel(int k)
        {
            if (k <= maxLevel)
            {
                return;
            }
            Queue<AboutLink> queue = new Queue<AboutLink>();
            using (DataBaseContext db = new DataBaseContext())
            {
                foreach (var item in db.Linkes.Where(x => x.SiteId == siteId && x.IsInner && x.NestingLevel == maxLevel))
                {
                    queue.Enqueue(GetAboutLink(item.Uri, true, item));
                }
                maxLevel = k;
                while (queue.Count > 0)
                {
                    AboutLink aboutLink = queue.Dequeue();
                    FindLinksOnPage(aboutLink.NestingLevel + 1, aboutLink.Uri, queue, db);
                }
            }
        }

        private IEnumerable<AboutLink> FiltMas(IEnumerable<AboutLink> rezult, Filtr filtr)
        {
            if ((filtr & Filtr.Exception) == 0)
            {
                rezult = rezult.Where(x => x.ExceptionMessage.Length == 0);
            }
            if ((filtr & Filtr.Info) == 0)
            {
                rezult = rezult.Where(x => x.ExceptionMessage.Length != 0 || (int)x.Status > 199);
            }
            if ((filtr & Filtr.Success) == 0)
            {
                rezult = rezult.Where(x => x.ExceptionMessage.Length != 0 || ((int)x.Status < 200 || (int)x.Status > 299));
            }
            if ((filtr & Filtr.Redirect) == 0)
            {
                rezult = rezult.Where(x => x.ExceptionMessage.Length != 0 || ((int)x.Status < 300 || (int)x.Status > 399));
            }
            if ((filtr & Filtr.Client) == 0)
            {
                rezult = rezult.Where(x => x.ExceptionMessage.Length != 0 || ((int)x.Status < 400 || (int)x.Status > 499));
            }
            if ((filtr & Filtr.Servis) == 0)
            {
                rezult = rezult.Where(x => x.ExceptionMessage.Length != 0 || (int)x.Status < 500);
            }
            return rezult;
        }

        public IEnumerable<AboutLink> GetIner(int k = 1, Filtr filtr = (Filtr)255)
        {
            if ((filtr & Filtr.Inner) == 0)
            {
                return new List<AboutLink>();
            }
            List<Link> links;
            List<AboutLink> mas;
            if (k == maxLevel)
            {
                using (DataBaseContext db = new DataBaseContext())
                {
                    links = db.Linkes.Where(x => x.SiteId == siteId && x.IsInner).ToList();
                }
                mas = new List<AboutLink>();
                foreach (var item in links)
                {
                    if (item.AnswerId == null)
                    {
                        mas.Add(GetExeptionLink(item.Uri, true, item, k));
                    }
                    else
                    {
                        mas.Add(GetAboutLink(item.Uri, true, item, k));
                    }
                }
            }
            else if (k < maxLevel)
            {
                using (DataBaseContext db = new DataBaseContext())
                {
                    links = db.Linkes.Where(x => x.SiteId == siteId && x.IsInner && x.NestingLevel <= k).ToList();
                }
                mas = new List<AboutLink>();
                foreach (var item in links)
                {
                    if (item.AnswerId == null)
                    {
                        mas.Add(GetExeptionLink(item.Uri, true, item, k));
                    }
                    else
                    {
                        mas.Add(GetAboutLink(item.Uri, true, item, k));
                    }
                }
            }
            else
            {
                ReLevel(k);
                using (DataBaseContext db = new DataBaseContext())
                {
                    links = db.Linkes.Where(x => x.SiteId == siteId && x.IsInner).ToList();
                }
                mas = new List<AboutLink>();
                foreach (var item in links)
                {
                    if (item.AnswerId == null)
                    {
                        mas.Add(GetExeptionLink(item.Uri, true, item, k));
                    }
                    else
                    {
                        mas.Add(GetAboutLink(item.Uri, true, item, k));
                    }
                }
            }
            return FiltMas(mas, filtr);
        }

        public IEnumerable<AboutLink> GetOuter(int k = 1, Filtr filtr = (Filtr)255)
        {
            if ((filtr & Filtr.Outer) == 0)
            {
                return new List<AboutLink>();
            }
            List<Link> links;
            List<AboutLink> mas;
            if (k == maxLevel)
            {
                using (DataBaseContext db = new DataBaseContext())
                {
                    links = db.Linkes.Where(x => x.SiteId == siteId && !x.IsInner).ToList();
                }
                mas = new List<AboutLink>();
                foreach (var item in links)
                {
                    if (item.AnswerId == null)
                    {
                        mas.Add(GetExeptionLink(item.Uri, false, item, k));
                    }
                    else
                    {
                        mas.Add(GetAboutLink(item.Uri, false, item, k));
                    }
                }
            }
            else if (k < maxLevel)
            {
                using (DataBaseContext db = new DataBaseContext())
                {
                    links = db.Linkes.Where(x => x.SiteId == siteId && !x.IsInner && x.NestingLevel <= k).ToList();
                }
                mas = new List<AboutLink>();
                foreach (var item in links)
                {
                    if (item.AnswerId == null)
                    {
                        mas.Add(GetExeptionLink(item.Uri, false, item, k));
                    }
                    else
                    {
                        mas.Add(GetAboutLink(item.Uri, false, item, k));
                    }
                }
            }
            else
            {
                ReLevel(k);
                using (DataBaseContext db = new DataBaseContext())
                {
                    links = db.Linkes.Where(x => x.SiteId == siteId && !x.IsInner).ToList();
                }
                mas = new List<AboutLink>();
                foreach (var item in links)
                {
                    if (item.AnswerId == null)
                    {
                        mas.Add(GetExeptionLink(item.Uri, false, item, k));
                    }
                    else
                    {
                        mas.Add(GetAboutLink(item.Uri, false, item, k));
                    }
                }
            }
            return FiltMas(mas, filtr);
        }

        public AboutLink GetAboutLink(string uri = "", bool isIner = false, Link link = null, int recLevel = -1)
        {
            AboutLink aboutLink;
            using (DataBaseContext db = new DataBaseContext())
            {
                if (link == null)
                {
                    link = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(uri) && x.IsInner == isIner);
                }
                if (link == null)
                {
                    return null;
                }
                List<LinkRedirect> red = new List<LinkRedirect>();
                red.Add(new LinkRedirect((HttpStatusCode)db.Answers.First(x => x.AnswerId == link.AnswerId).Value, link.Uri));
                var redirects = db.Redirects.Where(x => x.LinkId == link.LinkId).ToList();
                foreach (Redirect item in redirects)
                {
                    red.Add(new LinkRedirect((HttpStatusCode)db.Answers.First(x => x.AnswerId == item.AnswerId).Value, item.LinkStr));
                }
                List<LinkPosition> pos = new List<LinkPosition>();
                var relationships = db.Relationships.Where(x => x.ChildId == link.LinkId).ToList();
                foreach (Relationship item in relationships)
                {
                    string parentUri;
                    if (item.ParentId == null)
                    {
                        parentUri = SiteUri.AbsoluteUri;
                    }
                    else
                    {
                        parentUri = db.Linkes.FirstOrDefault(x => x.LinkId == item.ParentId).Uri;
                    }
                    foreach (var line in item.Lines)
                    {
                        pos.Add(new LinkPosition(parentUri, line.LineNumber));
                    }

                }
                aboutLink = new AboutLink(pos, red, link.OriginalUri, link.NestingLevel);
            }
            return aboutLink;
        }

        public AboutLink GetExeptionLink(string uri = "", bool isIner = false, Link link = null, int recLevel = -1)
        {
            AboutLink aboutLink;
            using (DataBaseContext db = new DataBaseContext())
            {
                if (link == null)
                {
                    link = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(uri) && x.AnswerId == null);
                }
                if (link == null)
                {
                    return null;
                }
                List<LinkRedirect> red = new List<LinkRedirect>();
                red.Add(new LinkRedirect(HttpStatusCode.OK, link.Uri));
                List<LinkPosition> pos = new List<LinkPosition>();
                var relationships = db.Relationships.Where(x => x.ChildId == link.LinkId).ToList();
                foreach (Relationship item in relationships)
                {
                    string parentUri;
                    if (item.ParentId == null)
                    {
                        parentUri = SiteUri.AbsoluteUri;
                    }
                    else
                    {
                        parentUri = db.Linkes.FirstOrDefault(x => x.LinkId == item.ParentId).Uri;
                    }
                    foreach (var line in item.Lines)
                    {
                        pos.Add(new LinkPosition(parentUri, line.LineNumber));
                    }
                }
                aboutLink = new AboutLink(pos, red, link.OriginalUri, link.NestingLevel, db.Exceptions.First(x => x.ExceptionId == link.ExceptionId).Message);
            }
            return aboutLink;
        }

        public void RemoveLinkes()
        {
            using (DataBaseContext db = new DataBaseContext())
            {
                List<Link> links = db.Linkes.Where(x => x.SiteId == siteId).ToList();
                List<Redirect> redirects = new List<Redirect>();
                List<Relationship> relationships = new List<Relationship>();
                foreach (var item in links)
                {
                    redirects.AddRange(db.Redirects.Where(x => x.LinkId == item.LinkId).ToList());
                    relationships.AddRange(db.Relationships.Where(x => x.ChildId == item.LinkId).ToList());
                }
                List<Line> lines = new List<Line>();
                foreach (var item in relationships)
                {
                    lines.AddRange(db.Lines.Where(x => x.RelationshipId == item.RelationshipId));
                }
                db.Lines.RemoveRange(lines);
                db.Linkes.RemoveRange(links);
                db.Redirects.RemoveRange(redirects);
                db.Relationships.RemoveRange(relationships);
                db.SaveChanges();
            }
        }

        public void FindLinks()
        {
            InnerCount = 0;
            OuterCount = 0;
            using (DataBaseContext db = new DataBaseContext())
            {
                Queue<AboutLink> queue = new Queue<AboutLink>();
                queue.Enqueue(new AboutLink(SiteUri.AbsoluteUri));
                while (queue.Count > 0)
                {
                    AboutLink aboutLink = queue.Dequeue();
                    FindLinksOnPage(aboutLink.NestingLevel + 1, aboutLink.Uri, queue, db);
                }
            }
        }

        private void FindLinksOnPage(int k, string uri, Queue<AboutLink> queue, DataBaseContext db)
        {
            string line = LinkInfo.GetPageCode(new Uri(uri));
            Regex regexLink = new Regex(@"(?<=<a\s.*?href=[""\s«]).*?(?=[""\s»].*?>)");
            Regex regexLine = new Regex("\n");
            string[] lines = regexLine.Split(line);
            for (int j = 0; j < lines.Length; j++)
            {
                MatchCollection matches = regexLink.Matches(lines[j]);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        string link = match.Value;
                        string test = link.ToLower();
                        if (test.StartsWith("tel:") || test.StartsWith("mailto:") || test.StartsWith("skype:") || test.StartsWith("javascript:") || link.Length == 0)
                        {
                            continue;
                        }
                        AddLink(link, uri, j + 1, queue, db, k);
                    }
                }
            }
        }

        private void AddLink(string linkUri, string parentUri, int position, Queue<AboutLink> queue, DataBaseContext db, int k)
        {
            Uri uri = LinkInfo.GetFullUri(linkUri, new Uri(parentUri));
            if (uri == null)
            {
                return;
            }
            if (uri.Host.Equals(SiteUri.Host))
            {
                Link link = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(uri.AbsoluteUri) && x.IsInner);
                if (link == null)
                {
                    AboutLink linkInfo;
                    string message;
                    try
                    {
                        linkInfo = new AboutLink(uri.AbsoluteUri, linkUri, parentUri, position, k);
                    }
                    catch (WebException ex)
                    {
                        message = ex.Message;
                        Exception exception = GetException(db, message);
                        link = new Link { SiteId = siteId, Uri = uri.AbsoluteUri, AnswerId = null, IsInner = true, NestingLevel = k, OriginalUri = linkUri, ExceptionId = exception.ExceptionId };
                        db.Linkes.Add(link);
                        Link p = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(parentUri));
                        int? pId = null;
                        if (p != null)
                        {
                            pId = p.LinkId;
                        }
                        db.SaveChanges();
                        UpdateRelation(db, pId, link.LinkId, position);
                        InnerCount++;
                        return;
                    }
                    if (!uri.AbsoluteUri.Equals(SiteUri.AbsoluteUri) && k < maxLevel)
                    {
                        queue.Enqueue(linkInfo);
                    }
                    link = new Link { SiteId = siteId, Uri = linkInfo.Uri, AnswerId = GetAnswer(db, (int)linkInfo.Status).AnswerId, IsInner = true, NestingLevel = k, OriginalUri = linkUri };
                    db.Linkes.Add(link);
                    db.SaveChanges();
                    InnerCount++;
                    for (int i = 1; i < linkInfo.Redirects.Count; i++)
                    {
                        db.Redirects.Add(new Redirect { LinkStr = linkInfo.Redirects[i].Locations, AnswerId = GetAnswer(db, (int)linkInfo.Redirects[i].Status).AnswerId, LinkId = link.LinkId });
                    }
                }
                Link parent = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(parentUri));
                int? parId = null;
                if (parent != null)
                {
                    parId = parent.LinkId;
                }
                UpdateRelation(db, parId, link.LinkId, position);
            }
            else
            {
                Link link = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(uri.AbsoluteUri) && !x.IsInner);
                if (link == null)
                {
                    string message;
                    AboutLink linkInfo;
                    try
                    {
                        linkInfo = new AboutLink(uri.AbsoluteUri, linkUri, parentUri, position, k);
                    }
                    catch (WebException ex)
                    {
                        message = ex.Message;
                        Exception exception = GetException(db, message);
                        link = new Link { SiteId = siteId, Uri = uri.AbsoluteUri, AnswerId = null, IsInner = false, NestingLevel = k, OriginalUri = linkUri, ExceptionId = exception.ExceptionId };
                        db.Linkes.Add(link);
                        Link p = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(parentUri));
                        int? pId = null;
                        if (p != null)
                        {
                            pId = p.LinkId;
                        }
                        db.SaveChanges();
                        UpdateRelation(db, pId, link.LinkId, position);
                        OuterCount++;
                        return;
                    }
                    link = new Link { SiteId = siteId, Uri = linkInfo.Uri, AnswerId = GetAnswer(db, (int)linkInfo.Status).AnswerId, IsInner = false, NestingLevel = k, OriginalUri = linkUri };
                    db.Linkes.Add(link);
                    db.SaveChanges();
                    OuterCount++;
                    for (int i = 1; i < linkInfo.Redirects.Count; i++)
                    {
                        db.Redirects.Add(new Redirect { LinkStr = linkInfo.Redirects[i].Locations, AnswerId = GetAnswer(db, (int)linkInfo.Redirects[i].Status).AnswerId, LinkId = link.LinkId });
                    }
                }
                Link parent = db.Linkes.Where(x => x.SiteId == siteId).FirstOrDefault(x => x.Uri.Equals(parentUri));
                int? parId = null;
                if (parent != null)
                {
                    parId = parent.LinkId;
                }
                UpdateRelation(db, parId, link.LinkId, position);
            }
        }

        private void UpdateRelation(DataBaseContext db, int? parentId, int childId, int position)
        {
            var rel = db.Relationships.FirstOrDefault(r => r.ParentId == parentId && r.ChildId == childId);
            if (rel == null)
            {
                rel = new Relationship
                {
                    ParentId = parentId,
                    ChildId = childId
                };
                db.Relationships.Add(rel);
                db.SaveChanges();
                db.Lines.Add(new Line { LineNumber = position, RelationshipId = rel.RelationshipId });
                db.SaveChanges();
            }
            else
            {
                var line = rel.Lines.FirstOrDefault(r => r.LineNumber == position);
                if (line == null)
                {
                    db.Lines.Add(new Line { LineNumber = position, RelationshipId = rel.RelationshipId });
                    db.SaveChanges();
                }
            }
        }

        private Answer GetAnswer(DataBaseContext db, int value)
        {
            Answer answer = db.Answers.FirstOrDefault(x => x.Value == value);
            if (answer == null)
            {
                answer = new Answer { Value = value };
                db.Answers.Add(answer);
                db.SaveChanges();
            }
            return answer;
        }

        private Exception GetException(DataBaseContext db, string value)
        {
            Exception exception = db.Exceptions.FirstOrDefault(x => x.Message == value);
            if (exception == null)
            {
                exception = new Exception { Message = value };
                db.Exceptions.Add(exception);
                db.SaveChanges();
            }
            return exception;
        }
    }
}

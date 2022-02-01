using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace AlmaCMS.Helpers
{
    public class RssReaderHelper
    {

        public class Rss
        {
            public string Link { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }





        public class SiteRssReader
        {

            public static IEnumerable<Rss> GetRssFeed(string _blogURL)
            {

                XDocument feedXml = XDocument.Load(_blogURL);
                var feeds = from feed in feedXml.Descendants("item")
                            select new Rss
                            {
                                Title = feed.Element("title").Value,
                                Link = feed.Element("link").Value,
                                Description = Regex.Match(feed.Element("description").Value, @"^.{1,180}\b(?<!\s)").Value

                            };

                return feeds;

            }
        }
    }
}
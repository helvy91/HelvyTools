using System;
using System.Linq;
using HtmlAgilityPack;
using HelvyTools.Html.Data;
using HelvyTools.Extensions;
using System.Collections.Generic;

namespace HelvyTools.Html
{
    public class HtmlParser
    {
        public HtmlElement GetElementById(string id, string html, int maxDepth)
        {
            var doc = GetHtmlDocument(html, maxDepth);
            var node = doc.GetElementbyId(id);

            return GetHtmlElement(node, 0, maxDepth);
        }

        public HtmlElement GetElementByClass(string @class, string html, int maxDepth, bool literalComparison = false)
        {
            var doc = GetHtmlDocument(html, maxDepth);

            var descendants = doc.DocumentNode.Descendants();
            var nodes = FilterDescendantsByClass(@class, descendants, literalComparison);
            FailIfNodesCountDifferentThanOne(nodes);

            return GetHtmlElement(nodes.Single(), 0, maxDepth);
        }

        public HtmlElement GetElementByAttributeValue(string key, string value, string html, int maxDepth)
        {
            var doc = GetHtmlDocument(html, maxDepth);

            var descendants = doc.DocumentNode.Descendants();
            var nodes = descendants.Where(x => x.GetAttributeValue<string>(key, "") == value);
            FailIfNodesCountDifferentThanOne(nodes);


            return GetHtmlElement(nodes.Single(), 0, maxDepth);
        }

        public HtmlElement GetElementByTagName(string name, string html, int maxDepth)
        {
            var doc = GetHtmlDocument(html, maxDepth);

            var descendants = doc.DocumentNode.Descendants();
            var nodes = doc.DocumentNode.Descendants().Where(x => x.Name == name);
            FailIfNodesCountDifferentThanOne(nodes);

            return GetHtmlElement(nodes.Single(), 0, maxDepth);
        }

        public HtmlElement RemoveNodeByClass(string @class, string html, int maxDepth, bool literalComparison = false)
        {
            var doc = GetHtmlDocument(html, maxDepth);
            
            var descendants = doc.DocumentNode.Descendants();
            var nodes = FilterDescendantsByClass(@class, descendants, literalComparison);
            FailIfNodesCountDifferentThanOne(nodes);

            doc.DocumentNode.RemoveChild(nodes.Single());
            
            return GetHtmlElement(doc.DocumentNode, 0, maxDepth);
        }

        private List<HtmlElement> GetDescendants(HtmlNode node, int currentDepth, int maxDepth)
        {
            currentDepth++;

            if (!node.Descendants().Any() || currentDepth == maxDepth)
            {
                return null;
            }

            var descendants = new List<HtmlElement>();
            foreach (var descendant in node.Descendants())
            {
                descendants.Add(GetHtmlElement(descendant, currentDepth, maxDepth));
            }

            return descendants;
        }

        private HtmlElement GetHtmlElement(HtmlNode node, int currentDepth, int maxDepth)
        {
            if (node == null)
            {
                return null;
            }

            return new HtmlElement()
            {
                Id = node.Id,
                Classes = node.GetClasses().ToList(),
                InnerText = node.InnerText,
                OuterHtml = node.OuterHtml,
                Name = node.Name,
                
                Attributes = node.GetAttributes()
                                 .GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                                 .ToDictionary(x => x.Key, y => y.First().Value),

                Descendants = GetDescendants(node, currentDepth, maxDepth)
            };
        }

        private HtmlDocument GetHtmlDocument(string html, int maxDepth)
        {
            FailIfZeroOrLess(maxDepth, nameof(maxDepth));

            var doc = new HtmlDocument();
            doc.OptionReadEncoding = false;
            doc.LoadHtml(html);

            return doc;
        }

        private IEnumerable<HtmlNode> FilterDescendantsByClass(string @class, IEnumerable<HtmlNode> descendants, bool literalComparison)
        {
            var nodes = literalComparison?
                       descendants.Where(x => x.GetClasses().Join(" ").Equals(@class)) :
                       descendants.Where(x => x.GetClasses().IsSupersetOf(@class.Split(' ')));

            return nodes;
        }

        private void FailIfNodesCountDifferentThanOne(IEnumerable<HtmlNode> nodes)
        {
            if (nodes.Count() != 1)
            {
                throw new InvalidOperationException("There is more than one node or no nodes for provided search value.");
            }
        }

        private void FailIfZeroOrLess(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Provided parameter value can't be lesser than 1.", parameterName);
            }
        }
    }
}

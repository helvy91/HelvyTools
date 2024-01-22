using HelvyTools.Extensions;

namespace HelvyTools.Html.Data
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string InnerText { get; set; }
        public string OuterHtml { get; set; }
        public string Name { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public List<HtmlElement> Descendants { get; set; } = new List<HtmlElement>();
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public List<HtmlElement> GetDescendantsByClass(string @class, bool literalComparison = false)
        {
            return literalComparison ?
                Descendants.Where(x => x.Classes.Join(" ").Equals(@class)).ToList() :
                Descendants.Where(x => x.Classes.IsSupersetOf(@class.Split(" "))).ToList();
        }

        public List<HtmlElement> GetDescendantsById(string id)
        {
            return Descendants.Where(x => x.Id == id).ToList();
        }

        public List<HtmlElement> GetDescendantsByTagName(string name)
        {
            return Descendants.Where(x => x.Name == name).ToList();
        }

        public List<HtmlElement> GetDescendantsByAttributeValue(string key, string value)
        {
            return Descendants.Where(x => x.GetAttributeValue(key) == value).ToList();
        }

        public string GetAttributeValue(string key)
        {
            return Attributes.TryGetValue(key, out var value) ? value : null;
        }
    }
}

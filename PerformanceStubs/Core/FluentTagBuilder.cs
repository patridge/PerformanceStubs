namespace PerformanceStubs.Core {
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    public class FluentTagBuilder : TagBuilder {
        public TagRenderMode RenderMode { get; set; }
        public IList<FluentTagBuilder> ChildTags { get; set; }
        public FluentTagBuilder(string tagName) : this(tagName, null) { }
        public FluentTagBuilder(string tagName, params FluentTagBuilder[] childTags) : base(tagName) {
            this.ChildTags = childTags ?? new List<FluentTagBuilder>().ToArray();
            this.RenderMode = TagRenderMode.Normal;
        }

        public FluentTagBuilder WithAttribute(string key, string value) {
            MergeAttribute(key, value);
            return this;
        }
        public FluentTagBuilder WithCssClass(string value) {
            AddCssClass(value);
            return this;
        }
        public FluentTagBuilder WithId(string name) {
            GenerateId(name);
            return this;
        }
        private string InnerText { get; set; }
        public FluentTagBuilder WithText(string text) {
            // base.SetInnerText overwrites InnerHtml, so just capture it until it is needed.
            InnerText = text;
            return this;
        }
        public FluentTagBuilder AddChild(FluentTagBuilder childTag) {
            ChildTags.Add(childTag);
            return this;
        }
        public FluentTagBuilder AddChild(string tagName, params FluentTagBuilder[] childTags) {
            ChildTags.Add(new FluentTagBuilder(tagName, childTags));
            return this;
        }

        public override string ToString() {
            StringBuilder childHtml = new StringBuilder();
            childHtml.Append(HttpUtility.HtmlEncode(this.InnerText));
            foreach (FluentTagBuilder childTag in ChildTags) {
                childHtml.Append(childTag.ToString());
            }
            base.InnerHtml = childHtml.ToString();
            return base.ToString(this.RenderMode);
        }

        public static string MakeIntoHtml5Page(string pageTitle, params FluentTagBuilder[] bodyContent) {
            FluentTagBuilder html = new FluentTagBuilder("html").AddChild(new FluentTagBuilder("head").AddChild(new FluentTagBuilder("title").WithText(pageTitle))
                                                                                                      .AddChild(new FluentTagBuilder("meta") { RenderMode = TagRenderMode.SelfClosing }.WithAttribute("http-equiv", "content-type").WithAttribute("content", "text/html;charset=utf-8")))
                                                                .AddChild(new FluentTagBuilder("body", bodyContent));
            return "<!DOCTYPE html>" + html.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace VendorSystem.HtmlControl
{
    public class NestedTagBuilder : TagBuilder
    {
        private readonly IList<NestedTagBuilder> _innerTags = new List<NestedTagBuilder>();
        public NestedTagBuilder(string tagName)
          : base(tagName)
        {

        }
        public IEnumerable<NestedTagBuilder> InnerTags
        {
            get
            {
                return new ReadOnlyCollection<NestedTagBuilder>(this._innerTags);
            }
        }
        public void Add(NestedTagBuilder tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException("tag");
            }

            this._innerTags.Add(tag);
        }
        public override string ToString()
        {
            if (this.InnerTags.Count() > 0)
            {
                this.InnerHtml = RenderSubTags(this);
            }
            return base.ToString();
        }
        private string RenderSubTags(NestedTagBuilder tag)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in tag.InnerTags)
            {
                sb.Append(t.ToString());
            }
            return sb.ToString();
        }
        public static NestedTagBuilder Create(string tagName)
        {
            return new NestedTagBuilder(tagName);
        }
        public NestedTagBuilder AddCss(string css)
        {
            this.AddCssClass(css);
            return this;
        }
        public NestedTagBuilder AddCssIf(bool condition, string css)
        {
            if (condition)
            {
                this.AddCssClass(css);
            }
            return this;
        }
        public NestedTagBuilder AddChild(NestedTagBuilder tag)
        {
            this.Add(tag);
            return this;
        }
        public NestedTagBuilder SetAttribute(string key, string value)
        {
            this.MergeAttribute(key, value);
            return this;
        }
        public NestedTagBuilder SetText(string text)
        {
            this.SetInnerText(text);
            return this;
        }
        public NestedTagBuilder SetInnerHtml(string html)
        {
            this.InnerHtml = html;
            return this;
        }
    }
}
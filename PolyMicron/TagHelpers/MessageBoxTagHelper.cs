using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ProjectPlaguemangler.Controllers;
using ProjectPlaguemangler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPlaguemangler.TagHelpers
{
    [HtmlTargetElement("message-boxes")]
    public class MessageBoxTagHelper: TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private string TypeToCssClass(BoxType type)
        {
            switch (type)
            {
                case BoxType.Success: return "is-success";
                case BoxType.Warning: return "is-warking";
                case BoxType.Error: return "is-danger";
                default: return "";
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var key = context.AllAttributes["key"]?.Value.ToString();
            key = key ?? "Boxes";

            var messageBoxes = ViewContext.TempData.Get<List<MessageBox>>(key);

            if (messageBoxes == null) return;

            var content = new StringBuilder();
            var typeAttr = context.AllAttributes.FirstOrDefault(a => a.Name == "type");

            if (typeAttr != null)
            {
                messageBoxes = messageBoxes.Where(m => m.Type == Enum.Parse<BoxType>(typeAttr.Value as string))
                    .ToList();
            }

            foreach(var item in messageBoxes)
            {
                content.AppendLine($@"<div class=""message {TypeToCssClass(item.Type)}"">")
                    .AppendLine($@"<div class=""message-body"">{item.Message}</div>")
                    .AppendLine(@"</div>");
                    
            }

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(content.ToString());
        }
    }
}

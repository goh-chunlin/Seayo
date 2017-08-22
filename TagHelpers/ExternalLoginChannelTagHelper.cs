using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace Seayo.TagHelpers
{
	[HtmlTargetElement("externalLoginChannel")]
    public class ExternalLoginChannelTagHelper : TagHelper
    {
		private IHostingEnvironment _env;

		[HtmlAttributeName("sy-login-channel")]
		public string Channel { get; set; }

		[HtmlAttributeName("sy-login-message")]
		public string CallForActionMessage { get; set; }

		public ExternalLoginChannelTagHelper(IHostingEnvironment env)
		{
			_env = env;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			string templateContent = "";
			using (var streamReader = new StreamReader(System.IO.Path.Combine(_env.WebRootPath, "tag-helper-templates/external-login-channel.html"), Encoding.UTF8))
			{
				templateContent = await streamReader.ReadToEndAsync();
			}

			var dictionaryValuesForTemplate = new Dictionary<string, string>();

			dictionaryValuesForTemplate.Add("CHANNEL_NAME", Channel);
			dictionaryValuesForTemplate.Add("CHANNEL_THUMBNAIL_CSS_CLASS_NAME", $"login-card-{Channel.ToLower()}");
			dictionaryValuesForTemplate.Add("CALL_FOR_ACTION_MESSAGE", CallForActionMessage);

			output.Content.SetHtmlContent(FormatTemplateContent(dictionaryValuesForTemplate, templateContent));
		}

		private string FormatTemplateContent(Dictionary<string, string> dictionaryValues, string templateContent)
		{
			string output = templateContent;

			foreach(var keyValue in dictionaryValues)
			{
				output = output.Replace("{{" + keyValue.Key + "}}", keyValue.Value);
			}

			return output;
		}
    }
}
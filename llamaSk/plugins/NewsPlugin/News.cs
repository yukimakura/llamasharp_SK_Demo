using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Linq;
using Microsoft.SemanticKernel;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml.Linq;
using System.Text;

namespace LLamaSK.Plugins.NewsPlugin;
public class News
{

    [SKFunction, Description("ニュースを要求され、場所の名前を与えられたら、ニュースを教えます。")]
    public string GetNews([Description("場所の名前")] string input)
    {
        string url = @$"https://news.google.com/news/rss/headlines/section/geo/{input}?hl=ja&gl=JP&ceid=JP:ja";
        var retStrbuilder = new StringBuilder($"googleから{input}の{DateTime.Now.ToString("yyyy/MM/dd")}のニュースをお届けします。");
        try
        {
            XElement element = XElement.Load(url);
            XElement channelElement = element.Element("channel");
            IEnumerable<XElement> elementItems = channelElement.Elements("item");

            for (int i = 0; i < 5; i++)
            {
                XElement item = elementItems.ElementAt(i);
                retStrbuilder.Append(item.Element("title").Value + "\n");
            }

        }
        catch (Exception e)
        {
            return e.Message;
        }

        return retStrbuilder.ToString();

    }

}

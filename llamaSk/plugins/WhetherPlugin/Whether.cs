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

namespace LLamaSK.Plugins.WhetherPlugin;
public class Whether
{
    [SKFunction, Description("天気を要求され、場所の名前を与えられたら、天気を教えます")]
    public string WhatWhether([Description("場所の名前")] string input)
    {
        // string baseUrl = "https://weather.tsukumijima.net/api/forecast";
        // //東京都のID
        // string cityname = "130010";

        // //地域取得
        // string locurl = $"https://weather.tsukumijima.net/primary_area.xml";
        // string locjson = new HttpClient().GetStringAsync(locurl).Result;
        // var locnode = JsonNode.Parse(locjson);
        // var loclist = locnode["forecasts"][0]["telop"].ToString();

        // string url = $"{baseUrl}?city={cityname}";
        // string json = new HttpClient().GetStringAsync(url).Result;
        // // JObject jobj = JObject.Parse(json);


        // var node = JsonNode.Parse(json);
        // var list = node["forecasts"][0]["telop"].ToString();
        // string todayweather = (string)((jobj["forecasts"][0]["telop"] as JValue).Value);
        return $"{input}は槍の雨が降るでしょう。(嘘です。実装めんどいからやってない)";
    }


    [SKFunction, Description("ニュースを要求され、場所の名前を与えられたら、ニュースを教えます。")]
    public string GetNews([Description("場所の名前")] string input)
    {
        string url = @$"https://news.google.com/news/rss/headlines/section/geo/{input}?hl=ja&gl=JP&ceid=JP:ja";
        var retStrbuilder = new StringBuilder($"googleから{input}の{DateTime.Now.ToString("yyyy/MM/dd")}のニュースをお届けします。");
        try
        {
            // RSS読み込み
            XElement element = XElement.Load(url);

            // channelの取得
            XElement channelElement = element.Element("channel");

            //itemの取得
            IEnumerable<XElement> elementItems = channelElement.Elements("item");

            for (int i = 0; i < 5; i++)
            {
                XElement item = elementItems.ElementAt(i);
                retStrbuilder.Append(item.Element("title").Value + "\n");
            }

            // Console.WriteLine("完了");
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return retStrbuilder.ToString();

    }

    // [SKFunction, Description("モーターを回すことを要求され、方向を与えられたら、モーターを回します")]
    // public string Rotate([Description("方向")] string input)
    // {
    //     return $"{input}側のモーターをまわします！boooooooo!";
    // }




}

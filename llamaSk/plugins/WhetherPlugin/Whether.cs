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
        return $"{input}は槍の雨が降るでしょう。(嘘です。実装めんどいからやってない)";
    }

}

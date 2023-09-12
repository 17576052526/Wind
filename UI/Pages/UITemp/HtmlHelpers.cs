using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text.RegularExpressions;
using System.Text;

public static class HtmlHelpers
{
    private const string ScriptsKey = "__CustomSection";

    public static IDisposable Section(this IHtmlHelper helper)
    {
        return new ScriptBlock(helper.ViewContext);
    }

    public static HtmlString RenderPartialSection(this IHtmlHelper helper)
    {
        return new HtmlString(Get(helper.ViewContext.HttpContext));
    }

    private static void Set(HttpContext context, string str)
    {
        List<string> list = (List<string>)context.Items[ScriptsKey];
        if (list == null)
        {
            list = new List<string>();
            context.Items[ScriptsKey] = list;
        }
        list.Add(str);
    }
    private static string Get(HttpContext context)
    {
        List<string> list = (List<string>)context.Items[ScriptsKey];
        if (list == null) { return null; }
        //return string.Join(Environment.NewLine, list.Distinct());//List 去重复了
        //多个<style>标签的内容合并到一个里面去
        StringBuilder style = new StringBuilder();
        StringBuilder other = new StringBuilder();
        foreach (var m in list.Distinct())//List 去重复了
        {
            //获取所有的<style>
            string str = Regex.Match(m, @"(?<=^\s*<style[^>]*>)[\s\S]*(?=</style>\s*$)").Value;
            if (str.Length > 0)
            {
                style.Append(str);
            }
            else
            {
                other.Append(m);
            }
        }
        other.Append("<style type=\"text/css\">");
        other.Append(style);
        other.Append("</style>");
        return other.ToString();
    }

    private class ScriptBlock : IDisposable
    {
        private readonly TextWriter _originalWriter;
        private readonly StringWriter _scriptsWriter;

        private readonly ViewContext _viewContext;

        public ScriptBlock(ViewContext viewContext)
        {
            _viewContext = viewContext;
            _originalWriter = _viewContext.Writer;
            _viewContext.Writer = _scriptsWriter = new StringWriter();
        }

        public void Dispose()
        {
            _viewContext.Writer = _originalWriter;//还原 Writer
            Set(_viewContext.HttpContext, _scriptsWriter.ToString());
        }
    }
}
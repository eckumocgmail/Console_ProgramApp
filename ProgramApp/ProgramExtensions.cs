using ConsoleApp;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Reflection.Assembly;

public static class ProgramExtensions
{
    public static IEnumerable<string> GetTypeNames(this Assembly assembly) => assembly.GetTypes().Where(type=>type.IsClass).Select(type=>type.Name);
    public static IEnumerable<int> GetIndexes(this string text, string substring) => TextUtils.indexes(text, substring);
    public static IEnumerable<string> GetSplit(this IEnumerable<string> items, string sep)
    {
        var list = new List<string>();
        foreach (var item in items)
        {
            list.AddRange(item.GetSplit(sep));
        }
        return list;
    }
    public static bool HasChar(this string text, char ch) => text.ToCharArray().Contains(ch);
    public static string GetInterpolatedString(this string text, string s1, string s2)
    {
        int i1=text.IndexOf(s1)>=0? (text.IndexOf(s1)+s1.Length): -1;
        int i2=text.LastIndexOf(s2);
        return (i1>=0 && i2>=0)? text.Substring(i1, i2 - i1): "";
    }
    
    public static bool IsDir(this string word) => System.IO.Directory.Exists(word);
    public static bool IsFile(this string word) => System.IO.File.Exists(word);
    public static bool IsNumeric(this string word)
    {
        foreach(var ch in word)
        {
            if ("0123456789".Contains(ch) == false)
                return false;
        }
        return true;
    }
    public static bool IsFloat(this string word)
    {
        int c1 = word.GetCountChars('.');
        int c2 = word.GetCountChars(',');
        if ((c1 + c2) > 1)
            return false;
        if (word.StartsWith("."))
            return false;
        bool IsNegative = word.StartsWith(".");
        string numberStr = (IsNegative) ? word.Substring(1) : word;
        string[] parts = numberStr.GetSplitAny(".,").ToArray();
        if (parts.Length != 2)
            return false;
        string naturPart = parts[0];
        string decimalPart = parts[1];
        if (!naturPart.IsNumeric() || !decimalPart.IsNumeric())
            return false;
        return true;        
    }
        
    
    public static bool HasAllChars(this string test, string characters)
    {
        foreach (var ch in characters)
        {
            if (test.IndexOf(ch) == -1)
                return false;
        }
        return true;
    }
    public static int GetCountChars(this string text, char ch)
    {
        int ctn = 0;
        foreach (var p in text)
        {
            if (p == ch)
                ctn++;
        }
        return ctn;
    }


    public static bool IsType(this string text) => GetExecutingAssembly().GetTypeNames().Contains(text);
    public static string GetReplaceAll(this string text, string s1, string s2) => TextUtils.replaceAll(text, s1, s2);
    public static IEnumerable<string> GetSplitAny(this string text, string sep) => TextUtils.splitAny(text, sep);
    public static IList<string> GetSplitList(this string text, string sep) => new List<string>( TextUtils.split(text, sep) );
    public static IEnumerable<string> GetSplit(this string text, string sep) => TextUtils.split(text, sep);
    public static string GetConcat(this IEnumerable<string> items, string sep = " ") => TextUtils.concat(items, sep);
}
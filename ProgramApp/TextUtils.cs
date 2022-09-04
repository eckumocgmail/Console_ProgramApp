using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TextUtils
{          
        
    /// <summary>
    /// Разделяет тест любым симвыолов из подстроки переданной во второй аргумент
    /// </summary>
    public static IEnumerable<string> @splitAny(string text, string substring)
    {
        var result = new List<string>() { text };
                               
        foreach (char ch in substring.ToCharArray())
        {
            var arr = result.ToArray();
            result.Clear();
            foreach(var chunk in arr)
            {
                result.AddRange(chunk.Split(ch));
            }
                
        }                       
        return result.Where(s=>String.IsNullOrWhiteSpace(s)==false);
    }

    /// <summary>
    /// Разделяет тест через подстроку переданную во второй аргумент
    /// </summary>
    public static IEnumerable<string> @split(string text, string substring)
    {
        var list = new List<string>();
        int pText = 0;
        do
        {
            if (pText >= text.Length) continue;
            int p = text.Substring(pText).IndexOf(substring);                
            if (p > 0)
            {                    
                var word = text.Substring(pText).Substring(0, p);                
                list.Add(word);
            } 
            else
            {
                list.Add(text.Substring(pText));
                break;
            }
                
            pText = pText + p + substring.Length;
            if (pText == text.Length)
                break;
        } while (pText > 0 && pText < text.Length);
        return list;
    }

    /// <summary>
    /// Соединяет текст исп. разделитель
    /// </summary>
    public static string @concat(IEnumerable<string> items, string sep=" ")
    {
            
        string result = "";
        var arr = items.ToArray();
        for (int i = 0; i < arr.Count() - 1; i++)
        {
            result += arr[i] + sep;
        }
        result += arr[arr.Length - 1];

        return result;
    }


    /// <summary>
    /// Заменяет подстроки в тексте
    /// </summary>
    public static string @replaceAll(string text, string subtext, string newtext)
    {
        string result = text;
        while (result.IndexOf(subtext) >= 0)
            result = result.Replace(subtext, newtext);
        return result;
    }

    /// <summary>
    /// Возвращает индексы подстрок 
    /// </summary>
    public static IEnumerable<int> @indexes(string text, string substring)
    {
        var list = new List<int>();            
        int pText = 0;
        do
        {
            if (pText >= text.Length)
                continue;
            string subtext = text.Substring(pText);
            Console.WriteLine(subtext);
            int p = subtext.IndexOf(substring);
            if(p > 0)
            {
                list.Add(pText + p);
                pText = pText + p + substring.Length;
            }
            else if(p==-1)
            {
                break;
            }
                
                
        } while (pText > 0 && pText<text.Length);
        return list;
    }
} 
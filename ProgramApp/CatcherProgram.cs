
using System;

[assembly: CLSCompliant(true)]
namespace ConsoleApp
{
    using System;
    using System.IO;
    using System.Linq;
 
    using System.Reflection;
    using System.Collections.Generic;
    using static Newtonsoft.Json.JsonConvert;
    using static System.Reflection.Assembly;
    using static System.Convert;
    using static System.Environment;
    using static System.Console;

    public class CatcherProgram
    {

        public static List<List<string>> StackTraceLines
        {
            get
            {
                // делим на строки
                var list = new List<string>(
                    StackTrace.GetSplit("\r\n")
                );

                // удаляем System.Environment.get_StackTrace ...
                list.RemoveAt(1);
                list.RemoveAt(0);

                var result = new List<List<string>>();
                list.Where(line => line.StartsWith(typeof(CatcherProgram).FullName) == false) .ToList().ForEach(line =>
                {                         
                    string filename = "";
                    string linenumber = "";
                    foreach (var word in line.Trim().Replace("  ", " ").Split(' '))
                    {
                        if (word.EndsWith(":строка")|| word.EndsWith(":строка"))
                        {
                            string path = word.Substring(0, word.Length - ":строка".Length);
                            if (path.IsFile())
                            {
                                filename = path;
                            }
                            else
                            {
                                throw new Exception("Не удалось определить путь к исходному файлу из стэка");
                            }
                        }
                        if (word.IsNumeric())
                            linenumber = word;
                    }
                    WriteLine($"{filename} goto {linenumber}");
                });
                return result;
            }
        }


        public static void Trace()
        {
            foreach (var line in StackTraceLines)
                foreach (var word in line)
                    WriteLine(word);
        }


        public static void ToDo(params Action[] todos)
        {
            foreach (var todo in todos)
                todo();
        }

        /****************************************************
          1.Разработать на c# консольное приложение,        *
	        которое должно отслеживать появление новых      *
	        текстовых файлов в заданном каталоге.           */

        public static void Run(params string[] args)
        {
            var program = new CatcherProgram();
            foreach (var stack in CatcherProgram.StackTraceLines)
            {
                foreach(var line in stack)
                {
                    WriteLine(line);
                }
            }


        }
    }

                     
  


}


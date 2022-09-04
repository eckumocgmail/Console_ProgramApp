using System;
using static System.Console;
using static System.IO.File;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    /// <summary>
    /// Открывает локальный документ(ы)
    /// Указатель на документ будет получен через аргументы.    
    /// </summary>
    internal class SwitchProgram 
    {

        internal static int step = 1;


        // путь к файлу конфигурации, где прописаны прикладные программы 
        // вроде логично файл хранить на рабочем столе (в скрытом виде)
        private static string configutation = "console-app.settings.json";


        /// путь к рабочему столу
        private static string desctop = System.IO.Directory.GetCurrentDirectory();


        /// абсолютный путь к файлу конфигурации
        private static string filepath = Path.Combine(desctop, configutation);

        /// <summary>
        /// В качестве аргумента получает побытия от FileWatcher
        /// </summary>
        /// <param name="args">"ChangeType" "FileName" "FilePath"</param>
        internal static void RunAsConsoleApplication(params string[] args)
        {             
            ReadAssociations();
            foreach(var arg in args)
            {
                WriteLine("#" + step);
                switch (step)
                {
                    case 1:
                        if (associations.ContainsKey(arg) == false)
                            using (Stream sw = new MemoryStream())
                            {
                                WriteAssociationDefault(sw);
                                throw new ArgumentException("Первый аргумент может один из: ");
                            }
                        break;
                    default: throw new ArgumentException("Аргументы образуют не правильную последовательность");
                                

                }
            }
             
        }

        /// <summary>
        /// Разворачивает строки попарно, ключ-значение
        /// </summary>
        private static IDictionary<string, string> Get(string[] args, params string[] keys)
        {
            var resultset = new Dictionary<string, string>();
            if (args.Length != keys.Length)
                throw new ArgumentException("Неверное колличество аргументов");
            for (int i = 0; i < args.Length; i++)
            {
                resultset[keys[i]] = args[i];
            }            
            return resultset;
        }


        /// <summary>
        /// Конфигурация исполнителя
        /// </summary>
        static IDictionary<string, string> associations;
        static IDictionary<string, string> associationsDefaults = new Dictionary<string, string>()
        {
            { "text/html",
                "ConsoleApp1.exe \"exec \"" },
            { "text/css",
                "ConsoleApp1.exe \"css\"" },
            { "text/cs",
                "ConsoleApp1.exe \"cs\"" }           
        };


        /// <summary>
        /// Записыванием базовую конфигурацию на диск
        /// </summary>
        static void WriteAssociationDefault()
        {            
            var lines = new List<string>();
            foreach (var kv in associationsDefaults)
            {
                lines.Add(kv.Key);
                lines.Add(kv.Value);
            }
            lines.ForEach(WriteLine);
            
            WriteAllText(filepath, "");
        }


        // <summary>
        /// Записыванием базовую конфигурацию на диск
        /// </summary>
        static void WriteAssociationDefault(Stream stream)
        {
            WriteAssociationDefault(stream, Encoding.UTF8);
        }
        static void WriteAssociationDefault(Stream stream, Encoding enc)
        {
            foreach (var kv in associationsDefaults)
            {
                var bytes = enc.GetBytes(kv.Key + "\n");
                stream.Write(bytes,0,bytes.Length);
                bytes = enc.GetBytes(kv.Value);
                stream.Write(bytes, 0, bytes.Length);
            }
        }


        /// <summary>
        /// считывание конфигурации приложения
        /// </summary>
        private static void ReadAssociations()
        {

            // если файл конфигурации не найден, то программа пытается создать его в режи
            Dictionary<string, string> readedResult = new Dictionary<string, string>();
            if (System.IO.File.Exists(filepath) == false)
            {
                WriteAssociationDefault();
                associations = associationsDefaults;
            }
            else
            {                        
                if (associations!=null)
                    associations.Clear();

                string key = null;
                string value = null;
                int counter = 0;
                foreach (var line in System.IO.File.ReadAllLines(filepath))
                {

                    // чётные строки содержат паттерны, нечетные дальнейшие инструкции
                    if ((++counter) % 1 == 0)
                    {
                        key = line.ToLower();
                    }
                    else
                    {
                        associations[key] = value = line.ToLower();
                    }
                }


            }


        }
    }

    
}

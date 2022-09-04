using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;
using static System.Text.Json.JsonSerializer;
using System.Threading.Tasks;
using ConsoleApp;

internal class TextUtilsTests
{

    public static void Test()
    {

        CatcherProgram.Run();
        WriteLine(Serialize(TextUtils.@indexes("This is a test.", " ")));
        WriteLine(Serialize(TextUtils.splitAny("One,two,three fore.", " ,.")));
    }

  
}
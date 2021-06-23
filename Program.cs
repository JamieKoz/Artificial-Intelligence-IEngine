using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            //args contains:
            //  [0] - filename containing puzzle(s)
            //  [1] - method name


            string[] file = File.ReadAllLines(string.Format(args[0], args[1]));

            string tell = file[1];
            string ask = file[3];
            if (args[1].ToUpper() == "FC")
            {
                ForwardChaining fc = new(ask, tell);
                Console.WriteLine(fc.PrintSolution());
            }
            if (args[1].ToUpper() == "BC")
            {
                BackwardsChaining bc = new(ask, tell);

                Console.WriteLine(bc.PrintSolution());
            }
            if (args[1].ToUpper() == "TT")
            {
                TruthTable truthTable = new(ask, tell);

                Console.WriteLine(truthTable.PrintSolution());
            }
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet run -- ./<filename> <search-method>.");
                Environment.Exit(1);
            }
            if (args[1] != "FC" && args[1] != "BC" && args[1] != "TT")
            {
                Console.WriteLine("Invalid search function. Try using 'FC' or 'BC'.");
                Environment.Exit(1);
            }
        }

        public static string RemoveWhiteSpacesInFile(string fileName)
        {
            List<char> result = fileName.ToList();
            result.RemoveAll(c => c == ' ');
            fileName = new string(result.ToArray());
            return fileName;
            // string text = File.ReadAllText(fileName);
            // string cleanText = String.Concat(text.Where(c => !Char.IsWhiteSpace(c)));
            // return cleanText;
        }


    }
}

using MSExtension.Models;
using System.Collections.Generic;
using System.IO;

namespace MSExtension.CommandsGen
{
    public static class GenOutput
    {
        public static string GenerateOutput(MainCodeGenerator main, CodeGeneratorMethod method)
        {
            var output = new List<string>();
            output.Add("using Newtonsoft.Json;");
            output.Add("");
            output.Add($"namespace {main.BaseNamespace}.Arguments;");
            output.Add("");
            output.Add($"public class Output{method.MethodName}{main.BaseName}");
            output.Add("{");
            output.Add("}");

            return string.Join(Environment.NewLine, output);
        }

        public static void GenerateOutputFile(MainCodeGenerator main, CodeGeneratorMethod method, string output, string rootPath)
        {
            var filePath = $"{rootPath}.Arguments\\Arguments\\{main.BaseName}\\Output{method.MethodName}{main.BaseName}.cs";
            if (File.Exists(filePath))
            {
                return;
            }

            var pathOutput = $"{rootPath}.Arguments\\Arguments\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathOutput);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathOutput);

            using (StreamWriter sw = new StreamWriter($"{pathOutput}\\Output{method.MethodName}{main.BaseName}.cs"))
            {
                sw.WriteLine(output);
            }
        }
    }
}

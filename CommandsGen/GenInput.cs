using System.Collections.Generic;
using System.IO;
using System.Text;
using MSExtension.Models;

namespace MSExtension.CommandsGen
{
    public static class GenInput
    {
        public static string GenerateInput(CodeGeneratorClassParam param)
        {
            var input = new List<string>();

            input.Add("using Newtonsoft.Json;");
            input.Add("");
            input.Add($"public class {param.ParamType}");
            input.Add("{");
            input.Add("}");

            return string.Join(Environment.NewLine, input);
        }

        public static void GenerateInputFile(MainCodeGenerator main, CodeGeneratorClassParam param, string input, string rootPath)
        {
            var filePath = $"{rootPath}.Arguments\\Arguments\\{main.BaseName}\\{param.ParamType}.cs";
            if (File.Exists(filePath))
            {
                return;
            }

            var pathInput = $"{rootPath}.Arguments\\Arguments\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathInput);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathInput);

            using (StreamWriter sw = new StreamWriter($"{pathInput}\\{param.ParamType}.cs"))
            {
                sw.WriteLine(input);
            }
        }
    }
}

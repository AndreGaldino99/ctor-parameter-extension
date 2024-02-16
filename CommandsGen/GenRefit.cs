using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MSExtension.CommandsGen.Utils;
using MSExtension.Models;

namespace MSExtension.CommandsGen
{
    public static class GenRefit
    {
        public static string GenerateIRefit(MainCodeGenerator main)
        {
            var iRefit = new List<string>();
            iRefit.Add("using Refit;");

            iRefit.Add("");

            iRefit.Add($"namespace {main.BaseNamespace}.ApiClient.RefitInterfaces;");
            iRefit.Add($"public interface I{main.BaseName}Refit");
            iRefit.Add("{");

            foreach (var m in main.Method)
            {
                iRefit.Add($"//{m.MethodDesc}");
                iRefit.Add($"[{m.Method.ToString().Replace("Http", "")}(\"{m.RefitRoute}\")]");
                iRefit.Add($"Task<ApiResponse<string>> {m.MethodName}({ParamGenerator.GetParams(m.Params, true)});");
            }

            iRefit.Add("}");

            return string.Join(Environment.NewLine, iRefit);
        }

        public static void GenerateRefitFile(MainCodeGenerator main, string refit, string rootPath)
        {
            var pathRefit = $"{rootPath}.ApiClient\\RefitInterfaces\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathRefit);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathRefit);

            using (StreamWriter sw = new StreamWriter($"{pathRefit}\\I{main.BaseName}Refit.cs"))
            {
                sw.WriteLine(refit);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MSExtension.CommandsGen.Base;
using MSExtension.CommandsGen.Utils;
using MSExtension.Models;

namespace MSExtension.CommandsGen
{
    public static class GenRefit
    {
        public static string GenerateIRefit(MainCodeGenerator main, string rootPath)
        {
            var pathRefit = $"{rootPath}.ApiClient\\RefitInterfaces\\{main.BaseName}\\I{main.BaseName}Refit.cs";

            if (!File.Exists(pathRefit))
            {
                var iRefit = new List<string>
                {
                    "using Refit;",
                    "",
                    $"namespace {main.BaseNamespace}.ApiClient.RefitInterfaces;",
                    $"public interface I{main.BaseName}Refit",
                    "{"
                };

                iRefit.AddRange(InsertMethods(main));

                iRefit.Add("}");

                return string.Join(Environment.NewLine, iRefit);
            }
            else
            {
                var conteudo = File.ReadAllText(pathRefit);
                return BaseGen.PushContentBeforeLastOccurrence(conteudo, '}', string.Join(Environment.NewLine, InsertMethods(main)));
            }

        }

        private static List<string> InsertMethods(MainCodeGenerator main)
        {
            List<string> iRefit = new();
            foreach (var m in main.Method)
            {
                iRefit.Add($"    //{m.MethodDesc}");
                iRefit.Add($"    [{m.Method.ToString().Replace("Http", "")}(\"{m.RefitRoute}\")]");
                iRefit.Add($"    Task<ApiResponse<string>> {m.MethodName}({ParamGenerator.GetParams(m.Params, true)});");
            }
            return iRefit;
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

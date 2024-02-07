using System.Collections;
using System.Collections.Generic;
using System.Text;
using VSIXProject1.CommandsGen.Utils;
using VSIXProject1.Models;

namespace VSIXProject1.CommandsGen
{
    public static class GenRefit
    {
        public static string GenerateIRefit(MainCodeGenerator main)
        {
            var iRefit = new List<string>();
            iRefit.Add($"namespace {main.BaseNamespace}.ApiClient.RefitInterfaces;");
            iRefit.Add($"public interface I{main.BaseName}Refit");
            iRefit.Add("{");

            foreach (var m in main.Method)
            {
                iRefit.Add($"//{m.MethodDesc}");
                iRefit.Add($"[{m.Method}(\"{m.RefitRoute}\")]");
                iRefit.Add($"Task<ApiResponse<string>> {m.MethodName}({ParamGenerator.GetParams(m.Params, true)});");
            }

            iRefit.Add("}");

            return string.Join(Environment.NewLine, iRefit);
        }
    }
}

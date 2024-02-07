using System.Collections.Generic;
using VSIXProject1.Models;

namespace VSIXProject1.CommandsGen.Utils
{
    public static class ParamGenerator
    {
        public static string GetParams(List<CodeGeneratorClassParam> parameter, bool genRefitAnnotations = false)
        {
            var listP = new List<string>();
            foreach (var p in parameter)
            {
                listP.Add($"{(p.isQuery && genRefitAnnotations ? $"[Query(\"{p.ParamName}\")]" : "")}{p.ParamType} {p.ParamName}");
            }
            string result = string.Join(",", listP);
            return result;
        }
    }
}
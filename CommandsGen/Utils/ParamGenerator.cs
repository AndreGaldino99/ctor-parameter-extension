using MSExtension.Models;
using System.Collections.Generic;

namespace MSExtension.CommandsGen.Utils
{
    public static class ParamGenerator
    {
        public static string GetParams(List<CodeGeneratorClassParam> parameter, bool genRefitAnnotations = false)
        {
            var listP = new List<string>();
            foreach (var p in parameter)
            {
                if (p.isHeader && p.isQuery)
                {
                    throw new Exception("Parâmentro não pode ser do tipo query e header ao mesmo tempo");
                }
                if (!p.isHeader)
                {
                    listP.Add($"{(p.isQuery && genRefitAnnotations ? $"[Query(\"{(!string.IsNullOrEmpty(p.QueryParamName) ? p.QueryParamName : p.ParamName)}\")]" : "")}{p.ParamType} {p.ParamName}");
                }
                else
                {
                    if (genRefitAnnotations)
                    {
                        listP.Add($"{(p.isQuery && genRefitAnnotations ? $"[Query(\"{(!string.IsNullOrEmpty(p.QueryParamName) ? p.QueryParamName : p.ParamName)}\")]" : "")}{p.ParamType} {p.ParamName}");
                    }
                }
            }
            string result = string.Join(",", listP);
            return result;
        }

        public static string GetParamsWithoutType(List<CodeGeneratorClassParam> parameter)
        {
            var listP = new List<string>();
            foreach (var p in parameter)
            {
                listP.Add($"{p.ParamName}");
            }
            string result = string.Join(",", listP);
            return result;
        }
    }
}
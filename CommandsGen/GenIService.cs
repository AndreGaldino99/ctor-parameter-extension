using System.Collections.Generic;
using System.IO;
using System.Text;
using MSExtension.CommandsGen.Base;
using MSExtension.CommandsGen.Utils;
using MSExtension.Models;

namespace MSExtension.CommandsGen
{
    public static class GenIService
    {
        public static string GenerateIService(MainCodeGenerator main, string rootPath)
        {
            var pathIService = $"{rootPath}.Domain\\Interfaces\\{main.BaseName}\\I{main.BaseName}Service.cs";
            if (!File.Exists(pathIService))
            {
                var iService = new List<string>();
                iService.Add($"using Harmonit.Microservice.Base.Library.BaseService;");
                iService.Add($"using {main.BaseNamespace}.Arguments;");
                iService.Add($"using {main.BaseNamespace}.Domain.ApiResponse;");
                iService.Add($"");

                iService.Add($"namespace {main.BaseNamespace}.Domain.Interfaces;");
                iService.Add($"public interface I{main.BaseName}Service : IBaseService_0");
                iService.Add("{");

                iService.AddRange(InsertMethods(main));

                iService.Add("}");

                return string.Join(Environment.NewLine, iService);
            }
            else
            {
                var conteudo = File.ReadAllText(pathIService);
                return BaseGen.PushContentBeforeLastOccurrence(conteudo, '}', string.Join(Environment.NewLine, InsertMethods(main)));
            }
        }

        private static List<string> InsertMethods(MainCodeGenerator main)
        {
            List<string> iService = new();
            foreach (var m in main.Method)
            {
                iService.Add($"Task<BaseResponseApiContent<List<Output{m.MethodName}{main.BaseName}>, ApiResponseException>> {m.MethodName}({ParamGenerator.GetParams(m.Params)});");
            }
            return iService;
        }

        public static void GenerateIServiceFile(MainCodeGenerator main, string iService, string rootPath)
        {
            var pathIService = $"{rootPath}.Domain\\Interfaces\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathIService);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathIService);

            using (StreamWriter sw = new StreamWriter($"{pathIService}\\I{main.BaseName}Service.cs"))
            {
                sw.WriteLine(iService);
            }
        }
    }
}

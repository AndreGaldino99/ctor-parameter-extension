using MSExtension.CommandsGen.Utils;
using MSExtension.Models;
using System.Collections.Generic;
using System.IO;

namespace MSExtension.CommandsGen
{
    public static class GenService
    {
        public static string GenerateService(MainCodeGenerator main)
        {
            var service = new List<string>();
            service.Add("using Harmonit.Microservice.Base.Library.BaseService;");
            service.Add($"using {main.BaseNamespace}.ApiClient.RefitInterfaces;");
            service.Add($"using {main.BaseNamespace}.Arguments;");
            service.Add($"using {main.BaseNamespace}.Domain.ApiResponse;");
            service.Add($"using {main.BaseNamespace}.Domain.Interfaces;");
            service.Add("");

            service.Add($"namespace {main.BaseNamespace}.Domain.Services;");
            service.Add($"public class {main.BaseName}Service(I{main.BaseName}Refit refit) : BaseService_1<I{main.BaseName}Refit>(refit), I{main.BaseName}Service");
            service.Add("{");

            foreach (var m in main.Method)
            {
                service.Add($"public async Task<BaseResponseApiContent<List<Output{m.MethodName}{main.BaseName}>, ApiResponseException>> {m.MethodName}({ParamGenerator.GetParams(m.Params)})");
                service.Add("{");
                service.Add($"var response = await _refit!.{m.MethodName}({ParamGenerator.GetParamsWithoutType(m.Params)});");
                if (m.ReturnResponseIsList)
                {
                    service.Add($"return ReturnResponse<List<Output{m.MethodName}{main.BaseName}>, ApiResponseException>(response)!;");
                }
                else
                {
                    service.Add($"return ReturnResponse<Output{m.MethodName}{main.BaseName}, ApiResponseException>(response)!;");
                }
                service.Add("}");
            }

            service.Add("}");

            return string.Join(Environment.NewLine, service);
        }

        public static void GenerateServiceFile(MainCodeGenerator main, string service, string rootPath)
        {
            var pathService = $"{rootPath}.Domain\\Services\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathService);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathService);

            using (StreamWriter sw = new StreamWriter($"{pathService}\\{main.BaseName}Service.cs"))
            {
                sw.WriteLine(service);
            }
        }
    }
}

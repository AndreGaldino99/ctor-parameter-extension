﻿using Community.VisualStudio.Toolkit;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSIXProject1.CommandsGen.Utils;
using VSIXProject1.Models;

namespace VSIXProject1.CommandsGen
{
    public static class GenService
    {
        public static string GenerateService(MainCodeGenerator main)
        {
            var service = new List<string>();
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
            var pathController = $"{rootPath}.Domain\\Services\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathController);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathController);

            using (StreamWriter sw = new StreamWriter($"{pathController}\\{main.BaseName}Service.cs"))
            {
                sw.WriteLine(service);
            }
        }
    }
}

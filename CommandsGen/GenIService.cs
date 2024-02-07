using System.Collections.Generic;
using System.IO;
using System.Text;
using VSIXProject1.CommandsGen.Utils;
using VSIXProject1.Models;

namespace VSIXProject1.CommandsGen
{
    public static class GenIService
    {
        public static string GenerateIService(MainCodeGenerator main)
        {
            var iService = new List<string>();
            iService.Add($"namespace {main.BaseNamespace}.Domain.Interfaces;");
            iService.Add($"public interface I{main.BaseName}Service : IBaseService_0");
            iService.Add("{");

            foreach (var m in main.Method)
            {
                iService.Add($"Task<BaseResponseApiContent<List<Output{m.MethodName}{main.BaseName}>, ApiResponseException>> {m.MethodName}({ParamGenerator.GetParams(m.Params)});");
            }

            iService.Add("}");

            return string.Join(Environment.NewLine, iService);
        }

        public static void GenerateIServiceFile(MainCodeGenerator main, string iService, string rootPath)
        {
            var pathController = $"{rootPath}.Domain\\Interfaces\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathController);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathController);

            using (StreamWriter sw = new StreamWriter($"{pathController}\\I{main.BaseName}Service.cs"))
            {
                sw.WriteLine(iService);
            }
        }
    }
}

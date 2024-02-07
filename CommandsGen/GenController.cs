using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using VSIXProject1.CommandsGen.Utils;
using VSIXProject1.Models;
using VSIXProject1.Models.Utils;

namespace VSIXProject1.CommandsGen
{
    public static class GenController
    {
        public static string GenerateController(MainCodeGenerator main)
        {
            var controller = new List<string>();
            controller.Add($"namespace {main.BaseNamespace}.Api.Controllers;");
            controller.Add("");
            controller.Add("[Route(\"api/v1/[controller]\")]");
            controller.Add($"public class {main.BaseName}Controller(IApiDataService apiDataService, I{main.BaseName}Service service) : BaseController_1<I{main.BaseName}Service>(apiDataService, service)");
            controller.Add("{");

            foreach (var m in main.Method)
            {
                try
                {
                    var languageDescriptionPortuguese = m.MethodDesc;
                    var languageDescriptionEnglish = JsonConvert.DeserializeObject<TranslatedMessage>(TranslateService.TraduzirMyMemory(m.MethodDesc, "en"))?.matches?.FirstOrDefault()?.translation;
                    var languageDescriptionSpanish = JsonConvert.DeserializeObject<TranslatedMessage>(TranslateService.TraduzirMyMemory(m.MethodDesc, "es"))?.matches?.FirstOrDefault()?.translation;
                    controller.Add($"[LanguageDescription(\"pt-br\", \"{languageDescriptionPortuguese}\")]");
                    controller.Add($"[LanguageDescription(\"en\", \"{languageDescriptionEnglish}\")]");
                    controller.Add($"[LanguageDescription(\"es\", \"{languageDescriptionSpanish}\")]");
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Falha ao utilizar serviço de tradução: {ex.Message}");
                }
                controller.Add($"[ProducesResponseType<Output{m.MethodName}{main.BaseName}>(StatusCodes.{m.SuccessCode})]");
                foreach (var e in m.ErrorsCode)
                {
                    controller.Add($"[ProducesResponseType(StatusCodes.{e})]");
                }
                controller.Add($"[{m.Method}(\"{m.MethodName}\")]");
                if (m.ReturnResponseIsList)
                {
                    controller.Add($"public async Task<ActionResult<BaseResponseApi<List<Output{m.MethodName}{main.BaseName}>, ApiResponseException>>> {m.MethodName}({ParamGenerator.GetParams(m.Params)})");
                }
                else
                {
                    controller.Add($"public async Task<ActionResult<BaseResponseApi<Output{m.MethodName}{main.BaseName}, ApiResponseException>>> {m.MethodName}({ParamGenerator.GetParams(m.Params)})");
                }
                controller.Add("{");
                controller.Add("try");
                controller.Add("{");
                if (m.ReturnResponseIsList)
                {
                    controller.Add($"return await ResponseAsync(PrepareListReturn(await _service!.{m.MethodName}({ParamGenerator.GetParamsWithoutType(m.Params)})));");
                }
                else
                {
                    controller.Add($"return await ResponseAsync(PrepareReturn(await _service!.{m.MethodName}({ParamGenerator.GetParamsWithoutType(m.Params)})));");
                }
                controller.Add("}");
                controller.Add("catch (BaseResponseException ex)");
                controller.Add("{");
                controller.Add("return await BaseResponseExceptionAsync(ex);");
                controller.Add("}");
                controller.Add("catch (Exception ex)");
                controller.Add("{");
                controller.Add("return await ResponseExceptionAsync(ex);");
                controller.Add("}");
                controller.Add("}");

            }

            controller.Add("}");

            return string.Join(Environment.NewLine, controller);
        }

        public static void GenerateControllerFile(MainCodeGenerator main, string controller, string rootPath)
        {
            var pathController = $"{rootPath}.Api\\Controllers\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathController);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathController);

            using (StreamWriter sw = new StreamWriter($"{pathController}\\{main.BaseName}Controller.cs"))
            {
                sw.WriteLine(controller);
            }
        }
    }
}

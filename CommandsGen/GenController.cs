using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using MSExtension.CommandsGen.Utils;
using MSExtension.Models;
using MSExtension.Models.Utils;
using MSExtension.CommandsGen.Base;

namespace MSExtension.CommandsGen
{
    public static class GenController
    {
        public static string GenerateController(MainCodeGenerator main, string rootPath)
        {
            var pathController = $"{rootPath}.Api\\Controllers\\{main.BaseName}\\{main.BaseName}Controller.cs";
            if (!File.Exists(pathController))
            {
                var controller = new List<string>
                {
                    $"using Harmonit.Microservice.Base.Library.BaseController;",
                    $"using Harmonit.Microservice.Base.Library.Generic;",
                    $"using {main.BaseNamespace}.Arguments;",
                    $"using {main.BaseNamespace}.Domain.ApiResponse;",
                    $"using {main.BaseNamespace}.Domain.Interfaces;",
                    $"using Harmonit.Notifications.Arguments;",
                    $"using Microsoft.AspNetCore.Mvc;",
                    "",
                    $"namespace {main.BaseNamespace}.Api.Controllers;",
                    "",
                    $"[Route(\"api/{main.MicroServiceName}/[controller]\")]",
                    $"public class {main.BaseName}Controller(IApiDataService apiDataService, I{main.BaseName}Service service) : BaseController_1<I{main.BaseName}Service>(apiDataService, service)",
                    "{"
                };

                controller.AddRange(InsertMethods(main));

                controller.Add("}");

                return string.Join(Environment.NewLine, controller);
            }
            else
            {
                var conteudo = File.ReadAllText(pathController);
                return BaseGen.PushContentBeforeLastOccurrence(conteudo, '}', string.Join(Environment.NewLine, InsertMethods(main)));
            }
        }

        private static List<string> InsertMethods(MainCodeGenerator main)
        {
            List<string> controller = new();
            foreach (var m in main.Method)
            {
                try
                {
                    var languageDescriptionPortuguese = m.MethodDesc;
                    if (main.GenerateTranslations)
                    {
                        var languageDescriptionEnglish = JsonConvert.DeserializeObject<TranslatedMessage>(TranslateService.TraduzirMyMemory(m.MethodDesc, "en"))?.matches?.FirstOrDefault()?.translation;
                        var languageDescriptionSpanish = JsonConvert.DeserializeObject<TranslatedMessage>(TranslateService.TraduzirMyMemory(m.MethodDesc, "es"))?.matches?.FirstOrDefault()?.translation;
                        controller.Add($"    [LanguageDescription(\"pt-br\", \"{languageDescriptionPortuguese}\")]");
                        controller.Add($"    [LanguageDescription(\"en\", \"{languageDescriptionEnglish}\")]");
                        controller.Add($"    [LanguageDescription(\"es\", \"{languageDescriptionSpanish}\")]");
                    }
                    else
                    {
                        controller.Add($"    [LanguageDescription(\"pt-br\", \"{languageDescriptionPortuguese}\")]");
                        controller.Add($"    [LanguageDescription(\"en\", \"\")]");
                        controller.Add($"    [LanguageDescription(\"es\", \"\")]");
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Falha ao utilizar serviço de tradução: {ex.Message}");
                }
                controller.Add($"    [ProducesResponseType<Output{m.MethodName}{main.BaseName}>(StatusCodes.{m.SuccessCode})]");
                foreach (var e in m.ErrorsCode)
                {
                    controller.Add($"    [ProducesResponseType(StatusCodes.{e})]");
                }
                controller.Add($"    [{m.Method}(\"{m.MethodName}\")]");
                if (m.ReturnResponseIsList)
                {
                    controller.Add($"    public async Task<ActionResult<BaseResponseApi<List<Output{m.MethodName}{main.BaseName}>, ApiResponseException>>> {m.MethodName}({ParamGenerator.GetParams(m.Params)})");
                }
                else
                {
                    controller.Add($"    public async Task<ActionResult<BaseResponseApi<Output{m.MethodName}{main.BaseName}, ApiResponseException>>> {m.MethodName}({ParamGenerator.GetParams(m.Params)})");
                }
                controller.Add("    {");
                controller.Add("       try");
                controller.Add("       {");
                if (m.ReturnResponseIsList)
                {
                    controller.Add($"          return await ResponseAsync(PrepareListReturn(await _service!.{m.MethodName}({ParamGenerator.GetParamsWithoutType(m.Params)})));");
                }
                else
                {
                    controller.Add($"          return await ResponseAsync(PrepareReturn(await _service!.{m.MethodName}({ParamGenerator.GetParamsWithoutType(m.Params)})));");
                }
                controller.Add("       }");
                controller.Add("       catch (BaseResponseException ex)");
                controller.Add("       {");
                controller.Add("          return await BaseResponseExceptionAsync(ex);");
                controller.Add("       }");
                controller.Add("       catch (Exception ex)");
                controller.Add("       {");
                controller.Add("          return await ResponseExceptionAsync(ex);");
                controller.Add("       }");
                controller.Add("    }");

            }
            return controller;
        }

        public static void GenerateControllerFile(MainCodeGenerator main, string controller, string rootPath)
        {
            var pathController = $"{rootPath}.Api\\Controllers\\{main.BaseName}";
            bool exists = System.IO.Directory.Exists(pathController);
            if (!exists)
                System.IO.Directory.CreateDirectory(pathController);

            using (StreamWriter sw = new($"{pathController}\\{main.BaseName}Controller.cs"))
            {
                sw.WriteLine(controller);
            }
        }
    }
}

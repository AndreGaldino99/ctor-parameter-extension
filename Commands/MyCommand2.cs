
using Community.VisualStudio.Toolkit;
using EnvDTE;
using System.Collections.Generic;
using System.IO;
using VSIXProject1.CommandsGen;
using VSIXProject1.CommandsGen.Utils;
using VSIXProject1.Models;

namespace VSIXProject1
{
    [Command(PackageIds.MyCommand2)]
    internal sealed class MyCommand2 : BaseCommand<MyCommand2>
    {
        protected override void Execute(object sender, EventArgs e)
        {
            var main = new MainCodeGenerator
            {
                BaseName = "Billet",
                BaseNamespace = "Harmonit.Microservice.PjBank",
                GenerateRoot = true,
                Method = new List<CodeGeneratorMethod>
                {
                    new CodeGeneratorMethod
                    {
                        ErrorsCode = new List<HttpStatusCode>
                        {
                            HttpStatusCode.Status400BadRequest,
                            HttpStatusCode.Status404NotFound,
                            HttpStatusCode.Status401Unauthorized
                        },
                        Method = EnumMethod.HttpGet,
                        ReturnResponseIsList = true,
                        MethodDesc = "Consultar boletos por recebimento para quem não tem conta digital",
                        MethodName = "GetAllReceiptNoDigitalAccount",
                        Params = new List<CodeGeneratorClassParam>
                        {
                            new CodeGeneratorClassParam { isQuery = true, QueryParamName = "data_inicio", ParamName = "initialDateMMDDYYYY", ParamType = "string?" },
                            new CodeGeneratorClassParam { isQuery = true, QueryParamName = "data_fim", ParamName = "finalDateMMDDYYYY", ParamType = "string?" },
                            new CodeGeneratorClassParam { isQuery = true, QueryParamName = "pago", ParamName = "paid", ParamType = "bool?" },
                            new CodeGeneratorClassParam { isQuery = true, QueryParamName = "pagina", ParamName = "page", ParamType = "string?" }
                        },
                        RefitRoute = "/recebimentos/{credencialBoleto}/transacoes",
                        SuccessCode = HttpStatusCode.Status200OK
                    }
                }
            };

            var controller = GenController.GenerateController(main);
            var iService = GenIService.GenerateIService(main);
            var service = GenService.GenerateService(main);
            var refit = GenRefit.GenerateIRefit(main);
            GenInput.GenerateInput(main);
            GenOutput.GenerateOutput(main);

            var rootPath = "C:\\Temp";

            if (main.GenerateRoot)
            {
                DocumentView docView = VS.Documents.GetActiveDocumentViewAsync().Result;
                var filePath = docView.FilePath;
                var solutionPath = filePath.GetFilePath();
                DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
                var solutionName = System.IO.Path.GetFileNameWithoutExtension(dte.Solution.FullName);
                rootPath = $"{solutionPath}\\{solutionName}";

            }
            GenController.GenerateControllerFile(main, controller, rootPath);
            GenIService.GenerateIServiceFile(main, iService, rootPath);
            GenService.GenerateServiceFile(main, service, rootPath);
            GenRefit.GenerateRefitFile(main, refit, rootPath);
        }
    }
}
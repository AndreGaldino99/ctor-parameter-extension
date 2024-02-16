using EnvDTE;
using MSExtension.CommandsGen;
using MSExtension.CommandsGen.Utils;
using MSExtension.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MSExtension
{
    [Command(PackageIds.CommandGenerateMSClasses)]
    internal sealed class CommandGenerateMSClasses : BaseCommand<CommandGenerateMSClasses>
    {
        protected override void Execute(object sender, EventArgs e)
        {
            DocumentView docView = VS.Documents.GetActiveDocumentViewAsync()?.Result;
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

            var fileContent = File.ReadAllText(docView.FilePath);

            var mainList = JsonConvert.DeserializeObject<List<MainCodeGenerator>>(fileContent);

            foreach (var main in mainList)
            {

                var rootPath = "C:\\Temp";
                if (main.GenerateRoot)
                {
                    var filePath = docView.FilePath;
                    var solutionPath = filePath.GetFilePath();
                    var solutionName = Path.GetFileNameWithoutExtension(dte.Solution.FullName);
                    rootPath = $"{solutionPath}\\{solutionName}";
                }

                var controller = GenController.GenerateController(main);
                var iService = GenIService.GenerateIService(main);
                var service = GenService.GenerateService(main);
                var refit = GenRefit.GenerateIRefit(main);

                var listInput = new List<(string Input, CodeGeneratorClassParam Param)>();
                var listOutput = new List<(string Output, CodeGeneratorMethod Method)>();
                foreach (var method in main.Method)
                {
                    listOutput.Add(new(GenOutput.GenerateOutput(main, method),method));
                    foreach (var param in method.Params?.Where(x=>x.isObject))
                    {
                        listInput.Add(new(GenInput.GenerateInput(param), param));
                    }
                }

                GenController.GenerateControllerFile(main, controller, rootPath);
                GenIService.GenerateIServiceFile(main, iService, rootPath);
                GenService.GenerateServiceFile(main, service, rootPath);
                GenRefit.GenerateRefitFile(main, refit, rootPath);

                foreach (var (Input, Param) in listInput)
                {
                    GenInput.GenerateInputFile(main, Param, Input, rootPath);
                }
                foreach (var (Output, Method) in listOutput)
                {
                    GenOutput.GenerateOutputFile(main, Method, Output, rootPath);
                }
            }
        }
    }
}
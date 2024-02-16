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
                string rootPath = GetRootPath(docView, dte, main);

                main.Method = DefineMethodsToApplication(main, rootPath);

                string controller, iService, service, refit;
                List<(string Input, CodeGeneratorClassParam Param)> listInput;
                List<(string Output, CodeGeneratorMethod Method)> listOutput;
                GenerateContentClasses(main, rootPath, out controller, out iService, out service, out refit, out listInput, out listOutput);

                GenerateFileClasses(main, rootPath, controller, iService, service, refit, listInput, listOutput);
            }
        }

        private static string GetRootPath(DocumentView docView, DTE dte, MainCodeGenerator main)
        {
            var rootPath = "C:\\Temp";
            if (main.GenerateRoot)
            {
                var filePath = docView.FilePath;
                var solutionPath = filePath.GetFilePath();
                var solutionName = Path.GetFileNameWithoutExtension(dte.Solution.FullName);
                rootPath = $"{solutionPath}\\{solutionName}";
            }

            return rootPath;
        }

        private static void GenerateFileClasses(MainCodeGenerator main, string rootPath, string controller, string iService, string service, string refit, List<(string Input, CodeGeneratorClassParam Param)> listInput, List<(string Output, CodeGeneratorMethod Method)> listOutput)
        {
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

        private static void GenerateContentClasses(MainCodeGenerator main, string rootPath, out string controller, out string iService, out string service, out string refit, out List<(string Input, CodeGeneratorClassParam Param)> listInput, out List<(string Output, CodeGeneratorMethod Method)> listOutput)
        {
            controller = GenController.GenerateController(main, rootPath);
            iService = GenIService.GenerateIService(main, rootPath);
            service = GenService.GenerateService(main, rootPath);
            refit = GenRefit.GenerateIRefit(main, rootPath);
            listInput = new List<(string Input, CodeGeneratorClassParam Param)>();
            listOutput = new List<(string Output, CodeGeneratorMethod Method)>();
            foreach (var method in main.Method)
            {
                listOutput.Add(new(GenOutput.GenerateOutput(main, method), method));
                foreach (var param in method.Params?.Where(x => x.isObject))
                {
                    listInput.Add(new(GenInput.GenerateInput(param), param));
                }
            }
        }

        private static List<CodeGeneratorMethod> DefineMethodsToApplication(MainCodeGenerator main, string rootPath)
        {
            var pathRefit = $"{rootPath}.ApiClient\\RefitInterfaces\\{main.BaseName}\\I{main.BaseName}Refit.cs";
            if (File.Exists(pathRefit))
            {
                var fileContent = File.ReadAllText(pathRefit);

                var methods = main.Method;

                var listRemove = new List<CodeGeneratorMethod>();

                foreach (var method in methods)
                {
                    if (fileContent.Contains($"{method.MethodName}("))
                    {
                        listRemove.Add(method);
                    }
                }

                foreach (var method in listRemove)
                {
                    methods.Remove(method);
                }

                return methods;
            }
            else
            {
                return main.Method;
            }
        }
    }
}
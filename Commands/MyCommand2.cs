
using EnvDTE;
using System.Collections.Generic;
using VSIXProject1.CommandsGen;
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
                BaseName = "Teste",
                BaseNamespace = "MSHarmonit.Teste",
                GenerateRoot = false,
                Method = new List<CodeGeneratorMethod>
                {
                    new CodeGeneratorMethod
                    {
                        ErrorsCode = new List<int> {400,404,401},
                        Method = EnumMethod.HttpGet,
                        MethodDesc = "Teste Descricao",
                        MethodName = "TestMethod",
                        Params = new List<CodeGeneratorClassParam>
                        {
                            new CodeGeneratorClassParam
                            {
                                isQuery = true,
                                ParamName = "query",
                                ParamType = "string",
                                isObject = false
                            }
                        },
                        RefitRoute = "/api/code/",
                        SuccessCode = 200
                    }
                }
            };

            GenController.GenerateController(main);
            GenIService.GenerateIService(main);
            GenService.GenerateService(main);
            var refit = GenRefit.GenerateIRefit(main);
            GenInput.GenerateInput(main);
            GenOutput.GenerateOutput(main);
        }
    }
}
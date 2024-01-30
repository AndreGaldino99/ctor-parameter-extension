using EnvDTE;
using System.Collections.Generic;
using System.Linq;

namespace VSIXProject1
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {


        protected override void Execute(object sender, EventArgs e)
        {
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

            CodeClass codeClass = GetActiveClass(dte);

            if (codeClass != null)
            {
                List<(string, string, string)> lista = new List<(string, string, string)> ();
                foreach (CodeProperty property in codeClass.Members.OfType<CodeProperty>())
                {
                    string propertyName = property.Name;
                    var propertyType = property.Type;
                    string typeName = propertyType.AsFullName;
                    string shortTypeName = propertyType.AsString;

                    lista.Add((propertyName, typeName, shortTypeName));
                }
            }
        }

        private static CodeClass GetActiveClass(DTE dte)
        {
            TextSelection selection = (TextSelection)dte.ActiveDocument.Selection;
            CodeElement codeElement = selection.ActivePoint.CodeElement[vsCMElement.vsCMElementClass];

            if (codeElement is CodeClass codeClass)
            {
                return codeClass;
            }

            return null;
        }
    }
}
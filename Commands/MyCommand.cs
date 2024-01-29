using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace VSIXProject1
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {

        protected override void Execute(object sender, EventArgs e)
        {
            DocumentView docView = VS.Documents.GetActiveDocumentViewAsync().Result;

            var retorno = CDLL.CompilarDll(docView.FilePath);

            if (docView?.TextView == null) return;
            SnapshotPoint position = docView.TextView.Caret.Position.BufferPosition;
            docView.TextBuffer?.Insert(position, retorno.ToString());
        }
    }

    public static class CDLL
    {
        public static StringBuilder CompilarDll(string filePath)
        {
            var guid = Guid.NewGuid();

            string codigoFonte = "using System; " + System.IO.File.ReadAllText(filePath);
            string pastaSaidaCompilacao = "C:\\Temp";

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parametros = new CompilerParameters();

            #region References
            
            string myPath = "C:\\Users\\Andre Galdino\\source\\repos\\";
            string tempPath = "C:\\Temp\\";
            parametros.ReferencedAssemblies.Add($"{tempPath}System.dll");
            parametros.ReferencedAssemblies.Add($"{tempPath}System.Runtime.dll");
            parametros.ReferencedAssemblies.Add($"{myPath}Import.Movimentacao\\bin\\Debug\\net6.0\\Import.Movimentacao.dll");

            #endregion References

            parametros.OutputAssembly = $"{pastaSaidaCompilacao}\\SysthExtension{guid}.dll";

            CompilerResults resultados = provider.CompileAssemblyFromSource(parametros, codigoFonte);

            Assembly teste = Assembly.LoadFrom($"{pastaSaidaCompilacao}\\SysthExtension{guid}.dll");


            var classType = teste.GetExportedTypes().FirstOrDefault();

            var listConstructorProperty = new List<string>();
            foreach (var type in classType.GetProperties())
            {
                var pt = string.Empty;
                var pn = string.Empty;
                try
                {
                    pt = GetTypeName(type.PropertyType);
                }
                catch (Exception e)
                {
                    var listalinhas = codigoFonte.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                    var x = listalinhas?.Where(x => x.Contains(type.Name))?.FirstOrDefault()?.Trim()?.Split()?.ToList() ?? new List<string>();

                    var index = x.IndexOf(type.Name);

                    pt = !string.IsNullOrEmpty(x[index-1]) ? x[index-1] : type.Name;   
                }
                try
                {
                    pn = GetPropertyName(type.Name);
                }
                catch (Exception e)
                {
                    pn = "";
                }

                listConstructorProperty.Add($"{pt} {pn}");
            }


            var listBodyConstructorProperty = new List<string>();
            foreach (var type in classType.GetProperties())
            {
                var pt = string.Empty;
                
                try
                {
                    pt = $"{type.Name} = {GetPropertyName(type.Name)};";
                }
                catch (Exception e)
                {
                    pt = $"{type.Name} = ;";
                }


                listBodyConstructorProperty.Add(pt);
            }


            StringBuilder populatedConstructor = new StringBuilder();
            populatedConstructor.AppendLine($"public {classType.Name}({string.Join(", ", listConstructorProperty)})");
            populatedConstructor.AppendLine("{");

            foreach (string item in listBodyConstructorProperty)
                populatedConstructor.AppendLine(item);

            populatedConstructor.AppendLine("}");

            return populatedConstructor;

        }


        private static string GetPropertyName(string name)
        {
            return char.ToLower(name[0], CultureInfo.CurrentCulture) + name.Substring(1);
        }
        private static string GetTypeName(Type type)
        {
            switch (type)
            {
                case Type _ when type == typeof(int):
                    return "int";
                case Type _ when type == typeof(int?):
                    return "int?";
                case Type _ when type == typeof(long):
                    return "long";
                case Type _ when type == typeof(long?):
                    return "long?";
                case Type _ when type == typeof(string):
                    return "string";
                    default: return type.Name;
            }
        }
    }
}
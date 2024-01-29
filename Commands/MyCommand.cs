using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

            var newFileContent = File.ReadAllText(filePath);
            string pastaSaidaCompilacao = "C:\\Temp";

            using (StreamWriter sw = new StreamWriter($"{pastaSaidaCompilacao}\\newfile{guid}.cs"))
            {
                sw.WriteLine(newFileContent);
            }

            filePath = $"{pastaSaidaCompilacao}\\newfile{guid}.cs";

            // Comando que você deseja executar (csc.exe neste caso)

            using (StreamWriter sw = new StreamWriter($"{pastaSaidaCompilacao}\\CompilarNewFile{guid}.bat"))
            {
                sw.WriteLine($@"path=%path%;C:\Windows\Microsoft.NET\Framework\v4.0.30319");
                sw.WriteLine($@"csc -out:{pastaSaidaCompilacao}\SysthExtension{guid}.dll -target:library {filePath}");
            }

            System.Diagnostics.Process.Start($"{pastaSaidaCompilacao}\\CompilarNewFile{guid}.bat").WaitForExit();

            Assembly teste = Assembly.LoadFrom($"{pastaSaidaCompilacao}\\SysthExtension{guid}.dll");


            var classType = teste.GetExportedTypes().FirstOrDefault();

            List<string> listConstructorProperty = (from i in classType.GetProperties()
                                                    select $"{GetTypeName(i.PropertyType)} {GetPropertyName(i.Name)}").ToList();

            List<string> listBodyConstructorProperty = (from i in classType.GetProperties()
                                                        select $"{i.Name} = {GetPropertyName(i.Name)};").ToList();

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
            }

            return default;
        }
    }
}
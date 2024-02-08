using System.IO;

namespace MSExtension.CommandsGen.Utils
{
    public static class StringExtension
    {
        public static string GetFilePath(this string input)
        {
            if (!input.Contains("Harmonit.Microservice"))
            {
                throw new Exception("Local onde foi executado a extensão não encontrou nenhum diretório com o prefixo \"Harmonit.Microservice\". Verifique a estrutura do microserviço.");
            }

            var x = new DirectoryInfo(input);
            var currentRepo = x.Name;

            var count = 0;

            do
            {
                count++;
                x = x.Parent;
                currentRepo = x.Name;
                if (count > 100)
                {
                    throw new Exception("Pasta não encontrada");
                }
            }
            while (!currentRepo.StartsWith("Harmonit.Microservice"));

            return x.Parent.FullName;
        }
    }
}
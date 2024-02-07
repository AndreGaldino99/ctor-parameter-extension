using System.IO;

namespace VSIXProject1.CommandsGen.Utils
{
    public static class StringExtension
    {
        public static string GetFilePath(this string input)
        {
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
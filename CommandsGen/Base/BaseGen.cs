using System.Text;

namespace MSExtension.CommandsGen.Base
{
    public static class BaseGen
    {
        public static string PushContentBeforeLastOccurrence(string text, char character, string aditionalContent)
        {
            StringBuilder sb = new StringBuilder(text);
            int lastOccurrence = sb.ToString().LastIndexOf(character); 

            if (lastOccurrence != -1) 
            {
                sb.Insert(lastOccurrence, aditionalContent); 
            }

            return sb.ToString();
        }
    }
}

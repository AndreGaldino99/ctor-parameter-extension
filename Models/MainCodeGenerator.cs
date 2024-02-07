using System.Collections.Generic;

namespace VSIXProject1.Models
{
    public class MainCodeGenerator
    {
        public string BaseName { get; set; }
        public string BaseNamespace { get; set; }
        public bool GenerateRoot { get; set; }
        public List<CodeGeneratorMethod> Method { get; set; }
    }
}

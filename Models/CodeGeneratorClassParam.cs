namespace VSIXProject1.Models
{
    public class CodeGeneratorClassParam
    {
        public string ParamType { get; set; }
        public string ParamName { get; set; }
        public string QueryParamName { get; set; }
        public bool isQuery { get; set; }
        public bool isHeader { get; set; }
        public bool isObject { get; set; }
    }
}

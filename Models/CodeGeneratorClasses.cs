﻿using System.Collections.Generic;

namespace VSIXProject1.Models
{
    public class CodeGeneratorClasses
    {
        public string MethodName { get; set; }
        public string MethodDesc { get; set; }
        public string RefitRoute { get; set; }
        public EnumMethod Method { get; set; }
        public List<CodeGeneratorClassParam> Params { get; set; }
        public bool GenerateRoot { get; set; }
        public List<int> ErrorsCode { get; set; }
        public int SuccessCode { get; set; }
    }
}

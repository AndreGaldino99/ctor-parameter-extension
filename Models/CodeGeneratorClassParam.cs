namespace VSIXProject1.Models
{
    public class CodeGeneratorClassParam
    {
        /// <summary>
        /// Tipo do parâmetro, ex: [string, int, object]
        /// </summary>
        public string ParamType { get; set; }
        /// <summary>
        /// Nome do parâmetro
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// Apenas informar se o parametro for query, para montar a rota do refit, será utilizado dentro da annotation [Query("")]
        /// </summary>
        public string QueryParamName { get; set; }
        /// <summary>
        /// Se for verdadeiro a rota do refit criará a annotation [Query("")]
        /// </summary>
        public bool isQuery { get; set; }
        /// <summary>
        /// Se for verdadeiro a rota do refit criará a annotation [Header("")]
        /// </summary>
        public bool isHeader { get; set; }
        /// <summary>
        /// Se for verdadeiro a extensão criará o classe do objeto vazia
        /// </summary>
        public bool isObject { get; set; }
    }
}

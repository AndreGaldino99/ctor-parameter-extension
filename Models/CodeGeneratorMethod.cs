using System.Collections.Generic;

namespace MSExtension.Models
{
    public class CodeGeneratorMethod
    {
        /// <summary>
        /// Nome do método
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Descrição que será usada nos controllers e interface refit
        /// </summary>
        public string MethodDesc { get; set; }
        /// <summary>
        /// Rota da integração
        /// </summary>
        public string RefitRoute { get; set; }
        /// <summary>
        /// HttpPost, HttpGet, HttpPut, HttpDelete
        /// </summary>
        public EnumMethod Method { get; set; }
        /// <summary>
        /// Lista de parâmetros do método
        /// </summary>
        public List<CodeGeneratorClassParam> Params { get; set; }
        /// <summary>
        /// Lista de códigos de erros que serão utilizados em cada método do controller na annotation [ProducesResponseType]
        /// </summary>
        public List<HttpStatusCode> ErrorsCode { get; set; }
        /// <summary>
        /// Código de sucesso que será utilizado em cada método do controller na annotation [ProducesResponseType]
        /// </summary>
        public HttpStatusCode SuccessCode { get; set; }
        /// <summary>
        /// Indica se o tipo de retorno do método é uma lista. Necessário para criar a tipagem dos retornos
        /// </summary>
        public bool ReturnResponseIsList { get; set; }
    }
}

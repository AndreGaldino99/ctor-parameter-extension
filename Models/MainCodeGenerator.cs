using System.Collections.Generic;

namespace MSExtension.Models
{
    public class MainCodeGenerator
    {
        /// <summary>
        /// Nome que será utilizado como base de criação de todas as classes
        /// </summary>
        public string BaseName { get; set; }
        /// <summary>
        /// Usar namespace até o nome do projeto
        /// </summary>
        public string BaseNamespace { get; set; }
        /// <summary>
        /// Indica que as classes serão criadas dentro do repositório em suas devidas pastas. Se essa opção for verdadeira, caso já exista um arquivo com o mesmo nome ele será sobrescrito
        /// </summary>
        public bool GenerateRoot { get; set; }
        /// <summary>
        /// Lista de métodos que serão gerados
        /// </summary>
        public List<CodeGeneratorMethod> Method { get; set; }
        /// <summary>
        /// Nome do microserviço. Será utilizado para criar a rota do controller
        /// </summary>
        public string MicroServiceName { get; internal set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azure_function_contatos.Entities
{
    public class Message
    {
        public string Action { get; set; }
        public ContatosResponse Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocViseu.Model.ModelView
{

    public class MessageForwardingModelView
    {
        public string Destinatorios { set; get; }
        public string Mensagem { get; set; }
        public string Assunto { get; set; }
        public string? CC { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }

}

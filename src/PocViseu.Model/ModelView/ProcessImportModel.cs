using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocViseu.Model.ModelView
{
    public class ProcessImportModel
    {
        //campos WS-PROTHEUS

        public string? empresa { set; get; }
        public string? nomeempresa { set; get; }

        public string? filial { set; get; }
        public string? nomefilial { set; get; }


        public string? pedido { set; get; }


        public string? item { set; get; }


        public string? produto { set; get; }

        public string? descprod { set; get; }

        public decimal? quantidade { set; get; }


        public string? lote { set; get; }


        public string? cultura { set; get; }

        public string? cliente { set; get; }

        public string? cnpjcpf { get; set; }

        public string? chavenfe { get; set; }
        
        public string? loja { set; get; }


        public string? razao { set; get; }
        public string? estado { set; get; }


        public string? municipio { set; get; }

        public string? fazenda { set; get; }
        public string? inscricao { set; get; }
        public string? cep { set; get; }


        public string? ncm { set; get; }
        
        public string? nota { set; get; }

        public string? serie { set; get; }
        public string? obsitem { set; get; }

        public string? geolocal { set; get; }

        public string? descarte { set; get; }

        public string? endcomple { get; set; }

        public string? emailAgronomo { get; set; }

        public string? mailagro { get; set; }
        public string? emaildepo { get; set; }


        //--end-campos WS-PROTHEUS

        public string? codUnidadeMedida { set; get; }

        public int? qntEmbalagem { set; get; }
        public int? areaQntTratada { set; get; }


    }
}

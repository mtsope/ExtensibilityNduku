using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExtensibilityNduku.Model
{
        public enum TipoOperacao
        {
            Insert,
            Update
        }

        public class MapeamentoColuna
        {
            public int ColunaExcel { get; set; }
            public string ColunaSQL { get; set; }
        }

        internal class ConfiguracaoFolha
            {
        public string NomeFolha { get; set; }

            public string TabelaSQL { get; set; }

            public TipoOperacao Operacao { get; set; }

            public int PrimeiraLinhaDados { get; set; }

            public int ColunaChaveExcel { get; set; }

            public string ColunaChaveSQL { get; set; }

            public string ColunaDataSQL { get; set; }

            public List<MapeamentoColuna> Colunas { get; set; }
        }
    }

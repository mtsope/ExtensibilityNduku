using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibilityNduku.Model
{
    internal class ResultadoProcessamento
    {
            public int TotalInseridos { get; set; }

            public int TotalAtualizados { get; set; }

            public int TotalNaoEncontrados { get; set; }

            public int TotalLinhasIgnoradas { get; set; }

            public int TotalFolhas { get; set; }

            public Dictionary<string, string> Detalhes { get; set; }

            public ResultadoProcessamento()
            {
                Detalhes = new Dictionary<string, string>();
            }
        }
    }

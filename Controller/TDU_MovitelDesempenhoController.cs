using ClosedXML.Excel;
using ExtensibilityNduku.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;


namespace ExtensibilityNduku.Controller
{
    internal class TDU_MovitelDesempenhoController
    {
            // Configurações das folhas
            private Dictionary<string, ConfiguracaoFolha> configuracoes;

            public TDU_MovitelDesempenhoController()
            {
                configuracoes = CriarConfiguracoes();
            }

            // =====================================================
            // 1. CONFIGURAR AS FOLHAS
            // =====================================================

            private Dictionary<string, ConfiguracaoFolha>
                CriarConfiguracoes()
            {
                var configs =
                    new Dictionary<string, ConfiguracaoFolha>(
                        StringComparer.OrdinalIgnoreCase);

                // MONTHLY REPORT
                configs.Add("Monthly Report",
                    new ConfiguracaoFolha
                    {
                        NomeFolha = "Monthly Report",
                        TabelaSQL = "TDU_MovitelDesempenho",
                        Operacao = TipoOperacao.Insert,
                        PrimeiraLinhaDados = 6,
                        ColunaChaveExcel = 3,
                        ColunaChaveSQL = "CDU_ID",
                        ColunaDataSQL = "CDU_Data",

                        Colunas = new List<MapeamentoColuna>
                        {
                        new MapeamentoColuna
                        {
                            ColunaExcel = 2,
                            ColunaSQL = "CDU_Agente"
                        },
                        new MapeamentoColuna
                        {
                            ColunaExcel = 5,
                            ColunaSQL = "CDU_Line"
                        }
                        }
                    });

            // LISTEN OFFLINE
            configs.Add("Listen Offline",
                new ConfiguracaoFolha
                {
                    NomeFolha = "Listen Offline",
                    TabelaSQL = "TDU_MovitelDesempenho",
                    Operacao = TipoOperacao.Update,
                    PrimeiraLinhaDados = 6,
                    ColunaChaveExcel = 3,
                    ColunaChaveSQL = "CDU_ID",
                    ColunaDataSQL = "CDU_Data",

                    Colunas = new List<MapeamentoColuna>
                    {
                            new MapeamentoColuna
                        {
                            ColunaExcel = 8,
                            ColunaSQL = "CDU_LOffS1"
                        },
                        new MapeamentoColuna
                        {
                            ColunaExcel = 9,
                            ColunaSQL = "CDU_LOffS2"
                        },
                         new MapeamentoColuna
                        {
                            ColunaExcel = 10,
                            ColunaSQL = "CDU_LOffS3"
                        },
                           new MapeamentoColuna
                        {
                            ColunaExcel = 11,
                            ColunaSQL = "CDU_LOffS4"
                           }
                    }
                });
                // TEST
                configs.Add("Test",
                    new ConfiguracaoFolha
                    {
                        NomeFolha = "Test",
                        TabelaSQL = "TDU_MovitelDesempenho",
                        Operacao = TipoOperacao.Update,
                        PrimeiraLinhaDados = 6,
                        ColunaChaveExcel = 3,
                        ColunaChaveSQL = "CDU_ID",
                        ColunaDataSQL = "CDU_Data",

                        Colunas = new List<MapeamentoColuna>
                        {
                        new MapeamentoColuna
                        {
                            ColunaExcel = 7,
                            ColunaSQL = "CDU_Test1"
                        },
                        new MapeamentoColuna
                        {
                            ColunaExcel = 8,
                            ColunaSQL = "CDU_Test2"
                        }
                        }
                    });

                // REGULATION
                configs.Add("Regulation",
                    new ConfiguracaoFolha
                    {
                        NomeFolha = "Regulation",
                        TabelaSQL = "TDU_MovitelDesempenho",
                        Operacao = TipoOperacao.Update,
                        PrimeiraLinhaDados = 6,
                        ColunaChaveExcel = 3,
                        ColunaChaveSQL = "CDU_ID",
                        ColunaDataSQL = "CDU_Data",
                        Colunas = new List<MapeamentoColuna>
                        {
                            new MapeamentoColuna
                             {
                                 ColunaExcel = 8,
                                 ColunaSQL = "CDU_RGArriveLate"
                             },
                            new MapeamentoColuna
                            {
                                ColunaExcel = 9,
                                ColunaSQL = "CDU_RGAbsence"
                            },
                            new MapeamentoColuna
                            {
                                ColunaExcel = 10,
                                ColunaSQL = "CDU_RGBadAttitude"
                            },
                            new MapeamentoColuna
                            {
                                ColunaExcel = 11,
                                ColunaSQL = "CDU_RGMissInfoBCCS"
                            },
                             new MapeamentoColuna
                            {
                                ColunaExcel = 12,
                                ColunaSQL = "CDU_RGAdvertencia"
                            }
                    }
                    });

                // RECIEVED CALLS
                configs.Add("Received Calls",
                    new ConfiguracaoFolha
                    {
                        NomeFolha = "Received Calls",
                        TabelaSQL = "TDU_MovitelDesempenho",
                        Operacao = TipoOperacao.Update,
                        PrimeiraLinhaDados = 6,
                        ColunaChaveExcel = 5,
                        ColunaChaveSQL = "CDU_ID",
                        ColunaDataSQL = "CDU_Data",
                        Colunas = new List<MapeamentoColuna>
                        { 
                              new MapeamentoColuna
                            {
                                ColunaExcel = 8,
                                ColunaSQL = "CDU_RCNumeroChamadas"
                            },
                        
                        }
                    });
                // DIGITAL
                configs.Add("Digital",
                    new ConfiguracaoFolha
                    {
                        NomeFolha = "Digital",
                        TabelaSQL = "TDU_MovitelDesempenho",
                        Operacao = TipoOperacao.Update,
                        PrimeiraLinhaDados = 12,
                        ColunaChaveExcel = 9,
                        ColunaChaveSQL = "CDU_ID",
                        ColunaDataSQL = "CDU_Data",
                        Colunas = new List<MapeamentoColuna>
                        {
                              new MapeamentoColuna
                        {
                            ColunaExcel = 10,
                            ColunaSQL = "CDU_DigiChat"
                        },
                        new MapeamentoColuna
                        {
                            ColunaExcel = 11,
                            ColunaSQL = "CDU_DigiTicket"
                        }
                        }
                    });

                return configs;
            }

            // =====================================================
            // 2. VALIDAR IDENTIFICADORES SQL
            // =====================================================

            private void ValidarIdentificador(string nome)
            {
                if (string.IsNullOrWhiteSpace(nome) ||
                    !Regex.IsMatch(nome, @"^[a-zA-Z0-9_]+$"))
                {
                    throw new Exception(
                        "Identificador SQL inválido: " + nome);
                }
            }

            // =====================================================
            // 3. ESCAPAR VALORES SQL
            // =====================================================

            private string SqlValor(string valor)
            {
                return "'" + (valor ?? "").Replace("'", "''") + "'";
            }

            private string ValorExcel(IXLCell celula)
            {
                if (celula.DataType == XLDataType.Number)
                {
                    return celula.GetDouble().ToString(
                        CultureInfo.InvariantCulture);
                }

                string valor = celula.GetFormattedString().Trim();
                decimal numero;
                NumberStyles estilos = NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.AllowExponent |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite;

                if (decimal.TryParse(
                    valor,
                    estilos,
                    CultureInfo.CurrentCulture,
                    out numero))
                {
                    return numero.ToString(CultureInfo.InvariantCulture);
                }

                return valor;
            }


        // =====================================================
        // 4. PROCESSAR TODAS AS FOLHAS
        // =====================================================

        public ResultadoProcessamento ProcessarTodasFolhas(
            dynamic BSO,
            string caminhoExcel,
            IEnumerable<string> folhasSelecionadas,
            DateTime dataSelecionada,
            Func<string, bool> confirmarSubstituicao,
            Action<string> reportarProgresso = null)
        {
            if (string.IsNullOrWhiteSpace(caminhoExcel) ||
                !File.Exists(caminhoExcel))
            {
                throw new Exception(
                    "Selecione um ficheiro Excel válido.");
            }

            var folhas = folhasSelecionadas
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (folhas.Count == 0)
                throw new Exception(
                    "Não existem folhas para processar.");

            var resultado = new ResultadoProcessamento();

            string dataSQL = dataSelecionada.ToString("yyyyMMdd");

            // Preparar os dados antes de iniciar a transação
            var dados = new Dictionary<string,
                List<Dictionary<string, string>>>(
                    StringComparer.OrdinalIgnoreCase);

            // =================================================
            // 5. LER TODAS AS FOLHAS DO EXCEL
            // =================================================

            using (var workbook = new XLWorkbook(caminhoExcel))
            {
                foreach (string nomeFolha in folhas)
                {
                    if (!configuracoes.ContainsKey(nomeFolha))
                    {
                        throw new Exception(
                            "Não existe configuração para a folha: " +
                            nomeFolha);
                    }

                    var config = configuracoes[nomeFolha];

                    if (reportarProgresso != null)
                    {
                        reportarProgresso("A ler folha: " + nomeFolha);
                    }

                    ValidarIdentificador(config.TabelaSQL);
                    ValidarIdentificador(config.ColunaChaveSQL);
                    ValidarIdentificador(config.ColunaDataSQL);

                    if (config.Colunas == null ||
                        config.Colunas.Count == 0)
                    {
                        throw new Exception(
                            "Não existem colunas configuradas para: " +
                            nomeFolha);
                    }

                    foreach (var coluna in config.Colunas)
                    {
                        ValidarIdentificador(coluna.ColunaSQL);
                    }

                    var worksheet = workbook.Worksheets
                        .FirstOrDefault(w =>
                            w.Name.Equals(
                                nomeFolha,
                                StringComparison.OrdinalIgnoreCase));

                    if (worksheet == null)
                    {
                        throw new Exception(
                            "Folha não encontrada no Excel: " +
                            nomeFolha);
                    }

                    var listaRegistos =
                        new List<Dictionary<string, string>>();
                    dados[nomeFolha] = listaRegistos;

                    var colunasLeitura = config.Colunas
                        .Select(c => c.ColunaExcel)
                        .Concat(new[] { config.ColunaChaveExcel })
                        .Distinct()
                        .ToList();

                    var linhasComDados = colunasLeitura
                        .SelectMany(coluna => worksheet.Column(coluna)
                            .CellsUsed(XLCellsUsedOptions.Contents)
                            .Select(celula => celula.Address.RowNumber))
                        .Where(linha => linha >= config.PrimeiraLinhaDados)
                        .Distinct()
                        .OrderBy(linha => linha)
                        .ToList();

                    if (linhasComDados.Count == 0)
                        continue;

                    var chaves = new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                    // =============================================
                    // PERCORRER AS LINHAS DA FOLHA
                    // =============================================

                    for (int indiceLinha = 0;
                         indiceLinha < linhasComDados.Count;
                         indiceLinha++)
                    {
                        int i = linhasComDados[indiceLinha];
                        string chave = worksheet
                            .Cell(i, config.ColunaChaveExcel)
                            .GetFormattedString().Trim();

                        var valores =
                            new Dictionary<string, string>();
                        bool linhaComDados =
                            !string.IsNullOrWhiteSpace(chave);

                        foreach (var coluna in config.Colunas)
                        {
                            string valor = ValorExcel(
                                worksheet.Cell(i, coluna.ColunaExcel));

                            valores[coluna.ColunaSQL] = valor;
                            linhaComDados = linhaComDados ||
                                !string.IsNullOrWhiteSpace(valor);
                        }

                        if (!linhaComDados)
                            continue;

                        if (reportarProgresso != null &&
                            ((indiceLinha + 1) % 500 == 0 ||
                             indiceLinha == linhasComDados.Count - 1))
                        {
                            reportarProgresso(
                                "A ler " + nomeFolha + ": linha " + i +
                                " (" + (indiceLinha + 1) + " de " +
                                linhasComDados.Count + ")");
                        }

                        if (string.IsNullOrWhiteSpace(chave))
                        {
                            resultado.TotalLinhasIgnoradas++;
                            continue;
                        }

                        // Verificar chaves duplicadas
                        if (!chaves.Add(chave))
                        {
                            throw new Exception(
                                "Chave duplicada " + chave +
                                " na folha " + nomeFolha +
                                ", linha " + i);
                        }

                        valores["__CHAVE__"] = chave;

                        listaRegistos.Add(valores);
                    }

                    }
            }

            // A partir daqui mantém o restante código original:
            // processamento dos dados, INSERT, UPDATE e transação.


        // =================================================
        // 6. CONFIRMAR SUBSTITUIÇÃO PARA INSERT
        // =================================================

        bool temInsert = folhas.Any(f =>
                    configuracoes[f].Operacao == TipoOperacao.Insert);

                bool existeRegistos = false;

                if (temInsert)
                {
                    // As folhas de INSERT partilham a tabela?
                    var tabelasInsert = folhas
                        .Where(f =>
                            configuracoes[f].Operacao ==
                            TipoOperacao.Insert)
                        .Select(f => configuracoes[f].TabelaSQL)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    if (tabelasInsert.Count != 1)
                    {
                        throw new Exception(
                            "As folhas de INSERT utilizam tabelas diferentes. " +
                            "A confirmação de substituição deve ser feita " +
                            "individualmente por tabela.");
                    }

                    string tabela = tabelasInsert[0];

                    string sqlConsulta =
                        "SELECT TOP 1 * FROM " + tabela +
                        " WHERE CONVERT(date, CDU_Data) = " +
                        "CONVERT(date, '" + dataSQL + "', 112)";

                    var lista = BSO.Consulta(sqlConsulta);

                    existeRegistos = !lista.Vazia();

                    if (existeRegistos)
                    {
                        bool substituir = confirmarSubstituicao(
                            "Já existem registos para a data selecionada. " +
                            "Deseja substituir os registos existentes?");

                        if (!substituir)
                            return resultado;
                    }
                }

                // =================================================
                // 7. EXECUTAR INSERT E UPDATE
                // =================================================

                int totalRegistos = dados.Values.Sum(lista => lista.Count);
                int registosProcessados = 0;

                BSO.IniciaTransaccao();

                bool transacaoIniciada = true;

                try
                {
                    foreach (string nomeFolha in folhas)
                    {
                        var config = configuracoes[nomeFolha];

                        if (!dados.ContainsKey(nomeFolha))
                            continue;

                        foreach (var registo in dados[nomeFolha])
                        {
                            string chave = registo["__CHAVE__"];

                            string chaveSQL = SqlValor(chave);

                            if (config.Operacao == TipoOperacao.Insert)
                            {
                                // INSERT
                                if (existeRegistos)
                                {
                                    // Não eliminar dados de outras folhas.
                                    // A substituição é limitada às chaves
                                    // desta folha.
                                    string sqlDelete =
                                        "DELETE FROM " + config.TabelaSQL +
                                        " WHERE " + config.ColunaChaveSQL +
                                        " = " + chaveSQL +
                                        " AND CONVERT(date, " +
                                        config.ColunaDataSQL + ") = " +
                                        "CONVERT(date, '" + dataSQL + "', 112)";

                                    BSO.DSO.ExecuteSQL(sqlDelete);
                                }

                                var colunas = new List<string>
                            {
                                config.ColunaChaveSQL
                            };

                                var valores = new List<string>
                            {
                                chaveSQL
                            };

                                foreach (var coluna in config.Colunas)
                                {
                                    colunas.Add(coluna.ColunaSQL);

                                    valores.Add(
                                        SqlValor(registo[coluna.ColunaSQL]));
                                }

                                colunas.Add(config.ColunaDataSQL);

                                valores.Add(
                                    "CONVERT(date, '" + dataSQL + "', 112)");

                                string sqlInsert =
                                    "INSERT INTO " + config.TabelaSQL +
                                    " (" + string.Join(", ", colunas) + ")" +
                                    " VALUES (" +
                                    string.Join(", ", valores) + ")";

                                BSO.DSO.ExecuteSQL(sqlInsert);

                                resultado.TotalInseridos++;
                            }
                            else
                            {
                                // UPDATE
                                string sqlVerificar =
                                    "SELECT TOP 1 * FROM " +
                                    config.TabelaSQL +
                                    " WHERE " + config.ColunaChaveSQL +
                                    " = " + chaveSQL +
                                    " AND CONVERT(date, " +
                                    config.ColunaDataSQL + ") = " +
                                    "CONVERT(date, '" + dataSQL + "', 112)";

                                var existente = BSO.Consulta(sqlVerificar);

                                if (!existente.Vazia())
                                {
                                    var atualizacoes = new List<string>();

                                    foreach (var coluna in config.Colunas)
                                    {
                                        atualizacoes.Add(
                                            coluna.ColunaSQL + " = " +
                                            SqlValor(registo[coluna.ColunaSQL]));
                                    }

                                    string sqlUpdate =
                                        "UPDATE " + config.TabelaSQL +
                                        " SET " +
                                        string.Join(", ", atualizacoes) +
                                        " WHERE " + config.ColunaChaveSQL +
                                        " = " + chaveSQL +
                                        " AND CONVERT(date, " +
                                        config.ColunaDataSQL + ") = " +
                                        "CONVERT(date, '" + dataSQL + "', 112)";

                                    BSO.DSO.ExecuteSQL(sqlUpdate);

                                    resultado.TotalAtualizados++;
                                }
                                        else
                                        {
                                            resultado.TotalNaoEncontrados++;
                                        }
                                    }

                                    registosProcessados++;
                                    if (reportarProgresso != null &&
                                        (registosProcessados % 5 == 0 ||
                                         registosProcessados == totalRegistos))
                                    {
                                        reportarProgresso(
                                            "A gravar " + registosProcessados +
                                            "/" + totalRegistos);
                                    }
                                }

                                resultado.Detalhes[nomeFolha] =
                            "Operação: " + config.Operacao +
                            " | Registos: " +
                            dados[nomeFolha].Count;
                    }

                    resultado.TotalFolhas = folhas.Count;

                    BSO.TerminaTransaccao();
                    transacaoIniciada = false;

                    return resultado;
                }
                catch
                {
                    if (transacaoIniciada)
                    {
                        BSO.DesfazTransaccao();
                        transacaoIniciada = false;
                    }

                    throw;
                }
            }
        }
    }

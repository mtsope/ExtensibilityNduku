using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtensibilityNduku.Controller
{
    internal class TDU_MovitelDesempenhoController
    {
            // Operações disponibilizadas pelo Primavera
            private readonly Action<string> executarSQL;
            private readonly Func<string, bool> existeRegisto;
            private readonly Action iniciarTransacao;
            private readonly Action confirmarTransacao;
            private readonly Action desfazerTransacao;

            public TDU_MovitelDesempenhoController(
                Action<string> executarSQL,
                Func<string, bool> existeRegisto,
                Action iniciarTransacao,
                Action confirmarTransacao,
                Action desfazerTransacao)
            {
                this.executarSQL = executarSQL;
                this.existeRegisto = existeRegisto;
                this.iniciarTransacao = iniciarTransacao;
                this.confirmarTransacao = confirmarTransacao;
                this.desfazerTransacao = desfazerTransacao;
            }

            // MÉTODO 1 - GRAVAR (INSERT)
            
            public int Gravar(
                DataGridView grid,
                DateTime dataSelecionada,
                Func<bool> confirmarSubstituicao)
            {
                if (grid == null || grid.Columns.Count <= 5)
                    throw new Exception("Estrutura do Excel inválida.");

                var dados = new List<Tuple<string, string, string>>();

                var ids = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

                var agentes = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

                // Validar dados antes da transação
                foreach (DataGridViewRow linha in grid.Rows)
                {
                    if (linha.IsNewRow)
                        continue;

                    string agente = Convert.ToString(
                        linha.Cells[2].Value ?? "").Trim();

                    string id = Convert.ToString(
                        linha.Cells[3].Value ?? "").Trim();

                    string linhaMovitel = Convert.ToString(
                        linha.Cells[5].Value ?? "").Trim();

                    // Ignorar linhas vazias e tabela de resumo
                    if (string.IsNullOrWhiteSpace(agente) &&
                        string.IsNullOrWhiteSpace(id))
                        continue;

                    if (string.IsNullOrWhiteSpace(agente) ||
                        string.IsNullOrWhiteSpace(id))
                    {
                        throw new Exception(
                            "Campos obrigatórios vazios na linha " +
                            (linha.Index + 1));
                    }

                    if (!ids.Add(id))
                        throw new Exception("ID duplicado: " + id);

                    if (!agentes.Add(agente))
                        throw new Exception("Agente duplicado: " + agente);

                    dados.Add(Tuple.Create(
                        agente, id, linhaMovitel));
                }

                if (dados.Count == 0)
                    throw new Exception("Não existem dados válidos para gravar.");

                string dataSQL = dataSelecionada.ToString("yyyyMMdd");

                string sqlConsulta =
                    "SELECT TOP 1 CDU_Data " +
                    "FROM TDU_MovitelDesempenho " +
                    "WHERE CONVERT(date, CDU_Data) = " +
                    "CONVERT(date, '" + dataSQL + "', 112)";

                bool existe = existeRegisto(sqlConsulta);

                if (existe && !confirmarSubstituicao())
                    return 0;

                bool transacaoIniciada = false;

                try
                {
                    iniciarTransacao();
                    transacaoIniciada = true;

                    if (existe)
                    {
                        string sqlDelete =
                            "DELETE FROM TDU_MovitelDesempenho " +
                            "WHERE CONVERT(date, CDU_Data) = " +
                            "CONVERT(date, '" + dataSQL + "', 112)";

                        executarSQL(sqlDelete);
                    }

                    foreach (var dado in dados)
                    {
                        string agente = dado.Item1.Replace("'", "''");
                        string id = dado.Item2.Replace("'", "''");
                        string linhaMovitel = dado.Item3.Replace("'", "''");

                        string sqlInsert =
                            "INSERT INTO TDU_MovitelDesempenho " +
                            "(CDU_Agente, CDU_ID, CDU_Line, CDU_Data) " +
                            "VALUES (" +
                            "N'" + agente + "', " +
                            "N'" + id + "', " +
                            "N'" + linhaMovitel + "', " +
                            "CONVERT(date, '" + dataSQL + "', 112))";

                        executarSQL(sqlInsert);
                    }

                    confirmarTransacao();
                    transacaoIniciada = false;

                    return dados.Count;
                }
                catch
                {
                    if (transacaoIniciada)
                    {
                        try { desfazerTransacao(); }
                        catch { }
                    }

                    throw;
                }
            }

            // MÉTODO 2 - UPDATE

            public int Update(
                string nomeFolha,
                DataGridView grid)
            {
                // Configuração das folhas
                var configuracoes =
                    new Dictionary<string, Tuple<string, int, string,
                        List<Tuple<int, string>>>>(
                        StringComparer.OrdinalIgnoreCase);

            // FOLHA Listen Offline
            configuracoes.Add("Listen Offline",
                    Tuple.Create(
                        "TDU_MovitelDesempenho",
                        3,
                        "CDU_ID",
                        new List<Tuple<int, string>>
                        {
                        Tuple.Create(8, "CDU_LOffS1"),
                        Tuple.Create(9, "CDU_LOffS2"),
                        Tuple.Create(10, "CDU_LOffS3"),
                        Tuple.Create(11, "CDU_LOffS4")
                        }
                    ));

            // FOLHA Test
            configuracoes.Add("Test",
                    Tuple.Create(
                        "TDU_MovitelDesempenho",
                        3,
                        "CDU_ID",
                        new List<Tuple<int, string>>
                        {
                        Tuple.Create(8, "CDU_Test1"),
                        Tuple.Create(9, "CDU_Test2")
                        }
                    ));

            // FOLHA Regulation
            configuracoes.Add("Regulation",
                    Tuple.Create(
                        "TDU_MovitelDesempenho",
                        3,
                        "CDU_ID",
                        new List<Tuple<int, string>>
                        {
                        Tuple.Create(8, "CDU_RGArriveLate"),
                        Tuple.Create(9, "CDU_RGAbsence"),
                        Tuple.Create(8, "CDU_RGBadAttitude"),
                        Tuple.Create(9, "CDU_RGMissInfoBCCS"),
                        Tuple.Create(8, "CDU_RGAdvertencia"),
                        }
                    ));

            // FOLHA Received Calls
            configuracoes.Add("Received Calls",
                    Tuple.Create(
                        "TDU_MovitelDesempenho",
                        5,
                        "CDU_ID",
                        new List<Tuple<int, string>>
                        {
                        Tuple.Create(9, "CDU_RCNumeroChamadas"),
                        }
                    ));

            // FOLHA Regulation
            configuracoes.Add("Digital",
                    Tuple.Create(
                        "TDU_MovitelDesempenho",
                        9,
                        "CDU_ID",
                        new List<Tuple<int, string>>
                        {
                        Tuple.Create(8, "CDU_DigiChat"),
                        Tuple.Create(9, "CDU_DigiTicket"),
                        }
                    ));

            if (!configuracoes.ContainsKey(nomeFolha))
                    throw new Exception(
                        "Folha não configurada: " + nomeFolha);

                if (grid == null || grid.Columns.Count == 0)
                    throw new Exception("DataGridView inválido.");

                var config = configuracoes[nomeFolha];

                string tabela = config.Item1;
                int colunaChave = config.Item2;
                string chaveSQL = config.Item3;
                var campos = config.Item4;

                var comandos = new List<string>();

                var chaves = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

                // Preparar os UPDATE
                foreach (DataGridViewRow linha in grid.Rows)
                {
                    if (linha.IsNewRow)
                        continue;

                    string chave = Convert.ToString(
                        linha.Cells[colunaChave].Value ?? "").Trim();

                    if (string.IsNullOrWhiteSpace(chave))
                        continue;

                    if (!chaves.Add(chave))
                        throw new Exception(
                            "Identificador duplicado: " + chave);

                    var valores = new List<string>();

                    foreach (var campo in campos)
                    {
                        string valor = Convert.ToString(
                            linha.Cells[campo.Item1].Value ?? "").Trim();

                        // Não atualizar campos vazios
                        if (string.IsNullOrWhiteSpace(valor))
                            continue;

                        valor = valor.Replace("'", "''");

                        valores.Add(
                            campo.Item2 + " = N'" + valor + "'");
                    }

                    if (valores.Count == 0)
                        continue;

                    string chaveSegura = chave.Replace("'", "''");

                    string sql =
                        "UPDATE " + tabela +
                        " SET " + string.Join(", ", valores) +
                        " WHERE " + chaveSQL +
                        " = N'" + chaveSegura + "'";

                    comandos.Add(sql);
                }

                if (comandos.Count == 0)
                    throw new Exception(
                        "Não existem dados válidos para atualizar.");

                bool transacaoIniciada = false;

                try
                {
                    iniciarTransacao();
                    transacaoIniciada = true;

                    foreach (string sql in comandos)
                    {
                        executarSQL(sql);
                    }

                    confirmarTransacao();
                    transacaoIniciada = false;

                    return comandos.Count;
                }
                catch
                {
                    if (transacaoIniciada)
                    {
                        try { desfazerTransacao(); }
                        catch { }
                    }

                    throw;
                }
            }
        }
    }

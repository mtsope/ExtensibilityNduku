using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensibilityNduku.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExtensibilityNduku.Service
{
    internal class TDU_MovitelDesempenhoService
    {
        public int Gravar(TDU_MovitelDesempenho desempenho, string connectionString)
        {
            if (desempenho == null)
                throw new ArgumentNullException(nameof(desempenho));

            return Gravar(new[] { desempenho }, connectionString);
        }

        public int Gravar(IEnumerable<TDU_MovitelDesempenho> desempenhos, string connectionString)
        {
            var colunas = new[]
            {
                "CDU_Agente", "CDU_Line", "CDU_LOffS1", "CDU_RGMissInfoBCCS",
                "CDU_RGAdvertencia", "CDU_RCNumeroChamadas", "CDU_Test1", "CDU_ID"
            };
            return Gravar(desempenhos, colunas, connectionString);
        }

        public int Gravar(
            IEnumerable<TDU_MovitelDesempenho> desempenhos,
            IEnumerable<string> colunas,
            string connectionString)
        {
            if (desempenhos == null)
                throw new ArgumentNullException(nameof(desempenhos));
            if (colunas == null)
                throw new ArgumentNullException(nameof(colunas));
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("A connection string deve ser informada.", nameof(connectionString));

            var lista = desempenhos.ToList();
            if (lista.Any(desempenho => desempenho == null))
                throw new ArgumentException("A lista não pode conter registos nulos.", nameof(desempenhos));
            if (lista.Count == 0)
                return 0;

            var colunasPermitidas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "CDU_Agente", "CDU_Line", "CDU_LOffS1", "CDU_RGMissInfoBCCS",
                "CDU_RGAdvertencia", "CDU_RCNumeroChamadas", "CDU_Test1", "CDU_ID"
            };
            var colunasGravar = colunas.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (colunasGravar.Count == 0 || colunasGravar.Any(coluna => !colunasPermitidas.Contains(coluna)))
                throw new ArgumentException("A lista de colunas para gravação é inválida.", nameof(colunas));

            string nomesColunas = string.Join(", ", colunasGravar.Select(coluna => "[" + coluna + "]"));
            string nomesParametros = string.Join(", ", colunasGravar.Select(coluna => "@" + coluna));
            string sql = "INSERT INTO TDU_MovitelDesempenho (" + nomesColunas + ") VALUES (" + nomesParametros + ");";

            int totalGravado = 0;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand(sql, connection, transaction))
                {
                    foreach (var desempenho in lista)
                    {
                        command.Parameters.Clear();
                        foreach (string coluna in colunasGravar)
                        {
                            object valor = ObterValor(desempenho, coluna);
                            SqlDbType tipoParametro = valor is string ? SqlDbType.NVarChar : SqlDbType.Float;
                            SqlParameter parametro = command.Parameters.Add("@" + coluna, tipoParametro);
                            if (tipoParametro == SqlDbType.NVarChar)
                                parametro.Size = 255;
                            parametro.Value = valor;
                        }
                        totalGravado += command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }

            return totalGravado;
        }

        private static object ObterValor(TDU_MovitelDesempenho desempenho, string coluna)
        {
            switch (coluna)
            {
                case "CDU_Agente": return desempenho.CDU_Agente;
                case "CDU_Line": return desempenho.CDU_Line;
                case "CDU_LOffS1": return desempenho.CDU_LOffS1;
                case "CDU_RGMissInfoBCCS": return desempenho.CDU_RGMissInfoBCCS;
                case "CDU_RGAdvertencia": return desempenho.CDU_RGAdvertencia;
                case "CDU_RCNumeroChamadas": return desempenho.CDU_RCNumeroChamadas;
                case "CDU_Test1": return desempenho.CDU_Test1;
                case "CDU_ID": return desempenho.CDU_ID;
                default: throw new ArgumentOutOfRangeException(nameof(coluna), coluna, "Coluna não suportada.");
            }
        }
    }
}

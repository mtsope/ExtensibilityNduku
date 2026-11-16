using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using ExtensibilityNduku.Model;
using ExtensibilityNduku.Service;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using StdBE100;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ExtensibilityNduku.Form
{
    public partial class PriCustomFormDemo : CustomForm
    {
        public PriCustomFormDemo()
        {
            InitializeComponent();
        }

        // Guardar o caminho do ficheiro selecionado
        private string caminhoExcel = "";

        // Controlar o carregamento das folhas
        private bool carregandoFolhas = false;


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Abrir janela para selecionar Excel

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter =
                    "Ficheiros Excel (*.xlsx)|*.xlsx";

                dialog.Title = "Selecionar ficheiro Excel";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    caminhoExcel = dialog.FileName;

                    carregandoFolhas = true;

                    cboFolha.Items.Clear();

                    using (XLWorkbook workbook =
                        new XLWorkbook(caminhoExcel))
                    {
                        // Carregar nomes de todas as folhas
                        foreach (IXLWorksheet folha
                            in workbook.Worksheets)
                        {
                            cboFolha.Items.Add(folha.Name);
                        }
                    }

                    carregandoFolhas = false;

                    if (cboFolha.Items.Count > 0)
                    {
                        // Selecionar automaticamente a primeira folha
                        cboFolha.SelectedIndex = 1;
                    }
                    else
                    {
                        MessageBox.Show(
                            "O ficheiro não contém folhas.",
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    carregandoFolhas = false;

                    MessageBox.Show(
                        "Erro ao abrir o ficheiro Excel:\n" +
                        ex.Message,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        // Navegar entre folhas
        // Ler todos os dados da folha selecionada

        private void CarregarDadosFolha(string nomeFolha)
        {
            try
            {
                DataTable tabela = new DataTable();

                using (XLWorkbook workbook =
                    new XLWorkbook(caminhoExcel))
                {
                    // Selecionar a folha pelo nome
                    IXLWorksheet folha =
                        workbook.Worksheet(nomeFolha);

                    // Obter o intervalo utilizado
                    IXLRange intervalo = folha.RangeUsed();

                    if (intervalo == null)
                    {
                        dataGridView1.DataSource = null;

                        MessageBox.Show(
                            "Esta folha não contém dados.",
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }

                    // Identificar limites do intervalo
                    int primeiraLinha =
                        intervalo.RangeAddress.FirstAddress.RowNumber;

                    int ultimaLinha =
                        intervalo.RangeAddress.LastAddress.RowNumber;

                    int primeiraColuna =
                        intervalo.RangeAddress.FirstAddress.ColumnNumber;

                    int ultimaColuna =
                        intervalo.RangeAddress.LastAddress.ColumnNumber;

                    const int linhaCabecalho = 5;
                    const int primeiraLinhaDados = 6;

                    // Manter uma coluna para identificar a linha original do Excel.
                    DataColumn colunaLinha = tabela.Columns.Add("Linha Excel");
                    colunaLinha.Caption = "Linha Excel";

                    // A grelha usa nomes internos únicos e mostra os cabeçalhos da linha 5.
                    for (int coluna = primeiraColuna;
                         coluna <= ultimaColuna;
                         coluna++)
                    {
                        string cabecalho = folha.Cell(linhaCabecalho, coluna).GetFormattedString().Trim();
                        if (string.IsNullOrWhiteSpace(cabecalho))
                            cabecalho = "Coluna " + (coluna - primeiraColuna + 1);

                        DataColumn colunaTabela = tabela.Columns.Add("Coluna_" + coluna);
                        colunaTabela.Caption = cabecalho;
                    }

                    // Percorrer as linhas de dados abaixo do cabeçalho.
                    for (int linha = primeiraLinhaDados;
                         linha <= ultimaLinha;
                         linha++)
                    {
                        DataRow novaLinha = tabela.NewRow();
                        novaLinha[0] = linha.ToString();

                        for (int coluna = primeiraColuna;
                             coluna <= ultimaColuna;
                             coluna++)
                        {
                            novaLinha[coluna - primeiraColuna + 1] =
                                folha.Cell(linha, coluna).GetFormattedString();
                        }

                        tabela.Rows.Add(novaLinha);
                    }
                }

                // Apresentar dados com os cabeçalhos do Excel e permitir edição das células.
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = tabela;
                dataGridView1.ReadOnly = false;
                for (int indice = 0; indice < tabela.Columns.Count; indice++)
                {
                    dataGridView1.Columns[indice].HeaderText = tabela.Columns[indice].Caption;
                    dataGridView1.Columns[indice].ReadOnly = false;
                }

                //    MessageBox.Show(
                //        "Folha carregada com sucesso!\n\n" +
                //        "Folha: " + nomeFolha + "\n" +
                //        "Total de linhas: " + tabela.Rows.Count + "\n" +
                //        "Total de colunas: " +
                //        (tabela.Columns.Count - 1),
                //        "Excel",
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erro ao carregar a folha:\n" +
                    ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cboFolha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (carregandoFolhas)
                return;

            if (cboFolha.SelectedItem == null)
                return;

            string nomeFolha =
                cboFolha.SelectedItem.ToString();

            CarregarDadosFolha(nomeFolha);

        }

        private void PriCustomFormDemo_Load(object sender, EventArgs e)
        {
            cboFolha.Items.Add("Listen Offline");
            cboFolha.Items.Add("Test");
            cboFolha.Items.Add("Regulation");
            cboFolha.Items.Add("Recieved Calls");
            cboFolha.Items.Add("Digital");
        }


        private void btnGravar_Click(object sender, EventArgs e)
        {
            bool transacaoIniciada = false;

            try
            {
                // 1. Validar a folha selecionada
                if (cboFolha.Text != "Monthly Report")
                {
                    PSO.Dialogos.MostraAviso(
                        "Selecione a folha Monthly Report."
                    );
                    return;
                }

                // 2. Validar se existem colunas suficientes
                if (dataGridView1.Columns.Count <= 5)
                {
                    PSO.Dialogos.MostraAviso(
                        "A estrutura do Excel não corresponde ao formato esperado."
                    );
                    return;
                }

                // 3. Listas para armazenar os dados válidos
                var dadosParaGravar =
                    new List<Tuple<string, string, string>>();

                // HashSets separados para evitar falsos duplicados
                var idsExistentes = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase
                );

                var agentesExistentes = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase
                );

                // 4. Percorrer e validar o DataGridView
                foreach (DataGridViewRow linha in dataGridView1.Rows)
                {
                    if (linha.IsNewRow)
                        continue;

                    // Obter os valores das colunas
                    string agente = Convert.ToString(
                        linha.Cells[2].Value ?? "").Trim();

                    string id = Convert.ToString(
                        linha.Cells[3].Value ?? "").Trim();

                    string linhaMovitel = Convert.ToString(
                        linha.Cells[5].Value ?? "").Trim();

                    // 5. Ignorar linhas completamente vazias
                    if (string.IsNullOrWhiteSpace(agente) &&
                        string.IsNullOrWhiteSpace(id))
                    {
                        continue;
                    }

                    // 6. Validar campos obrigatórios
                    if (string.IsNullOrWhiteSpace(agente) ||
                        string.IsNullOrWhiteSpace(id))
                    {
                        PSO.Dialogos.MostraAviso(
                            "Existem campos obrigatórios vazios na linha " +
                            (linha.Index + 1) +
                            ". Verifique o Agente e o ID."
                        );

                        return;
                    }

                    // 7. Verificar IDs duplicados
                    if (!idsExistentes.Add(id))
                    {
                        PSO.Dialogos.MostraAviso(
                            "ID duplicado encontrado: " + id +
                            ". Verifique a linha " + (linha.Index + 1) + "."
                        );

                        return;
                    }

                    // 8. Verificar agentes duplicados
                    if (!agentesExistentes.Add(agente))
                    {
                        PSO.Dialogos.MostraAviso(
                            "Agente duplicado encontrado: " + agente +
                            ". Verifique a linha " + (linha.Index + 1) + "."
                        );

                        return;
                    }

                    // 9. Adicionar apenas os dados válidos
                    dadosParaGravar.Add(
                        Tuple.Create(agente, id, linhaMovitel)
                    );
                }

                // 10. Verificar se existem dados válidos
                if (dadosParaGravar.Count == 0)
                {
                    PSO.Dialogos.MostraAviso(
                        "Não foram encontrados registos válidos para gravar."
                    );
                    return;
                }

                // 11. Obter a data selecionada
                DateTime dataSelecionada =
                    Convert.ToDateTime(dtoDataUpload.Value);

                string dataSQL =
                    dataSelecionada.ToString("yyyyMMdd");

                // 12. Verificar se já existem registos para a data
                string sqlConsulta =
                    "SELECT TOP 1 CDU_Data " +
                    "FROM TDU_MovitelDesempenho " +
                    "WHERE CONVERT(date, CDU_Data) = " +
                    "CONVERT(date, '" + dataSQL + "', 112)";

                StdBELista lista = BSO.Consulta(sqlConsulta);

                bool existeRegistos = !lista.Vazia();

                // 13. Solicitar confirmação para substituir
                if (existeRegistos)
                {
                    bool substituir =
                        PSO.Dialogos.MostraPerguntaSimples(
                            "Já existem registos para a data selecionada. " +
                            "Deseja substituir os registos existentes?"
                        );

                    if (!substituir)
                        return;
                }

                // 14. Iniciar transação
                BSO.IniciaTransaccao();
                transacaoIniciada = true;

                // 15. Eliminar registos anteriores da mesma data
                if (existeRegistos)
                {
                    string sqlDelete =
                        "DELETE FROM TDU_MovitelDesempenho " +
                        "WHERE CONVERT(date, CDU_Data) = " +
                        "CONVERT(date, '" + dataSQL + "', 112)";

                    BSO.DSO.ExecuteSQL(sqlDelete);
                }

                // 16. Inserir os novos registos
                int totalGravados = 0;

                foreach (var dado in dadosParaGravar)
                {
                    string agente = dado.Item1.Replace("'", "''");
                    string id = dado.Item2.Replace("'", "''");
                    string linhaMovitel = dado.Item3.Replace("'", "''");

                    string sqlInsert =
                        "INSERT INTO TDU_MovitelDesempenho " +
                        "(CDU_Agente, CDU_ID, CDU_Line, CDU_Data) " +
                        "VALUES (" +
                        "'" + agente + "', " +
                        "'" + id + "', " +
                        "'" + linhaMovitel + "', " +
                        "CONVERT(date, '" + dataSQL + "', 112)" +
                        ")";

                    BSO.DSO.ExecuteSQL(sqlInsert);

                    totalGravados++;
                }

                // 17. Confirmar transação
                BSO.TerminaTransaccao();
                transacaoIniciada = false;

                // 18. Mensagem de sucesso
                PSO.Dialogos.MostraAviso(
                    totalGravados +
                    " registos gravados com sucesso!"
                );
            }
            catch (Exception ex)
            {
                // 19. Desfazer transação em caso de erro
                if (transacaoIniciada)
                {
                    try
                    {
                        BSO.DesfazTransaccao();
                    }
                    catch
                    {
                        // Preservar o erro original
                    }
                }

                PSO.MensagensDialogos.MostraErro(
                    "Erro ao registar os dados: " + ex.Message
                );
            }
        }
        private void grbData_Enter(object sender, EventArgs e)
        {

        }
    }
}

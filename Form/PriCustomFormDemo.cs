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
            /*
            PSO.Dialogos.MostraAviso(""+dtoDataUpload.Value);
            try
            {
                

                PSO.Dialogos.MostraAviso("Tryyyy");

                StdBELista lista = BSO.Consulta("Select * from TDU_MovitelDesempenho  where CDU_Data = @CDU_Data");
                PSO.MensagensDialogos.MostraTDU(lista+"");
                

                bool existeRegistos = !lista.Vazia();
                if (existeRegistos)
                {

                    existeRegistos = PSO.Dialogos.MostraPerguntaSimples("Já existem registos para a data selecionada. Deseja substituir os registos existentes?");
                    //BSO.DSO.ExecuteSQL("Insert into TDU_MovitelDesempenho (CDU_Agente, CDU_ID, CDU_Line, CDU_Data) values (@CDU_Agente, @CDU_ID, @CDU_Line, @CDU_Data)");
                    if (existeRegistos)
                    {
                        existeRegistos = false;
                        BSO.DSO.ExecuteSQL("Delete from TDU_MovitelDesempenho where CDU_Data = @CDU_Data");
                    }
                    existeRegistos = true;

                }


                if (existeRegistos == false && cboFolha.Text == "Monthly Report")
                {
                    BSO.DSO.ExecuteSQL("Insert into TDU_MovitelDesempenho (CDU_Agente, CDU_ID, CDU_Line, CDU_Data) values (@CDU_Agente, @CDU_ID, @CDU_Line, @CDU_Data)");
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        //BSO.DSO.Parametros.Edita("CDU_Agente", dataGridView1.Rows[i].Cells[1].Value.ToString());
                        //BSO.DSO.Parametros.Edita("CDU_ID", dataGridView1.Rows[i].Cells[2].Value.ToString());
                        //BSO.DSO.Parametros.Edita("CDU_Line", dataGridView1.Rows[i].Cells[3].Value.ToString());
                        //BSO.DSO.Parametros  ("CDU_Data", grbData.Text);
                        BSO.DSO.ExecuteSQL($"Insert into TDU_MovitelDesempenho (CDU_Agente, CDU_ID, CDU_Line, CDU_Data) " +
                            $"values ('{dataGridView1.Rows[i].Cells[1].Value.ToString()}', '{dataGridView1.Rows[i].Cells[2].Value.ToString()}','{dataGridView1.Rows[i].Cells[3].Value.ToString()}',  {grbData.Text})");
                    }
                }
            }
            catch (Exception ex)
            {
                PSO.MensagensDialogos.MostraErro("Erro ao registar os dados");
            }
        }

            *
            */

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

                // 2. Validar o DataGridView
                if (dataGridView1.Rows.Count == 0)
                {
                    PSO.Dialogos.MostraAviso(
                        "Não existem dados para gravar."
                    );
                    return;
                }

                // 3. Obter a data selecionada
                DateTime dataSelecionada =
                    Convert.ToDateTime(dtoDataUpload.Value);

                string dataSQL =
                    dataSelecionada.ToString("yyyyMMdd");

                // 4. Verificar se já existem registos para a data
                string sqlConsulta =
                    "SELECT * FROM TDU_MovitelDesempenho " +
                    "WHERE CONVERT(date, CDU_Data) = " +
                    "CONVERT(date, '" + dataSQL + "', 112)";

                StdBELista lista = BSO.Consulta(sqlConsulta);

                bool existeRegistos = !lista.Vazia();

               
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

                // 6. Iniciar transação
                BSO.IniciaTransaccao();
                transacaoIniciada = true;

                // 7. Eliminar registos anteriores, caso existam
                if (existeRegistos)
                {
                    string sqlDelete =
                        "DELETE FROM TDU_MovitelDesempenho " +
                        "WHERE CONVERT(date, CDU_Data) = " +
                        "CONVERT(date, '" + dataSQL + "', 112)";

                    BSO.DSO.ExecuteSQL(sqlDelete);
                }

                // 8. Percorrer o DataGridView e gravar os dados
                int totalGravados = 0;
                var isDuplicated = new HashSet<string>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewRow linha = dataGridView1.Rows[i];

                    if (linha.IsNewRow)
                        continue;

                    // Obter valores do Excel
                    string agente = Convert.ToString(
                        linha.Cells[2].Value ?? "").Trim();

                    string id = Convert.ToString(
                        linha.Cells[3].Value ?? "").Trim();

                    string linhaMovitel = Convert.ToString(
                        linha.Cells[5].Value ?? "").Trim();

                    // Validar campos obrigatórios
                    if (string.IsNullOrWhiteSpace(agente) ||
                        string.IsNullOrWhiteSpace(id))
                    {
                        PSO.Dialogos.MostraAviso(
                             "Existem campos obrigatórios vazios na linha " +
                            (i + 1)
                        );
                    } else 
                        if (!isDuplicated.Add(id) || !isDuplicated.Add(agente))
                        {
                            PSO.Dialogos.MostraAviso(
                                "Valor duplicado encontrado no Excel: " +
                                id + " - " + agente +
                                ". Verifique a linha " + (i + 1) + "."
                            );
                            return;
                        }

                    // Escapar apóstrofos para SQL
                    agente = agente.Replace("'", "''");
                    id = id.Replace("'", "''");
                    linhaMovitel = linhaMovitel.Replace("'", "''");

                    // Inserir na tabela CDU
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

                // 9. Confirmar transação
                BSO.TerminaTransaccao();
                transacaoIniciada = false;

                // 10. Mensagem de sucesso
                PSO.Dialogos.MostraAviso(
                    totalGravados +
                    " registos gravados com sucesso!"
                );
            }
            catch (Exception ex)
            {
                // Desfazer operações em caso de erro
                if (transacaoIniciada)
                {
                    try
                    {
                        BSO.DesfazTransaccao();
                    }
                    catch
                    {
                        // Preservar a mensagem do erro original
                    }
                }

                PSO.MensagensDialogos.MostraErro(
                    "Erro ao registar os dados: " + ex.Message
                );
            }
        }

    }
}

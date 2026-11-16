using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using ExtensibilityNduku.Model;
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
            try
            {
                // Criar o controller
                var controller = new
                    ExtensibilityNduku.Controller.TDU_MovitelDesempenhoController(

                        // Executar SQL
                        sql => BSO.DSO.ExecuteSQL(sql),

                        // Verificar se existem registos
                        sql => !BSO.Consulta(sql).Vazia(),

                        // Iniciar transação
                        () => BSO.IniciaTransaccao(),

                        // Confirmar transação
                        () => BSO.TerminaTransaccao(),

                        // Desfazer transação
                        () => BSO.DesfazTransaccao()
                    );

                int total;

                // MONTHLY REPORT - INSERT
               

                if (cboFolha.Text == "Monthly Report")
                {
                    total = controller.Gravar(
                        dataGridView1,
                        Convert.ToDateTime(dtoDataUpload.Value),

                        // Confirmar substituição dos registos
                        () => PSO.Dialogos.MostraPerguntaSimples(
                            "Já existem registos para esta data. " +
                            "Deseja substituir os registos existentes?"
                        )
                    );

                    if (total == 0)
                        return;

                    PSO.Dialogos.MostraAviso(
                        total + " registos gravados com sucesso!"
                    );
                }

                // OUTRAS FOLHAS - UPDATE

                else
                {
                    total = controller.Update(
                        cboFolha.Text,
                        dataGridView1
                    );

                    PSO.Dialogos.MostraAviso(
                        total + " comandos de atualização executados " +
                        "na folha " + cboFolha.Text + "."
                    );
                }
            }
            catch (Exception ex)
            {
                PSO.MensagensDialogos.MostraErro(
                    "Erro ao processar os dados: " + ex.Message
                );
            }
        }

        private void grbListagm_Enter(object sender, EventArgs e)
        {

        }
    }
}

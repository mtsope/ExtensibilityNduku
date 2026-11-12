using ClosedXML.Excel;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ExtensibilityNduku.Model;
using ExtensibilityNduku.Service;
using StdBE100;

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

        private void grbListagm_Enter(object sender, EventArgs e)
        {

        }

        private void btnGravar_Click(object sender, EventArgs e)
        {

            var mapeamentos = new[]
            {
                new { IndiceColuna = 3,Nome = "CDU_Agente" },
                new { IndiceColuna = 4, Nome = "CDU_ID" },
                new { IndiceColuna = 5, Nome = "CDU_Line" },
                new { IndiceColuna = 6, Nome = "CDU_LOffS1" },
                new { IndiceColuna = 7, Nome = "CDU_RGMissInfoBCCS" },
                new { IndiceColuna = 8, Nome = "CDU_RGAdvertencia" },
                new { IndiceColuna = 9, Nome = "CDU_RCNumeroChamadas" },
                new { IndiceColuna = 10, Nome = "CDU_Test1" }
            };
            var colunasDisponiveis = mapeamentos
                //.Where(mapeamento => mapeamento.IndiceColuna + 1 < dataGridView1.Columns.Count)
                .ToList();

            if (colunasDisponiveis.Count == 0)
            {
                MessageBox.Show(
                    "A grelha não contém nenhuma coluna mapeada para gravação.",
                    "Dados inválidos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var desempenhos = new List<TDU_MovitelDesempenho>();
            try
            {
                string[] ficheirosConfiguracao =
                {
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    typeof(PriCustomFormDemo).Assembly.Location + ".config"
                };
                string connectionString = null;
                foreach (string ficheiroConfiguracao in ficheirosConfiguracao.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(ficheiroConfiguracao) || !System.IO.File.Exists(ficheiroConfiguracao))
                        continue;

                    var documentoConfiguracao = new XmlDocument();
                    documentoConfiguracao.Load(ficheiroConfiguracao);
                    XmlNode noConnectionString = documentoConfiguracao.SelectSingleNode(
                        "/configuration/connectionStrings/add[@name='MovitelDesempenho']");
                    XmlAttribute atributoConnectionString = noConnectionString == null
                        ? null
                        : noConnectionString.Attributes["connectionString"];
                    if (atributoConnectionString != null && !string.IsNullOrWhiteSpace(atributoConnectionString.Value))
                    {
                        connectionString = atributoConnectionString.Value;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException(
                        "Não foi encontrada uma connection string 'MovitelDesempenho' preenchida no ficheiro de configuração do ERP nem no ficheiro ExtensibilityNduku.dll.config.");

                foreach (DataGridViewRow linha in dataGridView1.Rows)
                {
                    if (linha.IsNewRow)
                        continue;

                    var desempenho = new TDU_MovitelDesempenho();
                    foreach (var mapeamento in colunasDisponiveis)
                    {
                        int indiceCelula = mapeamento.IndiceColuna + 1;
                        object valor = linha.Cells[indiceCelula].Value;
                        string valorTexto = valor == null || valor == DBNull.Value
                            ? string.Empty
                            : Convert.ToString(valor, CultureInfo.CurrentCulture).Trim();

                        switch (mapeamento.Nome)
                        {
                            case "CDU_Agente":
                                desempenho.CDU_Agente = valorTexto;
                                break;
                            case "CDU_ID":
                                desempenho.CDU_ID = valorTexto;
                                break;
                            case "CDU_Line":
                                desempenho.CDU_Line = valorTexto;
                                break;
                            default:
                                double valorNumerico;
                                if (!double.TryParse(valorTexto,
                                    NumberStyles.Float | NumberStyles.AllowThousands,
                                    CultureInfo.CurrentCulture, out valorNumerico))
                                {
                                    string linhaExcel = Convert.ToString(linha.Cells[0].Value);
                                    throw new FormatException(
                                        "Valor inválido na linha " + linhaExcel +
                                        ", coluna '" + dataGridView1.Columns[indiceCelula].HeaderText +
                                        "'. Esperado um número.");
                                }

                                switch (mapeamento.Nome)
                                {
                                    case "CDU_LOffS1": desempenho.CDU_LOffS1 = valorNumerico; break;
                                    case "CDU_RGMissInfoBCCS": desempenho.CDU_RGMissInfoBCCS = valorNumerico; break;
                                    case "CDU_RGAdvertencia": desempenho.CDU_RGAdvertencia = valorNumerico; break;
                                    case "CDU_RCNumeroChamadas": desempenho.CDU_RCNumeroChamadas = valorNumerico; break;
                                    case "CDU_Test1": desempenho.CDU_Test1 = valorNumerico; break;
                                }
                                break;
                        }
                    }

                    desempenhos.Add(desempenho);
                }

                if (desempenhos.Count == 0)
                {
                    MessageBox.Show(
                        "Não existem linhas para gravar.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                var service = new TDU_MovitelDesempenhoService();
                int gravados = service.Gravar(
                    desempenhos,
                    colunasDisponiveis.Select(mapeamento => mapeamento.Nome),
                    connectionString);
                MessageBox.Show(
                    gravados + " registo(s) gravado(s) com sucesso.",
                    "Gravação concluída",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Não foi possível gravar os dados:\n" + ex.Message,
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void grbListagm_Enter_1(object sender, EventArgs e)
        {

        }
    }
}

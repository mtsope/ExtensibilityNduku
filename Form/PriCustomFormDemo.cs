using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using ExtensibilityNduku.Controller;
using ExtensibilityNduku.Model;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using StdBE100;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
            try
            {
                using (OpenFileDialog openFile = new OpenFileDialog())
                {
                    openFile.Title = "Selecionar ficheiro Excel";

                    openFile.Filter =
                        "Ficheiros Excel (*.xlsx;*.xlsm)|*.xlsx;*.xlsm";

                    openFile.Multiselect = false;

                    if (openFile.ShowDialog() != DialogResult.OK)
                        return;

                    // Guardar caminho do ficheiro
                    caminhoExcel = openFile.FileName;
                    carregandoFolhas = true;

                    try
                    {
                        cboFolha.Items.Clear();
                        dataGridView1.DataSource = null;

                        using (var workbook = new XLWorkbook(caminhoExcel))
                        {
                            // foreach (IXLWorksheet worksheet in workbook.Worksheets)
                            foreach (IXLWorksheet worksheet in workbook.Worksheets.Skip(1))
                            {
                                cboFolha.Items.Add(worksheet.Name);
                            }
                        }

                        if (cboFolha.Items.Count > 0)
                        {
                            cboFolha.SelectedIndex = 0;
                        }
                    }
                    finally
                    {
                        carregandoFolhas = false;
                    }

                    if (cboFolha.SelectedItem != null)
                    {
                        CarregarDadosFolha(cboFolha.SelectedItem.ToString());
                    }

                    PSO.Dialogos.MostraAviso(
                        "Ficheiro carregado com sucesso!\n\n" +
                        "Ficheiro: " + Path.GetFileName(caminhoExcel) +
                        "\nTotal de folhas: " + cboFolha.Items.Count
                    );
                }
            }
            catch (Exception ex)
            {
                caminhoExcel = string.Empty;
                cboFolha.Items.Clear();
                dataGridView1.DataSource = null;

                PSO.Dialogos.MostraAviso(
                    "Erro ao carregar ficheiro Excel: " + ex.Message
                );
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
            {
                string nomeFolha = "";
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

        }


        private void btnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Verificar se existe ficheiro selecionado
                if (string.IsNullOrWhiteSpace(caminhoExcel) ||
                    !File.Exists(caminhoExcel))
                {
                    PSO.Dialogos.MostraAviso(
                        "Primeiro, selecione um ficheiro Excel."
                    );
                    return;
                }

                // 2. Verificar se existem folhas
                if (cboFolha.Items.Count == 0)
                {
                    PSO.Dialogos.MostraAviso(
                        "Não existem folhas para processar."
                    );
                    return;
                }

                // 3. Obter todas as folhas do ComboBox
                var folhas = cboFolha.Items
                    .Cast<object>()
                    .Select(x => x.ToString())
                    .ToList();

                // 4. Confirmar processamento
                bool continuar =
                    PSO.Dialogos.MostraPerguntaSimples(
                        "Serão processadas todas as " +
                        folhas.Count + " folhas do Excel.\n\n" +
                        "Deseja continuar?"
                    );

                if (!continuar)
                    return;

                // 5. Criar Controller
                var controller =
                    new TDU_MovitelDesempenhoController();

                ResultadoProcessamento resultado;
                lblFicheiro.Text = "A iniciar processamento...";
                this.UseWaitCursor = true;
                btnGravar.Enabled = false;
                btnBuscar.Enabled = false;
                cboFolha.Enabled = false;
                dataGridView1.Enabled = false;

                try
                {
                    resultado = controller.ProcessarTodasFolhas(
                        BSO,
                        caminhoExcel,
                        folhas,
                        dtoDataUpload.Value,
                        mensagem =>
                            PSO.Dialogos.MostraPerguntaSimples(mensagem),
                                estado =>
                                {
                                    lblFicheiro.Text = estado;
                                    Application.DoEvents();
                                });
                        }
                        finally
                        {
                            lblFicheiro.Text = Path.GetFileName(caminhoExcel);
                            this.UseWaitCursor = false;
                    btnGravar.Enabled = true;
                    btnBuscar.Enabled = true;
                    cboFolha.Enabled = true;
                    dataGridView1.Enabled = true;
                }

                // 7. Apresentar resumo
                string resumo =
                    "PROCESSAMENTO CONCLUÍDO!\n\n" +
                    "Folhas processadas: " +
                    resultado.TotalFolhas + "\n\n" +
                    "Registos inseridos: " +
                    resultado.TotalInseridos + "\n" +
                    "Registos atualizados: " +
                    resultado.TotalAtualizados + "\n" +
                    "Registos não encontrados: " +
                    resultado.TotalNaoEncontrados + "\n" +
                    "Linhas ignoradas sem chave: " +
                    resultado.TotalLinhasIgnoradas;

                PSO.Dialogos.MostraAviso(resumo);
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraAviso(
                    "Erro ao processar o ficheiro Excel:\n" +
                    ex.Message
                );
            }
        }

        private void cboFolha_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}


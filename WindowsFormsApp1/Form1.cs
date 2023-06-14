using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Label[] labelResults;
        DataGridView[] gridViewsTables;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxType.SelectedIndex = 0;
            labelResultF.Visible = false;
            dataGridViewInputMatrix.RowCount = 4;
            dataGridViewInputMatrix.ColumnCount = 3;
            dataGridViewInputMatrix[0, 3].Value = "-";
            dataGridViewInputMatrix[0, 3].ReadOnly = true;
        }


        private void numericUpDownRows_ValueChanged(object sender, EventArgs e)
        {
            int countRows = Convert.ToInt32(numericUpDownRows.Value);
            dataGridViewInputMatrix.RowCount = countRows;
            if (countRows > 1)
            {
                dataGridViewInputMatrix[0, dataGridViewInputMatrix.RowCount - 2].Value = null;
                dataGridViewInputMatrix[0, dataGridViewInputMatrix.RowCount - 2].ReadOnly = false;
            }
            dataGridViewInputMatrix[0, dataGridViewInputMatrix.RowCount - 1].Value = "-";
            dataGridViewInputMatrix[0, dataGridViewInputMatrix.RowCount - 1].ReadOnly = true;
        }

        private void numericUpDownColumns_ValueChanged(object sender, EventArgs e)
        {
            int countColumns = Convert.ToInt32(numericUpDownColumns.Value);
            dataGridViewInputMatrix.ColumnCount = countColumns;
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            tabControlTables.TabPages.Clear();
            int countRows = Convert.ToInt32(numericUpDownRows.Value);
            int countColumns = Convert.ToInt32(numericUpDownColumns.Value);
            Fraction[,] source = new Fraction[countRows, countColumns];
            Fraction[] result = new Fraction[source.GetLength(1) - 1];
            List<Fraction[,]> tablesResult;
            if (labelResults == null)
                labelResults = new Label[result.Length];
            else
            {
                foreach (Label item in labelResults)
                {
                    tabPageMain.Controls.Remove(item);
                }
            }

            try
            {
                for (int i = 0; i < countRows; i++)
                {
                    for (int j = 0; j < countColumns; j++)
                    {
                        if (dataGridViewInputMatrix.Rows[i].Cells[j].Value.ToString() == "-")
                        {
                            source[i, j] = new Fraction(0);
                            continue;
                        }
                        if (i == countRows - 1)
                        {
                            source[i, j] = new Fraction(Convert.ToInt64(dataGridViewInputMatrix.Rows[i].Cells[j].Value) * -1);
                            continue;
                        }
                        source[i, j] = new Fraction(Convert.ToInt64(dataGridViewInputMatrix.Rows[i].Cells[j].Value));
                    }
                }
                Simplex simplex = new Simplex(source);
                tablesResult = simplex.Calculate(result);
                for (int i = 0; i < result.Length; i++)
                {
                    labelResults[i] = new Label();
                    labelResults[i].Text = $"x{i + 1} = {result[i]}";
                    labelResults[i].Font = new Font("Microsoft Sans Serif", 14);
                    labelResults[i].Location = new Point(20 + i * 100, 270);
                    tabPageMain.Controls.Add(labelResults[i]);
                }
                labelResultF.Visible = true;
                labelResultF.Text = $"F{comboBoxType.Text} = {simplex.GetFraction()}";
                TabPage[] tabPages = new TabPage[tablesResult.Count];
                gridViewsTables = new DataGridView[tablesResult.Count];
                for (int i = 0; i < tablesResult.Count; i++)
                {
                    string title;
                    if (i == tablesResult.Count - 1)
                    {
                        title = "Вихідна";
                    }
                    else
                        title = $"Проміжна №{i + 1}";
                    tabPages[i] = new TabPage(title);
                    tabControlTables.TabPages.Add(tabPages[i]);
                    gridViewsTables[i] = GetStyleDataGridView();
                    for (int j = 0; j < countRows; j++)
                    {
                        for (int k = 0; k < countColumns * 2; k++)
                        {
                            if (i == tablesResult.Count - 1 && k == 0)
                            {
                                foreach (Fraction res in result)
                                {
                                    if (tablesResult[i][j, k] == res)
                                    {
                                        gridViewsTables[i].Rows[j].Cells[k].Style.BackColor = Color.Cyan;
                                        break;
                                    }
                                }
                            }
                            try
                            {
                                gridViewsTables[i].Rows[j].Cells[k].Value = tablesResult[i][j, k];
                            }
                            catch { }
                        }
                    }
                    if (i == tablesResult.Count - 1)
                        gridViewsTables[i].Rows[countRows - 1].Cells[0].Style.BackColor = Color.Red;
                    tabPages[i].Controls.Add(gridViewsTables[i]);
                    gridViewsTables[i].ClearSelection();
                }
            }
            catch (Exception ex)
            {
                foreach (Label item in labelResults)
                {
                    tabPageMain.Controls.Remove(item);
                }
                labelResultF.Visible = false;
                if (ex is NullReferenceException)
                {
                    MessageBox.Show("Введено не всі значення", "Сталася помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show(ex.Message, "Сталася помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataGridView GetStyleDataGridView()
        {
            int countRows = Convert.ToInt32(numericUpDownRows.Value);
            int countColumns = Convert.ToInt32(numericUpDownColumns.Value);
            var dataGridView = new DataGridView();
            dataGridView.ReadOnly = true;
            dataGridView.ColumnCount = countColumns * 2;
            dataGridView.RowCount = countRows;
            dataGridView.Size = new Size(345, 170);
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.ColumnHeadersVisible = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.MultiSelect = false;
            return dataGridView;
        }

        private void dataGridViewInputMatrix_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);

            TextBox tb = e.Control as TextBox;
            if (tb != null)
            {
                tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
            }
        }
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
    }
}

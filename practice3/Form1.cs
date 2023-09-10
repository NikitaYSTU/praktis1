using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace practice3
{
    public partial class Form1 : Form
    {
        private string id = "";

        public Form1()
        {
            InitializeComponent();
            resetMe();
        }
		private void resetMe()
		{
			this.id = string.Empty;

			textBox1.Text = "";

			button2.Text = "Update";
			button3.Text = "Delete";
		}

        private void Form1_Load(object sender, EventArgs e)
        {
			loadData();
        }

		private void loadData()
		{
			CRUD.sql = "SELECT * FROM diagnosis";

			CRUD.cmd = new NpgsqlCommand(CRUD.sql, CRUD.con);
			CRUD.cmd.Parameters.Clear();

			DataTable dt = CRUD.PerformCRUD(CRUD.cmd);

			if (dt.Rows.Count > 0)
			{
				int Row = Convert.ToInt32(dt.Rows.Count.ToString());
			}


			DataGridView dgv1 = dataGridView1;

			dgv1.AutoGenerateColumns = true;
			dgv1.DataSource = dt;
		}
	

		private void execute(string mySQL, string param)
		{
			CRUD.cmd = new NpgsqlCommand(mySQL, CRUD.con);
			addParameters(param);
			CRUD.PerformCRUD(CRUD.cmd);
		}

		private void addParameters(string str)
		{
			CRUD.cmd.Parameters.Clear();
			CRUD.cmd.Parameters.AddWithValue("diagnosis_name", textBox1.Text.Trim());

			if (str == "Update" || str == "Delete" && !string.IsNullOrEmpty(this.id))
			{
				CRUD.cmd.Parameters.AddWithValue("Diagnosis_ID", this.id);
			}
		}

        private void button1_Click(object sender, EventArgs e) //insert
        {
			if (string.IsNullOrEmpty(textBox1.Text.Trim()))
			{
				MessageBox.Show("Введи диагноз пж");
				return;
			}

			CRUD.sql = "INSERT INTO diagnosis (diagnosis_name) VALUES (@diagnosis_name)";
			execute(CRUD.sql, "Insert");

			MessageBox.Show("Диагноз введен");

			loadData();

			resetMe();
		}

        private void button2_Click(object sender, EventArgs e) //update
        {
			if (dataGridView1.Rows.Count == 0)
			{
				return;
			}

			if (string.IsNullOrEmpty(this.id))
			{
				MessageBox.Show("Выбери(те) элемент");
				return;
			}

			if (string.IsNullOrEmpty(textBox1.Text.Trim()))
			{
				MessageBox.Show("Введи");
				return;
			}

			CRUD.sql = "UPDATE diagnosis SET diagnosis_name = @diagnosis_name WHERE diagnosis_id = @diagnosis_id::integer";
			execute(CRUD.sql, "Update");
			MessageBox.Show("Обновился");

			loadData();
			resetMe();
		}

        private void button3_Click(object sender, EventArgs e) //delete
        {
			if (dataGridView1.Rows.Count == 0)
			{
				return;
			}

			if (string.IsNullOrEmpty(this.id))
			{
				MessageBox.Show("Выбери диагноз");
				return;
			}
			if (MessageBox.Show("Вы правда хотите удалить его? :( ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
			{
				CRUD.sql = "DELETE FROM diagnosis WHERE diagnosis_id = @diagnosis_id::integer";
				execute(CRUD.sql, "Update");
				MessageBox.Show("Удалился");

				loadData();
				resetMe();
			}
		}

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.RowIndex != -1)
			{
				DataGridView dgv1 = dataGridView1;

				this.id = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
				button2.Text = "Update " +"(" + this.id + ")";
				button3.Text = "Delete " +"(" + this.id + ")";

				textBox1.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);

			}
		}

    }
}

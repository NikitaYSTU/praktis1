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
    public partial class ticket : Form
    {
        private string id = "";
     
        public ticket()
        {
            InitializeComponent();
            resetMe();
        }

        private void resetMe()
        {
            this.id = string.Empty;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            dateTimePicker1.Text = "";

            updatebtn.Text = "Update";
            deletebtn.Text = "Delete";
        }

        private void ticket_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            CRUD.sql = "SELECT * FROM ticket";

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
            CRUD.cmd.Parameters.AddWithValue("patient_id", Convert.ToInt32(textBox1.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("doctor_id", Convert.ToInt32(textBox2.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("price_id", Convert.ToInt32(textBox3.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("id_visit_purpose", Convert.ToInt32(textBox4.Text.Trim()));
            CRUD.cmd.Parameters.AddWithValue("date", dateTimePicker1.Value.Date);

            if (str == "Update" || str == "Delete" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue("ticket_id", this.id);
            }
        }

        private void insertbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("Введи(те) данные о записи");
                return;
            }

            CRUD.sql = "INSERT INTO ticket (patient_id, doctor_id, price_id, id_visit_purpose, date) VALUES (@patient_id, @doctor_id, @price_id, @id_visit_purpose, @date)";
            execute(CRUD.sql, "Insert");

            MessageBox.Show("Данные о записи введены");

            loadData();

            resetMe();
        }

        private void updatebtn_Click(object sender, EventArgs e)
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
                MessageBox.Show("Введи(те)");
                return;
            }

            CRUD.sql = "UPDATE ticket SET patient_id = @patient_id, doctor_id = @doctor_id, price_id = @price_id, id_visit_purpose = @id_visit_purpose, date = @date WHERE ticket_id = @ticket_id::integer";

            execute(CRUD.sql, "Update");

            MessageBox.Show("Данные о записи обновлены");

            loadData();

            resetMe();
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Выбери(те) врача");
                return;
            }
            if (MessageBox.Show("Вы правда хотите удалить их? :( ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                CRUD.sql = "DELETE FROM ticket WHERE ticket_id = @ticket_id::integer";
                execute(CRUD.sql, "Update");
                MessageBox.Show("Удалились");

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
                updatebtn.Text = "Update " + "(" + this.id + ")";
                deletebtn.Text = "Delete " + "(" + this.id + ")";

                textBox1.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                textBox2.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                textBox3.Text = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                textBox4.Text = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
                dateTimePicker1.Text = Convert.ToString(dgv1.CurrentRow.Cells[6].Value);
            }
        }
    }
}

using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace practice3
{
    public partial class personal_data : Form
    {
        private string id = "";

        public personal_data()
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
            textBox5.Text = "";
            dateTimePicker1.Text = "";

            updatebtn.Text = "Update";
            deletebtn.Text = "Delete";
        }

        private void personal_data_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            CRUD.sql = "SELECT * FROM personal_data";

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
            CRUD.cmd.Parameters.AddWithValue("surname", textBox1.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("name", textBox2.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("second_name", textBox3.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("adress", textBox4.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("gender", textBox5.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("date_of_birth", dateTimePicker1.Value.Date);

            if (str == "Update" || str == "Delete" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue("doctor_id", this.id);
            }
        }

        private void insertbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("Введи(те) персональные данные");
                return;
            }

            CRUD.sql = "INSERT INTO personal_data (surname, name, second_name, adress, gender, date_of_birth) VALUES (@surname, @name, @second_name, @adress, @gender, @date_of_birth)";
            execute(CRUD.sql, "Insert");

            MessageBox.Show("Персональные данные введены");

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

            CRUD.sql = "UPDATE personal_data SET surname = @surname, name = @name, second_name = @second_name, adress = @adress, gender = @gender, date_of_birth = @date_of_birth WHERE id_personal_data = @id_personal_data::integer";

            execute(CRUD.sql, "Update");

            MessageBox.Show("Персональные данные обновлены");

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
                MessageBox.Show("Выбери(те) персональные данные");
                return;
            }
            if (MessageBox.Show("Вы правда хотите удалить их? :( ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                CRUD.sql = "DELETE FROM personal_data WHERE id_personal_data = @id_personal_data::integer";
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
                textBox5.Text = Convert.ToString(dgv1.CurrentRow.Cells[5].Value);
                dateTimePicker1.Text = Convert.ToString(dgv1.CurrentRow.Cells[6].Value);
            }
        }
    }
}

﻿using System;
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
    public partial class category : Form
    {
        private string id = "";

        public category()
        {
            InitializeComponent();
            resetMe();
        }
        private void resetMe()
        {
            this.id = string.Empty;

            textBox1.Text = "";

            updatebtn.Text = "Update";
            deletebtn.Text = "Delete";
        }

        private void category_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            CRUD.sql = "SELECT * FROM category";

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
            CRUD.cmd.Parameters.AddWithValue("Category_name", textBox1.Text.Trim());

            if (str == "Update" || str == "Delete" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue("Category_ID", this.id);
            }
        }

        private void insertbtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("Введи новую категорию пж");
                return;
            }

            CRUD.sql = "INSERT INTO category (category_name) VALUES (@category_name)";
            execute(CRUD.sql, "Insert");

            MessageBox.Show("Категория введена");

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
                MessageBox.Show("Введи");
                return;
            }

            CRUD.sql = "UPDATE category SET category_name = @category_name WHERE category_id = @category_id::integer";
            execute(CRUD.sql, "Update");
            MessageBox.Show("Обновился");

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
                MessageBox.Show("Выбери категорию");
                return;
            }
            if (MessageBox.Show("Вы правда хотите удалить ее? :( ", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                CRUD.sql = "DELETE FROM category WHERE category_id = @category_id::integer";
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
                updatebtn.Text = "Update " + "(" + this.id + ")";
                deletebtn.Text = "Delete " + "(" + this.id + ")";

                textBox1.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);

            }
        }
    }
}

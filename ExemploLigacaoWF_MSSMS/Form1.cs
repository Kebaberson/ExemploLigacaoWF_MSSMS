using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExemploLigacaoWF_MSSMS
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=ContatosDB;Integrated Security=True";


        public Form1()
        {
            InitializeComponent();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            string nome = txtnome.Text;
            string email = txtemail.Text;
            string telefone = txttelefone.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                    connection.Open();

                string query = "INSERT INTO Contatos (ct_nome, ct_email, ct_telefone) VALUES (@Nome, @Email, @Telefone)";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Nome", nome);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Telefone", telefone);

                    int rowsaffected = command.ExecuteNonQuery();

                    if (rowsaffected > 0)
                    {
                        MessageBox.Show("Contato gravado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show(nome, ", erro ao gravar o Contato",
                        (MessageBoxButtons)MessageBoxIcon.Error);
                    }

                
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvContatos.SelectedRows.Count > 0)
            {
                int idRowSelected = Convert.ToInt32(dgvContatos.SelectedRows[0].Cells["ct_Id"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Contatos WHERE ct_Id = @ct_Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ct_Id", idRowSelected);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("\tContato eliminado com sucesso!");
                    }
                }
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {

            if (dgvContatos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvContatos.SelectedRows[0].Cells["ct_Id"].Value);

                string nome = txtnome.Text;
                string email = txtemail.Text;
                string telefone = txttelefone.Text;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Contatos SET ct_nome = @Nome, ct_email = @Email, ct_telefone = @Telefone WHERE ct_Id = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nome", nome);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Telefone", telefone);
                        command.Parameters.AddWithValue("@ID", id);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Contato atualizado com sucesso!");

                            string selectQuery = "SELECT * FROM Contatos";
                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection))
                            {
                                DataTable dataTable = new DataTable();
                                dataAdapter.Fill(dataTable); 
                                dgvContatos.DataSource = dataTable; 
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um contato para atualizar.");
            }
        }

        private void btnRegistDeUtilizadores_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Contatos";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                dataAdapter.Fill(dataTable);

                dgvContatos.DataSource = dataTable;
            }

        }

        private void dgvContatos_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvContatos.Rows[e.RowIndex];


                txtnome.Text = row.Cells["ct_nome"].Value.ToString();
                txtemail.Text = row.Cells["ct_email"].Value.ToString();
                txttelefone.Text = row.Cells["ct_telefone"].Value.ToString();
            }
        }
    }
}

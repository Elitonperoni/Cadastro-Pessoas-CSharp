using Npgsql;
using System.Data;
using System.Data.SqlTypes;

namespace BaseDeDados
{
    public partial class Form1 : Form
    {
        NpgsqlConnection con = new NpgsqlConnection();
        public Form1()
        {
            InitializeComponent();
        } 
        private void btnConectar_Click(object sender, EventArgs e)
        {
            this.DefinirConnectionString();
            this.TestarConexao();
        }

        public string DefinirConnectionString()
        {
            return con.ConnectionString = "server=localhost;port=5432;user id=postgres;password=123456;database=DB_Enterprise";
        }
    
        public NpgsqlConnection TestarConexao()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    con.Open();
                    lblResultado.Text = "Conectado ao PostgreSQL";                    
                }
                catch(Exception)
                {
                    lblResultado.Text = "Erro ao conectar-se ao banco de dados";
                }
                finally
                {
                    con.Close();
                }                
            }
            return con;           
        }

        private void CriarTabela()
        {
            try
            {
                con.Open();
                NpgsqlCommand comando = new NpgsqlCommand();
                comando.Connection = con;    

                comando.CommandText = "CREATE TABLE pessoas (ID SERIAL NOT NULL PRIMARY KEY, nome VARCHAR(50), email VARCHAR(50))";
                comando.ExecuteNonQuery();

                lblResultado.Text = "Tabela Criada";
                comando.Dispose();
            }
            catch (Exception)
            {
                lblResultado.Text = "Erro ao conectar-se ao banco de dados";
            }
            finally
            {
                con.Close();
            }
        }
        private void btnCriarTabela_Click(object sender, EventArgs e)
        {
            this.DefinirConnectionString();            
            this.CriarTabela();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            this.DefinirConnectionString();
            this.InserirRegistro();           
        }

        private void InserirRegistro()
        {
            #region Inserir
            try
            {
                con.Open();
                NpgsqlCommand comando = new NpgsqlCommand();
                comando.Connection = con;

                string nome = txtNome.Text;
                string email = txtEmail.Text;

                comando.CommandText =
                    "INSERT INTO pessoas" +
                    " (nome, email) " +
                    " VALUES ( ' " + nome +
                            "', '" + email + "' ) ";

                comando.ExecuteNonQuery();

                lblResultado.Text = "Registro inserido no Banco de Dados";
                comando.Dispose();
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }
            #endregion
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            this.DefinirConnectionString();
            this.ProcurarRegistro();
        }

        private void ProcurarRegistro()
        {
            #region Procurar
            //lblResultado.Text = string.Empty;
            dgvLista.Rows.Clear();

            try
            {
                string query = "SELECT * FROM  pessoas";

                if (txtNome.Text != string.Empty)
                {
                    query = "SELECT * FROM pessoas  WHERE nome like '%" + txtNome.Text + "%'";
                }

                DataTable dados = new DataTable();

                NpgsqlDataAdapter adaptador = new NpgsqlDataAdapter(query, DefinirConnectionString());            

                con.Open();

                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows) 
                {
                    dgvLista.Rows.Add(linha.ItemArray);
                }
            }
            catch (Exception ex)
            {
                dgvLista.Rows.Clear();
                lblResultado.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }
            #endregion
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            this.DefinirConnectionString();
            this.ExcluirRegistro();
            this.ProcurarRegistro();
        }

        private void ExcluirRegistro()
        {
            try
            {
                con.Open();
                NpgsqlCommand comando = new NpgsqlCommand();
                comando.Connection = con;

                int id = (int)dgvLista.SelectedRows[0].Cells["id"].Value;

                comando.CommandText =
                    "DELETE FROM pessoas WHERE Id = '" + id + "'";

                comando.ExecuteNonQuery();

                MessageBox.Show("Registro Excluído com Sucesso");
                comando.Dispose();
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            this.DefinirConnectionString();
            this.EditarRegistro();
        }

        private void EditarRegistro()
        {
            try
            {
                con.Open();
                NpgsqlCommand comando = new NpgsqlCommand();
                comando.Connection = con;

                int id = (int)dgvLista.SelectedRows[0].Cells["id"].Value;
                string nome = txtNome.Text;
                string email = txtEmail.Text;

                string query = "UPDATE PESSOAS SET nome = '" + nome + "', email= '" + email + "' WHERE id = " + id + " ";

                comando.CommandText = query;

                comando.ExecuteNonQuery();

                lblResultado.Text = "Registro Editado com Sucesso";
                comando.Dispose();
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Data.SqlClient;

public partial class administracao_cargaMailling : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnLerExcel_Click(object sender, EventArgs e)
    {
        //Ativo paciente;
        if (fupArquivo.FileContent != null)
        {

            if (Permitir(fupArquivo.FileName))
            {
                try
                {
                    string Excel = AppDomain.CurrentDomain.BaseDirectory + fupArquivo.FileName;
                    fupArquivo.SaveAs(Excel);
                    DataTable Dados = DadosExcel(Excel);

                    if (Dados != null)
                    {
                        gvExcel.DataSource = Dados;
                        gvExcel.DataBind();
                    }

                    foreach (DataRow DR in Dados.Rows)
                    {
                        /// TODO: refatorar

                        //paciente = new Ativo();

                        //paciente.Nome = DR[1].ToString();
                        //paciente.Dt_Consulta = Convert.ToDateTime(DR[2].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        //paciente.Prontuario = Convert.ToInt32(DR[1].ToString());
                        //paciente.Grade = Convert.ToInt32(DR[3].ToString());
                        


                        string nomePaciente = DR[0].ToString();
                        string prontuario = DR[1].ToString();
                        string dt_consulta = Convert.ToDateTime(DR[2].ToString()).ToString("yyyy-dd-MM HH:mm:ss");
                        string grade = DR[3].ToString();
                        string equipe = DR[4].ToString();
                        string profissional = DR[5].ToString();
                        string codigo = DR[6].ToString();
                        string telefone1 = DR[7].ToString();
                        string telefone2 = DR[8].ToString();
                        string telefone3 = DR[9].ToString();
                        string telefone4 = DR[10].ToString();

                        string dt_carga = DateTime.Now.ToString("yyyy-dd-MM HH:mm:ss");

                        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
                        {
                            SqlCommand cmm = new SqlCommand();
                            cmm.Connection = cnn;
                            cnn.Open();
                            SqlTransaction mt = cnn.BeginTransaction();
                            cmm.Transaction = mt;
                            try
                            {
                                bool existeCadastroPaciente = PacienteMailingDAO.getExisteCadastroPacienteMailing(Convert.ToInt32(prontuario)); 

                                if(existeCadastroPaciente.Equals(false))
                                {
                                    cmm.CommandText = "Insert into paciente_Mailling (prontuario, nome_paciente,telefone1, telefone2, telefone3, telefone4)"
                                    + " values ('" + prontuario + "','" + nomePaciente + "', '" + telefone1 + "', '" + telefone2 + "','" + telefone3 + "', '" + telefone4 + "');";
                                    cmm.ExecuteNonQuery();
                                }
                                
                                cmm.CommandText = "Insert into consulta (prontuario, dt_consulta, grade, equipe, profissional, codigo_consulta, dt_carga)"
                                    + " values ('" + prontuario + "','" + dt_consulta + "', '" + grade + "', '" + equipe + "', '" + profissional + "','" + codigo + "','" + dt_carga + "');";
                                cmm.ExecuteNonQuery();

                                mt.Commit();
                                mt.Dispose();
                                cnn.Close();
                            }
                            catch (Exception ex)
                            {
                                string erro = ex.Message;
                                Response.Write(erro);
                                mt.Rollback();
                            }
                        }
                    }//foreach
                    string arquivo = Excel;
                    if (File.Exists(arquivo))
                    {
                        File.Delete(arquivo);
                    }
                }
                catch (HttpException ex)
                {
                    Response.Write("<script language='javascript'>alert('Erro ao carregar o arquivo.');</script>");
                }
            }
            else
            {
                Response.Write("<script language='javascript'>alert('Extensão de arquivo não permitida.');</script>");
            }
        }
    }

    private bool Permitir(string arquivo)
    {
        string extensao = System.IO.Path.GetExtension(arquivo).ToLower();
        string[] permitido = { ".xls" };
        for (int i = 0; i < permitido.Length; i++)
        {
            if (string.Compare(extensao, permitido[i]) == 0)
            {
                return true;
            }
        } return false;
    }

    private DataTable DadosExcel(string Arquivo)
    {
        Char aspas = '"';
        string Conexao = "Provider=Microsoft.ACE.OLEDB.12.0;" +
        "Data Source=" + Arquivo + ";" +
        "Extended Properties=" + aspas + "Excel 8.0;HDR=YES" + aspas;
        System.Data.OleDb.OleDbConnection Cn = new System.Data.OleDb.OleDbConnection();
        Cn.ConnectionString = Conexao;
        Cn.Open();
        object[] Restricoes = { null, null, null, "TABLE" };
        DataTable DTSchema = Cn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, Restricoes);
        if (DTSchema.Rows.Count > 0)
        {
            string Sheet = DTSchema.Rows[0]["TABLE_NAME"].ToString();
            System.Data.OleDb.OleDbCommand Comando = new System.Data.OleDb.OleDbCommand("SELECT * FROM [" + Sheet + "]", Cn);
            DataTable Dados = new DataTable();
            System.Data.OleDb.OleDbDataAdapter DA = new System.Data.OleDb.OleDbDataAdapter(Comando);
            DA.Fill(Dados);
            Cn.Close();
            return Dados;
        }
        return null;
    }
}

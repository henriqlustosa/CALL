using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Restrito_CadastraStatusLigacao : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CarregaStatus(); // Carrega os dados no GridView
        }
    }

    // Método para cadastrar um novo status de ligação
    protected void btnCadastrar_Click(object sender, EventArgs e)
    {
        string descricao = Request.Form["descricaoStatus"];
        string tentativa = Request.Form["tenta"];
        string ativo = Request.Form["ativo"];

        try
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SuaConnectionString"].ToString()))
            {
                conn.Open();
                string query = "INSERT INTO [hspmCall].[dbo].[status_consulta] (status, temta, ativo) VALUES (@Descricao, @Tentativa, @Ativo)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Descricao", descricao);
                    cmd.Parameters.AddWithValue("@Tentativa", tentativa);
                    cmd.Parameters.AddWithValue("@Ativo", ativo);
                    cmd.ExecuteNonQuery();
                }
            }

            lbMensagem.Text = "Registro cadastrado com sucesso!";
        }
        catch (Exception ex)
        {
            lbMensagem.Text = "Erro ao cadastrar: " + ex.Message;
        }

        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalMsg').modal('show');", true);
        CarregaStatus(); // Atualiza o GridView
    }

    // Método para carregar os status de ligação no GridView
    private void CarregaStatus()
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            string query = @"SELECT 
                            id_status AS Id_status, 
                            status AS Descricao, 
                            tenta AS Tentativa, 
                            ativo AS Ativo 
                         FROM [hspmCall].[dbo].[status_consulta]  where deletado ='N'
                         ORDER BY id_status";


            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvStatusLigacao.DataSource = dt;
                    gvStatusLigacao.DataBind();
                }
            }
        }
    }

    // Método para atualizar um status de ligação
    protected void BtnSalvarEdicao_Click(object sender, EventArgs e)
    {
        string idStr = Request.Form["edit-id"];
        string descricao = Request.Form["edit-descricao"];
        string tentativa = Request.Form["edit-tentativa"];
        string ativo = Request.Form["edit-ativo"];
        int id; 
        if (int.TryParse(idStr, out  id))
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
                {
                    conn.Open();
                    string query = @"UPDATE [hspmCall].[dbo].[status_consulta] 
                             SET status = @Descricao, tenta = @Tentativa, ativo = @Ativo 
                             WHERE id_status = @Id";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Descricao", descricao);
                        cmd.Parameters.AddWithValue("@Tentativa", tentativa);
                        cmd.Parameters.AddWithValue("@Ativo", ativo);
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Atualiza a lista de status no GridView
                CarregaStatus();
                lbMensagem.Text = "";
            }
            catch (Exception ex)
            {
                lbMensagem.Text = "Erro ao atualizar: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalMsg').modal('show');", true);
            }
        }
        else
        {
            lbMensagem.Text = "ID inválido.";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalMsg').modal('show');", true);
        }
    }




    // Método para tratar comandos do GridView (Excluir)
    protected void gvStatusLigacao_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            try
            {
                int id = Convert.ToInt32(e.CommandArgument);

                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
{
    conn.Open();
    string query = "UPDATE [dbo].[status_consulta]  SET    [deletado] = 'S' WHERE id_status = @Id";

    using (var cmd = new SqlCommand(query, conn))
    {
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }
}

lbMensagem.Text = "Registro excluído com sucesso!";
            }
            catch (Exception ex)
            {
                lbMensagem.Text = "Erro ao excluir: " + ex.Message;
            }

            //ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalMsg').modal('show');", true);
CarregaStatus();
            UpdatePanel1.Update();
        }
    }
}

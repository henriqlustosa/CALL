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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.MobileControls;

public partial class Paciente_historico : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Msg.Text = "";

        if (!IsPostBack)
        {
            Msg.Text = "Por favor, forneça um número de prontuário.";
            CarregarStatusConsulta(); // <- aqui
        }
    }

    public void SearchHistorico_OnClick(object sender, EventArgs e)
    {
        int _prontuario = Convert.ToInt32(txbProntuario.Text);

        PacienteMailling paciente = new PacienteMailling();
        paciente = PacienteMailingDAO.getDadosPaciente(_prontuario);

        txbNomePaciente.Text = paciente.Nome;
        txbTelefone1.Text = paciente.Telefone1;
        txbTelefone2.Text = paciente.Telefone2;
        txbTelefone3.Text = paciente.Telefone3;
        txbTelefone4.Text = paciente.Telefone4;

        GridView1.DataSource = ConsultasDAO.ListaConsultasPaciente(_prontuario);
        GridView1.DataBind();
    }

    protected void btnAtualizaTelefones_Click(object sender, EventArgs e)
    {
        PacienteMailling paciente = new PacienteMailling();

        paciente.Prontuario = Convert.ToInt32(txbProntuario.Text);
        paciente.Telefone1 = Regex.Replace(txbTelefone1.Text, "[^0-9]", "");
        paciente.Telefone2 = Regex.Replace(txbTelefone2.Text, "[^0-9]", "");
        paciente.Telefone3 = Regex.Replace(txbTelefone3.Text, "[^0-9]", "");
        paciente.Telefone4 = Regex.Replace(txbTelefone4.Text, "[^0-9]", "");

        string mensagem = PacienteMailingDAO.AtualizaTelefones(paciente.Prontuario, paciente.Telefone1, paciente.Telefone2, paciente.Telefone3, paciente.Telefone4);
        
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + mensagem + "');", true);
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditarPopup")
        {
            int idAtivo = Convert.ToInt32(e.CommandArgument);
            hfIdConsulta.Value = idAtivo.ToString();

            DataTable dt = ToDataTable( BuscarConsultaPorId(idAtivo));
            if (dt.Rows.Count > 0)
            {
                ddlStatusModal.SelectedValue = dt.Rows[0][2].ToString();
                txtObservacaoModal.Text = dt.Rows[0][3].ToString();
            }
            UpdatePanelModal.Update(); // <- isso força o refresh da parte com os dados

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "abrirModal();", true);
        //    // ⚠️ Força o recarregamento completo da página após meio segundo
     
        }
    }
    private void CarregarStatusConsulta()
    {
        string connStr = ConfigurationManager.ConnectionStrings["gtaConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "SELECT id_status, status FROM status_consulta ORDER BY status";
            SqlCommand cmd = new SqlCommand(sql, conn);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            ddlStatusModal.DataSource = reader;
            ddlStatusModal.DataTextField = "status";
            ddlStatusModal.DataValueField = "id_status";
            ddlStatusModal.DataBind();

            // Opcional: item inicial
            ddlStatusModal.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }
    }

    protected void btnSalvarModal_Click(object sender, EventArgs e)
    {
        int idConsulta = int.Parse(hfIdConsulta.Value);
        string status = ddlStatusModal.SelectedValue;
        string observacao = txtObservacaoModal.Text.Trim();

        AtualizarHistorico(idConsulta, status, observacao);
        BindGrid();
     

        // Fecha o modal de forma segura
        ScriptManager.RegisterStartupScript(this, GetType(), "HideModal", "$('#modalEditar').modal('hide');", true);

        // ⚠️ Força o recarregamento completo da página após meio segundo
        ScriptManager.RegisterStartupScript(this, GetType(), "ReloadPage", "setTimeout(function() { location.reload(); }, 500);", true);
    }


    private void AtualizarHistorico(int idativo, string status, string observacao)
    {
        string connStr = ConfigurationManager.ConnectionStrings["gtaConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "UPDATE [dbo].[ativo_ligacao] SET status = @Status, observacao = @Obs WHERE idativo = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Obs", observacao);
            cmd.Parameters.AddWithValue("@Id", idativo);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
    private List<Ativo_Ligacao> BuscarConsultaPorId(int idConsulta)
    {
        var lista = new List<Ativo_Ligacao>();


        string connStr = ConfigurationManager.ConnectionStrings["gtaConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "SELECT id_Consulta,a.status as id_status,observacao  FROM [dbo].[ativo_ligacao] as a join status_consulta s on a.status = s.id_status WHERE idativo = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", idConsulta);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Ativo_Ligacao ativo = new Ativo_Ligacao();
                ativo.Codigo_Consulta = reader.GetInt32(reader.GetOrdinal("id_Consulta"));
                ativo.Status = reader["id_status"].ToString(); // Agora traz o ID
                ativo.Observacao = reader.GetString(reader.GetOrdinal("observacao"));
                lista.Add(ativo);
            }
        }
        return lista;
    }
        private void BindGrid()
    {
        int prontuario;
        if (int.TryParse(txbProntuario.Text.Trim(), out prontuario))
        {
           
            GridView1.DataSource = ConsultasDAO.ListaConsultasPaciente(prontuario);
            GridView1.DataBind();
          
        }
    }
    public static DataTable ToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);

        // Obtém todas as propriedades da classe
        var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        foreach (var prop in props)
        {
            dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var item in items)
        {
            var values = new object[props.Length];
            for (int i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(item, null);
            }

            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

}
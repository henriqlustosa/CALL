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

public partial class Paciente_historico : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Msg.Text = "";

        if (!IsPostBack)
        {
            Msg.Text = "Por favor, forneça um número de prontuário.";
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
}
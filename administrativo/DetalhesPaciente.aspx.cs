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

public partial class administrativo_DetalhesPaciente : System.Web.UI.Page
{
    private int _tenta { get; set; }
    private int _id_ativo { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PacienteMailling paciente = new PacienteMailling();

            int prontuario = Convert.ToInt32(Request.QueryString["prontuario"]);
            int tentativa = Convert.ToInt32(Request.QueryString["tentativa"]);
            _tenta = tentativa;

            lbProntuario.Text = prontuario.ToString();

            paciente = PacienteMailingDAO.getDadosPaciente(prontuario);

            txbNomePaciente.Text = paciente.Nome;
            txbNomePacienteModal.Text = paciente.Nome;
            txbTelefone1.Text = paciente.Telefone1;
            txbTelefone2.Text = paciente.Telefone2;
            txbTelefone3.Text = paciente.Telefone3;
            txbTelefone4.Text = paciente.Telefone4;

            BindGrind(prontuario);
            BindGrind1Tentativas(prontuario, tentativa);
        }
    }

    private void BindGrind(int _prontuario)
    {
        GridView1.DataSource = AtivoDAO.ListaConsultasPaciente(_prontuario);
        GridView1.DataBind();
    }

    private void BindGrind1Tentativas(int _prontuario, int _tentativa)
    {
        string _realizada = "N";
        GridView2.DataSource = AtivoDAO.ListaTentativaContatoPaciente(_prontuario, _tentativa, _realizada);
        GridView2.DataBind();
    }

    protected void grdMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index;

        if (e.CommandName.Equals("editRecord"))
        {
            index = Convert.ToInt32(e.CommandArgument);

            int ID = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString());

            var clickedRecord = from consulta in AtivoDAO.ListaConsultasPaciente(Convert.ToInt32(lbProntuario.Text))
                                where consulta.Id_Consulta == ID
                                select consulta;

            foreach (var d in clickedRecord.ToList())
            {
                txbID.Text = d.Id_Consulta.ToString();
                txbdtConsulta.Text = d.Dt_Consulta.ToString();
                txbEquipe.Text = d.Equipe;
                txbProfissional.Text = d.Nome_Profissional;
                txbCodConsulta.Text = d.Codigo_Consulta.ToString();
            }
           
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none",
                    "<script>$('#editModal').modal('show');</script>", false);
        }
    }

    protected void btnAtualizaTelefones_Click(object sender, EventArgs e)
    {
        PacienteMailling paciente = new PacienteMailling();

        paciente.Prontuario = Convert.ToInt32(lbProntuario.Text);
        paciente.Telefone1 = Regex.Replace(txbTelefone1.Text, "[^0-9]" ,"");
        paciente.Telefone2 = Regex.Replace(txbTelefone2.Text, "[^0-9]", "");
        paciente.Telefone3 = Regex.Replace(txbTelefone3.Text, "[^0-9]", "");
        paciente.Telefone4 = Regex.Replace(txbTelefone4.Text, "[^0-9]", "");

        string mensagem = PacienteMailingDAO.AtualizaTelefones(paciente.Prontuario ,paciente.Telefone1, paciente.Telefone2, paciente.Telefone3, paciente.Telefone4);
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + mensagem + "');", true);
    }


    protected void btnGravar_Click(object sender, EventArgs e)
    {
        Ativo_Ligacao ativo = new Ativo_Ligacao();

        ativo.Prontuario = Convert.ToInt32(lbProntuario.Text);
        ativo.Observacao = txbObservacao.Text.ToUpper();
        ativo.Usuario_Contato = System.Web.HttpContext.Current.User.Identity.Name;
        ativo.Id_Consulta = Convert.ToInt32(txbID.Text);
        int _status = Convert.ToInt32(ddlStatus.SelectedValue);

        _id_ativo = 0;

        _tenta = 1;

        string mensagem = AtivoDAO.GravaStatusAtivo(_status, ativo.Observacao, ativo.Usuario_Contato, ativo.Id_Consulta, _tenta, _id_ativo);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("$('.modal-backdrop').remove();");
        sb.Append("$(document.body).removeClass('modal-open');");
        
        ScriptManager.RegisterStartupScript(Page, this.Page.GetType(), "clientscript", sb.ToString(), true);

        ClearInputs(Page.Controls);// limpa os textbox
        Response.Redirect("~/administrativo/ListaAtivos.aspx");
    }

    void ClearInputs(ControlCollection ctrls)
    {
        foreach (Control ctrl in ctrls)
        {
            if (ctrl is TextBox)
                ((TextBox)ctrl).Text = string.Empty;
            ClearInputs(ctrl.Controls);
        }
    }

    protected void btnVoltar1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/administrativo/ListaAtivos.aspx");
    }
}
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

public partial class Consultas_InformaCancelamento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Ativo_Ligacao consulta = new Ativo_Ligacao();
            ConsultasRemarcar consulta = new ConsultasRemarcar();

            int _idConsulta = Convert.ToInt32(Request.QueryString["idconsulta"]);
            string _status = Request.QueryString["status"].ToString();
            
            txbID.Text = _idConsulta.ToString();

            consulta = ConsultasDAO.getDadosConsulta(_idConsulta, _status);

            lbConCancelada.Text = consulta.id_cancela.ToString();

            lbStatus.Text = consulta.Status;
            txbNomePaciente.Text = consulta.Nome;
            txbNomeProntuario.Text = consulta.Prontuario.ToString();

            ///PROBLEMA
            txbdtConsulta.Text = consulta.Dt_Consulta.ToString();
            txbCodConsulta.Text = consulta.Codigo_Consulta.ToString();
            txbEquipe.Text = consulta.Equipe;
            txbProfissional.Text = consulta.Nome_Profissional;
            txbObservacao.Text = consulta.Observacao;

            txbTelefone1.Text = consulta.Telefone1;
            txbTelefone2.Text = consulta.Telefone2;
            txbTelefone3.Text = consulta.Telefone3;
            txbTelefone4.Text = consulta.Telefone4;
        }
    }
    protected void btnGravar_Click(object sender, EventArgs e)
    {
        ConsultasRemarcar consulta = new ConsultasRemarcar();

        consulta.id_cancela = Convert.ToInt32(lbConCancelada.Text);
        consulta.DescricaoRemarcar = txbInformacao.Text;
        consulta.Usuario_Contato = System.Web.HttpContext.Current.User.Identity.Name;

        string mensagem = ConsultasRemarcarDAO.GravaInformacaoConCancelada(consulta.id_cancela, consulta.DescricaoRemarcar, consulta.Usuario_Contato);

        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + mensagem + "');window.location = 'ConsultasRemarcar.aspx';", true);

        ClearInputs(Page.Controls);// limpa os textbox
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
}
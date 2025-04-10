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

public partial class endocrino_ListaAtivosEquipe2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    private void BindGrid()
    {
        int _ativo = 1; // já foi feito ativo desta consulta, ela não aparece na primeira listagem (1ª tentativa)
        int _tentativaLigacao = 1; // consultas 1ª tentativa

        int _quantidadeConsultas = AtivoDAO.QuantidadeConsultasRealizarAtivoEndocrino(_ativo, _tentativaLigacao);

        if (_quantidadeConsultas != 0)
        {
            string _realizadas = "N";

            GridView1.DataSource = AtivoDAO.ListaTentativaContatoEndocrino(_tentativaLigacao, _realizadas);
            GridView1.DataBind();
        }
        lbQtdConsultas.Text = _quantidadeConsultas + " Consultas a Realizar a Ativo";
    }

    protected void grdMain_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void grdMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index;
        if (e.CommandName.Equals("editRecord"))
        {
            index = Convert.ToInt32(e.CommandArgument);

            int _prontuario = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString());
            Response.Redirect("~/endocrino/DetalhesPacienteEndocrinoTentativa2.aspx?prontuario=" + _prontuario);
        }
    }
}
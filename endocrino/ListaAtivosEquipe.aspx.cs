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

public partial class administrativo_ListaAtivosEquipe : System.Web.UI.Page
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
        int _ativo = 0;
        string _status = "S";
        int _tentativaLigacao = 0;

        GridView1.DataSource = AtivoDAO.ListaConsultasEndocrino(_ativo, _status, _tentativaLigacao);
        GridView1.DataBind();
        lbQtdConsultas.Text = AtivoDAO.QuantidadeConsultasRealizarAtivoEndocrino(_ativo, _tentativaLigacao) + " Consultas a Realizar a Ativo Endócrino";
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

            int _prontuario = Convert.ToInt32(GridView1.DataKeys[index].Value.ToString()); //id da consulta
            Response.Redirect("~/endocrino/DetalhesPacienteEndocrino.aspx?prontuario=" + _prontuario + "&tentativa=0");
        }
    }
}
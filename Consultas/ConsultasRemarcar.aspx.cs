using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class publico_ConsultasRemarcar : Page
{
    private int CurrentPageIndex
    {
        get { return ViewState["CurrentPageIndex"] != null ? (int)ViewState["CurrentPageIndex"] : 0; }
        set { ViewState["CurrentPageIndex"] = value; }
    }

    private PagedDataSource pdsGlobal;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string eventTarget = Request["__EVENTTARGET"];
            string eventArgument = Request["__EVENTARGUMENT"];
            int savedPage;

            if (eventTarget == "SetGridPage" && int.TryParse(eventArgument, out savedPage))
            {
                // Atualiza a página apenas se for diferente
                if (CurrentPageIndex != savedPage)
                    CurrentPageIndex = savedPage;
            }
        }

        if (!IsPostBack && !ScriptManager.GetCurrent(this).IsInAsyncPostBack)
        {
            RegistrarScriptLocalStorage(); // Lê localStorage apenas no 1º carregamento real
            CarregarGrid();
        }
    }

    private void RegistrarScriptLocalStorage()
    {
        string script = @"
        (function () {
            var savedPage = localStorage.getItem('GridViewPageIndex');
            if (savedPage !== null && savedPage !== '0') {
                console.log('📦 PostBack para página salva:', savedPage);
                __doPostBack('SetGridPage', savedPage);
            }
        })();
    ";
        ScriptManager.RegisterStartupScript(this, GetType(), "LerPaginaSalva", script, true);
    }

    private void CarregarGrid()
    {
        int status = 3;
        int pageSize = GridView1.PageSize;
        int pageIndex = CurrentPageIndex;
        int totalRows;

        ViewState["CurrentPage"] = pageIndex;

        var pagina = ConsultasDAO.ListaConsultasCancelar(status, pageIndex + 1, pageSize, out totalRows);
        int totalPages = (int)Math.Ceiling((double)totalRows / pageSize);


        if (totalPages > 0 && pageIndex + 1 >= totalPages )
        {
            pageSize = pagina.Count;

        }
       

        var pds = new PagedDataSource
        {
            AllowPaging = true,
            AllowCustomPaging = true,
            PageSize = pageSize,
            CurrentPageIndex = pageIndex,
            VirtualCount = totalRows,
            DataSource = pagina
        };

        pdsGlobal = pds;
        GridView1.DataSource = pds;
        GridView1.DataBind();

        pagerRepeater.DataSource = Enumerable.Range(1, Math.Max(1, totalPages)).ToList();
        pagerRepeater.DataBind();

        lblTotal.Text = "Total de registros: " + totalRows;
        GridView1.PageSize = 10;
        // Salva no localStorage (C# 3.0)
        ScriptManager.RegisterStartupScript(
            this,
            GetType(),
            "SalvarPaginaAtual",
            string.Format("localStorage.setItem('GridViewPageIndex', {0});", pageIndex),
            true
        );

    }


    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentPageIndex = 0;
        CarregarGrid();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        CurrentPageIndex = e.NewPageIndex;
        CarregarGrid();
    }

   

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count > 0 && GridView1.BottomPagerRow != null)
        {
            GridView1.UseAccessibleHeader = true;
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridView1.BottomPagerRow.TableSection = TableRowSection.TableFooter;

            Label lblPagerInfo = (Label)GridView1.BottomPagerRow.FindControl("lblPagerInfo");
            if (lblPagerInfo != null && pdsGlobal != null)
            {
                int totalPages = pdsGlobal.PageCount;
                int totalItems = pdsGlobal.VirtualCount;
                int currentPage = CurrentPageIndex + 1;

                lblPagerInfo.Text = "Página " + currentPage + " de " + totalPages +
                                    " | Exibindo " + GridView1.Rows.Count + " de " + totalItems + " registros";
            }
        }
    }

    protected void pagerRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            int newPageIndex = Convert.ToInt32(e.CommandArgument) - 1;
            CurrentPageIndex = newPageIndex;
            CarregarGrid();
        }
    }

    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        if (CurrentPageIndex > 0)
        {
            CurrentPageIndex--;
            CarregarGrid();
        }
    }

    protected void btnProximo_Click(object sender, EventArgs e)
    {
        int totalRows = 0;


        // Obtem o total de linhas apenas
        totalRows = ConsultasDAO.ContarConsultasCancelar(3);

        int totalPages = (int)Math.Ceiling((double)totalRows / GridView1.PageSize);
        if (totalPages > 0 && CurrentPageIndex + 1 >= totalPages)
        {
            // Já está na última página, não faz nada
            return;
        }

        // Se chegou aqui, pode avançar para a próxima página
        CurrentPageIndex++;
        CarregarGrid();

    }

}

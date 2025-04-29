
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true"
    CodeFile="ConsultasRemarcar.aspx.cs"
    Inherits="publico_ConsultasRemarcar"
    Title="Call HSPM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- Estilos / scripts -->
    <link href="../build/css/jquery.dataTable.css" rel="stylesheet" type="text/css" />
    <script src='<%= ResolveUrl("~/moment/jquery-3.7.0.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/moment.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/jquery.dataTables.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/datetime.js") %>' charset="utf8" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <h3 class="mb-3">
        <asp:Label ID="lbTitulo" runat="server" Text="CONSULTAS PARA CANCELAR/REMARCAR" />
    </h3>

    <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <!-- Filtro -->
            <div class="row mb-2">
                <div class="col-md-2 col-sm-6 form-group">
                    <label>Status de Ligação <span class="required">*</span></label>
                    <asp:DropDownList ID="ddlStatus" runat="server"
                        CssClass="form-control"
                        AutoPostBack="True"
                        DataSourceID="SqlDataSource1"
                        DataTextField="status"
                        DataValueField="id_status" />
                </div>
            </div>

            <!-- Grid -->
            <asp:GridView ID="GridView1" runat="server"
              
                AutoGenerateColumns="False"
                DataKeyNames="Id_consulta"
                AllowPaging="True"
                AllowCustomPaging="True"
                OnPageIndexChanging="GridView1_PageIndexChanging"
                OnPreRender="GridView1_PreRender"
                CssClass="table table-striped table-bordered">

                <Columns>
                    <asp:BoundField DataField="Id_consulta" HeaderText="ID CONSULTA" />
                    <asp:BoundField DataField="Nome" HeaderText="Paciente" />
                    <asp:BoundField DataField="Prontuario" HeaderText="Prontuário/RH" />
                    <asp:BoundField DataField="Dt_Consulta" HeaderText="Data da Consulta" />
                    <asp:BoundField DataField="Grade" HeaderText="Grade" />
                    <asp:BoundField DataField="Equipe" HeaderText="Equipe" />
                    <asp:BoundField DataField="Data_Contato" HeaderText="Data da Ligação" />
                    <asp:BoundField DataField="Status" HeaderText="Status da Ligação" />
                    <asp:BoundField DataField="Observacao" HeaderText="Observação" />
                    <asp:BoundField DataField="Usuario_Contato" HeaderText="Contato" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server"
                                CommandName="editRecord"
                                CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                CssClass="btn btn-info btn-sm">
                                <i class="fa fa-pencil-square-o"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerTemplate>
                    <div class="d-flex justify-content-between align-items-center px-3 py-2">
                        <asp:Label ID="lblPagerInfo" runat="server" CssClass="text-muted" />
                        <div>
                            <asp:LinkButton ID="btnPrev" runat="server" CommandName="Page" CommandArgument="Prev" CssClass="btn btn-outline-primary btn-sm me-2">Anterior</asp:LinkButton>
                            <asp:LinkButton ID="btnNext" runat="server" CommandName="Page" CommandArgument="Next" CssClass="btn btn-outline-primary btn-sm">Próximo</asp:LinkButton>
                        </div>
                    </div>
                </PagerTemplate>

                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#f7f6f3" ForeColor="#333" />
            </asp:GridView>

            <!-- Paginação personalizada -->
            <div class="d-flex justify-content-center mt-3">
                <asp:Repeater ID="pagerRepeater" runat="server" OnItemCommand="pagerRepeater_ItemCommand">
                    <HeaderTemplate>
                        <nav><ul class="pagination pagination-sm">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class='page-item <%# (Convert.ToInt32(ViewState["CurrentPage"]) + 1 == Convert.ToInt32(Container.DataItem)) ? "active" : "" %>'>
                            <asp:LinkButton ID="lnkPage" runat="server"
                                CssClass="page-link"
                                CommandName="Page"
                                CommandArgument='<%# Container.DataItem %>'
                                Text='<%# Container.DataItem %>' />
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul></nav>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

            <!-- Botões Anterior/Próximo -->
            <div class="d-flex justify-content-between mt-2">
                <asp:LinkButton ID="btnAnterior" runat="server" CssClass="btn btn-outline-primary btn-sm"
                    OnClick="btnAnterior_Click" Text="Anterior" />
                <asp:LinkButton ID="btnProximo" runat="server" CssClass="btn btn-outline-primary btn-sm"
                    OnClick="btnProximo_Click" Text="Próximo" />
            </div>

            <asp:Label ID="lblTotal" runat="server" CssClass="mt-2 d-block" />

        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="PageIndexChanging" />
            <asp:AsyncPostBackTrigger ControlID="btnAnterior" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnProximo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- DataSource -->
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        ConnectionString="<%$ ConnectionStrings:gtaConnectionString %>"
        SelectCommand="SELECT id_status, status FROM status_consulta WHERE id_status IN (3)" />

  <script type="text/javascript">
      var $j = jQuery.noConflict();

      Sys.Application.add_load(function () {
          console.log("⚡ Sys.Application.add_load disparado");

          var table = $j('#<%= GridView1.ClientID %>');

        if ($j.fn.DataTable.isDataTable(table)) {
            console.log("♻️ Destruindo DataTable existente");
            table.DataTable().destroy();
        }

        table.DataTable({
            paging: false,
            searching: true,
            info: false,
            lengthChange: false,
            ordering: true,
            language: {
                search: "Pesquisar:",
                emptyTable: "Nenhum registro encontrado"
            },
            columnDefs: [
                { targets: [3, 6], render: DataTable.render.moment('DD/MM/YYYY HH:mm:ss', 'DD/MM/YYYY HH:mm:ss', 'pt-br') }
            ],
            order: [[0, 'desc']]
        });

    });

      // Smart Resize para responsividade
      (function ($, sr) {
          var debounce = function (func, threshold, execAsap) {
              var timeout;
              return function debounced() {
                  var obj = this, args = arguments;
                  function delayed() {
                      if (!execAsap) func.apply(obj, args);
                      timeout = null;
                  }
                  if (timeout) clearTimeout(timeout);
                  else if (execAsap) func.apply(obj, args);
                  timeout = setTimeout(delayed, threshold || 100);
              };
          };
          $.fn[sr] = function (fn) {
              return fn ? this.on('resize', debounce(fn)) : this.trigger(sr);
          };
      })($j, 'smartresize');

</script>



</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ListaAtivos3.aspx.cs" Inherits="administrativo_ListaAtivos3" Title="Call HSPM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <link href="../build/css/jquery.dataTable.css" rel="stylesheet" type="text/css" />


 
  <script src='<%= ResolveUrl("~/moment/jquery-3.7.0.js") %>' type="text/javascript"></script>
 <script src='<%= ResolveUrl("~/moment/moment.min.js") %>' type="text/javascript"></script>
 <script src='<%= ResolveUrl("~/moment/jquery.dataTables.min.js") %>' type="text/javascript"></script>
 <script src='<%= ResolveUrl("~/moment/datetime.js") %>' charset="utf8" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <h1>Ativo de Consultas Marcadas - 3ª Tentativa</h1>
    </div>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
              <h3>
                <asp:Label ID="lbQtdConsultas" runat="server" Text=""></asp:Label></h3>
                     <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="False"
 DataKeyNames="Id_consulta" OnRowCommand="grdMain_RowCommand"
CellPadding="4" ForeColor="#333333" GridLines="Horizontal" BorderColor="#e0ddd1" Width="100%" >    
                <Columns>
                    <asp:BoundField DataField="Id_consulta" HeaderText="ID" SortExpression="Id_consulta"
                        ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                    <asp:BoundField DataField="Nome" HeaderText="Paciente" SortExpression="Nome" ItemStyle-CssClass="hidden-md"
                        HeaderStyle-CssClass="hidden-md" />
                    <asp:BoundField DataField="Prontuario" HeaderText="PRONTUÁRIO/RH" SortExpression="Prontuario"
                        HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                    <asp:BoundField DataField="Dt_consulta" HeaderText="DATA DA CONSULTA" SortExpression="Dt_consulta"
                        ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                    <asp:TemplateField HeaderStyle-CssClass="sorting_disabled">
                        <ItemTemplate>
                            <div class="form-inline">
                                <asp:LinkButton ID="gvlnkEdit" CommandName="editRecord" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'
                                    CssClass="btn btn-info" runat="server">
                                    <i class="fa fa-phone" title="Ligar"></i> 
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
              
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
         <script type="text/javascript">
             $(document).ready(function () {
                 $.noConflict();

                 let table = $('#<%= GridView1.ClientID %>');

             // Mover o primeiro TR para o THEAD corretamente
             let firstRow = table.find("tr:first").detach();
             table.prepend($("<thead></thead>").append(firstRow));

             // Inicializar o DataTable corretamente
             table.DataTable({
                 destroy: true, // Permite reinicializar a tabela sem erro
                 language: {
                     search: "<i class='fa fa-search' aria-hidden='true'></i>",
                     processing: "Processando...",
                     lengthMenu: "Mostrando _MENU_ registros por página",
                     info: "Mostrando página _PAGE_ de _PAGES_",
                     infoEmpty: "Nenhum registro encontrado",
                     infoFiltered: "(filtrado de _MAX_ registros no total)"
                 },
                 columnDefs: [
                     { targets: [3], render: DataTable.render.moment('DD/MM/YYYY HH:mm:ss', 'DD/MM/YYYY HH:mm:ss', 'pt-br') }
                 ]
             });

         }); </script>
</asp:Content>


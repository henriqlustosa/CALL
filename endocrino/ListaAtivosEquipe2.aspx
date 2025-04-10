<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ListaAtivosEquipe2.aspx.cs" Inherits="endocrino_ListaAtivosEquipe2" Title="Call HSPM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
              <link href="../build/css/jquery.dataTable.css" rel="stylesheet" type="text/css" />
 <script src='<%= ResolveUrl("~/moment/jquery-3.7.0.js") %>' type="text/javascript"></script>
<script src='<%= ResolveUrl("~/moment/moment.min.js") %>' type="text/javascript"></script>
<script src='<%= ResolveUrl("~/moment/jquery.dataTables.min.js") %>' type="text/javascript"></script>
<script src='<%= ResolveUrl("~/moment/datetime.js") %>' charset="utf8" type="text/javascript"></script>
        <script src='<%= ResolveUrl("~/moment/jquery.smartresize.js") %>'></script>
        <style>
.dataTables_paginate {
    display: block !important;
    overflow-x: auto !important;
    overflow-y: hidden !important;
    white-space: nowrap !important;
    scroll-behavior: smooth;

    width: 100% !important;
    max-width: 100% !important;
    box-sizing: border-box;

    padding: 10px 0;

    /* 👇 Aqui está o ajuste de altura */
    height: auto !important;          /* deixa o container crescer */
    min-height: 55px;                 /* altura mínima para caber os botões */
}


/* 🔧 Container da paginação com scroll horizontal e linha única */
.dataTables_paginate {
    display: block !important;
    overflow-x: auto !important;
    overflow-y: hidden !important;
    white-space: nowrap !important;
    padding: 10px 0;
    text-align: left;
    scroll-behavior: smooth;
}

/* 🔧 Garante que todos os botões fiquem lado a lado */
.dataTables_paginate span {
    display: inline-block;
}

/* ✅ Estilização dos botões */
.dataTables_paginate .paginate_button {
    display: inline-block;
    min-width: 40px;
    padding: 6px 12px;
    margin: 2px;
    font-size: 14px;
    background-color: #e0e0e0;
    color: #333;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.2s ease;
    text-align: center;
    white-space: nowrap;
    vertical-align: middle;
}

/* Botão ativo (página atual) */
.dataTables_paginate .paginate_button.current {
    background-color: red !important;
    color: white !important;
    font-weight: bold;
}

/* Hover */
.dataTables_paginate .paginate_button:hover {
    background-color: #c0c



    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <h1>Ativo de Consultas Marcadas Endócrino - 2ª Tentativa</h1>
    </div>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
              <h3>
                <asp:Label ID="lbQtdConsultas" runat="server" Text=""></asp:Label></h3>
      <%--      <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                OnPageIndexChanging="grdMain_PageIndexChanging" DataKeyNames="Prontuario" OnRowCommand="grdMain_RowCommand"
                CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />--%>
                    <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="False" ClientIDMode="Static" 

  DataKeyNames="Prontuario" OnRowCommand="grdMain_RowCommand"
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
     Sys.Application.add_load(function () {
         $.noConflict();

         let table = $('#<%= GridView1.ClientID %>');
         $.fn.DataTable.ext.pager.numbers_length = 20; // padrão é 7

         // Verificar se já foi inicializado antes de reprocessar o THEAD
         if (!$.fn.DataTable.isDataTable(table)) {
             let firstRow = table.find("tr:first").detach();
             table.prepend($("<thead></thead>").append(firstRow));
         }

         // Destruir instância anterior se necessário
         if ($.fn.DataTable.isDataTable(table)) {
             table.DataTable().destroy();
         }

         // Inicializar o DataTable com paginação completa
         table.DataTable({
             destroy: true,
             pageLength: 10,
             lengthChange: false,
             ordering: false, // <- isso remove as setinhas de todas as colunas
             order: [[3, 'asc']], // Ordena pela coluna 3 (dt_consulta)
             pagingType: "full_numbers",
             stateSave: true,
             stateSaveCallback: function (settings, data) {
                 sessionStorage.setItem('DataTables_' + settings.sInstance, JSON.stringify(data));
             },
             stateLoadCallback: function (settings) {
                 return JSON.parse(sessionStorage.getItem('DataTables_' + settings.sInstance));
             },
             language: {
                 search: "<i class='fa fa-search' aria-hidden='true'></i>",
                 processing: "Processando...",
                 lengthMenu: "Mostrando _MENU_ registros por página",
                 info: "Mostrando página _PAGE_ de _PAGES_",
                 infoEmpty: "Nenhum registro encontrado",
                 infoFiltered: "(filtrado de _MAX_ registros no total)",
                 paginate: {
                     first: "Primeiro",
                     last: "Último",
                     next: "Próximo",
                     previous: "Anterior"
                 }
             },
             columnDefs: [
                 {
                     targets: [3], // coluna de data
                     render: DataTable.render.moment('DD/MM/YYYY HH:mm:ss', 'DD/MM/YYYY HH:mm:ss', 'pt-br')
                 }
             ]
         });
     });

     Sys.Application.add_load(function () {
         $('#GridView1').DataTable().destroy(); // Destroi o anterior
         $('#GridView1').DataTable({
             stateSave: true,
             stateSaveCallback: function (settings, data) {
                 sessionStorage.setItem('DataTables_' + settings.sInstance, JSON.stringify(data));
             },
             stateLoadCallback: function (settings) {
                 return JSON.parse(sessionStorage.getItem('DataTables_' + settings.sInstance));
             }
         });
     });



 </script>
</asp:Content>


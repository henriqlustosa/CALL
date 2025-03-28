<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="historico.aspx.cs" Inherits="Paciente_historico" Title="Call HSPM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link href="../build/css/jquery.dataTable.css" rel="stylesheet" type="text/css" />
    <script src='<%= ResolveUrl("~/moment/jquery-3.7.0.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/parsley.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/moment.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/jquery.dataTables.min.js") %>' type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/datetime.js") %>' charset="utf8" type="text/javascript"></script>
    <script src='<%= ResolveUrl("~/moment/jquery.smartresize.js") %>'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <div class="x_title">
        <h2>
            Histórico de Ativos Call HSPM</h2>
        <div class="clearfix">
        </div>
    </div>
        <div id="demo-form2" data-parsley-validate class="form-horizontal form-label-left input_mask">
            <div class="row">
                <div class="form-group">
                    <asp:Label ID="Msg" runat="server" ForeColor="maroon" class="control-label col-md-12" /><br />
                </div>
            </div>
            <div class="row">
                <div class="form-group">
                    <label class="control-label col-md-4" for="UsernameTextBox">
                        Prontuário: <span class="required">*</span>
                    </label>
                    <div class="col-md-8">
                        <asp:TextBox ID="txbProntuario" class="form-control" runat="server" AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="UsernameRequiredValidator" runat="server" ControlToValidate="txbProntuario"
                            ForeColor="red" Display="Static" ErrorMessage="Required" /><br />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-4 col-sm-4 col-xs-8 ">
                        <asp:Button ID="SearchButton" Text="Pesquisar" runat="server" Enabled="true" class="btn btn-primary"
                            OnClick="SearchHistorico_OnClick" />
                    </div>
                </div>
            </div>
            
            <div class="row">
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-6 col-sm-12 col-xs-12 form-group">
                            <label>
                                Nome</label>
                            <asp:TextBox ID="txbNomePaciente" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 col-sm-12 col-xs-12 form-group">
                            <label>
                                Telefone 1</label>
                            <asp:TextBox ID="txbTelefone1" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-sm-12 col-xs-12 form-group">
                            <label>
                                Telefone 2</label>
                            <asp:TextBox ID="txbTelefone2" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-sm-12 col-xs-12 form-group">
                            <label>
                                Telefone 3</label>
                            <asp:TextBox ID="txbTelefone3" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2 col-sm-12 col-xs-12 form-group">
                            <label>
                                Telefone 4</label>
                            <asp:TextBox ID="txbTelefone4" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 col-sm-12 col-xs-12 form-group">
                            <asp:Button ID="btnAtualizaTelefones" class="btn btn-primary" runat="server" Text="Atualizar Telefones"
                                OnClick="btnAtualizaTelefones_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <hr />
    <!-- Outros campos (prontuário, nome, telefones) -->
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                        DataKeyNames="Id_ativo"  OnRowCommand="GridView1_RowCommand" CellPadding="4" ForeColor="#333333" GridLines="None"
                        Width="100%">
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <Columns>
                            <asp:BoundField DataField="Id_ativo" HeaderText="ID ATIVO" SortExpression="Id_ativo"
    ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Id_consulta" HeaderText="ID CONSULTA" SortExpression="Id_consulta"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Codigo_Consulta" HeaderText="Consulta" SortExpression="Codigo_Consulta"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Dt_Consulta" HeaderText="DATA DA CONSULTA" SortExpression="Dt_consulta"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade" ItemStyle-CssClass="hidden-xs"
                                HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Equipe" HeaderText="Equipe" SortExpression="Equipe" ItemStyle-CssClass="hidden-xs"
                                HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Nome_Profissional" HeaderText="Profissional" SortExpression="Nome_Profissional"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Status" HeaderText="Status Ligação" SortExpression="Status"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Observacao" HeaderText="Observação" SortExpression="Observacao"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Data_Contato" HeaderText="Data de Contato" SortExpression="Data_Contato"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="Usuario_Contato" HeaderText="Usuario Contato" SortExpression="Usuario_Contato"
                                ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="DescricaoRemarcar" HeaderText="Informação Complementar"
                                SortExpression="DescricaoRemarcar" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                               <asp:TemplateField HeaderText="Ações">
       <ItemTemplate>
           <asp:LinkButton ID="btnEditar" runat="server" Text="Editar"
               CommandName="EditarPopup" CommandArgument='<%# Eval("Id_ativo") %>'
               CssClass="btn btn-sm btn-primary" />
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
  
<asp:UpdatePanel ID="UpdatePanelModal" runat="server" UpdateMode="Conditional">
 <ContentTemplate>
    <div class="modal fade" id="modalEditar" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Editar Ligacao</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfIdConsulta" runat="server" />
                    <div class="form-group">
                        <label>Status Ligacao</label>
                        <asp:DropDownList ID="ddlStatusModal" runat="server" CssClass="form-control">
                          
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Observação</label>
                        <asp:TextBox ID="txtObservacaoModal" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSalvarModal" runat="server" CssClass="btn btn-success" Text="Salvar" OnClick="btnSalvarModal_Click"  ClientIDMode="Static" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                </div>
            </div>
        </div>
    </div>
        </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function abrirModal() {
        $('#modalEditar').modal('show');
    }
    var $j = jQuery.noConflict();
    function inicializarDataTable() {
       

        let table = $j('#<%= GridView1.ClientID %>');

        // Remove qualquer <thead> duplicado antes
        table.find("thead").remove();

        // Pega a primeira linha como cabeçalho
        let firstRow = table.find("tr:first").clone();
        table.find("tr:first").remove();

        // Cria o novo <thead>
        table.prepend($("<thead></thead>").append(firstRow));


        table.DataTable({
            destroy: true,
            stateSave: true, // <- mantém paginação, ordenação, busca
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
                    targets: [3, 9],
                    render: DataTable.render.moment('DD/MM/YYYY HH:mm:ss', 'DD/MM/YYYY HH:mm:ss', 'pt-br')
                }
            ],
            order: [[0, 'desc']]
        });
    }


  //  $(document).ready(function () {
// inicializarDataTable();
//    });
    Sys.Application.add_load(function () {
        

        // Fecha o modal somente se o postback veio do botão Salvar
        if (__EVENTTARGET === "btnSalvarModal") {
            $('#modalEditar').modal('hide');
        }
        inicializarDataTable();
    });


</script>
<!-- CDN necessário para funcionamento do modal -->
<script src="../moment/bootstrap.bundle.min.js"></script>
    
</asp:Content>
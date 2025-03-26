
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CadastraStatusLigacao.aspx.cs" Inherits="Restrito_CadastraStatusLigacao"
    Title="Call HSPM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <!-- Estilos -->
<link rel="stylesheet" href="../vendors/iCheck/skins/flat/green.css" />
    <link href="../fontawesome-free-6.7.2-web/css/all.min.css" rel="stylesheet" />

<link href="../build/css/jquery.dataTable.css" rel="stylesheet" type="text/css" />
<!-- Scripts -->
<script src='<%= ResolveUrl("~/moment/jquery-3.7.0.js") %>'></script>
<script src='<%= ResolveUrl("~/moment/bootstrap.bundle.min.js") %>'></script>
<script src='<%= ResolveUrl("~/moment/moment.min.js") %>'></script>
<script src='<%= ResolveUrl("~/moment/jquery.dataTables.min.js") %>'></script>
<script src='<%= ResolveUrl("~/moment/datetime.js") %>' charset="utf8"></script>
<script src='<%= ResolveUrl("~/moment/icheck.min.js") %>'></script>
<script src='<%= ResolveUrl("~/moment/jquery.smartresize.js") %>'></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <div class="x_title">
        <h2>Cadastro <small>Status de ligações</small></h2>
        <div class="clearfix"></div>
    </div>

    <div class="x_content">
     <%--   <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">--%>
            <ContentTemplate>

                <!-- Botão oculto para postback do modal -->
                <asp:Button ID="btnSalvarEdicao" runat="server" OnClick="BtnSalvarEdicao_Click" Style="display:none;" />

                <!-- Formulário de Cadastro -->
     
            
                        <div id="demo-form" class="col-sm-4" data-parsley-validate>

                            <label for="descricaoStatus">Descrição * :</label>

                            <input type="text" id="descricaoStatus" class="form-control" name="descricaoStatus" required />

                            <label style="padding-top: 20px;">Tentativa de Ligação? *:</label>

                            <p>
                                Sim: <input type="radio" class="flat" name="tenta" id="tentaS" value="S" checked required />
                                Não: <input type="radio" class="flat" name="tenta" id="tentaN" value="N" />
                            </p>

                            <label style="padding-top: 10px;">Ativo? *:</label>

                            <p>
                                Sim: <input type="radio" class="flat" name="ativo" id="ativoS" value="S" checked required />
                                Não: <input type="radio" class="flat" name="ativo" id="ativoN" value="N" />
                            </p>

                            <br />
                            <asp:Button ID="btnCadastrar" class="btn btn-primary" runat="server" Text="Cadastrar" OnClick="btnCadastrar_Click" />
                        </div>

                        <!-- GridView -->
                      
                                   <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                       <ContentTemplate>
                                               <asp:GridView ID="gvStatusLigacao" runat="server"  AutoGenerateColumns="False" 
 DataKeyNames="Id_status"   OnRowCommand="gvStatusLigacao_RowCommand"
CellPadding="4" ForeColor="#333333" GridLines="Horizontal" BorderColor="#e0ddd1" Width="100%" >    

                            <Columns>
                                <asp:BoundField DataField="Id_status" HeaderText="ID" SortExpression="Id_status" />
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" SortExpression="Descricao" />
                                <asp:BoundField DataField="Tentativa" HeaderText="Tentativa" SortExpression="Tentativa" />
                                <asp:BoundField DataField="Ativo" HeaderText="Ativo" SortExpression="Ativo" />

                                <asp:TemplateField HeaderText="Ações">
                                    <ItemTemplate>
                                        <button type="button" class="btn btn-sm btn-primary btn-editar"
                                            data-id='<%# Eval("Id_status") %>'
                                            data-descricao='<%# Eval("Descricao") %>'
                                            data-tentativa='<%# Eval("Tentativa") %>'
                                            data-ativo='<%# Eval("Ativo") %>'>✏️</button>

                                    <asp:LinkButton ID="btnExcluir" runat="server" CssClass="btn btn-sm btn-danger"
                                            CommandName="Excluir" CommandArgument='<%# Eval("Id_status") %>'
                                            OnClientClick="return confirm('Tem certeza que deseja excluir?');">
                                            ❌
                                        </asp:LinkButton>
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

       <%-- </asp:UpdatePanel>--%>
    </div>

    <!-- Modal de Mensagem -->
<div class="modal fade" id="modalMsg" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog"><div class="modal-content">
        <div class="modal-header">
            <h5 class="modal-title">Mensagem</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="modal-body">
            <asp:Label ID="lbMensagem" runat="server" Text="" CssClass="text-danger"></asp:Label>
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
        </div>
    </div></div>
</div>

   <div class="modal fade" id="modalEditar" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <!-- Cabeçalho do Modal -->
            <div class="modal-header">
                <h5 class="modal-title">Editar Status</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>

            <!-- Corpo do Modal -->
            <div class="modal-body">
                <input type="hidden" id="edit-id" />

                <!-- Campo Descrição -->
                <div class="form-group">
                    <label for="edit-descricao"><strong>Descrição:</strong></label>
                    <input type="text" id="edit-descricao" class="form-control" placeholder="Digite a descrição" />
                </div>

                <!-- Grupo Tentativa -->
                <div class="form-group">
                    <label class="d-block"><strong>Tentativa:</strong></label>
                    <div class="form-check form-check-inline">
                        <input class="form-check-input" type="radio" name="edit-tentativa" id="tentativaSim" value="S">
                        <label class="form-check-label" for="tentativaSim">Sim</label>
                    </div>
                    <div class="form-check form-check-inline">
                        <input class="form-check-input" type="radio" name="edit-tentativa" id="tentativaNao" value="N">
                        <label class="form-check-label" for="tentativaNao">Não</label>
                    </div>
                </div>

                <!-- Grupo Ativo -->
                <div class="form-group">
                    <label class="d-block"><strong>Ativo:</strong></label>
                    <div class="form-check form-check-inline">
                        <input class="form-check-input" type="radio" name="edit-ativo" id="ativoSim" value="S">
                        <label class="form-check-label" for="ativoSim">Sim</label>
                    </div>
                    <div class="form-check form-check-inline">
                        <input class="form-check-input" type="radio" name="edit-ativo" id="ativoNao" value="N">
                        <label class="form-check-label" for="ativoNao">Não</label>
                    </div>
                </div>
            </div>

            <!-- Rodapé do Modal -->
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                    <i class="fas fa-times"></i> Cancelar
                </button>
                <button type="button" class="btn btn-success" id="btnSalvar">
                    <i class="fas fa-check"></i> Salvar alterações
                </button>
            </div>
        </div>
    </div>
</div>


    <!-- Scripts -->
    <script type="text/javascript">
        Sys.Application.add_load(function () {
            $.noConflict();

            let table = $('#<%= gvStatusLigacao.ClientID %>');

          if ($.fn.DataTable.isDataTable(table)) {
              table.DataTable().destroy(); // Destroi a instância antiga
              table.find('thead').remove(); // Remove o thead anterior
          }

          // Move o cabeçalho correto para o thead novamente
          let firstRow = table.find("tr:first").detach();
          table.prepend($("<thead></thead>").append(firstRow));

          // Reinicializa
          table.DataTable({
              paging: true,
              searching: true,
              ordering: true,
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
              }
          });
      });


    // Abrir modal de edição
    $(document).on("click", ".btn-editar", function () {
        $("#edit-id").val($(this).data("id"));
        $("#edit-descricao").val($(this).data("descricao"));

        let tentativa = $(this).data("tentativa");
        let ativo = $(this).data("ativo");

        $("input[name='edit-tentativa']").prop("checked", false);
        $("input[name='edit-ativo']").prop("checked", false);

        if (tentativa === "S") {
            $("#tentativaSim").prop("checked", true);
        } else {
            $("#tentativaNao").prop("checked", true);
        }

        if (ativo === "S") {
            $("#ativoSim").prop("checked", true);
        } else {
            $("#ativoNao").prop("checked", true);
        }

        $("#modalEditar").modal("show");
    });

    // Salvamento da edição
    $("#btnSalvar").on("click", function () {
        let tentativa = $("input[name='edit-tentativa']:checked").val();
        let ativo = $("input[name='edit-ativo']:checked").val();

        if (!tentativa || !ativo) {
            alert("Selecione as opções de Tentativa e Ativo antes de salvar.");
            return;
        }

        $("form input[name^='edit-']").remove();
        $('<input>').attr({ type: 'hidden', name: 'edit-id', value: $("#edit-id").val() }).appendTo('form');
        $('<input>').attr({ type: 'hidden', name: 'edit-descricao', value: $("#edit-descricao").val() }).appendTo('form');
        $('<input>').attr({ type: 'hidden', name: 'edit-tentativa', value: tentativa }).appendTo('form');
        $('<input>').attr({ type: 'hidden', name: 'edit-ativo', value: ativo }).appendTo('form');
        __doPostBack('<%= btnSalvarEdicao.UniqueID %>', '');
        $("#modalEditar").modal("hide");

        //// Aguarda 1 segundo e recarrega a página
        //setTimeout(function () {
        //    location.reload();
        //}, 1000);
    });
 


    </script>
</asp:Content>

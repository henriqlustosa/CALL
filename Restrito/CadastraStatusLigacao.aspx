<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CadastraStatusLigacao.aspx.cs" Inherits="Restrito_CadastraStatusLigacao"
    Title="Call HSPM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <!-- Estilos -->
  <link rel="stylesheet" href="../vendors/iCheck/skins/flat/green.css" />
  <link href="../fontawesome-free-6.7.2-web/css/all.min.css" rel="stylesheet" />
  <link href="../build/css/jquery.dataTable.css" rel="stylesheet" type="text/css" />

  <style>
    .badge { display:inline-block; padding:.35rem .6rem; border-radius:.5rem; font-weight:600; font-size:.85rem; }
    .badge-yes { background:#e6f4ea; color:#137333; border:1px solid #c6e7cc; }
    .badge-no  { background:#fce8e6; color:#a50e0e; border:1px solid #f4c7c3; }
    .badge-na  { background:#eef2ff; color:#1e40af; border:1px solid #dbeafe; }
    .btn-icon { display:inline-flex; align-items:center; gap:.35rem; }
    .dt-header i{ margin-right:.35rem; opacity:.85; }
  </style>

  <!-- Scripts -->
  <script src='<%= ResolveUrl("~/moment/jquery-3.7.0.js") %>'></script>
  <script src='<%= ResolveUrl("~/moment/bootstrap.bundle.min.js") %>'></script>
  <script src='<%= ResolveUrl("~/moment/moment.min.js") %>'></script>
  <script src='<%= ResolveUrl("~/moment/jquery.dataTables.min.js") %>'></script>
  <script src='<%= ResolveUrl("~/moment/datetime.js") %>' charset="utf8"></script>
  <script src='<%= ResolveUrl("~/moment/icheck.min.js") %>'></script>
  <script src='<%= ResolveUrl("~/moment/jquery.smartresize.js") %>'></script>

  <script type="text/javascript">
    (function ($) {
      "use strict";

      // --------- DataTables (reinit seguro) ----------
      function initDataTables() {
        var $tbl = $('#<%= gvStatusLigacao.ClientID %>');
        if (!$tbl.length) return;

        if ($.fn.DataTable.isDataTable($tbl[0])) {
          $tbl.DataTable().clear().destroy();
        }

        $tbl.DataTable({
          paging: true,
          searching: true,
          ordering: true,
          stateSave: true,
          language: {
            search: "<i class='fa fa-search' aria-hidden='true'></i>",
            processing: "Processando...",
            lengthMenu: "Mostrando _MENU_ registros por página",
            info: "Mostrando página _PAGE_ de _PAGES_",
            infoEmpty: "Nenhum registro encontrado",
            infoFiltered: "(filtrado de _MAX_ registros no total)",
            paginate: { first:"Primeiro", last:"Último", next:"Próximo", previous:"Anterior" }
          }
        });

        $('[data-toggle="tooltip"]').tooltip({ boundary: 'window' });
      }

      // --------- iCheck SOMENTE fora do modal (classe .flat) ----------
      function initICheck() {
        $('input.flat').each(function () {
          var $i = $(this);
          if ($i.parent('.icheckbox_flat-green, .iradio_flat-green').length) {
            try { $i.iCheck('destroy'); } catch(e) {}
          }
          $i.iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
          });
        });
      }

      // --------- Handlers com namespaces ----------
      function wireHandlers() {
        // Abrir modal de edição
        $(document).off('click.btnEditar', '.btn-editar')
          .on('click.btnEditar', '.btn-editar', function (e) {
            e.preventDefault();

            var $btn = $(this);
            $('#edit-id').val($btn.attr('data-id') || '');
            $('#edit-descricao').val($btn.attr('data-descricao') || '');

            var tentativa = String($btn.attr('data-tentativa') || '').toUpperCase();
            var ativo     = String($btn.attr('data-ativo')     || '').toUpperCase();

            $("input[name='edit-tentativa']").prop("checked", false);
            $("input[name='edit-ativo']").prop("checked", false);

            if (tentativa === "S") $("#tentativaSim").prop("checked", true);
            else if (tentativa === "N") $("#tentativaNao").prop("checked", true);

            if (ativo === "S") $("#ativoSim").prop("checked", true);
            else if (ativo === "N") $("#ativoNao").prop("checked", true);

            $('#modalEditar').modal('show');
          });

        // Ao ABRIR o modal: garanta que NENHUM iCheck esteja aplicado lá dentro
        $('#modalEditar').off('show.bs.modal.repair')
          .on('show.bs.modal.repair', function () {
            $(this).find('input[type=radio], input[type=checkbox]').each(function () {
              var $i = $(this), $p = $i.parent();
              if ($p.hasClass('icheckbox_flat-green') || $p.hasClass('iradio_flat-green')) {
                try { $i.iCheck('destroy'); } catch(e) {}
              }
              $i.css({ position:'static', opacity:1, display:'inline-block' });
            });
          });

        // (Opcional) limpar campos ao FECHAR o modal
        $('#modalEditar').off('hidden.bs.modal.clear')
          .on('hidden.bs.modal.clear', function () {
            $("#edit-id").val("");
            $("#edit-descricao").val("");
            $("input[name='edit-tentativa'], input[name='edit-ativo']").prop("checked", false);
          });

        // Salvar (NÃO fecha o modal aqui; quem fecha é o endRequest)
        $(document).off('click.btnSalvar', '#btnSalvar')
          .on('click.btnSalvar', '#btnSalvar', function (e) {
            e.preventDefault();

            var tentativa = $("input[name='edit-tentativa']:checked").val();
            var ativo     = $("input[name='edit-ativo']:checked").val();
            if (!tentativa || !ativo) {
              alert("Selecione as opções de Tentativa e Ativo antes de salvar.");
              return;
            }

            // remove SOMENTE os inputs hidden criados anteriormente (marcados com data-edit-shadow)
            $("form input[type='hidden'][data-edit-shadow='1']").remove();

            // recria os campos hidden para o servidor (prefixo exclusivo hdn-edit-)
            $('<input>', { type:'hidden', name:'hdn-edit-id',        value: $("#edit-id").val(),        'data-edit-shadow':'1' }).appendTo('form');
            $('<input>', { type:'hidden', name:'hdn-edit-descricao', value: $("#edit-descricao").val(), 'data-edit-shadow':'1' }).appendTo('form');
            $('<input>', { type:'hidden', name:'hdn-edit-tentativa', value: tentativa,                   'data-edit-shadow':'1' }).appendTo('form');
            $('<input>', { type:'hidden', name:'hdn-edit-ativo',     value: ativo,                       'data-edit-shadow':'1' }).appendTo('form');

            // FECHAR O MODAL IMEDIATAMENTE
            $('#modalEditar').modal('hide');
            // dispara o postback do botão oculto
            __doPostBack('<%= btnSalvarEdicao.UniqueID %>', '');
          });
      }

      function initAll() {
        initDataTables();
        initICheck();
        wireHandlers();
        $('[data-toggle="tooltip"]').tooltip({ boundary: 'window' });
      }

      // --------- Fechar modal APÓS o UpdatePanel terminar ----------
      (function hookEndRequest() {
        var prm = window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager
          ? Sys.WebForms.PageRequestManager.getInstance()
          : null;

        if (!prm) {
          $(function () { initAll(); });
          return;
        }

        $(function () { initAll(); });

        var closing = false;
        prm.add_endRequest(function () {
          if (!closing) {
            closing = true;

            // fecha o modal após a atualização parcial
            $('#modalEditar').modal('hide');

            // re-inicializa componentes
            initDataTables();
            initICheck();
            wireHandlers();

            setTimeout(function () { closing = false; }, 0);
          }
        });
      })();

      // expõe para o code-behind opcionalmente chamar após DataBind
      window.initDataTables = initDataTables;
      window.initICheck = initICheck;

    })(jQuery);
  </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
  <asp:ScriptManager ID="ScriptManager1" runat="server" />

  <div class="x_title">
    <h2><i class="fa fa-phone"></i> Cadastro <small>Status de ligações</small></h2>
    <div class="clearfix"></div>
  </div>

  <div class="x_content">
    <!-- Botão oculto p/ postback do modal de edição -->
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
      <asp:Button ID="btnCadastrar" CssClass="btn btn-primary btn-icon" runat="server"
                  Text=" Cadastrar" OnClick="btnCadastrar_Click" />
      <script>
        document.addEventListener('DOMContentLoaded', function () {
          var el = document.getElementById('<%= btnCadastrar.ClientID %>');
            if (el) el.insertAdjacentHTML('afterbegin', '<i class="fa fa-plus-circle"></i>');
        });
      </script>
    </div>

    <!-- GridView -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:GridView ID="gvStatusLigacao" runat="server" AutoGenerateColumns="False"
                      DataKeyNames="Id_status"
                      OnRowCommand="gvStatusLigacao_RowCommand"
                      OnRowDataBound="gvStatusLigacao_RowDataBound"
                      CellPadding="4" ForeColor="#333333" GridLines="Horizontal"
                      BorderColor="#e0ddd1" Width="100%">
          <Columns>
            <asp:TemplateField HeaderText="<span class='dt-header'><i class='fa fa-hashtag'></i>ID</span>" SortExpression="Id_status">
              <ItemTemplate><%# Eval("Id_status") %></ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<span class='dt-header'><i class='fa fa-align-left'></i>Descrição</span>" SortExpression="Descricao">
              <ItemTemplate><%# Eval("Descricao") %></ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<span class='dt-header'><i class='fa fa-redo-alt'></i>Tentativa</span>" SortExpression="Tentativa">
              <ItemTemplate>
                <%# Eval("Tentativa").ToString() == "Sim"
                    ? "<span class='badge badge-yes'>Sim</span>"
                    : (Eval("Tentativa").ToString() == "Não"
                        ? "<span class='badge badge-no'>Não</span>"
                        : "<span class='badge badge-na'>Não informado</span>") %>
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<span class='dt-header'><i class='fa fa-toggle-on'></i>Ativo</span>" SortExpression="Ativo">
              <ItemTemplate>
                <%# Eval("Ativo").ToString() == "Sim"
                    ? "<span class='badge badge-yes'>Sim</span>"
                    : (Eval("Ativo").ToString() == "Não"
                        ? "<span class='badge badge-no'>Não</span>"
                        : "<span class='badge badge-na'>Não informado</span>") %>
              </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="<span class='dt-header'><i class='fa fa-tools'></i>Ações</span>">
              <ItemTemplate>
                <!-- EDITAR: atributos data-* são aplicados no RowDataBound no code-behind -->
                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-primary btn-icon btn-editar"
                                OnClientClick="return false;" data-toggle="tooltip" data-placement="top" title="Editar">
                  <i class="fa fa-pen"></i><span>Editar</span>
                </asp:LinkButton>

                <asp:LinkButton ID="btnExcluir" runat="server" CssClass="btn btn-sm btn-danger btn-icon"
                                CommandName="Excluir"
                                CommandArgument='<%# Eval("Id_status") %>'
                                data-toggle="tooltip" data-placement="top" title="Excluir"
                                OnClientClick="return confirm('Tem certeza que deseja excluir?');">
                  <i class="fa fa-trash"></i><span>Excluir</span>
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
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSalvarEdicao" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCadastrar" EventName="Click" />
      </Triggers>
    </asp:UpdatePanel>
  </div>

  <!-- Modal de Mensagem -->
  <div class="modal fade" id="modalMsg" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog"><div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title"><i class="fa fa-info-circle"></i> Mensagem</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:Label ID="lbMensagem" runat="server" Text="" CssClass="text-danger"></asp:Label>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal"><i class="fa fa-times"></i> Fechar</button>
      </div>
    </div></div>
  </div>

  <!-- Modal de Edição -->
  <div class="modal fade" id="modalEditar" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog"><div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title"><i class="fa fa-pen"></i> Editar Status</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>

      <div class="modal-body">
        <input type="hidden" id="edit-id" />
        <div class="form-group">
          <label for="edit-descricao"><strong>Descrição:</strong></label>
          <input type="text" id="edit-descricao" class="form-control" placeholder="Digite a descrição" maxlength="100" />
        </div>

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

      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">
          <i class="fa fa-times"></i> Cancelar
        </button>
        <button type="button" class="btn btn-success" id="btnSalvar">
          <i class="fa fa-check"></i> Salvar alterações
        </button>
      </div>
    </div></div>
  </div>
</asp:Content>

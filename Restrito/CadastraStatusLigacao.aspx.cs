using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Restrito_CadastraStatusLigacao : System.Web.UI.Page
{
    //protected void Page_Init(object sender, EventArgs e)
    //{
    //    Page.ViewStateUserKey = Session.SessionID; // proteção básica
    //}

    private string ConnStr { get { return ConfigurationManager.ConnectionStrings["gtaConnectionString"].ConnectionString; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        VerificarPermissaoUsuario();
        CarregaStatus();
        AplicarPermissoesNosControlesDeTela();
    }

    private void VerificarPermissaoUsuario()
    {
        string usuarioLogado = (User != null && User.Identity != null) ? User.Identity.Name : string.Empty;
        List<string> perfisUsuario = ObterPerfisUsuario(usuarioLogado);
        bool isAdmin = perfisUsuario.Any(delegate (string p) {
            return string.Equals(p, "Administradores", StringComparison.OrdinalIgnoreCase);
        });
        ViewState["PermitirEdicao"] = isAdmin;
        ViewState["PermitirExclusao"] = isAdmin;
    }

    private List<string> ObterPerfisUsuario(string nomeUsuario)
    {
        var perfis = new List<string>();
        const string sql = @"
            SELECT r.RoleName
            FROM [hspmCall].[dbo].[aspnet_Users] u
            JOIN [hspmCall].[dbo].[aspnet_UsersInRoles] ur ON u.UserId = ur.UserId
            JOIN [hspmCall].[dbo].[aspnet_Roles] r        ON ur.RoleId = r.RoleId
            WHERE u.UserName = @Username;";

        using (var conn = new SqlConnection(ConnStr))
        using (var cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 256).Value =
                nomeUsuario != null ? (object)nomeUsuario : DBNull.Value;

            conn.Open();
            using (var rd = cmd.ExecuteReader())
                while (rd.Read()) perfis.Add(rd.GetString(0));
        }
        return perfis;
    }

    private int ExecNonQuery(string sql, params SqlParameter[] ps)
    {
        using (var conn = new SqlConnection(ConnStr))
        using (var cmd = new SqlCommand(sql, conn))
        {
            if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }

    private DataTable ExecDataTable(string sql, params SqlParameter[] ps)
    {
        using (var conn = new SqlConnection(ConnStr))
        using (var cmd = new SqlCommand(sql, conn))
        using (var da = new SqlDataAdapter(cmd))
        {
            if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    private bool PermitirEdicao { get { return (ViewState["PermitirEdicao"] is bool) ? (bool)ViewState["PermitirEdicao"] : false; } }
    private bool PermitirExclusao { get { return (ViewState["PermitirExclusao"] is bool) ? (bool)ViewState["PermitirExclusao"] : false; } }

    private static bool IsNullOrWhiteSpaceCompat(string s) { return s == null || s.Trim().Length == 0; }

    private void AplicarPermissoesNosControlesDeTela()
    {
        Button btnCadastrar = FindControl("btnCadastrar") as Button;
        if (btnCadastrar != null) btnCadastrar.Visible = PermitirEdicao;
    }

    // Cadastrar
    protected void btnCadastrar_Click(object sender, EventArgs e)
    {
        if (!PermitirEdicao) { MostrarMensagem("Você não tem permissão para cadastrar."); return; }

        string descricao = ((Request.Form["descricaoStatus"] ?? string.Empty)).Trim();
        string tentativa = ((Request.Form["tenta"] ?? string.Empty)).Trim();   // "S" / "N"
        string ativo = ((Request.Form["ativo"] ?? string.Empty)).Trim();   // "S" / "N"
        const string deletado = "N";

        if (IsNullOrWhiteSpaceCompat(descricao)) { MostrarMensagem("Informe a descrição do status."); return; }

        try
        {
            const string sql = @"
                INSERT INTO [hspmCall].[dbo].[status_consulta] (status, tenta, ativo, deletado)
                VALUES (@Descricao, @Tentativa, @Ativo, @Deletado);";

            ExecNonQuery(sql,
                new SqlParameter("@Descricao", SqlDbType.NVarChar, 200) { Value = descricao },
                new SqlParameter("@Tentativa", SqlDbType.NVarChar, 20) { Value = (object)(tentativa.Length > 0 ? tentativa : null) ?? DBNull.Value },
                new SqlParameter("@Ativo", SqlDbType.NVarChar, 1) { Value = (object)(ativo.Length > 0 ? ativo : null) ?? DBNull.Value },
                new SqlParameter("@Deletado", SqlDbType.NVarChar, 1) { Value = deletado }
            );

            CarregaStatus();
            MostrarMensagem("Registro cadastrado com sucesso!");
            ReexecutarJSDataTables();
        }
        catch (Exception ex) { MostrarMensagem("Erro ao cadastrar: " + ex.Message); }
    }

    // Listar (traz RAW e humanizado)
    private void CarregaStatus()
    {
        const string sql = @"
            SELECT 
                id_status   AS Id_status,
                status      AS Descricao,

                -- BRUTOS p/ modal
                tenta       AS TentativaRaw,
                ativo       AS AtivoRaw,

                -- Humanizados p/ exibição
                CASE 
                    WHEN tenta IS NULL THEN 'Não informado'
                    WHEN UPPER(tenta) = 'S' THEN 'Sim'
                    ELSE 'Não'
                END AS Tentativa,
                CASE 
                    WHEN ativo IS NULL THEN 'Não informado'
                    WHEN UPPER(ativo) = 'S' THEN 'Sim'
                    ELSE 'Não'
                END AS Ativo
            FROM [hspmCall].[dbo].[status_consulta]
            WHERE deletado = 'N'
            ORDER BY id_status;";

        DataTable dt = ExecDataTable(sql);
        gvStatusLigacao.DataSource = dt;
        gvStatusLigacao.DataBind();

        gvStatusLigacao.UseAccessibleHeader = true;
        if (gvStatusLigacao.HeaderRow != null)
            gvStatusLigacao.HeaderRow.TableSection = TableRowSection.TableHeader;
        if (gvStatusLigacao.FooterRow != null)
            gvStatusLigacao.FooterRow.TableSection = TableRowSection.TableFooter;
    }

    // Atualizar
    protected void BtnSalvarEdicao_Click(object sender, EventArgs e)
    {
        if (!PermitirEdicao) { MostrarMensagem("Você não tem permissão para editar."); return; }

        string idStr = Request.Form["hdn-edit-id"];
        string descricao = (Request.Form["hdn-edit-descricao"] ?? string.Empty).Trim();
        string tentativa = (Request.Form["hdn-edit-tentativa"] ?? string.Empty).Trim();
        string ativo = (Request.Form["hdn-edit-ativo"] ?? string.Empty).Trim();


        int id;
        if (!int.TryParse(idStr, out id) || id <= 0) { MostrarMensagem("ID inválido."); return; }
        if (IsNullOrWhiteSpaceCompat(descricao)) { MostrarMensagem("Informe a descrição do status."); return; }

        try
        {
            const string sql = @"
                UPDATE [hspmCall].[dbo].[status_consulta]
                SET status = @Descricao,
                    tenta  = @Tentativa,
                    ativo  = @Ativo
                WHERE id_status = @Id;";

            ExecNonQuery(sql,
                new SqlParameter("@Descricao", SqlDbType.NVarChar, 200) { Value = descricao },
                new SqlParameter("@Tentativa", SqlDbType.NVarChar, 20) { Value = (object)(tentativa.Length > 0 ? tentativa : null) ?? DBNull.Value },
                new SqlParameter("@Ativo", SqlDbType.NVarChar, 1) { Value = (object)(ativo.Length > 0 ? ativo : null) ?? DBNull.Value },
                new SqlParameter("@Id", SqlDbType.Int) { Value = id }
            );

            CarregaStatus();
            MostrarMensagem("Registro atualizado com sucesso!");
            ReexecutarJSDataTables();
        }
        catch (Exception ex) { MostrarMensagem("Erro ao atualizar: " + ex.Message); }
    }

    // Excluir (soft delete)
    protected void gvStatusLigacao_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Excluir") return;
        if (!PermitirExclusao) { MostrarMensagem("Você não tem permissão para excluir."); return; }

        int id;
        if (!int.TryParse(Convert.ToString(e.CommandArgument), out id) || id <= 0)
        {
            MostrarMensagem("ID inválido para exclusão."); return;
        }

        try
        {
            const string sql = "UPDATE [hspmCall].[dbo].[status_consulta] SET deletado = 'S' WHERE id_status = @Id;";
            ExecNonQuery(sql, new SqlParameter("@Id", SqlDbType.Int) { Value = id });

            CarregaStatus();
            MostrarMensagem("Registro excluído com sucesso!");
            ReexecutarJSDataTables();
        }
        catch (Exception ex) { MostrarMensagem("Erro ao excluir: " + ex.Message); }
    }

    // Permissões por linha + data-* em S/N
    protected void gvStatusLigacao_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        DataRowView drv = e.Row.DataItem as DataRowView;

        LinkButton btnExcluir = e.Row.FindControl("btnExcluir") as LinkButton;
        if (btnExcluir != null) btnExcluir.Visible = PermitirExclusao;

        LinkButton btnEditar = e.Row.FindControl("btnEditar") as LinkButton;
        if (btnEditar != null && drv != null)
        {
            btnEditar.Visible = PermitirEdicao;

            btnEditar.Attributes["data-id"] = Convert.ToString(drv["Id_status"]);
            btnEditar.Attributes["data-descricao"] = Convert.ToString(drv["Descricao"]);

            // pegue os brutos
            var tRaw = Convert.ToString(drv["TentativaRaw"]);
            var aRaw = Convert.ToString(drv["AtivoRaw"]);

            // normalize para S/N
            tRaw = string.Equals(tRaw, "S", StringComparison.OrdinalIgnoreCase) ? "S" : "N";
            aRaw = string.Equals(aRaw, "S", StringComparison.OrdinalIgnoreCase) ? "S" : "N";

            btnEditar.Attributes["data-tentativa"] = tRaw; // S|N
            btnEditar.Attributes["data-ativo"] = aRaw; // S|N

            btnEditar.ToolTip = "Editar";
            btnEditar.Attributes["data-toggle"] = "tooltip";
        }
    }

    // UI helpers
    private void MostrarMensagem(string texto)
    {
        lbMensagem.Text = texto ?? string.Empty;

        string safe = (texto ?? string.Empty)
            .Replace("\\", "\\\\").Replace("'", "\\'")
            .Replace("\r", "").Replace("\n", "\\n");

        string js = "document.getElementById('" + lbMensagem.ClientID + "').innerHTML='" + safe + "';" +
                    "$('#modalMsg').modal('show');";

        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", js, true);
    }

    private void ReexecutarJSDataTables()
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "ReInitDT",
            "if(window.initDataTables){ initDataTables(); }", true);
    }
}

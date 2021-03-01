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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Data.SqlClient;

public partial class encaminhamento_retornomarcado : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Pedido pedido = new Pedido();

            int _idPedido = Convert.ToInt32(Request.QueryString["idpedido"]);
            // string _status = Request.QueryString["status"].ToString();

            lbCodPedido.Text = _idPedido.ToString();

            pedido = PedidoDAO.getPedidoConsulta(_idPedido);
            txbNomePaciente.Text = pedido.nome_paciente;
            txbProntuario.Text = pedido.prontuario.ToString();

            if(pedido.status_pedido.Equals(0)){
                 lbStatus.Text = "Aguardando Vaga.";
            }
            

            txbdtPedido.Text = pedido.data_pedido.ToShortDateString(); ;
            txbdtCadastrado.Text = pedido.data_cadastro.ToString();
            txbEspecialidade.Text = pedido.descricao_espec;
            txbSolicitante.Text = pedido.solicitante;
            txbExamesSolicitados.Text = pedido.exames_solicitados;
            txbOutrasInformacoes.Text = pedido.outras_informacoes;

            btnGravar.Enabled = false;
        }
    }

    protected void btngetConsulta_Click(object sender, EventArgs e)
    {
        ClearInputs();
        int _nrConsulta = Convert.ToInt32(txbNrConsulta.Text);
        int _pronntuario = Convert.ToInt32(txbProntuario.Text);
        

        ConsultaSGH consulta = new ConsultaSGH();

        consulta = ConsultaSGHDAO.CarregaConsultaPaciente(_nrConsulta);


        if (_pronntuario.Equals(consulta.cd_prontuario) && consulta.nm_especialidade != txbEspecialidade.Text )
        {
            lbMensagemConsulta.Text = "Especialidade da consulta é diferente deste Pedido!";
            txbConMarcada.Text = consulta.cd_consulta.ToString();
            txbProntMarcada.Text = consulta.cd_prontuario.ToString();
            txbNPacienteMarcada.Text = consulta.nm_paciente;
            txbDtConsultaMarcada.Text = consulta.dt_consulta;
            txbEspecMarcada.Text = consulta.nm_especialidade;
            txbEquipeMarcada.Text = consulta.nm_equipe;
            txbProfissionalMarcada.Text = consulta.nm_profissional;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "validaCampo()", true);
            btnGravar.Enabled = true;
        }else if (_pronntuario.Equals(consulta.cd_prontuario))
        {
            txbConMarcada.Text = consulta.cd_consulta.ToString();
            txbProntMarcada.Text = consulta.cd_prontuario.ToString();
            txbNPacienteMarcada.Text = consulta.nm_paciente;
            txbDtConsultaMarcada.Text = consulta.dt_consulta;
            txbEspecMarcada.Text = consulta.nm_especialidade;
            txbEquipeMarcada.Text = consulta.nm_equipe;
            txbProfissionalMarcada.Text = consulta.nm_profissional;

            btnGravar.Enabled = true;
        }
        else if(consulta.cd_prontuario.Equals(0)){
            lbMensagemConsulta.Text = "Registro de consulta não encontrado!";
        }else
        {
            txbConMarcada.Text = consulta.cd_consulta.ToString();
            lbMensagemConsulta.Text = "Número do prontuário da consulta é diferente deste Pedido!";
            txbProntMarcada.Text = consulta.cd_prontuario.ToString();
            txbNPacienteMarcada.Text = consulta.nm_paciente;
            txbDtConsultaMarcada.Text = consulta.dt_consulta;
            txbEspecMarcada.Text = consulta.nm_especialidade;
            txbEquipeMarcada.Text = consulta.nm_equipe;
            txbProfissionalMarcada.Text = consulta.nm_profissional;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "validaCampo()", true);
        }
    }

    protected void ClearInputs()
    {
        lbMensagemConsulta.Text = "";
        txbConMarcada.Text = "";
        txbProntMarcada.Text = "";
        txbNPacienteMarcada.Text = "";
        txbDtConsultaMarcada.Text = "";
        txbEspecMarcada.Text = "";
        txbEquipeMarcada.Text = "";
        txbProfissionalMarcada.Text = "";
        btnGravar.Enabled = false;
    }

    protected void btnGrava_Click(object sender, EventArgs e)
    {
        string msg = "";
        ConsultaSGH consulta = new ConsultaSGH();

        consulta.cd_consulta = Convert.ToInt32(txbConMarcada.Text);
        consulta.cod_pedido = Convert.ToInt32(lbCodPedido.Text);
        consulta.dt_consulta = txbDtConsultaMarcada.Text;
        consulta.nm_especialidade = txbEspecMarcada.Text;
        consulta.nm_equipe = txbEquipeMarcada.Text;
        consulta.nm_profissional = txbProfissionalMarcada.Text;

        msg = gravaConsultaMarcada(consulta.cd_consulta, consulta.cod_pedido, consulta.dt_consulta, consulta.nm_especialidade, consulta.nm_equipe, consulta.nm_profissional, System.Web.HttpContext.Current.User.Identity.Name.ToUpper());

        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msg + "');", true);
        Response.Redirect("~/encaminhamento/pedidospendentesporrh.aspx");
    
    }

    public static void gravaLog(string descript_log, string origem, string usuario)
    {

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = new SqlCommand();
            cmm.Connection = cnn;
            cnn.Open();
            SqlTransaction mt = cnn.BeginTransaction();
            cmm.Transaction = mt;

            try
            {

                cmm.CommandText = "Insert into log (description_log, origem, usuario, dt_gravacao)" +
                       "values (@description_log, @origem, @usuario, @dt_gravacao)";

                cmm.Parameters.Add("@description_log", SqlDbType.VarChar).Value = descript_log;
                cmm.Parameters.Add("@origem", SqlDbType.VarChar).Value = origem;
                cmm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                cmm.Parameters.Add("@dt_gravacao", SqlDbType.DateTime).Value = DateTime.Now;
               
                
                cmm.ExecuteNonQuery();

                mt.Commit();
                mt.Dispose();
                cnn.Close();
               

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                try
                {
                    mt.Rollback();
                }
                catch (Exception ex1)
                { }
            }
        }
    }

    

    public string gravaConsultaMarcada(int _cod_consulta, int _cod_pedido, string _dt_consulta, string _especialidade, string _equipe, string _profissional, string usuario)
    {
        string msg = "";

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = new SqlCommand();
            cmm.Connection = cnn;
            cnn.Open();
            SqlTransaction mt = cnn.BeginTransaction();
            cmm.Transaction = mt;

            try
            {



                cmm.CommandText = "Insert into marcada (cod_consulta, cod_pedido, dt_consulta, especialidade, equipe, profissional)" +
                       "values (@cod_consulta, @cod_pedido, @dt_consulta, @especialidade, @equipe, @profissional)";

                cmm.Parameters.Add("@cod_consulta", SqlDbType.Int).Value = _cod_consulta;
                cmm.Parameters.Add("@cod_pedido", SqlDbType.Int).Value = _cod_pedido;
                cmm.Parameters.Add("@dt_consulta", SqlDbType.DateTime).Value = Convert.ToDateTime(_dt_consulta);
                cmm.Parameters.Add("@especialidade", SqlDbType.VarChar).Value = _especialidade;
                cmm.Parameters.Add("@equipe", SqlDbType.VarChar).Value = _equipe;
                cmm.Parameters.Add("@profissional", SqlDbType.VarChar).Value = _profissional;
                
                cmm.ExecuteNonQuery();

               // Atualiza tabela de pedido de consulta
                cmm.CommandText = "UPDATE pedido_consulta" +
                        " SET status = 1 " +
                        " WHERE  cod_pedido = @cod_ped";
                cmm.Parameters.Add(new SqlParameter("@cod_ped", _cod_pedido));
                cmm.ExecuteNonQuery();
                
                mt.Commit();
                mt.Dispose();
                cnn.Close();

                gravaLog("INSERT: CONSULTA " + _cod_consulta+ " CÓDIGO PEDIDO " +_cod_pedido, "TABELA MARCADA", usuario);
                msg = "Cadastro realizado com sucesso!";

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                msg = error;
                try
                {
                    mt.Rollback();
                }
                catch (Exception ex1)
                { }
            }
        }

        return msg;
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for PedidoDAO
/// </summary>
public class PedidoDAO
{
    public static string GravaPedidoConsulta(int _prontuario, string _nome_paciente, DateTime _data_pedido, int _cod_espec, string _exames, string _outras_info, string _solicitante, string _usuario)
    {
        string mensagem = "";
        string _dtcadastro = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = new SqlCommand();
            cmm.Connection = cnn;
            cnn.Open();
            SqlTransaction mt = cnn.BeginTransaction();
            cmm.Transaction = mt;
            
            try
            {

                cmm.CommandText = "Insert into pedido_consulta (prontuario, nome_paciente, data_pedido, data_cadastro, cod_especialidade, exames_solicitados, outras_informacoes, solicitante, status, usuario)" +
                       "values (@prontuario, @nome_paciente, @data_pedido, @data_cadastro, @cod_especialidade, @exames_solicitados, @outras_informacoes, @solicitante, @status, @usuario)";

                cmm.Parameters.Add("@prontuario", SqlDbType.Int).Value = _prontuario;
                cmm.Parameters.Add("@nome_paciente", SqlDbType.VarChar).Value = _nome_paciente;
                cmm.Parameters.Add("@data_pedido", SqlDbType.DateTime).Value = _data_pedido;
                cmm.Parameters.Add("@data_cadastro", SqlDbType.DateTime).Value = _dtcadastro;
                cmm.Parameters.Add("@cod_especialidade", SqlDbType.Int).Value = _cod_espec;
                cmm.Parameters.Add("@exames_solicitados", SqlDbType.VarChar).Value = _exames;
                cmm.Parameters.Add("@outras_informacoes", SqlDbType.VarChar).Value = _outras_info;
                cmm.Parameters.Add("@solicitante", SqlDbType.VarChar).Value = _solicitante;
                cmm.Parameters.Add("@status", SqlDbType.Int).Value = 0;
                cmm.Parameters.Add("@usuario", SqlDbType.VarChar).Value = _usuario;
                
                cmm.ExecuteNonQuery();

                mt.Commit();
                mt.Dispose();
                cnn.Close();
                mensagem = "Cadastro realizado com sucesso!";

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                mensagem = error;
                try
                {
                    mt.Rollback();
                }
                catch (Exception ex1)
                { }
            }
        }
        return mensagem;
    }

    public static List<Pedido> getListaPedidoConsultaPendentePorRH(int _prontuario)
    {
        var listaPedidos = new List<Pedido>();
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();


            string sqlConsulta = "SELECT [cod_pedido]" +
                              ",[prontuario]" +
                              ",[nome_paciente]" +
                              ",[data_pedido]" +
                              ",[data_cadastro]" +
                              ",[cod_especialidade]" +
                              ",[exames_solicitados]" +
                              ",[outras_informacoes]" +
                              ",[solicitante]" +
                              ",[usuario]" +
                              " FROM [pedido_consulta] " +
                              " WHERE  [status] = 0" +
                              " AND [prontuario] = "+ _prontuario +
                              " ORDER BY data_pedido DESC";

            cmm.CommandText = sqlConsulta;

            try
            {
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();

                //char[] ponto = { '.', ' ' };
                while (dr1.Read())
                {
                    Especialidade espec = new Especialidade();
                    Pedido p = new Pedido();
                    p.cod_pedido = dr1.GetInt32(0);
                    p.prontuario = dr1.GetInt32(1);
                    p.nome_paciente = dr1.GetString(2);
                    p.data_pedido = dr1.GetDateTime(3);
                    p.data_cadastro = dr1.GetDateTime(4);
                    p.cod_especialidade = dr1.GetInt32(5);
                    p.descricao_espec = EspecialidadeDAO.getEspecialidade(p.cod_especialidade);
                    p.exames_solicitados = dr1.GetString(6);
                    p.outras_informacoes = dr1.GetString(7);
                    p.solicitante = dr1.GetString(8);
                    p.usuario = dr1.GetString(9);

                    listaPedidos.Add(p);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


            return listaPedidos;
        }
    }

    public static List<Pedido> getListaPedidoConsultaPendente()
    {
        var listaPedidos = new List<Pedido>();
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();


            string sqlConsulta = "SELECT [cod_pedido]" +
                              ",[prontuario]" +
                              ",[nome_paciente]" +
                              ",[data_pedido]" +
                              ",[data_cadastro]" +
                              ",[cod_especialidade]" +
                              ",[exames_solicitados]" +
                              ",[outras_informacoes]" +
                              ",[solicitante]" +
                              ",[usuario]" +
                              " FROM [pedido_consulta] " +
                              " WHERE  [status] = 0" +
                              " ORDER BY data_pedido DESC";

            cmm.CommandText = sqlConsulta;

            try
            {
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();

                //char[] ponto = { '.', ' ' };
                while (dr1.Read())
                {
                    Especialidade espec = new Especialidade();
                    Pedido p = new Pedido();
                    p.cod_pedido = dr1.GetInt32(0);
                    p.prontuario = dr1.GetInt32(1);
                    p.nome_paciente = dr1.GetString(2);
                    p.data_pedido = dr1.GetDateTime(3);
                    p.data_cadastro = dr1.GetDateTime(4);
                    p.cod_especialidade = dr1.GetInt32(5);
                    p.descricao_espec = EspecialidadeDAO.getEspecialidade(p.cod_especialidade);
                    p.exames_solicitados = dr1.GetString(6);
                    p.outras_informacoes = dr1.GetString(7);
                    p.solicitante = dr1.GetString(8);
                    p.usuario = dr1.GetString(9);

                    listaPedidos.Add(p);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }


            return listaPedidos;
        }
    }

    public static Pedido getPedidoConsulta(int _idPedido)
    {
        Especialidade espec = new Especialidade();
        Pedido pedido = new Pedido();
        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();


            string sqlConsulta = "SELECT [cod_pedido]" +
                              ",[prontuario]" +
                              ",[nome_paciente]" +
                              ",[data_pedido]" +
                              ",[data_cadastro]" +
                              ",[cod_especialidade]" +
                              ",[exames_solicitados]" +
                              ",[outras_informacoes]" +
                              ",[solicitante]" +
                              ",[usuario]" +
                              " FROM [pedido_consulta] " +
                              " WHERE  cod_pedido = " + _idPedido;

            cmm.CommandText = sqlConsulta;

            try
            {
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();
                if (dr1.Read())
                {
                    pedido.cod_pedido = dr1.GetInt32(0);
                    pedido.prontuario = dr1.GetInt32(1);
                    pedido.nome_paciente = dr1.GetString(2);
                    pedido.data_pedido = dr1.GetDateTime(3);
                    pedido.data_cadastro = dr1.GetDateTime(4);
                    pedido.cod_especialidade = dr1.GetInt32(5);
                    pedido.descricao_espec = EspecialidadeDAO.getEspecialidade(pedido.cod_especialidade);
                    pedido.exames_solicitados = dr1.GetString(6);
                    pedido.outras_informacoes = dr1.GetString(7);
                    pedido.solicitante = dr1.GetString(8);
                    pedido.usuario = dr1.GetString(9);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return pedido;
        }

    }
}

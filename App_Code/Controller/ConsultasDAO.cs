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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Summary description for ConsultasDAO
/// </summary>
public class ConsultasDAO
{
	public ConsultasDAO()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private static string InformacaoConCancelada(int _id_consulta)
    {
        string informacao = "";

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT i.[descricao_con_cancelada]" +
                              " FROM [hspmCall].[dbo].[consultas_cancelar] c ,[hspmCall].[dbo].[info_con_cancelada] i " +
                              " WHERE c.id_cancela = i.id_consultas_cancelar " +
                              " AND c.id_consulta = " + _id_consulta;

            try
            {
                cnn.Open();

                SqlDataReader dr1 = cmm.ExecuteReader();

                if (dr1.Read())
                {
                    informacao = dr1.GetString(0);
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }
        return informacao;
    }

    public static List<Ativo_Ligacao> ListaConsultasPaciente(int _prontuario)
    {
        var lista = new List<Ativo_Ligacao>();

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
           

            string sqlConsulta = "SELECT  [id_consulta]" +
                              ",[equipe]" +
                              ",[dt_consulta]" +
                              ",[codigo_consulta]" +
                              ",[grade]" +
                              ",[equipe]" +
                              ",[profissional]" +
                              ",[ativo]" +
                              " FROM [consulta] " +
                              " WHERE  [prontuario] = " + _prontuario +
                              " AND ativo = 0" +
                              " ORDER BY dt_consulta DESC";

            cmm.CommandText = sqlConsulta;
            
            try
            {  
                cnn.Open();
                SqlDataReader dr1 = cmm.ExecuteReader();

                //char[] ponto = { '.', ' ' };
                while (dr1.Read())
                {
                    ConsultasRemarcar consulta = new ConsultasRemarcar();
                    consulta.Id_Ativo = 0;
                    consulta.Id_Consulta = dr1.GetInt32(0);
                    consulta.Equipe = dr1.GetString(1);
                    consulta.Dt_Consulta = dr1.GetDateTime(2);
                    consulta.Codigo_Consulta = dr1.GetInt32(3);
                    consulta.Grade = dr1.GetInt32(4);
                    consulta.Equipe = dr1.GetString(5);
                    consulta.Nome_Profissional = dr1.GetString(6);
                    consulta.Ativo_Status = dr1.GetBoolean(7);
                    lista.Add(consulta);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        using (SqlConnection cnn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm1 = cnn1.CreateCommand();
            string sqlAtivo_consulta = "SELECT [idativo], [id_consulta]" +
                                        "  ,[equipe]" +
                                        "  ,[dt_consulta]" +
                                        "  ,[codigo_consulta]" +
                                        "  ,[grade]" +
                                        "  ,[equipe]" +
                                        "  ,[profissional]" +
                                        "  ,[status]" +
                                        "  ,[observacao]" +
                                        "  ,[data_ligacao]" +
                                        "  ,[usuario]" +
                                        " FROM [hspmCall].[dbo].[vw_relatorio_ativos]" +
                                        " WHERE  [prontuario] = " + _prontuario + 
                                        " ORDER BY data_ligacao desc";
            cmm1.CommandText = sqlAtivo_consulta;

            try
            {
                cnn1.Open();
                SqlDataReader dr2 = cmm1.ExecuteReader();
                
                while (dr2.Read())
                {
                    ConsultasRemarcar consulta1 = new ConsultasRemarcar();
                    consulta1.Id_Ativo = dr2.GetInt32(0);
                    consulta1.Id_Consulta = dr2.GetInt32(1);
                    consulta1.Equipe = dr2.GetString(2);
                    consulta1.Dt_Consulta = dr2.GetDateTime(3);
                    consulta1.Codigo_Consulta = dr2.GetInt32(4);
                    consulta1.Grade = dr2.GetInt32(5);
                    consulta1.Equipe = dr2.GetString(6);
                    consulta1.Nome_Profissional = dr2.GetString(7);
                    consulta1.Status = dr2.GetString(8);
                    consulta1.Observacao = dr2.GetString(9);
                    consulta1.Data_Contato = dr2.GetDateTime(10);
                    consulta1.Usuario_Contato = dr2.GetString(11);
                    consulta1.DescricaoRemarcar = InformacaoConCancelada(dr2.GetInt32(1));
                    lista.Add(consulta1);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        return lista;
    }

    //public static List<ConsultasRemarcar> ListaConsultasCancelar(int _status)
    //{
    //    // colocar regra para listar consultas do paciente com tentativas abaixo de 3 (terceira tentativa)

    //    var lista = new List<ConsultasRemarcar>();

    //    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
    //    {
    //        SqlCommand cmm = cnn.CreateCommand();

    //        cmm.CommandText = "SELECT [id_cancela] " +
    //                ",[id_consulta]" +
    //                ",[prontuario]" +
    //                ",[nome_paciente]" +
    //                ",[equipe]" +
    //                ",[dt_consulta]" +
    //                ",[codigo_consulta]" +
    //                ",[desc_status]" +
    //                ",[observacao]" +
    //                ",[data_ligacao]" +
    //                ",[stat_cancelar]" +
    //                ",[status]" +
    //                ",[usuario]" +
    //                ",[grade]" +
    //                " FROM [vw_cancelar_consultas_2] " +
    //                " WHERE stat_cancelar = 1 " +
    //                " AND status = " + _status +
    //                " AND dt_consulta >= '2025-19-03'";

    //        try
    //        {
    //            cnn.Open();
    //            SqlDataReader dr1 = cmm.ExecuteReader();

    //            //char[] ponto = { '.', ' ' };
    //            while (dr1.Read())
    //            {
    //                ConsultasRemarcar consulta = new ConsultasRemarcar();
    //                consulta.id_cancela = dr1.GetInt32(0);
    //                consulta.Id_Consulta = dr1.GetInt32(1);
    //                consulta.Prontuario = dr1.GetInt32(2);
    //                consulta.Nome = dr1.GetString(3);
    //                consulta.Equipe = dr1.GetString(4);
    //                consulta.Dt_Consulta = dr1.GetDateTime(5);
    //                consulta.Codigo_Consulta = dr1.GetInt32(6);
    //                consulta.Status = dr1.GetString(7);
    //                consulta.Observacao = dr1.GetString(8);
    //                consulta.Data_Contato = dr1.GetDateTime(9);
    //                consulta.Usuario_Contato = dr1.GetString(12);
    //                consulta.Grade = dr1.GetInt32(13);

    //                lista.Add(consulta);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string error = ex.Message;
    //        }
    //    }
    //    return lista;
    //}
    public static List<ConsultasRemarcar> ListaConsultasCancelar(
      int status,
      int pageNumber,
      int pageSize,
      out int totalRows)
    {
        const string CONN = "gtaConnectionString";
        DateTime dataMin = new DateTime(2025, 3, 19);

        var lista = new List<ConsultasRemarcar>();
        totalRows = 0;

        using (SqlConnection cnn = new SqlConnection(
               ConfigurationManager.ConnectionStrings[CONN].ToString()))
        {
            cnn.Open();

            // ---------- 1) Contagem total ----------
            string countSql = @"
            SELECT COUNT(1)
            FROM vw_cancelar_consultas_2
            WHERE stat_cancelar = 1 AND id_status = @status";

            using (SqlCommand countCmd = new SqlCommand(countSql, cnn))
            {
                countCmd.Parameters.AddWithValue("@status", status);
                totalRows = (int)countCmd.ExecuteScalar();
            }

            // ---------- 2) Paginação com ROW_NUMBER() ----------
            int startRow = ((pageNumber -1) * pageSize) + 1;
            int endRow = Math.Min(pageNumber * pageSize, totalRows);


            string pageSql = @"
            WITH ConsultaPaginada AS (
                SELECT
                    ROW_NUMBER() OVER (ORDER BY dt_consulta DESC) AS RowNum,
                    id_cancela, id_consulta, prontuario, nome_paciente,
                    equipe, dt_consulta, codigo_consulta, desc_status,
                    observacao, data_ligacao, stat_cancelar, id_status,
                    usuario, grade
                FROM vw_cancelar_consultas_2
                WHERE stat_cancelar = 1 AND id_status = @status
            )
            SELECT *
            FROM ConsultaPaginada
            WHERE RowNum BETWEEN @startRow AND @endRow
            ORDER BY RowNum";

            using (SqlCommand cmd = new SqlCommand(pageSql, cnn))
            {
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@startRow", startRow);
                cmd.Parameters.AddWithValue("@endRow", endRow);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var c = new ConsultasRemarcar();
                        c.id_cancela = dr.GetInt32(dr.GetOrdinal("id_cancela"));
                        c.Id_Consulta = dr.GetInt32(dr.GetOrdinal("id_consulta"));
                        c.Prontuario = dr.GetInt32(dr.GetOrdinal("prontuario"));
                        c.Nome = dr.GetString(dr.GetOrdinal("nome_paciente"));
                        c.Equipe = dr.GetString(dr.GetOrdinal("equipe"));
                        c.Dt_Consulta = dr.GetDateTime(dr.GetOrdinal("dt_consulta"));
                        c.Codigo_Consulta = dr.GetInt32(dr.GetOrdinal("codigo_consulta"));
                        c.Status = dr.GetString(dr.GetOrdinal("desc_status"));
                        c.Observacao = dr.GetString(dr.GetOrdinal("observacao"));
                        c.Data_Contato = dr.GetDateTime(dr.GetOrdinal("data_ligacao"));
                        c.Usuario_Contato = dr.GetString(dr.GetOrdinal("usuario"));
                        c.Grade = dr.GetInt32(dr.GetOrdinal("grade"));

                        lista.Add(c);
                    }
                }
            }
        }

        return lista;
    }

    public static int  ContarConsultasCancelar(int status)
    {
        int totalRows = 0;

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT COUNT(1) FROM vw_cancelar_consultas_2 WHERE stat_cancelar = 1 AND id_status = " + status;

            try
            {
                cnn.Open();
                totalRows = (int)cmm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        return totalRows;
    }

    public static ConsultasRemarcar getDadosConsulta(int _idConsulta, string _status)
    {
        //Ativo_Ligacao consulta = new Ativo_Ligacao();
        ConsultasRemarcar consulta = new ConsultasRemarcar();

        using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["gtaConnectionString"].ToString()))
        {
            SqlCommand cmm = cnn.CreateCommand();
            cmm.CommandText = "SELECT [prontuario] " +
                                          ",[nome_paciente] " +
                                          ",[telefone1] " +
                                          ",[telefone2] " +
                                          ",[telefone3] " +
                                          ",[telefone4] " +
                                          ",[equipe] " +
                                          ",[profissional] " +
                                          ",[dt_consulta] " +
                                          ",[codigo_consulta] " +
                                          ",[desc_status] " +
                                          ",[observacao] " +
                                          ",[data_ligacao] " +
                                          ",[id_cancela] " +
                                          ",[usuario] " +
                                          "FROM vw_cancelar_consultas_2 " +
                                          " WHERE id_consulta = " + _idConsulta +
                                          " AND desc_status = '" + _status + "'";

            cnn.Open();
            SqlDataReader dr = cmm.ExecuteReader();
            if (dr.Read())
            {
                consulta.Prontuario = dr.GetInt32(0);
                consulta.Nome = dr.GetString(1);
                consulta.Telefone1 = dr.GetString(2);
                consulta.Telefone2 = dr.GetString(3);
                consulta.Telefone3 = dr.GetString(4);
                consulta.Telefone4 = dr.GetString(5);
                consulta.Equipe = dr.GetString(6);
                consulta.Nome_Profissional = dr.GetString(7);
                consulta.Dt_Consulta = dr.GetDateTime(8);
                consulta.Codigo_Consulta = dr.GetInt32(9);
                consulta.Status = dr.GetString(10);
                consulta.Observacao = dr.GetString(11) + " Funcionário que fez contato: " + dr.GetString(14);
                consulta.Data_Contato = dr.GetDateTime(12);
                consulta.id_cancela = dr.GetInt32(13);
            }
        }
        return consulta;
    }
}
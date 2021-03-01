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

/// <summary>
/// Summary description for Paciente
/// </summary>
public class Paciente
{
    public int Cd_prontuario { get; set; }
    public string Nm_situacao { get; set; }
    public string Nm_nome { get; set; }
    public string Nm_nome_social { get; set; }
    public string Nm_vinculo { get; set; }
    public string Nm_orgao { get; set; }
    public string Cd_rf_matricula { get; set; }
    public string In_sexo { get; set; }
    public string Dc_cor { get; set; }
    public string Dc_estado_civil { get; set; }
    public string Cd_mae { get; set; }
    public string Nm_mae { get; set; }
    public string Nm_pai { get; set; }
    public string Dt_data_nascimento { get; set; }
    public string Nr_idade { get; set; }
    public string Nm_nacionalidade { get; set; }
    public string Nm_naturalidade { get; set; }
    public string Sg_uf { get; set; }
    public string Dc_grau_instrucao { get; set; }
    public string Dc_ocupacao { get; set; }
    public string Nr_ddd_fone { get; set; }
    public string Nr_fone { get; set; }
    public string Nr_ddd_fone_recado { get; set; }
    public string Nr_fone_recado { get; set; }
    public string Cd_cep { get; set; }
    public string Dc_logradouro { get; set; }
    public string Nr_logradouro { get; set; }
    public string Dc_complemento_logradouro { get; set; }
    public string Dc_bairro { get; set; }
    public string Cd_ibge_cidade{ get; set; }
    public string Sg_uf_endereco { get; set; }
    public string Tp_endereco { get; set; }
    public string In_correspondencia { get; set; }
    public string Nr_rg { get; set; }
    public string Dc_orgao_emissor { get; set; }
    public string Sg_uf_sigla_emitiu_docto { get; set; }
    public string Dt_emissao_documento { get; set; }
    public string Nr_cpf { get; set; }
    public string Nr_pis { get; set; }
    public string In_documentos_apresentados { get; set; }
    public string Nm_certidao { get; set; }
    public string Nm_cartorio { get; set; }
    public string Nr_livro { get; set; }
    public string Nr_folhas { get; set; }
    public string Nr_termo { get; set; }
    public string Dt_emissao { get; set; }
    public string Nr_declaracao_nascido { get; set; }
    public string Nr_cartao_saude { get; set; }
    public string Nm_motivo_cadastro { get; set; }
    public string Dc_documento_referencia { get; set; }
    public string Nr_cartao_nacional_saude_mae { get; set; }
    public string Dt_entrada_br { get; set; }
    public string dt_naturalizacao { get; set; }
    public string Nr_portaria { get; set; }
    public string Dc_observacao { get; set; }
}

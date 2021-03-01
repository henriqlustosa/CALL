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
/// Summary description for Pedido
/// </summary>
public class Pedido: Especialidade
{
    public int cod_pedido { get; set; }
    public int prontuario { get; set; }
    public string nome_paciente { get; set; }
    public DateTime data_pedido { get; set; }
    public DateTime data_cadastro { get; set; }
    public string exames_solicitados { get; set; }
    public string outras_informacoes { get; set; }
    public string solicitante { get; set; }
    public int status_pedido { get; set; }
    public string usuario { get; set; }
}

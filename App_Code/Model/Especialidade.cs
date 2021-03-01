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
/// Summary description for Especialidade
/// </summary>
public class Especialidade
{
    public int cod_especialidade { get; set; }
    public string descricao_espec { get; set; }
    public string status_espec { get; set; }
}

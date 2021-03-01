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
/// Summary description for ConsultaSGH
/// </summary>
public class ConsultaSGH
{
    public int cod_pedido { get; set; }
    public int cd_consulta { get; set; }
    public int cd_prontuario { get; set; }
    public string nm_paciente { get; set; }
    public string sg_sexo { get; set; }
    public string dt_consulta { get; set; }
    public string nm_especialidade { get; set; }
    public string nm_equipe { get; set; }
    public string nm_profissional { get; set; }
}

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
/// Summary description for Ativo
/// </summary>
public class Ativo : PacienteMailling
{
    public int Id_Consulta { get; set; }
    public int Prontuario { get; set; }
    public int Grade { get; set; }
    public DateTime Dt_Consulta { get; set; }
    public string Equipe { get; set; }
    public string Nome_Profissional { get; set; }
    public int Codigo_Consulta { get; set; }
    public string Dt_Carga { get; set; }
    public bool Ativo_Status { get; set; }
}
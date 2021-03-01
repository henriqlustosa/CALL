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
using System.Net;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Summary description for ConsultaSGHDAO
/// </summary>
public class ConsultaSGHDAO
{
    public static ConsultaSGH CarregaConsultaPaciente(int _nrconsulta)
    {
        ConsultaSGH consulta = new ConsultaSGH();

        try
        {
            string URI = "http://10.48.21.64:5000/hspmsgh-api/pacientes/paciente/filipeta/" + _nrconsulta;
            WebRequest request = WebRequest.Create(URI);

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URI);
            // Sends the HttpWebRequest and waits for a response.
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var reader = new StreamReader(httpResponse.GetResponseStream());

                JsonSerializer json = new JsonSerializer();

                var objText = reader.ReadToEnd();

                consulta = JsonConvert.DeserializeObject<ConsultaSGH>(objText);
            }
        }
        catch (WebException ex)
        {
            string err = ex.Message;
        }
        catch (Exception ex1)
        {
            string err1 = ex1.Message;
        }
        return consulta;
    }
}

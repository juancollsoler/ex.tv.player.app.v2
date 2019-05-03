using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TvControl;
using TvDatabase;
using TvService;

namespace ConsoleApp1
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        //VirtualCard _card;
        //var _server = new TvServer();
        //IUser user = UserFactory.CreateBasicUser("setuptv");
        //TvResult result = _server.StartTimeShifting(ref user, 55, out _card);
        //if (result != TvResult.Succeeded)
        //{
        //  throw new Exception(result.ToString());
        //}

        try
        {
          NameValueCollection appSettings = ConfigurationManager.AppSettings;
          appSettings.Set("GentleConfigFile", @"C:\ProgramData\Team MediaPortal\MediaPortal TV Server\gentle.config");

          XmlDocument doc = new XmlDocument();
          doc.Load(String.Format(@"C:\ProgramData\Team MediaPortal\MediaPortal TV Server\gentle.config"));
          XmlNode nodeKey = doc.SelectSingleNode("/Gentle.Framework/DefaultProvider");
          XmlNode node = nodeKey.Attributes.GetNamedItem("connectionString");
          XmlNode nodeProvider = nodeKey.Attributes.GetNamedItem("name");

          Gentle.Framework.ProviderFactory.ResetGentle(true);
          Gentle.Framework.GentleSettings.DefaultProviderName = nodeProvider.InnerText;
          Gentle.Framework.ProviderFactory.GetDefaultProvider();
          Gentle.Framework.ProviderFactory.SetDefaultProviderConnectionString(node.InnerText);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }

        try
        {
          Server.ListAll();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }

        var _ourServer = Server.Retrieve(RemoteControl.Instance.IdServer);
        string ourServerName = _ourServer.HostName;
        try
        {
          ourServerName = Dns.GetHostEntry(_ourServer.HostName).HostName;
        }
        catch (Exception ex)
        {
          Console.WriteLine("Failed to get our server host name");
          Console.WriteLine(ex.Message);
        }
        List<string> ipAdresses = RemoteControl.Instance.ServerIpAdresses;


        foreach (string ipAdress in ipAdresses)
          Console.WriteLine(ipAdress);

        Console.WriteLine("RTSP PORT = {0}", _ourServer.RtspPort.ToString());

      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      Console.ReadLine();
    }
  }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Hedaozi.AppsManager
{
    internal class AppsManifest: Dictionary<string, List<AppInfo>>
    {
        private readonly static Uri manifestUri = 
            new Uri("https://hedaozi.com/resource/apps/AppsManifest.yaml");
        private readonly static IDeserializer deserializer = 
            new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        static AppsManifest() => ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        public static AppsManifest LoadManifest(out bool connectSuccess)
        {
            Stream stream;
            try
            {
                stream = WebRequest.Create(manifestUri).GetResponse().GetResponseStream();
            }
            catch
            {
                connectSuccess = false;
                return null;
            }
            connectSuccess = true;
            var reader = new StreamReader(stream);
            return deserializer.Deserialize<AppsManifest>(reader);
        }
    }

    internal class AppInfo
    {
        public string Name { get; set; }
        public string IconHex { get; set; }
        public string DescriptionZH { get; set; }
        public string DescriptionEN { get; set; }
        public string WindowsUrl { get; set; }
        public string MacUrl { get; set; }
        public string LinuxUrl { get; set; }
        public string ReposUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Seayo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(SetHost)
                .UseStartup<Startup>()
                .Build();

        private static void SetHost(Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions options)
        {
            var configuration = (IConfiguration)options.ApplicationServices.GetService(typeof(IConfiguration));
            var host = configuration.GetSection("RafHost").Get<Host>();
            foreach (var endpointKvp in host.Endpoints)
            {
                var endpointName = endpointKvp.Key;
                var endpoint = endpointKvp.Value;
                if (!endpoint.IsEnabled)
                {
                    continue;
                }

                var address = IPAddress.Parse(endpoint.Address);
                options.Listen(address, endpoint.Port, opt =>
                {
                    if (endpoint.Certificate != null)
                    {
                        switch (endpoint.Certificate.Source)
                        {
                            case "File":
                                opt.UseHttps(endpoint.Certificate.Path, endpoint.Certificate.Password);
                                break;
                            default:
                                throw new NotImplementedException($"The source {endpoint.Certificate.Source} is not yet implemented");
                        }
                    }
                });

                options.UseSystemd();
            }
        }

        public class Host
        {
            public Dictionary<string, Endpoint> Endpoints { get; set; }
        }

        public class Endpoint
        {
            public bool IsEnabled { get; set; }
            public string Address { get; set; }
            public int Port { get; set; }
            public Certificate Certificate { get; set; }
        }

        public class Certificate
        {
            public string Source { get; set; }
            public string Path { get; set; }
            public string Password { get; set; }
        }
    }
}

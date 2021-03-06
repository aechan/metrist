using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.Conventions;
using Nancy.Owin;

public class Server 
{
    public static void Main(string[] args)
    {
        int port = 8080;
        if(args.Length > 0) {
            int.TryParse(args[0], out port);  
            if(port == 0) {
                port = 8080;
            }
        }

        var host = new WebHostBuilder()
        .UseContentRoot(Path.Combine(Directory.GetCurrentDirectory(), "public"))
        .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "public"))
        .UseKestrel()
        .UseStartup<Startup>()
        .UseUrls("http://0.0.0.0:"+port)
        .Build();

        host.Run();
    }

    
}

public class Startup
{
    private readonly IConfiguration config;

    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath);
        config = builder.Build();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new MBootStrapper()));
    }
}

public class MBootStrapper : DefaultNancyBootstrapper
{
    public override void Configure(Nancy.Configuration.INancyEnvironment environment)
    {
        environment.Tracing(
                enabled: true,
                displayErrorTraces: true);
    }

    protected override void ConfigureConventions(NancyConventions conventions)
    {
        base.ConfigureConventions(conventions);
        Console.WriteLine("Configuring..");
        conventions.StaticContentsConventions.Add(
            StaticContentConventionBuilder.AddDirectory("/", "public")
        );

    }
}

public class RootPathProvider : IRootPathProvider
{
  public string GetRootPath()
  {
    return Directory.GetCurrentDirectory();
  }
}

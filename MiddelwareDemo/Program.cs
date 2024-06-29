using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MiddelwareDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = new ApplicationBuilder();
            //builder.Use(next =>
            //{
            //    return water =>
            //    {
            //        Console.WriteLine("开始去污");
            //        next(water);
            //        Console.WriteLine("完成去污");
            //    };
            //});
            //builder.Use(next =>
            //{
            //    return water =>//next存储到这个方法体里面了，这个执行完还是返回的一个委托
            //    {
            //        Console.WriteLine("开始消毒");
            //        next(water);
            //        Console.WriteLine("完成消毒");
            //    };
            //});
            //builder.Use(next =>
            //{
            //    return water =>
            //    {
            //        Console.WriteLine("开始消毒x");
            //        water.Invoke();
            //        Console.WriteLine("完成消毒x");
            //    };
            //});
            //var app = builder.Build();
            //var target = new Water();
            //app.Invoke(target);
            
            //var host = new WebHost();
            //host.AddFilter(new Filter1());
            //host.AddFilter(new Filter2());
            //var context = new HttpContext();
            //host.Execute(context, new HelloServlet());

            //IContainerBuilder containerBuilder = new ContainerBuilder();
            //containerBuilder.Add(c => new DbConnection());
            //containerBuilder.Add<DbContext>();
            //var container = containerBuilder.Build();
            //var context = container.GetService(typeof(DbContext));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

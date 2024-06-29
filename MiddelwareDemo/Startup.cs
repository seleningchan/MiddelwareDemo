using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiddelwareDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) => {
                var query = context.Request.QueryString.Value;
                // ����һ���м��ǰִ�е��߼�
                await context.Response.WriteAsync("Middleware 1 - Before\n");
                // ����������һ���м��
                await next();
                // ����һ���м����ִ�е��߼�
                await context.Response.WriteAsync("Middleware 1 - After\n");
            });

            app.Use(async (context, next) => {
                // ����һ���м��ǰִ�е��߼�
                await context.Response.WriteAsync("Middleware 2 - Before\n");
                // ����������һ���м��
                await next();
                // ����һ���м����ִ�е��߼�
                await context.Response.WriteAsync("Middleware 2 - After\n");
            });

            app.Run(async (context) => {
                await context.Response.WriteAsync("Hello, World!\n");
            });
        }
    }
}

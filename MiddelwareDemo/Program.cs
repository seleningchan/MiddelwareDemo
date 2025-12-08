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
     class Test
    {
        public string Name { get; set; }
    }
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Person))
            {
                return false;
            }
            var p = (Person)obj;
            Console.WriteLine(string.Format("Equals{0}", p.Name));
            return this.Name == p.Name && this.Age == p.Age;
        }

        public override int GetHashCode()
        {
            Console.WriteLine(string.Format("GetHashCode{0}", this.Name));
            return Name.GetHashCode() + Age * 37;
        }
    }
    public class Program 
    {
        public static void Main(string[] args)
        {
            var dic = new Dictionary<Person, int>();
            dic.Add(new Person { Name = "ABC", Age = 18 }, 1);
            dic.Add(new Person { Name = "Captain", Age = 24 }, 1);
            dic.Add(new Person { Name = "Knee", Age = 28 }, 1);
            dic.Add(new Person { Name = "Knee", Age = 28 }, 2);

            foreach (var item in dic)
            {
                Console.WriteLine(string.Format("{0},{1}", item.Key.Name, item.Key.Age));
            }


            //Dictionary<Test, string> dic= new Dictionary<Test, string>();
            //var testx = new Test();
            //dic.Add(new Test(), "aaa");
            //dic.Add(testx, "aaa");
            const string hamlet = @"Though yet of Hamlet our dear brother's death
The memory be green, and that it us befitted";
            var temp = hamlet.Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var test = 1;



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

            var host = new WebHost();
            host.AddFilter(new Filter1());
            host.AddFilter(new Filter2());
            var context = new HttpContext();
            host.Execute(context, new HelloServlet());

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

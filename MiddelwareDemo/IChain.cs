using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddelwareDemo
{
    public class HttpContext { }

    public interface IChain
    {
        Task  NextAsync();
    }
    public interface IFilter
    {
        Task  InvokeAsync(HttpContext httpContext, IChain chain);
    }
    public interface IServlet
    {
        Task DoPostAsync(HttpContext context);
    }

    public class FilterChain : IChain
    {
        private readonly IFilter _filter;
        private readonly HttpContext _context;
        private readonly IChain _next;
        public FilterChain(IFilter filter, IChain next, HttpContext httpContext)
        {
            _filter = filter;
            _context = httpContext;
            _next = next;
        }
        public async Task NextAsync()
        {
            await _filter.InvokeAsync(_context,_next);
        }
    }
    public class ServletChain : IChain
    {
        private readonly IServlet _servlet;
        private readonly HttpContext _context;
        public ServletChain(IServlet servlet, HttpContext httpContext)
        {
            _servlet = servlet;
            _context = httpContext;
        }
        public async Task NextAsync()
        {
            await _servlet.DoPostAsync(_context);
        }
    }
    public class Filter1 : IFilter
    {
        public async Task InvokeAsync(HttpContext httpContext, IChain chain)
        {
            Console.WriteLine("开始身份认证");
            await chain.NextAsync();
            Console.WriteLine("结束身份认证");
        }
    }
    public class Filter2 : IFilter
    {
        public async Task InvokeAsync(HttpContext httpContext, IChain chain)
        {
            Console.WriteLine("开始授权");
            await chain.NextAsync();
            Console.WriteLine("结束授权");
        }
    }
    public class HelloServlet : IServlet
    {
        public Task DoPostAsync(HttpContext context)
        {
            Console.WriteLine("Hello Servlet");
            //await Task.Delay(100);
            return  Task.CompletedTask;
        }
    }

    public class WebHost
    {
        private readonly List<IFilter> _filters = new List<IFilter>();

        public void AddFilter(IFilter filter)
        {
            _filters.Add(filter);
        }

        public void Execute(HttpContext context, IServlet servlet)
        {
            //自行处理filter为空的情况,就是直接执行serlvet就好了
            var stack = new Stack<IFilter>(_filters);
            var filter = stack.Pop();
            var chain = GetFilterChain(context, servlet, stack);
            filter.InvokeAsync(context, chain);
        }
        //构建链路器（递归算法）
        private IChain GetFilterChain(HttpContext context, IServlet servlet, Stack<IFilter> filters)
        {
            if (filters.Any())
            {
                var filter = filters.Pop();
                var next = GetFilterChain(context, servlet, filters);
                return new FilterChain(filter, next, context);
            }
            else
            {
                return new ServletChain(servlet, context);
            }
        }
    }
}

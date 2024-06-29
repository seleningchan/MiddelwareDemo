using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddelwareDemo
{
    public interface Iwater
    {
        void Invoke();
    }
    public class Water : Iwater
    {
        public void Invoke()
        {
            Console.WriteLine("水已经净化");
        }
    }
    public delegate void WaterDelegate(Iwater iwater);

    public class ApplicationBuilder
    {
        private readonly IList<Func<WaterDelegate, WaterDelegate>> _components = new List<Func<WaterDelegate, WaterDelegate>>();

        public void Use(Func<WaterDelegate,WaterDelegate> middleware)
        {
            _components.Add(middleware);
        }

        public WaterDelegate Build()
        {
            WaterDelegate app = w =>
            {
                Console.WriteLine("无效 的管道");
            };
            for(int index = _components.Count - 1; index >= 0; index--)
            {
                app = _components[index](app);
            }
            return app;
        }
    }
}

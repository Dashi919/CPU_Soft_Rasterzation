using System;
using System.CodeDom;
using System.Threading;

namespace CPU_Soft_Rasterization.Tools
{
    internal class MutilpleThread
    {
        Thread CreateThread()
        {
            Thread thread = new Thread(Worker);
            thread.Name = "sdad";
            thread.Start();
            thread.Join();
            Thread.Sleep(29999);
            
            return thread;
        }

        public static void Worker()
        {
          
        }

    }
}

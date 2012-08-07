using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Tavisca.RainDrop;

namespace RainDrop.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void GeneratesIdTest()
        {
            var id = GetId(HttpClient.Get(URL));
            Assert.IsTrue(id != 0);
        }

        [TestMethod]
        public void DuplicateIdThroughServiceTest()
        {
            
            List<long> ids = new List<long>();
            //ThreadPool.QueueUserWorkItem(state => ids.Add(GetId(URL)));

           List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 10; i++)
            {
                threads.Add(new Thread(() => ids.Add(GetId(HttpClient.Get(URL)))));
            }
            threads.ForEach(thread => thread.Start());
            threads.ForEach(thread => thread.Join());

            var newIds = ids.Distinct().ToList();
            Assert.IsTrue(newIds.Count == ids.Count );

        }

        [TestMethod]
        public void DuplicateIdThroughMethodTest()
        {
            Tavisca.RainDrop.RainDrop rd = new Tavisca.RainDrop.RainDrop();
            List<long> ids = new List<long>();
            //ThreadPool.QueueUserWorkItem(state => ids.Add(GetId(URL)));
            Stopwatch watch = new Stopwatch();
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 1000; i++)
            {
                threads.Add(new Thread(() => ids.Add(rd.GetNextId(1,1))));
            }
            watch.Start();
            threads.ForEach(thread => thread.Start());
            threads.ForEach(thread => thread.Join());
            watch.Stop();
            Console.WriteLine("Elapsed time : " + watch.ElapsedMilliseconds);
            ids.Sort();
            var newIds = ids.Distinct().ToList();
            
            Assert.IsTrue(newIds.Count == ids.Count);
        }

        private static long GetId(string result)
        {
            if (string.IsNullOrEmpty(result))
                return 0;

            var index = result.IndexOf(':');
            var lastIndex = result.IndexOf('}', index);
            var idString = result.Substring(index + 1, lastIndex - index -1).Trim('\n','\r','\t');
            return long.Parse(idString);

        }

        private static string URL
        {
            get { return ConfigurationManager.AppSettings["url"]; }
        }
    }
}

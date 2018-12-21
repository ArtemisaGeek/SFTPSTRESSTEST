using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WinSCP;

namespace SFTP_StressTest
{
    class Program
    {
        public static int contadorsftp = 0;
        public class ChristieObj
        {
           // public int contadorsftp;
            public int xCnt;
            public ManualResetEvent xEvent;
        }
        public static void SessionSFTP(object pEvent)
        {
            try
            {
                Configuration configuration = new Configuration();
                SessionOptions sessionOP = configuration.session();

                using (Session session = new Session())
                {
                    // Conexión
                    session.Open(sessionOP);
                    Random rnd = new Random();
                    ChristieObj xObjC = (ChristieObj)pEvent;
                    string RemoteDirectory = "/"; // or can be  root path and directory example '/Log/'"
                    //TESTWinscp is the first part of file name will save in the remote path, next is the id and the date
                    string remoteFTP = RemoteDirectory + "TESTWinscp - " + xObjC.xCnt.ToString() + "-" + DateTime.Now.ToString("yyyyMMddTHHmmssfff") + ".txt"; 
                    session.PutFiles("StressTestFile.txt", remoteFTP);
                    session.Close();
                    Console.WriteLine("Done: " + remoteFTP);
                    xObjC.xEvent.Set();
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                contadorsftp++;
            }
           
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Stress test  :)");
            Console.WriteLine("Enter interaction number:");
            int tasksnumber = Int32.Parse(Console.ReadLine());
            var events = new List<ManualResetEvent>();
          
            for (int i=1; i<= tasksnumber; i++)
            {
           
                var resetEvent = new ManualResetEvent(false);
                ChristieObj xEvent = new ChristieObj();
                xEvent.xCnt = i;
                xEvent.xEvent = resetEvent;
                events.Add(resetEvent);
                ThreadPool.QueueUserWorkItem(new WaitCallback(SessionSFTP), xEvent);
            }
            Console.WriteLine("Workers sent!");

            var wait = true;
            while (wait)
            {
                WaitHandle.WaitAll(events.Take(64).ToArray());
                events.RemoveRange(0, events.Count > 63 ? 64 : events.Count);
                wait = events.Any();
            }

            Console.WriteLine("Workers done!");
            Console.WriteLine("Errors  : " + contadorsftp);
            Console.WriteLine("Made with Love, by Christie");
            Console.WriteLine("Press a key to finish...");
            Console.ReadKey();
        }

    }
}

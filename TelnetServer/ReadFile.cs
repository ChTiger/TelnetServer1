using SuperSocket.SocketBase;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;

namespace TelnetServer
{
    public static class ReadFile
    {
        public static string Texttaici = ConfigurationManager.AppSettings["Texttaici"];
        public static void ReadFormText(AppSession session, string path, int time)
        {
            //  string path = "E:\\123.txt";
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] filename = dir.GetFiles();
            int fileNum = filename.Length; // 该目录下的文件数量。。

            Random rd = new Random();
            int i = rd.Next(0, fileNum); //1到max之间的数，

            string filepath = filename[i].FullName;

            StreamReader sr = new StreamReader(filepath, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                session.Send(line.ToString());
                Thread.Sleep(time);
            }
            sr.Close();

        }
        public static void Surprise(AppSession session, string path, string txtname)
        {

            string[] pathname = txtname.Split(',');
            string[] taici = Texttaici.Split(';');

            for (int i = 0; i < pathname.Length; i++)
            {
                session.Send(taici[i]);
                StreamReader sr = new StreamReader(path + "123\\" + pathname[i], Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    session.Send(line.ToString());

                }
                sr.Close();
                session.Send("\r\n");
                session.Send("\r\n");
                session.Send("\r\n");
                Thread.Sleep(3888);

            }



        }
    }
}

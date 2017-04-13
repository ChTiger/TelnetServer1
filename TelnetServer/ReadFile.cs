using SuperSocket.SocketBase;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace TelnetServer
{
    public static class ReadFile
    {
        public static void ReadFormText(AppSession session, string path, int time)
        {
            //  string path = "E:\\123.txt";
            DirectoryInfo dir = new DirectoryInfo(path);

            FileInfo[] filename = dir.GetFiles();
            int fileNum = filename.Length; // 该目录下的文件数量。。

            Random rd = new Random();
            int i = rd.Next(0, fileNum); //1到100之间的数，

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
    }
}

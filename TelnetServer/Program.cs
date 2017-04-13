using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Configuration;
using System.Text;
using System.Threading;

namespace TelnetServer
{

    public class Program
    {
        public static string Textpath = ConfigurationManager.AppSettings["Textpath"];
        public static int IntervalTime = int.Parse(ConfigurationManager.AppSettings["IntervalTime"]);
        static void Main(string[] args)
        {

            var appServer = new AppServer();
            //服务器端口  
            int port = 23;
            //设置服务监听端口  
            if (!appServer.Setup(port))
            {
                Console.WriteLine("端口设置失败!");
                Console.ReadKey();
                return;
            }

            //新连接事件  
            appServer.NewSessionConnected += new SessionHandler<AppSession>(NewSessionConnected);

            //收到消息事件  
            appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(NewRequestReceived);

            //连接断开事件  
            appServer.SessionClosed += new SessionHandler<AppSession, CloseReason>(SessionClosed);


            //启动服务  
            if (!appServer.Start())
            {
                Console.WriteLine("启动服务失败!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("启动服务成功!");


            while (true)
            {
                var str = Console.ReadLine();
                if (str.ToLower().Equals("exit"))
                {
                    break;
                }
                if (str.ToLower().Equals("count"))
                {
                    Console.WriteLine(appServer.SessionCount);
                }
            }

            Console.WriteLine();

            //停止服务  
            appServer.Stop();

            Console.WriteLine("服务已停止，按任意键退出!");
        }
        static void NewSessionConnected(AppSession session)
        {

            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "    User on-line IP:" + session.RemoteEndPoint.Address.ToString());
            LogHelper.Info("User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID" + session.SessionID);
            //设置会话的字符格式
            session.Charset = Encoding.GetEncoding("gbk");
            //向对应客户端发送数据  
            session.Send("Hello my friend!");
            Thread.Sleep(1500);
            session.Send("Today is " + DateTime.Now.ToString("yyyy-MM-dd") + "!");
            Thread.Sleep(1500);
            session.Send("Every day is the only day in your life,please love it!");
            session.Send("");
            Thread.Sleep(1500);
            //讲故事开始了
            ReadFile.ReadFormText(session, Textpath, IntervalTime);

            session.Send("");
            session.Send("The article is over,Could you tell me your name now?(用拼音)");

        }

        static void NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            /** 
             * requestInfo为客户端发送的指令，默认为命令行协议 
             * 例： 
             * 发送 ping 127.0.0.1 -n 5 
             * requestInfo.Key: "ping" 
             * requestInfo.Body: "127.0.0.1 -n 5" 
             * requestInfo.Parameters: ["127.0.0.1","-n","5"] 
             **/
            LogHelper.Info("User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID:" + session.SessionID + "  Check in this word:" + requestInfo.Key.ToString());
            switch (requestInfo.Key.ToUpper())
            {

                case ("LEILIN"):
                    session.Send("I Love You!");
                    session.Send("Please call me, my phone number is 18801088!");
                    session.Send("Don't make me wait too long time！");
                    break;

                default:
                    session.Send("Thank you!");
                    session.Close();
                    break;
            }
        }

        static void SessionClosed(AppSession session, CloseReason reason)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "     User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID:" + session.SessionID + " leave!!!!!");
            LogHelper.Info("User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID:" + session.SessionID + " leave!!!!!");
        }
    }
}

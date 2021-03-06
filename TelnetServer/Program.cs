﻿using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using TelnetServer.Log;
using TelnetServer.Models;

namespace TelnetServer
{

    public class Program
    {


        public static string Textpath = ConfigurationManager.AppSettings["Textpath"];
        public static int IntervalTime = int.Parse(ConfigurationManager.AppSettings["IntervalTime"]);
        public static string Textname = ConfigurationManager.AppSettings["Textname"];
        static void Main(string[] args)
        {

            var appServer = new AppServer();
            //服务器端口  
            int port = 23;
            Encoding a = appServer.TextEncoding;
            Console.WriteLine(a);

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

            Console.WriteLine("启动服务成功,如需帮助请输入help!");


            while (true)
            {
                var str = Console.ReadLine();
                if (str.ToLower().Equals("stop"))
                {
                    appServer.Stop();
                    Console.WriteLine(appServer.State.ToString());
                }
                if (str.ToLower().Equals("start"))
                {

                    appServer.Start();
                    Console.WriteLine(appServer.State.ToString());
                }
                if (str.ToLower().Equals("state"))
                {

                    Console.WriteLine(appServer.State.ToString());
                }

                if (str.ToLower().Equals("count"))
                {
                    Console.WriteLine(appServer.SessionCount);
                }
                if (str.ToLower().Equals("close"))
                {
                    SessionAllClosed(appServer);
                }
                if (str.ToLower().Equals("help"))
                {
                    Console.WriteLine("stop,start,state,count,close");
                }
            }

        }
        static void NewSessionConnected(AppSession session)
        {
            //控制台输出
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "    User on-line IP:" + session.RemoteEndPoint.Address.ToString());
            //日志记录
            LogHelper.Info("User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID" + session.SessionID);
            //写入数据库
            Ip ip = new Ip();
            ip.Action = "online";
            ip.IP = session.RemoteEndPoint.Address.ToString();
            ip.SessionId = session.SessionID;
            RecordIP.recordip(ip);


            //设置会话的字符格式
            session.Charset = Encoding.GetEncoding("gbk");
            //向对应客户端发送数据  
            session.Send("Hello my friend!");
            Thread.Sleep(1500);
            session.Send("Today is " + DateTime.Now.ToString("yyyy-MM-dd") + "!");
            Thread.Sleep(1500);
            session.Send("Everyday is the only day in your life,please love it!");
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
            //requestInfo.Parameters

            //记录日志
            LogHelper.Info("User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID:" + session.SessionID + "  key in:" + requestInfo.Key.ToString());

            //写入数据库
            Ip ip = new Ip();
            ip.Action = "input  " + requestInfo.Key.ToString();
            ip.IP = session.RemoteEndPoint.Address.ToString();
            ip.SessionId = session.SessionID;
            RecordIP.recordip(ip);


            switch (requestInfo.Key.ToUpper())
            {

                case ("ZHANGSHENGNAN"):
                    session.Send("这个站点专门为你做的，不要惊讶，请你认真看完下面的几幅画！");
                    session.Send("");
                    Thread.Sleep(2000);
                    ReadFile.Surprise(session, Textpath, Textname);
                    session.Send("谢谢你能看完！");

                    LogHelper.Info("****************************************************************");

                    ip.Action = "look";
                    ip.IP = session.RemoteEndPoint.Address.ToString();
                    ip.SessionId = session.SessionID;
                    RecordIP.recordip(ip);

                    session.Close();

                    break;

                default:
                    session.Send("Thank you!");
                    session.Send("祝您幸福!");
                    ReadFile.ReadFormText(session, Textpath + "123\\", 0);
                    session.Close();
                    break;
            }
        }

        static void SessionClosed(AppSession session, CloseReason reason)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "     User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID:" + session.SessionID + " leave!!!!!" + reason);
            LogHelper.Info("User IP:" + session.RemoteEndPoint.Address.ToString() + "   sessionID:" + session.SessionID + " leave!!!!!" + reason);
            Ip ip = new Ip();
            ip.Action = "leave  " + reason;
            ip.IP = session.RemoteEndPoint.Address.ToString();
            ip.SessionId = session.SessionID;
            RecordIP.recordip(ip);

        }

        static void SessionAllClosed(AppServer appserver)
        {
            var appsession = appserver.GetAllSessions();
            foreach (AppSession item in appsession)
            {
                item.Close();
            }

        }



    }
}

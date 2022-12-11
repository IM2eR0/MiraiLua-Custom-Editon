using System;
using System.Reactive.Linq;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Manganese.Text;

using KeraLua;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Linq;

namespace MiraiLua
{
    class Program
    {
        static public Util util = new Util();
        static public Lua lua = new Lua();
        static public MiraiBot bot;
        static public object o = new object();
        static void Test()
        {
            lua.GetGlobal("test");
            lua.PCall(0, 0, 0);
            if (lua.GetTop() >= 1)
                Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
            lua.Pop(lua.GetTop());
        }

        static void FileChanged(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(10);
            //Util.Print("�ļ�����..." + e.FullPath);
            lock (o)
            {
                if (lua.DoFile(e.FullPath))
                {
                    Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                    lua.Pop(1);
                }
            }
        }

        static public void LoadLibPlugins()
        {
            if (!Directory.Exists(@"./base-libs"))
            {
                Directory.CreateDirectory(@"./base-libs");
                Util.Print("δ�ҵ�ǰ�ò��Ŀ¼��������...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("���ڼ��� MiraiLua ��Ҫǰ��....", Util.PrintType.INFO);
                DirectoryInfo dir = new DirectoryInfo(@"./base-libs");
                DirectoryInfo[] ds = dir.GetDirectories();
                ds = ds.OrderBy(d => d.Name).ToArray();

                foreach (DirectoryInfo d in ds)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Changed += new FileSystemEventHandler(FileChanged);
                    watcher.Path = @"./base-libs/" + d.Name;
                    watcher.EnableRaisingEvents = true;

                    FileInfo[] fs = d.GetFiles();
                    fs = fs.OrderBy(f => f.Name).ToArray();
                    //lua.DoFile(f.FullName);
                    foreach (FileInfo f in fs)
                    {
                        if (f.Extension == ".lua")
                        {
                            Util.Print("> " + d.Name + "/" + f.Name);

                            if (lua.DoFile(@"./base-libs/" + d.Name + "/" + f.Name))
                            {
                                Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                                lua.Pop(1);
                            }
                        }
                    }
                }
            }
        }

        static public void LoadUserLib()
        {
            if (!Directory.Exists(@"./user-libs"))
            {
                Directory.CreateDirectory(@"./user-libs");
                Util.Print("δ�ҵ�ǰ�ò��Ŀ¼��������...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("���ڼ����û����ǰ��...", Util.PrintType.INFO);
                DirectoryInfo dir = new DirectoryInfo(@"./user-libs");
                DirectoryInfo[] ds = dir.GetDirectories();
                ds = ds.OrderBy(d => d.Name).ToArray();

                foreach (DirectoryInfo d in ds)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Changed += new FileSystemEventHandler(FileChanged);
                    watcher.Path = @"./user-libs/" + d.Name;
                    watcher.EnableRaisingEvents = true;

                    FileInfo[] fs = d.GetFiles();
                    fs = fs.OrderBy(f => f.Name).ToArray();
                    //lua.DoFile(f.FullName);
                    foreach (FileInfo f in fs)
                    {
                        if (f.Extension == ".lua")
                        {
                            // Util.Print("> " + d.Name + "/" + f.Name);

                            if (lua.DoFile(@"./user-libs/" + d.Name + "/" + f.Name))
                            {
                                Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                                lua.Pop(1);
                            }
                        }
                    }
                }
            }
        }
        static public void LoadPlugins()
        {
            if (!Directory.Exists(@"./user-plugins"))
            {
                Directory.CreateDirectory(@"./user-plugins");
                Util.Print("δ�ҵ����Ŀ¼��������...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("���ڼ����û����...", Util.PrintType.INFO);
                DirectoryInfo dir = new DirectoryInfo(@"./user-plugins");
                DirectoryInfo[] ds = dir.GetDirectories();

                foreach (DirectoryInfo d in ds)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Changed += new FileSystemEventHandler(FileChanged);
                    watcher.Path = @"./user-plugins/" + d.Name;
                    watcher.EnableRaisingEvents = true;

                    FileInfo[] fs = d.GetFiles();
                    //lua.DoFile(f.FullName);
                    foreach (FileInfo f in fs)
                    {
                        if (f.Extension == ".lua")
                        {
                            //Util.Print("���ز��..." + d.Name + "/" + f.Name);

                            if (lua.DoFile(@"./user-plugins/" + d.Name + "/" + f.Name))
                            {
                                Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                                lua.Pop(1);
                            }
                        }
                    }
                }
            }
        }
        static int Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("MiraiLua for Linux ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("v1.1");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nby OriginalSnow\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n\n\tMiraiLua by ABSD\n\n\n");

            Util.Print("��������MiraiLua...");

            ////////////////LUA///////////////////
            lua.Encoding = Encoding.UTF8;
            Util.Print("��ǰ����ʹ�ñ��룺" + lua.Encoding);

            lua.Register("print", LFunctions.Print);

            lua.NewTable();
            lua.SetGlobal("api");
            Util.PushFunction("api", "Reload", lua, LFunctions.Reload);
            Util.PushFunction("api", "SendGroupMsg", lua, LFunctions.SendGroupMsg);
            Util.PushFunction("api", "SendGroupMsgEX", lua, LFunctions.SendGroupMsgEX);
            Util.PushFunction("api", "OnReceiveGroup", lua, LFunctions.OnReceiveGroup);
            Util.PushFunction("api", "HttpGet", lua, LFunctions.HttpGetA);
            Util.PushFunction("api", "HttpPost", lua, LFunctions.HttpPostA);
            Util.PushFunction("api", "UploadImg", lua, LFunctions.UploadImg);
            Util.PushFunction("api", "At", lua, LFunctions.At);
            lua.Pop(lua.GetTop());
            //���ؽű�
            LoadLibPlugins();
            LoadUserLib();
            LoadPlugins();
            //////////////////////////////////////
            try
            {
                //XmlDocument��ȡxml�ļ�
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("settings.xml");
                //��ȡxml���ڵ�
                XmlNode xmlRoot = xmlDoc.DocumentElement;
                //���ݽڵ�˳���𲽶�ȡ
                //��ȡ��һ��name�ڵ�
                string a = xmlRoot.SelectSingleNode("Address").InnerText;
                string q = xmlRoot.SelectSingleNode("QQ").InnerText;
                string v = xmlRoot.SelectSingleNode("Key").InnerText;

                bot = new MiraiBot
                {
                    Address = a,
                    QQ = q,
                    VerifyKey = v
                };

                bot.LaunchAsync();

                Util.Print(String.Format("Bot�����ӣ�{0:G} / {1:G}", a, q));
            }
            catch (Exception e)
            {
                Util.Print(String.Format("��������{0:G}\n����settings.xml�Ƿ�����ҺϷ�.", e.Message));
                Console.ReadLine();
                return 0;
            }

            //������Ϣ
            bot.MessageReceived.OfType<GroupMessageReceiver>().Subscribe(x =>
            {
                if (x.Sender.Id == bot.QQ)
                    return;

                string msg = x.MessageChain.GetPlainMessage();

                lock (o)
                {
                    lua.GetGlobal("api");
                    lua.GetField(-1, "OnReceiveGroup");

                    lua.NewTable();

                    lua.PushString(x.Sender.Id);
                    lua.SetField(-2, "SenderID");

                    lua.PushString(x.Sender.Name);
                    lua.SetField(-2, "SenderName");

                    lua.PushNumber((int)x.Sender.Permission);
                    lua.SetField(-2, "SenderRank");

                    lua.PushString(x.GroupId);
                    lua.SetField(-2, "GroupID");

                    lua.PushString(x.GroupName);
                    lua.SetField(-2, "GroupName");

                    lua.PushString(x.Type.ToString());
                    lua.SetField(-2, "From");

                    lua.GetGlobal("util");
                    //Util.Print(lua.ToString(-1)+" "+ lua.ToString(-2) + " "+ lua.ToString(-3) + " "+ lua.ToString(-4) + " ");
                    lua.GetField(-1, "JSONToTable");

                    lua.PushString(x.MessageChain.ToJsonString());

                    lua.Call(1, 1);

                    lua.Remove(-2);

                    Util.Print(String.Format("[{0:G}][{1:G}]��{2:G}", x.GroupName, x.Sender.Name, msg));

                    lua.SetField(-2, "Data");

                    lua.PCall(1, 0, 0);
                    lua.Remove(1);

                    //Util.Print(lua.GetTop().ToString());

                    if (lua.GetTop() >= 1)
                        Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                    lua.Pop(lua.GetTop());
                }
            });

            Util.Print("׼������");

            while (true)
            {
                string cmd = Console.ReadLine();
                string[] cargs = cmd.Split(" ");
                lock (o)
                {
                    if (cargs.GetLength(0) < 1)
                    {
                        Util.Print("��Ч������. Ҫ��ȡ����������help.", Util.PrintType.INFO, ConsoleColor.Red);
                        continue;
                    }

                    if (cargs[0] == "exit")
                        break;
                    if (cargs[0] == "test")
                        Test();
                    if (cargs[0] == "reload")
                    {
                        LoadUserLib();
                        LoadPlugins();
                    }
                    else if (cargs[0] == "help")
                    {
                        Util.Print("�����б�", Util.PrintType.INFO);
                        Util.Print("help - ��ȡ����", Util.PrintType.INFO);
                        Util.Print("reload - ���ز��", Util.PrintType.INFO);
                        Util.Print("exit - �˳�MiraiLua", Util.PrintType.INFO);
                        Util.Print("lua <����> - ִ��һ��lua�ı�", Util.PrintType.INFO);
                        Util.Print("Powered by ABSD", Util.PrintType.INFO);
                    }
                    else if (cargs[0] == "lua")
                    {
                        if (cargs.GetLength(0) >= 2)
                        {
                            string s = cargs[1];
                            for (int i = 0; i < cargs.GetLength(0); i++)
                            {
                                if (i > 1)
                                    s += " " + cargs[i];
                            }

                            if (lua.DoString(s))
                            {
                                Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                                lua.Pop(1);
                            }
                        }
                        else
                            Util.Print("�����ʽ: lua <����>", Util.PrintType.INFO, ConsoleColor.Red);
                    }
                    else
                        Util.Print("��Ч������. Ҫ��ȡ����������help.", Util.PrintType.INFO, ConsoleColor.Red);
                }
            }
            return 0;
        }
    }
}
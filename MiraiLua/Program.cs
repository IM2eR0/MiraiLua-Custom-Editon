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
            //Util.Print("文件更新..." + e.FullPath);
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
                Util.Print("未找到前置插件目录，创建中...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("正在加载 MiraiLua 必要前置....", Util.PrintType.INFO);
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
                Util.Print("未找到前置插件目录，创建中...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("正在加载用户插件前置...", Util.PrintType.INFO);
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
                Util.Print("未找到插件目录，创建中...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("正在加载用户插件...", Util.PrintType.INFO);
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
                            //Util.Print("加载插件..." + d.Name + "/" + f.Name);

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

            Util.Print("正在启动MiraiLua...");

            ////////////////LUA///////////////////
            lua.Encoding = Encoding.UTF8;
            Util.Print("当前正在使用编码：" + lua.Encoding);

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
            //加载脚本
            LoadLibPlugins();
            LoadUserLib();
            LoadPlugins();
            //////////////////////////////////////
            try
            {
                //XmlDocument读取xml文件
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("settings.xml");
                //获取xml根节点
                XmlNode xmlRoot = xmlDoc.DocumentElement;
                //根据节点顺序逐步读取
                //读取第一个name节点
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

                Util.Print(String.Format("Bot已连接：{0:G} / {1:G}", a, q));
            }
            catch (Exception e)
            {
                Util.Print(String.Format("发生错误：{0:G}\n请检查settings.xml是否存在且合法.", e.Message));
                Console.ReadLine();
                return 0;
            }

            //接收消息
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

                    Util.Print(String.Format("[{0:G}][{1:G}]：{2:G}", x.GroupName, x.Sender.Name, msg));

                    lua.SetField(-2, "Data");

                    lua.PCall(1, 0, 0);
                    lua.Remove(1);

                    //Util.Print(lua.GetTop().ToString());

                    if (lua.GetTop() >= 1)
                        Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                    lua.Pop(lua.GetTop());
                }
            });

            Util.Print("准备就绪");

            while (true)
            {
                string cmd = Console.ReadLine();
                string[] cargs = cmd.Split(" ");
                lock (o)
                {
                    if (cargs.GetLength(0) < 1)
                    {
                        Util.Print("无效的命令. 要获取帮助请输入help.", Util.PrintType.INFO, ConsoleColor.Red);
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
                        Util.Print("帮助列表：", Util.PrintType.INFO);
                        Util.Print("help - 获取帮助", Util.PrintType.INFO);
                        Util.Print("reload - 重载插件", Util.PrintType.INFO);
                        Util.Print("exit - 退出MiraiLua", Util.PrintType.INFO);
                        Util.Print("lua <代码> - 执行一段lua文本", Util.PrintType.INFO);
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
                            Util.Print("命令格式: lua <代码>", Util.PrintType.INFO, ConsoleColor.Red);
                    }
                    else
                        Util.Print("无效的命令. 要获取帮助请输入help.", Util.PrintType.INFO, ConsoleColor.Red);
                }
            }
            return 0;
        }
    }
}
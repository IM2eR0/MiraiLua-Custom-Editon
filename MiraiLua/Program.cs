using System;
using System.Reactive.Linq;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Manganese.Text;

using KeraLua;
using System.IO;
using System.Threading;
using System.Xml;
using System.Linq;

using MiraiLua.Classes;
using System.Text;
using System.Threading.Tasks;

namespace MiraiLua
{
    static class Program
    {
        static string PROGRAM_NAME = "MiraiLua Custom Edition";
        static string AUTHOR = "初雪 OriginalSnow";
        static string VERSION = "1.3";


        static public Util util = new Util();
        static public Lua lua = new Lua();
        static public MiraiBot bot;
        static public object o = new object();
        static public string curFileDir { get; set; }
        static public char g = Path.DirectorySeparatorChar;
        static void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name.IndexOf(".lua") == -1)
                return;
            Thread.Sleep(10);
            curFileDir = e.FullPath.Replace(g + e.Name, "").Replace($".{g}plugins{g}", "");
            lock (o)
            {
                if (lua.DoFile(e.FullPath))
                {
                    Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                    lua.Pop(1);
                }
                curFileDir = "";
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

        static public void LoadPlugins()
        {
            if (!Directory.Exists($".{g}plugins"))
            {
                Directory.CreateDirectory($".{g}plugins");
                Util.Print("未找到插件目录，创建中...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("正在加载用户插件...", Util.PrintType.INFO);
                DirectoryInfo dir = new DirectoryInfo($".{g}plugins");
                DirectoryInfo[] ds = dir.GetDirectories();

                foreach (DirectoryInfo d in ds)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Changed += new FileSystemEventHandler(FileChanged);
                    watcher.Path = $".{g}plugins{g}" + d.Name;
                    watcher.EnableRaisingEvents = true;

                    curFileDir = d.Name;

                    FileInfo[] fs = d.GetFiles();
                    //lua.DoFile(f.FullName);
                    foreach (FileInfo f in fs)
                    {
                        if (f.Name == "init.lua")
                        {

                            if (lua.DoFile($".{g}plugins{g}" + d.Name + g + f.Name))
                            {
                                Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                                lua.Pop(1);
                            }
                        }
                    }
                    curFileDir = "";
                }
            }
        }
        static async Task<int> Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{PROGRAM_NAME} ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"v{VERSION}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\nby {AUTHOR}\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n\n\tMiraiLua v1.2 by ABSD\n\n\n");

            Util.Print("正在启动 MiraiLua Custom Editon");

            ////////////////LUA///////////////////
            Util.Print("打包框架：.NET 7.0");
            lua.Encoding = Encoding.UTF8;

            lua.Register("print", LFunctions.Print);
            lua.Register("include", LFunctions.include);
            lua.Register("GetDir", LFunctions.GetDir);
            lua.Register("ByteArray", ByteArray.New);
            lua.Register("LoadFile", ByteArray.LoadFile);
            lua.Register("SaveFile", ByteArray.SaveFile);

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
            Util.PushFunction("api", "TextToImage", lua, LFunctions.TextToImage);
            Util.PushFunction("api", "DelImg",lua,LFunctions.DelImg);
            lua.Pop(lua.GetTop());
            //加载脚本

            if (!Directory.Exists($".{g}临时文件夹"))
            {
                Directory.CreateDirectory($".{g}临时文件夹");
                Util.Print("正在初始化临时文件夹中...", Util.PrintType.WARNING);
            }

            LoadLibPlugins();
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

                Util.Print(String.Format("正在尝试连接Bot：{0:G} / {1:G}", a, q));

                await bot.LaunchAsync();

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

                    if (cargs[0] == "reload")
                    {
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
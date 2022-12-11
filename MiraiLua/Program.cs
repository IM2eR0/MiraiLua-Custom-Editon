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
<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b

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
<<<<<<< HEAD
            //Util.Print("Œƒº˛∏¸–¬..." + e.FullPath);
=======
            Util.Print("Êñá‰ª∂Êõ¥Êñ∞..." + e.FullPath);
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b
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
                Util.Print("Œ¥’“µΩ«∞÷√≤Âº˛ƒø¬º£¨¥¥Ω®÷–...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("’˝‘⁄º”‘ÿ MiraiLua ±ÿ“™«∞÷√....", Util.PrintType.INFO);
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
<<<<<<< HEAD
                            Util.Print("> " + d.Name + "/" + f.Name);

                            if (lua.DoFile(@"./base-libs/" + d.Name + "/" + f.Name))
=======
                            Util.Print("Âä†ËΩΩÊèí‰ª∂..." + d.Name + "\\" + f.Name);

                            if (lua.DoFile(@".\plugins\" + d.Name + "\\" + f.Name))
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b
                            {
                                Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                                lua.Pop(1);
                            }
                        }
                    }
                }
            }
        }
<<<<<<< HEAD

        static public void LoadUserLib()
        {
            if (!Directory.Exists(@"./user-libs"))
            {
                Directory.CreateDirectory(@"./user-libs");
                Util.Print("Œ¥’“µΩ«∞÷√≤Âº˛ƒø¬º£¨¥¥Ω®÷–...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("’˝‘⁄º”‘ÿ”√ªß≤Âº˛«∞÷√...", Util.PrintType.INFO);
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
                Util.Print("Œ¥’“µΩ≤Âº˛ƒø¬º£¨¥¥Ω®÷–...", Util.PrintType.WARNING);
            }
            else
            {
                Util.Print("’˝‘⁄º”‘ÿ”√ªß≤Âº˛...", Util.PrintType.INFO);
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
                            //Util.Print("º”‘ÿ≤Âº˛..." + d.Name + "/" + f.Name);

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
=======
        static int Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("MiraiLua v1.1 - Powered by ABSD\n");
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b

            Util.Print("’˝‘⁄∆Ù∂ØMiraiLua...");

            ////////////////LUA///////////////////
            lua.Encoding = Encoding.UTF8;
            Util.Print("µ±«∞’˝‘⁄ π”√±‡¬Î£∫" + lua.Encoding);

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
            //º”‘ÿΩ≈±æ
            LoadLibPlugins();
            LoadUserLib();
            LoadPlugins();
            //////////////////////////////////////
            try
            {
                //XmlDocument∂¡»°xmlŒƒº˛
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("settings.xml");
                //ªÒ»°xml∏˘Ω⁄µ„
                XmlNode xmlRoot = xmlDoc.DocumentElement;
                //∏˘æ›Ω⁄µ„À≥–Ú÷≤Ω∂¡»°
                //∂¡»°µ⁄“ª∏ˆnameΩ⁄µ„
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

                Util.Print(String.Format("Bot“—¡¨Ω”£∫{0:G} / {1:G}", a, q));
            }
            catch (Exception e)
            {
                Util.Print(String.Format("∑¢…˙¥ÌŒÛ£∫{0:G}\n«ÎºÏ≤Èsettings.xml «∑Ò¥Ê‘⁄«“∫œ∑®.", e.Message));
                Console.ReadLine();
                return 0;
            }

            //Ω” ’œ˚œ¢
            bot.MessageReceived.OfType<GroupMessageReceiver>().Subscribe(x =>
            {
                if (x.Sender.Id == bot.QQ)
                    return;
<<<<<<< HEAD

                string msg = x.MessageChain.GetPlainMessage();

                lock (o)
                {
=======
                
                string msg = x.MessageChain.GetPlainMessage();
                
                lock (o) {
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b
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

<<<<<<< HEAD
                    Util.Print(String.Format("[{0:G}][{1:G}]£∫{2:G}", x.GroupName, x.Sender.Name, msg));
=======
                    Util.Print(String.Format("[{0:G}][{1:G}]Ôºö{2:G}", x.GroupName, x.Sender.Name, msg));
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b

                    lua.SetField(-2, "Data");

                    lua.PCall(1, 0, 0);
                    lua.Remove(1);

                    //Util.Print(lua.GetTop().ToString());

                    if (lua.GetTop() >= 1)
                        Util.Print(lua.ToString(-1), Util.PrintType.ERROR, ConsoleColor.Red);
                    lua.Pop(lua.GetTop());
                }
            });

            Util.Print("◊º±∏æÕ–˜");

            while (true)
            {
                string cmd = Console.ReadLine();
                string[] cargs = cmd.Split(" ");
                lock (o)
                {
                    if (cargs.GetLength(0) < 1)
                    {
<<<<<<< HEAD
                        Util.Print("Œﬁ–ßµƒ√¸¡Ó. “™ªÒ»°∞Ô÷˙«Î ‰»Îhelp.", Util.PrintType.INFO, ConsoleColor.Red);
=======
                        Util.Print("Êó†ÊïàÁöÑÂëΩ‰ª§. Ë¶ÅËé∑ÂèñÂ∏ÆÂä©ËØ∑ËæìÂÖ•help.", Util.PrintType.INFO, ConsoleColor.Red);
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b
                        continue;
                    }

                    if (cargs[0] == "exit")
                        break;
                    if (cargs[0] == "test")
                        Test();
                    if (cargs[0] == "reload")
<<<<<<< HEAD
                    {
                        LoadUserLib();
                        LoadPlugins();
                    }
                    else if (cargs[0] == "help")
                    {
                        Util.Print("∞Ô÷˙¡–±Ì£∫", Util.PrintType.INFO);
                        Util.Print("help - ªÒ»°∞Ô÷˙", Util.PrintType.INFO);
                        Util.Print("reload - ÷ÿ‘ÿ≤Âº˛", Util.PrintType.INFO);
                        Util.Print("exit - ÕÀ≥ˆMiraiLua", Util.PrintType.INFO);
                        Util.Print("lua <¥˙¬Î> - ÷¥––“ª∂ŒluaŒƒ±æ", Util.PrintType.INFO);
=======
                        LoadPlugins();
                    else if (cargs[0] == "help")
                    {
                        Util.Print("Â∏ÆÂä©ÂàóË°®Ôºö", Util.PrintType.INFO);
                        Util.Print("help - Ëé∑ÂèñÂ∏ÆÂä©", Util.PrintType.INFO);
                        Util.Print("reload - ÈáçËΩΩÊèí‰ª∂", Util.PrintType.INFO);
                        Util.Print("exit - ÈÄÄÂá∫MiraiLua", Util.PrintType.INFO);
                        Util.Print("lua <‰ª£Á†Å> - ÊâßË°å‰∏ÄÊÆµluaÊñáÊú¨", Util.PrintType.INFO);
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b
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
<<<<<<< HEAD
                            Util.Print("√¸¡Ó∏Ò Ω: lua <¥˙¬Î>", Util.PrintType.INFO, ConsoleColor.Red);
                    }
                    else
                        Util.Print("Œﬁ–ßµƒ√¸¡Ó. “™ªÒ»°∞Ô÷˙«Î ‰»Îhelp.", Util.PrintType.INFO, ConsoleColor.Red);
=======
                            Util.Print("ÂëΩ‰ª§Ê†ºÂºè: lua <‰ª£Á†Å>", Util.PrintType.INFO, ConsoleColor.Red);
                    }
                    else
                        Util.Print("Êó†ÊïàÁöÑÂëΩ‰ª§. Ë¶ÅËé∑ÂèñÂ∏ÆÂä©ËØ∑ËæìÂÖ•help.", Util.PrintType.INFO, ConsoleColor.Red);
>>>>>>> 4b8ad74e9010fbdc9c3bbcfa685a471244811f3b
                }
            }
            return 0;
        }
    }
}
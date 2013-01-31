using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.Threading;

using MXit;

namespace FinalWar
{
    class Program
    {
        public static ExternalAppAPI.CommsClient client;
        public static DBUtil dbutil;

        public static List<DBUtil.UData> ActiveUsers;

        public static Config config;

        public static GameMenus gameMenus;
        public static GameActions gameActions;

        static void Main(string[] args)
        {
            try
            {
                config = new Config();
                config.Load();

                gameMenus = new GameMenus();
                gameActions = new GameActions();

                dbutil = new DBUtil();
                ActiveUsers = new List<DBUtil.UData>();

                ExternalAppAPI.CommsCallback callback = new Callback();
                InstanceContext context = new InstanceContext(callback);

                client = new ExternalAppAPI.CommsClient(context);
                client.Connect("", "", SDK.Instance);

                Console.WriteLine("Connected");

                Timer KeepAliveTimer = new Timer(new TimerCallback(KeepAlive), null, 3 * 60 * 1000, 3 * 60 * 1000);

                Console.WriteLine("Press <return> to disconnect");
                Console.ReadLine();

                for ( int j = ActiveUsers.Count - 1; j >= 0; j--)
                {
                    dbutil.SaveData(ActiveUsers[j].mxitid);
                }

                KeepAliveTimer.Dispose();

                client.Disconnect();
                client.Close();

                config.Save();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\n<Return> to exit\n");
            Console.ReadLine();
        }

        private static void KeepAlive(object stateinfo)
        {
            client.KeepAlive();
        }
    }
}

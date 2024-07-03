using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace QuestFishUnlimited
{
    [ApiVersion(2, 1)]
    public class QuestFishUnlimited : TerrariaPlugin
    {

        #region Plugin Info
        public override string Author => "Onusai 羽学";
        public override string Description => "Recieve a new fishing quest immediately after turning one in";
        public override string Name => "QuestFishUnlimited";
        public override Version Version => new Version(1, 1, 0, 0); 
        #endregion

        #region Register and Deregister
        public QuestFishUnlimited(Main game) : base(game) { }
        public override void Initialize()
        {
            LoadConfig(); //Ensure initialization creates configuration files
            GeneralHooks.ReloadEvent += (_) => LoadConfig();
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            ServerApi.Hooks.NetGetData.Register(this, OnNetGetData);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GeneralHooks.ReloadEvent -= (_) => LoadConfig();
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin);
                ServerApi.Hooks.NetGetData.Deregister(this, OnNetGetData);
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Configure overload read and write methods
        internal static Configuration Config = new();
        private static void LoadConfig()
        {
            Config = Configuration.Read();
            Config.Write();
            TShock.Log.ConsoleInfo("[QuestFishUnlimited] Reload configuration completed.");
        }
        #endregion

        #region GetData
        private static HashSet<string> PlayerDoneList = new HashSet<string>();
        private static void OnNetGetData(GetDataEventArgs args)
        {
            var plr = TShock.Players[args.Msg.whoAmI];
            if (args == null || plr == null || !plr.IsLoggedIn) return;

            if (args.MsgID == PacketTypes.CompleteAnglerQuest)
            {
                if (Config.SwitchTasks) { AnglerQuestSwap(plr); }

                PlayerDoneList.Add(plr.Name);
                Main.anglerWhoFinishedToday.Remove(plr.Name);
                NetMessage.SendAnglerQuest(plr.Index);
            }
        }
        #endregion

        #region Prevent players who complete tasks from not refreshing their tasks after re entering the server
        private void OnJoin(JoinEventArgs args)
        {
            TSPlayer plr = TShock.Players[args.Who];
            if (plr == null || args == null) return;

            if (PlayerDoneList.Contains(plr.Name))
            {
                Main.anglerWhoFinishedToday.Remove(plr.Name);
                NetMessage.SendAnglerQuest(plr.Index);
            }
        }
        #endregion

        #region Modify：Terraria.Main.AnglerQuestSwap();
        public static void AnglerQuestSwap(TSPlayer plr)
        {
            if (Main.netMode == 1) return;

            //I just want to remove the personal list, so I changed it to 'Remove'
            Main.anglerWhoFinishedToday.Remove(plr.Name);
            Main.anglerQuestFinished = false;
            bool flag = NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || Main.hardMode || NPC.downedSlimeKing || NPC.downedQueenBee;
            bool flag2 = true;
            while (flag2)
            {
                flag2 = false;
                Main.anglerQuest = Main.rand.Next(Main.anglerQuestItemNetIDs.Length);
                int num = Main.anglerQuestItemNetIDs[Main.anglerQuest];
                if (num == 2454 && (!Main.hardMode || WorldGen.crimson))
                {
                    flag2 = true;
                }

                if (num == 2457 && WorldGen.crimson)
                {
                    flag2 = true;
                }

                if (num == 2462 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2463 && (!Main.hardMode || !WorldGen.crimson))
                {
                    flag2 = true;
                }

                if (num == 2465 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2468 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2471 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2473 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2477 && !WorldGen.crimson)
                {
                    flag2 = true;
                }

                if (num == 2480 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2483 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2484 && !Main.hardMode)
                {
                    flag2 = true;
                }

                if (num == 2485 && WorldGen.crimson)
                {
                    flag2 = true;
                }

                if ((num == 2476 || num == 2453 || num == 2473) && !flag)
                {
                    flag2 = true;
                }
            }

            NetMessage.SendAnglerQuest(plr.Index);
        }
        #endregion

    }
}
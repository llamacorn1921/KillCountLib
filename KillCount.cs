using System;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace KillCountLib
{
    public class KillCount : Plugin<Config>
    {
        #region Plugin Info
        public override string Name { get; } = "KillCountLib";
        public override string Author { get; } = "llamacorn1921";
        public override string Prefix { get; } = "kcl";
        public override Version Version { get; } = new Version(1, 0, 0);
        #endregion

        private static KillCount Singleton = new KillCount();
        
        private Events _events;
        
        private KillCount()
        {
            
        }
        
        public static KillCount Instance => Singleton;

        public override PluginPriority Priority { get; } = PluginPriority.High;

        public override void OnEnabled()
        {
            if (!Config.IsEnabled) return;
            if (File.Exists(KillCount.Instance.Config.DatabasePath)) Cache.LoadCacheFromFile();
            Log.Error("HHHHHHEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEELLLLLLLLLLLLLLLOOOOOOOOOOOOOOOOOO");
            RegisterEvents();
            base.OnEnabled();
        }

        private void RegisterEvents()
        {
            this._events = new Events(Instance);
            
            Exiled.Events.Handlers.Player.Died += this._events.OnPlayerDeath;
            Exiled.Events.Handlers.Player.Spawned += this._events.OnPlayerSpawn;
            Exiled.Events.Handlers.Server.RoundEnded += this._events.OnRoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers += this._events.OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Joined += this._events.OnPlayerJoin;
        }
    }
}
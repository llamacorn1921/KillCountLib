using System.Text;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace KillCountLib
{
    public class Events
    {
        private KillCount _killCount;
        public Events(KillCount killCount)
        {
            this._killCount = killCount;
            EventManager.RegisterEvents(this);
        }
        public void OnPlayerSpawn(SpawnedEventArgs args)
        {
            // args.Player.Broadcast(15, $"{args.Reason.ToString()} | {args.OldRole.ToString()} | {args.SpawnFlags.ToString()}");
            switch (args.Reason) // check the reason for the spawn
            {
                case SpawnReason.ForceClass: // if spawned by forceclass
                {
                    args.Player.Broadcast(10, "<b>You have been spawned non-naturally.</b> <i>Any kills you get will not be counted.</i> Once you die and spawn naturally, it will be reset.", Broadcast.BroadcastFlags.Normal, false);
                    Cache.AddNonNaturalSpawn(args.Player); // add them to the cache so the plugin knows they do not count
                }
                    break;
                    
            }
        }
        public void OnPlayerDeath(DiedEventArgs args)
        {
            
            if (args.Attacker == null) return; // if the killer is null, return

            if (!Cache.DoesPlayerHaveNonNaturallSpawn(args.Player) && !Cache.DoesPlayerHaveNonNaturallSpawn(args.Attacker))
            {
               Cache.AddKill(args.Attacker);
               args.Attacker.Broadcast(15, $"You got a kill point! You have {Cache.GetKillCount(args.Attacker).ToString()} kill points");
            }

            if (Cache.DoesPlayerHaveNonNaturallSpawn(args.Player))
            {
                args.Attacker.Broadcast(15, "You did not get a kill point because you or the player you killed spawned non-naturally.");
            }
            Cache.RemoveNonNaturalSpawn(args.Player);
        }
        
        public void OnRoundEnded(RoundEndedEventArgs args)
        {
            FileWorker.SaveData();
            Log.Info("Saved data.");
        }
        public void OnWaitingForPlayers()
        {
            
        }

        public void OnPlayerJoin(JoinedEventArgs args)
        {
            args.Player.Broadcast(15, "<b><color=green>Welcome</color></b>\nThere is an event currently going on.\nTo get your current kill count, enter <b>.kc</b>", Broadcast.BroadcastFlags.Normal, false);
        }
        
        [PluginEvent(ServerEventType.RemoteAdminCommandExecuted)]
        public void OnAdminCommandExecuted(RemoteAdminCommandExecutedEvent args)
        {
            
            Log.Info($"===========================Command: {args.Command} |  | {args.Response}");
        }
        [PluginEvent(ServerEventType.RemoteAdminCommand)]
        public void OnAdminCommandEvent(RemoteAdminCommandEvent args)
        {
            if (args.Command == "doortp")
            {
                Player.TryGet(args.Arguments[0].Replace(".", ""), out Player player);
                Log.Info($"{player.Role} | ${player.UserId} | ${player.Nickname}");
            }
        }
    }
}
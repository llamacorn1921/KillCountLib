using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using KillCountLib.blocks;

namespace KillCountLib
{
    public static class Cache
    {
        private static List<Player> nonNaturallSpawn = new List<Player>();

        private static Dictionary<Player, KCPlayerData> killCount = new Dictionary<Player, KCPlayerData>();

        private static Dictionary<string, PlayerDataStruct> playerData = new Dictionary<string, PlayerDataStruct>();
        // private static uint MaxKillCount => killCount.Values.
        #region Kill Counmts
        /// <summary>
        /// Adds a kill to the players score. creates a new entry if the player does not have one.
        /// </summary>
        /// <param name="player"></param>
        public static void AddKill(Player player)
        {
            if (killCount.ContainsKey(player))
            {
                killCount[player].AddKill(); // to prevent the actual value being changed
            }
            else
            {
                killCount.Add(player, new KCPlayerData(player));
            }
        }
        public static int GetKillCount(Player player)
        {
            if (killCount.ContainsKey(player))
            {
                return killCount[player].GetKillCount();
            }
            else
            {
                return 0;
            }
        }

        public static bool HasKillCount(Player player)
        {
            return killCount.ContainsKey(player);
        }
        public static List<Player> GetHighestKillCount()
        {
            List<Player> playersWithMaxKills = new List<Player>();

// Find the maximum kill count
            int maxKillCount = killCount.Values.Max(data => data.GetKillCount());

// Find players with the maximum kill count
            foreach (KeyValuePair<Player, KCPlayerData> pair in killCount)
            {
                if (pair.Value.GetKillCount() == maxKillCount)
                {
                    playersWithMaxKills.Add(pair.Key);
                }
            }

            return playersWithMaxKills;
        }
        #endregion
        #region Non Natural Spawns
        public static void AddNonNaturalSpawn(Player player)
        {
            nonNaturallSpawn.Add(player);
        }
        public static Player TryGetNonNaturalSpawn(Player player)
        {
            if (nonNaturallSpawn.Contains(player))
            {
                return player;
            }
            else
            {
                return null;
            }
        }
        public static bool DoesPlayerHaveNonNaturallSpawn(Player player)
        {
            return nonNaturallSpawn.Contains(player);
        }
        public static void TryRemoveNonNaturalSpawn(Player player)
        {
            if (nonNaturallSpawn.Contains(player))
            {
                nonNaturallSpawn.Remove(player);
            }
        }
        public static void RemoveNonNaturalSpawn(Player player)
        {
            if (nonNaturallSpawn.Contains(player))
            {
                nonNaturallSpawn.Remove(player);
            }
        }
        #endregion
        #region Cache
        public static List<PlayerDataStruct> GetPlayerData()
        {
            SaveData();
            List<PlayerDataStruct> pd = new List<PlayerDataStruct>();
            foreach (KeyValuePair<string,PlayerDataStruct> pair in playerData)
            {
                pd.Add(new PlayerDataStruct(pair.Value.PlayerId, pair.Value.KillCount, Player.Get(pair.Value.PlayerId).DisplayNickname));
            }
            return pd;
        }

        public static Task LoadCacheFromFile()
        {
            playerData = FileWorker.LoadData();
            return Task.CompletedTask;
        }
        public static void SaveData()
        {
            foreach (KeyValuePair<Player, KCPlayerData> killCountData in killCount)
            {
                string playerId = killCountData.Value.GetPlayerId();
                int killCountValue = killCountData.Value.GetKillCount();
                string name = killCountData.Value.GetName();

                // Use TryGetValue for efficiency:
                if (playerData.TryGetValue(playerId, out PlayerDataStruct playerDataEntry))
                {
                    playerData[playerId] = new PlayerDataStruct(playerId, playerDataEntry.KillCount + killCountValue, name);
                }
                else
                {
                    playerData.Add(playerId, new PlayerDataStruct(playerId, killCountValue, name));
                }
            }
        }
        public static void ClearCache()
        {
            nonNaturallSpawn.Clear();
            killCount.Clear();
        }
        #endregion
    }
}
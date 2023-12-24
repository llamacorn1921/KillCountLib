using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using KillCountLib.blocks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KillCountLib
{
    public static class FileWorker
    {
        
        public static void SaveData()
        {
            
            string file = KillCount.Instance.Config.DatabasePath;
            List<PlayerDataStruct> playerData = Cache.GetPlayerData();
            Log.Info(playerData.Count);
            ISerializer serial = new SerializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
            string data = serial.Serialize(playerData);
            File.WriteAllText(file, data);
        }

        public static Dictionary<string, PlayerDataStruct> LoadData()
        {
            string file = File.ReadAllText(KillCount.Instance.Config.DatabasePath);
            if (file.Length == 0) return new Dictionary<string, PlayerDataStruct>();
            IDeserializer serial = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            List<PlayerDataStruct> playerData = serial.Deserialize<List<PlayerDataStruct>>(file);
            Dictionary<string, PlayerDataStruct> data = new Dictionary<string, PlayerDataStruct>();
            foreach (PlayerDataStruct pd in playerData)
            {
                data.Add(pd.PlayerId, pd);
            }

            return data;
        }
    }
}
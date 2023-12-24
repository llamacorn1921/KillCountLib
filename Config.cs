using System.IO;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace KillCountLib
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        public string DatabasePath { get; set; } = Path.Combine(Paths.Configs, "KillCountLib", "KillCount.yaml");
    }
}
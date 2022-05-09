using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.IO;
using Newtonsoft.Json;
using ImGuiNET;
using MapStudio.UI;
using Toolbox.Core;

namespace SampleMapEditor
{
    /// <summary>
    /// Represents UI for the plugin which is currently showing in the Paths section of the main menu UI.
    /// This is used to configure game paths.
    /// </summary>
    public class PluginConfig : IPluginConfig
    {
        //Only load the config once when this constructor is activated.
        internal static bool init = false;

        public PluginConfig() { init = true; }

        /*[JsonProperty]
        public static string GamePath = "";*/

        [JsonProperty]
        public static string S2GamePath = "";

        [JsonProperty]
        public static string S2AocPath = "";

        [JsonProperty]
        public static string S2ModPath = "";

        /// <summary>
        /// Renders the current configuration UI.
        /// </summary>
        public void DrawUI()
        {
            if (ImguiCustomWidgets.PathSelector("Splatoon 2", ref S2GamePath))
            {
                Save();
            }
            if (ImguiCustomWidgets.PathSelector("Splatoon 2: Octo Expansion", ref S2AocPath))
            {
                Save();
            }
            if (ImguiCustomWidgets.PathSelector("Splatoon 2 Mod Path", ref S2ModPath))
            {
                Save();
            }
        }

        /// <summary>
        /// Loads the config json file on disc or creates a new one if it does not exist.
        /// </summary>
        /// <returns></returns>
        public static PluginConfig Load() {
            Console.WriteLine("Loading config...");
            if (!File.Exists($"{Runtime.ExecutableDir}\\SampleMapEditorConfig.json")) { new PluginConfig().Save(); }

            var config = JsonConvert.DeserializeObject<PluginConfig>(File.ReadAllText($"{Runtime.ExecutableDir}\\SampleMapEditorConfig.json"));
            config.Reload();
            return config;
        }

        /// <summary>
        /// Saves the current configuration to json on disc.
        /// </summary>
        public void Save() {
            Console.WriteLine("Saving config...");
            File.WriteAllText($"{Runtime.ExecutableDir}\\SampleMapEditorConfig.json", JsonConvert.SerializeObject(this));
            Reload();
        }

        /// <summary>
        /// Called when the config file has been loaded or saved.
        /// </summary>
        public void Reload()
        {
            GlobalSettings.GamePath = S2GamePath;
            GlobalSettings.AOCPath = S2AocPath;
            GlobalSettings.ModOutputPath = S2ModPath;
        }
    }
}

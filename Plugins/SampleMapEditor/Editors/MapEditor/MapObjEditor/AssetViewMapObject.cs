using ImGuiNET;
using MapStudio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor.LayoutEditor
{
    public class AssetViewMapObject : IAssetLoader
    {
        public virtual string Name => "Map Objects";     //=> MapStudio.UI.TranslationSource.GetText("Map Objects");

        public bool IsFilterMode => filterObjPath;

        public static bool filterObjPath = false;

        public virtual List<AssetItem> Reload()
        {
            List<AssetItem> assets = new List<AssetItem>();

            var actorList = GlobalSettings.ActorDatabase.Values.ToList();

            assets.Clear();
            foreach (var actor in actorList)
                AddAsset(assets, actor);

            return assets;
        }

        public void AddAsset(List<AssetItem> assets, ActorDefinition actor)
        {
            string resName = actor.ResName;
            
            /*string icon = "Node";
            if (IconManager.HasIcon($"{Runtime.ExecutableDir}/Lib/Images/MapObjects/{resName}.png"))
                icon = $"{Runtime.ExecutableDir}/Lib/Images/MapObjects/{resName}.png";*/

            var icon = IconManager.GetTextureIcon("Node");
            if (IconManager.HasIcon($"{Runtime.ExecutableDir}\\Lib\\Images\\MapObjects\\{resName}.png"))
                icon = IconManager.GetTextureIcon($"{Runtime.ExecutableDir}\\Lib\\Images\\MapObjects\\{resName}.png");

            assets.Add(new MapObjectAsset($"MapObject_{actor.Name}")
            {
                Name = actor.Name,
                ActorDefinition = actor,
                Icon = icon,
            });
        }

        public bool UpdateFilterList()
        {
            bool filterUpdate = false;
            if (ImGui.Checkbox("Filter Path Objects", ref filterObjPath))
                filterUpdate = true;

            return filterUpdate;
        }
    }




    public class MapObjectAsset : AssetItem
    {
        //public int ObjID { get; set; }
        //public string ObjName { get; set; }
        public ActorDefinition ActorDefinition { get; set; }

        /*public override bool Visible
        {
            get
            {
                if (AssetViewMapObject.filterObjPath && ActorDefinition.PathType == (PathType)0)
                    return false;

                return true;
            }
        }*/
        public override bool Visible
        {
            get
            {
                if (AssetViewMapObject.filterObjPath && ActorDefinition.FmdbName.Length == 0)
                    return false;

                return true;
            }
        }

        public override void DoubleClicked()
        {
            string filePath = Obj.FindFilePath(Obj.GetResourceName(Name));
            FileUtility.SelectFile(filePath);
        }

        public MapObjectAsset(string id) : base(id)
        {

        }
    }
}

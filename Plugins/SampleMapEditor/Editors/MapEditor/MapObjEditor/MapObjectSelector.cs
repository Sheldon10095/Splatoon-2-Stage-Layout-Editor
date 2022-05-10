using ImGuiNET;
using MapStudio.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor.LayoutEditor
{
    public class MapObjectSelector
    {
        // Object and actor are used interchangeably here
#warning Double check this file for oddities later! Something may have been missed.

        //public int GetSelectedID() => selectedObject;
        //public void SetSelectedID(int id) => selectedObject = id;

        public string GetSelectedID() => selectedObject;
        public void SetSelectedID(string id) => selectedObject = id;

        public bool CloseOnSelect = false;

        public EventHandler SelectionChanged;

        //private bool filterSkybox = false;

        /*private int _selectedObject = 0;
        private int selectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (_selectedObject != value)
                {
                    _selectedObject = value;
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }*/

        private string _selectedObject = "";
        private string selectedObject
        {
            get { return _selectedObject; }
            set
            {
                if (_selectedObject != value)
                {
                    _selectedObject = value;
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private string _searchText = "";
        private bool isSearch = false;
        private List<ActorDefinition> actorList; // objectList;
        private List<ActorDefinition> filteredActors; // filteredObjects;

        public MapObjectSelector(List<ActorDefinition> actors)
        {
            this.actorList = actors;
        }

        public void Render(bool isDialog = true)
        {
            //A search box for filtering objects
            RenderSearchBox();
            //Track the current placement
            var posY = ImGui.GetCursorPosY();
            //Filtered or full object list
            //var objects = (isSearch || filterSkybox) ? filteredObjects : objectList;
            var objects = isSearch ? filteredActors : actorList;

            var itemHeight = 40;
            var windowSize = ImGui.GetWindowSize();

            //Setup a child with the clip size calculations for clipping the list.
            ImGuiNative.igSetNextWindowContentSize(new System.Numerics.Vector2(0.0f, objects.Count * (itemHeight + 1)));
            ImGui.BeginChild("##object_list1", new Vector2(windowSize.X, windowSize.Y - posY - (isDialog ? 30 : 0)));
            //Draw object list
            RenderObjectList();

            ImGui.EndChild();

            //Setup cancel/ok buttons for dialog type.
            if (isDialog)
            {
                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - 202);

                bool cancel = ImGui.Button("Cancel", new Vector2(100, 23)); ImGui.SameLine();
                bool applied = ImGui.Button("Ok", new Vector2(100, 23)) && selectedObject.Length > 0; //selectedObject != 0;

                if (cancel)
                {
                    DialogHandler.ClosePopup(false);
                }
                if (applied)
                {
                    DialogHandler.ClosePopup(true);
                }
            }
        }

        public void RenderSearchBox()
        {
            bool filterUpdate = false;
            /*if (ImGui.Checkbox(TranslationSource.GetText("FILTER_SKYBOXES"), ref filterSkybox))
                filterUpdate = true;*/

            //Search bar
            {
                ImGui.AlignTextToFramePadding();
                ImGui.Text("Search");
                ImGui.SameLine();

                var posX = ImGui.GetCursorPosX();
                var width = ImGui.GetWindowWidth();

                //Span across entire outliner width
                ImGui.PushItemWidth(width - posX);
                if (ImGui.InputText("##search_box", ref _searchText, 200))
                {
                    isSearch = !string.IsNullOrWhiteSpace(_searchText);
                    filterUpdate = true;
                }
                ImGui.PopItemWidth();
            }

            if (filterUpdate)
                filteredActors = UpdateSearch(actorList);
        }

        public void RenderObjectList()
        {
            //var objects = (isSearch || filterSkybox) ? filteredObjects : objectList;
            var objects = isSearch ? filteredActors : actorList;
            var itemHeight = 40;

            var clipper = new ImGuiListClipper2(objects.Count, itemHeight);
            clipper.ItemsCount = objects.Count;

            //Setup list spacing
            var spacing = ImGui.GetStyle().ItemSpacing;
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(spacing.X, 0));

            //2 columns, one for name, another for ID
            ImGui.BeginColumns("##objListColumns", 1);  //  2);

            for (int line_i = clipper.DisplayStart; line_i < clipper.DisplayEnd; line_i++) // display only visible items
            {
                var mapObject = objects[line_i];
                //string resName = mapObject.ResNames.FirstOrDefault();
                string resName = mapObject.ResName;

                //Get the icon
                var icon = IconManager.GetTextureIcon("Node");
                if (IconManager.HasIcon($"{Runtime.ExecutableDir}\\Lib\\Images\\MapObjects\\{resName}.png"))
                    icon = IconManager.GetTextureIcon($"{Runtime.ExecutableDir}\\Lib\\Images\\MapObjects\\{resName}.png");

                //Load the icon onto the list
                ImGui.Image((IntPtr)icon, new Vector2(itemHeight, itemHeight)); ImGui.SameLine();
                ImGuiHelper.IncrementCursorPosX(3);

                Vector2 itemSize = new Vector2(ImGui.GetWindowWidth(), itemHeight);

                //Selection handling
                //bool isSelected = selectedObject == mapObject.ObjId;
                bool isSelected = selectedObject == mapObject.Name;
                ImGui.AlignTextToFramePadding();
                //bool select = ImGui.Selectable($"{mapObject.Label}##{mapObject.ObjId}", isSelected, ImGuiSelectableFlags.SpanAllColumns, itemSize);
                bool select = ImGui.Selectable($"{mapObject.Name}##", isSelected, ImGuiSelectableFlags.SpanAllColumns, itemSize);
                bool hovered = ImGui.IsItemHovered();
                ImGui.NextColumn();

                //Display object ID
                ImGui.AlignTextToFramePadding();
                //ImGui.Text($"{mapObject.Name}");
                ImGui.NextColumn();

                if (select)
                {
                    //Update selection
                    selectedObject = mapObject.Name;
                }
                if (CloseOnSelect && hovered && ImGui.IsMouseClicked(0))
                {
                    //Update selection
                    selectedObject = mapObject.Name;
                    DialogHandler.ClosePopup(true);
                }

                //if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left) && selectedObject != 0)
                if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left) && selectedObject.Length > 0)
                    DialogHandler.ClosePopup(true);
            }
            ImGui.EndColumns();

            ImGui.PopStyleVar();
        }

        private List<ActorDefinition> UpdateSearch(List<ActorDefinition> actors)
        {
            List<ActorDefinition> filtered = new List<ActorDefinition>();
            for (int i = 0; i < actors.Count; i++)
            {
                /*if (filterSkybox && !actors[i].VR)
                    continue;*/

                /*bool HasText = actors[i].Label != null &&
                     actors[i].Label.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0;*/
                //HasText |= actors[i].ObjId.ToString().IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                
                
                bool HasText = actors[i].Name != null &&
                    actors[i].Name.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                //HasText |= actors[i].Name.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0; // Not necessary ???

                if (isSearch && HasText || !isSearch)
                    filtered.Add(actors[i]);
            }
            return filtered;
        }
    }
}

using ImGuiNET;
using MapStudio.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;
using SampleMapEditor.LayoutEditor;
using Toolbox.Core.ViewModels;
using GLFrameworkEngine;

namespace SampleMapEditor
{
    public class StageLayoutPlugin : FileEditor, IFileFormat
    {
        //public string[] Description => new string[] { "Byaml" };
        //public string[] Extension => new string[] { "*.byaml" };
        public string[] Description => new string[] { "Stage Layout Data" };
        public string[] Extension => new string[] { "*.szs" };

        /// <summary>
        /// Wether or not the file can be saved.
        /// </summary>
        public bool CanSave { get; set; } = true;

        /// <summary>
        /// Information of the loaded file.
        /// </summary>
        public File_Info FileInfo { get; set; }

        public MapLoader MapLoader;

        /*public ILayoutEditor ActiveEditor { get; set; }
        public List<ILayoutEditor> Editors = new List<ILayoutEditor>();*/





        public override Action RenderNewFileDialog => () =>
        {
            //ImGui.Checkbox("Mario Kart 8 Deluxe Map", ref IsNewSwitch);

            //ImguiCustomWidgets.FileSelector("Model File", ref model_path, new string[] { ".dae", ".fbx" });
            ImGui.Text("Splatoon 2 Stage Layout");

            var size = new System.Numerics.Vector2(ImGui.GetWindowWidth() / 2, 23);
            if (ImGui.Button(TranslationSource.GetText("CANCEL"), size))
                DialogHandler.ClosePopup(false);

            ImGui.SameLine();
            if (ImGui.Button(TranslationSource.GetText("OK"), size))
                DialogHandler.ClosePopup(true);
        };


        /*public override void RenderSaveFileSettings()
        {
            //Console.WriteLine("~ Called StageLayoutPlugin.RenderSaveFileSettings(); ~");
        }*/


        public bool Identify(File_Info fileInfo, Stream stream)
        {
            //Just load maps from checking the byaml extension atm.
            //return fileInfo.Extension == ".byaml";
            return fileInfo.Extension == ".szs" && !fileInfo.FileName.Contains("Nin_NX_NVN");
        }



        private bool IsNewProject = false;



        /*FileEditorMode EditorMode = FileEditorMode.LayoutEditor;

        enum FileEditorMode
        {
            LayoutEditor,
            *//*MapEditor,
            ModelEditor,
            CollisionEditor,
            MinimapEditor,
            LightingEditor,*//*
        }*/



        public override bool CreateNew()
        {
            //bool isSwitch = IsNewSwitch;

            IsNewProject = true;

            MapLoader = new MapLoader(this);
            //MapLoader.Init(this, isSwitch, model_path);
            MapLoader.Init(this);

            //Root.Header = "course_muunt.byaml";
            Root.Header = "Fld_CustomStage01_Vss.byaml (HEADER)";
            Root.Tag = this;

            //FileInfo.FileName = "course_muunt.byaml";
            //FileInfo.FileName = "Fld_Test01_Vss.byaml";
            FileInfo.FileName = "Fld_CustomStage01_Vss.szs";
            FileInfo.Compression = new Yaz0();

            Setup(MapLoader);

            return true;
        }


        /*public void LoadProject()
        {
            var projectFile = Workspace.Resources.ProjectFile;

            foreach (var editor in this.Editors)
            {
                if (editor.Name == projectFile.ActiveEditor)
                    SubEditor = editor.Name;
            }
            if (!string.IsNullOrEmpty(projectFile.SelectedWorkspace))
            {
                if (Enum.TryParse(typeof(FileEditorMode), projectFile.SelectedWorkspace, out object? mode))
                {
                    this.EditorMode = (FileEditorMode)mode;
                    //ReloadEditorMode();
                }
            }
        }

        public void SaveProject(string folder)
        {
            Workspace.Resources.ProjectFile.ActiveEditor = this.ActiveEditor.Name;
            Workspace.Resources.ProjectFile.SelectedWorkspace = this.EditorMode.ToString();

            //SaveProjectFile(folder, MapLoader.BfresEditor);
            //SaveProjectFile(folder, MapLoader.CollisionFile);
            *//*    SaveProjectFile(folder, MapLoader.BgenvFile);
                if (MapLoader.MapCamera != null)
                    MapLoader.MapCamera.Save($"{folder}\\course_mapcamera.bin");
                if (MapLoader.MapTexture != null && MapLoader.MapTexture is BflimTexture)
                    ((BflimTexture)MapLoader.MapTexture).Save(File.OpenWrite($"{folder}\\course_maptexture.bflim"));*//*
        }


        private void SaveProjectFile(string folder, IFileFormat fileFormat)
        {
            if (fileFormat != null)
                fileFormat.Save(File.OpenWrite($"{folder}\\{fileFormat.FileInfo.FileName}"));
        }*/



        public void Load(Stream stream)
        {
            string workingDir = MapStudio.UI.Workspace.ActiveWorkspace.Resources.ProjectFile.WorkingDirectory;

            MapLoader = new MapLoader(this);
            MapLoader.Load(this, FileInfo.FolderPath, FileInfo.FilePath, workingDir);

            //Setup(MapLoader);
            //MapLoader.SetupLighting();
        }

        public void Save(Stream stream)
        {
            //foreach (var editor in this.Editors)
            //    editor.OnSave(Resources.CourseDefinition);

            //MapLoader.StageDefinition.Save(stream);

            //if (IsNewProject)
            Console.WriteLine($"~ Called StageLayoutPlugin.Save(stream); ~");
            MapLoader.stageDefinition.Save(stream);
        }


        private void DrawEditorList()
        {
            //
        }


        /*private void DrawEditorList()
        {
            ImGui.PushItemWidth(230);
            var flags = ImGuiComboFlags.HeightLargest;
            var pos = ImGui.GetCursorPos();
            if (ImGui.BeginCombo("##editorMenu", $"", flags))
            {
                foreach (var editor in this.Editors)
                {
                    ImGuiHelper.IncrementCursorPosX(3);

                    bool select = editor == ActiveEditor;

                    ImGui.PushStyleColor(ImGuiCol.Text, editor.Root.IconColor);
                    ImGui.Text($"   {editor.Root.Icon}   ");
                    ImGui.PopStyleColor();

                    ImGui.SameLine();

                    if (ImGui.Selectable($"{editor.Name}", select, ImGuiSelectableFlags.SpanAllColumns))
                    {
                        SubEditor = editor.Name;
                        GLContext.ActiveContext.UpdateViewport = true;
                    }
                    if (select)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }
            if (ImGui.IsItemHovered()) //Check for combo box hover
            {
                var delta = ImGui.GetIO().MouseWheel;
                if (delta < 0) //Check for mouse scroll change going up
                {
                    int index = this.Editors.IndexOf(ActiveEditor);
                    if (index < this.Editors.Count - 1)
                    { //Shift upwards if possible
                        SubEditor = this.Editors[index + 1].Name;
                        GLContext.ActiveContext.UpdateViewport = true;
                    }
                }
                if (delta > 0) //Check for mouse scroll change going down
                {
                    int index = this.Editors.IndexOf(ActiveEditor);
                    if (index > 0)
                    { //Shift downwards if possible
                        SubEditor = this.Editors[index - 1].Name;
                        GLContext.ActiveContext.UpdateViewport = true;
                    }
                }
            }

            ImGui.SetCursorPos(pos);

            ImGui.AlignTextToFramePadding();
            ImGui.Text(" Tool:");
            ImGui.SameLine();

            ImGui.PushStyleColor(ImGuiCol.Text, ActiveEditor.Root.IconColor);

            ImGui.AlignTextToFramePadding();
            ImGui.Text($"    {ActiveEditor.Root.Icon}   ");
            ImGui.PopStyleColor();

            ImGui.SameLine();

            ImGui.AlignTextToFramePadding();
            ImGui.Text(ActiveEditor.Name);

            ImGui.PopItemWidth();
        }*/







        public void Setup(MapLoader mapResources)
        {
            var stage = mapResources.stageDefinition;

            Root.ContextMenus.Clear();
            Root.ContextMenus.Add(new MenuItemModel("MENU ITEM MODEL TEST THING ~ SETUP"));


            // ~


            Workspace.OnProjectLoad += delegate
            {
                //LoadProject();
            };
            Workspace.OnProjectSave += delegate
            {
                //SaveProject(Workspace.Resources.ProjectFolder);
            };
            Workspace.Outliner.SelectionChanged += (o, e) =>
            {
                /*var node = o as NodeBase;
                if (node == null || node.Parent == null || !node.IsSelected)
                    return;

                foreach (var editor in this.Editors)
                {
                    if (editor.Root == node.Parent || editor.Root == node.Parent.Parent)
                        UpdateMuuntEditor(editor, false);
                }*/
            };
            Workspace.ViewportWindow.DrawEditorDropdown += delegate
            {
                DrawEditorList();
            };

            _camera = GLContext.ActiveContext.Camera;


            Root.TagUI.Tag = mapResources.stageDefinition;
            GLContext.ActiveContext.Scene = Scene;

            //TurboSystem.Instance = new TurboSystem();
            //TurboSystem.Instance.MapFieldAccessor.Setup(MapLoader.CourseDefinition);

            /*CafeLibrary.Rendering.BfresMeshRender.DisplaySubMeshFunc = (o) =>
            {
                var bounding = o as BoundingNode;
                if (ClipSubMeshCulling.IsInside(bounding.Box))
                    return false;

                return true;
            };*/


            //A little hack atm. Make the model pickable during XRay
            //So the user cannot select things like lap paths through the map model.
            //But always allow selection to go through during xray mode
            /*MapLoader.BfresEditor.Renderer.EnablePicking = () =>          // Likely not needed here
            {
                if (ActiveEditor is CubePathEditor<LapPath, LapPathPoint>)
                    return !((CubePathEditor<LapPath, LapPathPoint>)ActiveEditor).IsXRAY;

                return true;
            };*/



            //Resources = mapResources;     // ???

            //Make sure the collision render isn't null
            /*if (MapLoader.CollisionFile.CollisionRender == null)
                MapLoader.CollisionFile.CreateNew();*/


            //Load a custom map object category for the asset handler.
            Workspace.AddAssetCategory(new AssetViewMapObject());
            //Workspace.AddAssetCategory(new AssetViewMapObjectVR());
        }




        private Camera _camera;
    }
}

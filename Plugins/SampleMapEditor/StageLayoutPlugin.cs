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
using OpenTK;

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

        public ILayoutEditor ActiveEditor { get; set; }
        public List<ILayoutEditor> Editors = new List<ILayoutEditor>();


        public MapLoader Resources;


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

        public bool FilterEditorsInOutliner = false;

        FileEditorMode EditorMode = FileEditorMode.LayoutEditor;

        enum FileEditorMode
        {
            LayoutEditor,
            //MapEditor,
            //ModelEditor,
            //CollisionEditor,
            //MinimapEditor,
            //LightingEditor,
        }



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

            Setup(MapLoader);
            //MapLoader.SetupLighting();
        }

        public void Save(Stream stream)
        {
            foreach (var editor in this.Editors)
            {
                //editor.OnSave(Resources.CourseDefinition);
                editor.OnSave(Resources.stageDefinition);
            }

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
                var node = o as NodeBase;
                if (node == null || node.Parent == null || !node.IsSelected)
                    return;
                
                foreach (var editor in this.Editors)
                {
                    if (editor.Root == node.Parent || editor.Root == node.Parent.Parent)
                        UpdateLayoutEditor(editor, false); //UpdateMuuntEditor(editor, false);
                }
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



            Resources = mapResources;     // ???

            //Make sure the collision render isn't null
            /*if (MapLoader.CollisionFile.CollisionRender == null)
                MapLoader.CollisionFile.CreateNew();*/


            //Load a custom map object category for the asset handler.
            Workspace.AddAssetCategory(new AssetViewMapObject());
            //Workspace.AddAssetCategory(new AssetViewMapObjectVR());





            // Load the editor(s)
            var colorSettings = GlobalSettings.PathDrawer;

            // Section List

            // Stage Info

            // Objs / Rails (?)
            Editors.Add(new ObjectEditor(this, stage.Objs));
            Editors.Add(new RailEditor(this, colorSettings.RailColor, stage.Rails));







            foreach (var editor in Editors)
                editor.MapEditor = this;
            
            if (Workspace.Resources.ProjectFile.ActiveWorkspaces.Count > 0)
            {
                foreach (var ed in Editors)
                    ed.IsActive = Workspace.Resources.ProjectFile.ActiveWorkspaces.Contains(ed.Name);
            }
            else
            {
                foreach (var ed in Editors)
                    ed.IsActive = false;

                Editors[0].IsActive = true;
            }


            NodeBase pathFolder = new NodeBase("Paths");    // ???
            NodeBase objFolder = new NodeBase("Objects");
            
            Root.Children.Clear();

            foreach (var editor in Editors)
            {
                if (editor is ObjectEditor) // || editor is SoundObjEditor || editor is RouteChangeEditor)
                    objFolder.AddChild(editor.Root);
                else
                    pathFolder.AddChild(editor.Root);
            }

            Root.AddChild(objFolder);
            Root.AddChild(pathFolder);


            // ~~~


            foreach (var ed in Editors)
            {
                ed.Root.OnSelected += delegate
                {
                    if (ActiveEditor == ed)
                        return;

                    ActiveEditor = ed;
                    UpdateLayoutEditor(ActiveEditor, false);    //UpdateMuuntEditor(ActiveEditor, false);
                };
                ed.Root.OnChecked += delegate
                {
                    foreach (var render in ed.Renderers)
                        render.IsVisible = ed.Root.IsChecked;
                };
            }


            //Set the active editor as the map object one.
            ActiveEditor = Editors.FirstOrDefault();
            UpdateLayoutEditor(ActiveEditor);

            Workspace.WorkspaceTools.Add(new MenuItemModel(
               $"   {'\uf279'}    Map Editor", () =>
               {
                   EditorMode = FileEditorMode.LayoutEditor;
                   ReloadEditorMode();
               }));
        }

        public override void DrawViewportMenuBar()
        {
            ActiveEditor.DrawEditMenuBar();
        }

        public override void DrawHelpWindow()
        {
            base.DrawHelpWindow();

            if (ImGuiNET.ImGui.CollapsingHeader("Editors", ImGuiNET.ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGuiHelper.BoldTextLabel("Alt + Click", "Add.");
                ImGuiHelper.BoldTextLabel("Del", "Delete.");
                ImGuiHelper.BoldTextLabel("Hold Ctrl", "Multi select.");
            }

            ActiveEditor?.DrawHelpWindow();
        }


        private Camera _camera;



        public override void AfterLoaded()
        {
            /*if (this.IsNewProject)
            {
                this.EditorMode = FileEditorMode.ModelEditor;
                ReloadEditorMode();
                Workspace.ActiveWorkspaceTool = Workspace.WorkspaceTools[1];
            }*/
        }


        private void ReloadEditorMode()
        {
            GLContext.ActiveContext.Camera = _camera;

            Workspace.Outliner.DeselectAll();
            Workspace.Outliner.Nodes.Clear();


            if (this.EditorMode == FileEditorMode.LayoutEditor)
            {
                Workspace.ActiveEditor = this;
                Workspace.SetupActiveEditor(this);
                Workspace.ReloadEditors();
            }

            GLContext.ActiveContext.UpdateViewport = true;
        }


        /*public override List<MenuItemModel> GetViewportMenuIcons()
        {
            List<MenuItemModel> menus = new List<MenuItemModel>();

            if (LightingEditorWindow != null)
            {
                menus.Add(new MenuItemModel($" {IconManager.LIGHT_ICON} ", () =>
                {
                    LightingEditorWindow.Opened = !LightingEditorWindow.Opened;
                    Workspace.ActiveWorkspace.ReloadViewportMenu();

                }, "LIGHTING_WINDOW", LightingEditorWindow.Opened));
            }

            return menus;
        }*/

        /*public override List<MenuItemModel> GetViewMenuItems()
        {
            List<MenuItemModel> menus = new List<MenuItemModel>();

            if (MapLoader.BfresEditor != null)
                menus.AddRange(MapLoader.BfresEditor.GetViewMenuItems());

            menus.Add(new MenuItemModel($"      {IconManager.MESH_ICON}      Show Collision", () =>
            {
                CollisionRender.Overlay = false;
                CollisionRender.DisplayCollision = !CollisionRender.DisplayCollision;
                if (MapLoader.BfresEditor != null)
                    MapLoader.BfresEditor.Renderer.IsVisible = !CollisionRender.DisplayCollision;

                Workspace.ActiveWorkspace.ReloadViewportMenu();
                GLContext.ActiveContext.UpdateViewport = true;
            }, "DISPLAY_COLLISION", CollisionRender.DisplayCollision));

            menus.Add(new MenuItemModel($"      {IconManager.MODEL_ICON}      Show Collision Overlay", () =>
            {
                CollisionRender.Overlay = !CollisionRender.Overlay;
                CollisionRender.DisplayCollision = false;
                if (MapLoader.BfresEditor != null)
                    MapLoader.BfresEditor.Renderer.IsVisible = true;

                Workspace.ActiveWorkspace.ReloadViewportMenu();
                GLContext.ActiveContext.UpdateViewport = true;
            }, "COLLISION_OVERLAY", CollisionRender.Overlay));
            return menus;
        }*/


        public override List<MenuItemModel> GetFilterMenuItems()
        {
            List<MenuItemModel> items = new List<MenuItemModel>();
            items.Add(new MenuItemModel("Filter Nodes By Editor", (sender, e)=>
            {
                FilterEditorsInOutliner = ((MenuItemModel)sender).IsChecked;
                ReloadOutliner(false);
            }, "", FilterEditorsInOutliner)
            { CanCheck = true });
            return items;
        }


        /*public void AutoGenerateCollision()
        {
            var models = MapLoader.BfresEditor.ModelFolder.Models.FirstOrDefault();
            if (models == null)
            {
                TinyFileDialog.MessageBoxInfoOk("No models found in scene!");
                return;
            }

            //Turn into an exportable scene for collision conversion
            var scene = BfresModelExporter.FromGeneric(models.ResFile, models.Model);
            MapLoader.CollisionFile.ImportCollision(scene);
        }*/


        /*public override List<MenuItemModel> GetEditMenuItems()
        {
            var items = new List<MenuItemModel>();
            //Import menus for collision
            items.AddRange(MapLoader.CollisionFile.GetEditMenuItems());
            items.Add(new MenuItemModel("        Auto Generate Collision From Bfres", AutoGenerateCollision));
            return items;
        }*/

        Vector3 previousPosition = Vector3.Zero;

        public override void DrawToolWindow()
        {
            /*if (ImGuiNET.ImGui.CollapsingHeader("Transform Scene"))
            {
                var transform = MapLoader.BfresEditor.Renderer.Transform;
                if (ImGuiHelper.InputTKVector3("Position", transform, "Position"))
                {
                    transform.UpdateMatrix(true);

                    if (previousPosition == Vector3.Zero)
                        previousPosition = transform.Position;

                    Vector3 positionDelta = transform.Position - previousPosition;

                    previousPosition = transform.Position;

                    MapLoader.CollisionFile.CollisionRender.Transform.TransformMatrix = transform.TransformMatrix;
                    MapLoader.CollisionFile.UpdateTransformedVertices = true;

                    //Transform each byaml object
                    foreach (var editor in this.Editors)
                    {
                        foreach (ITransformableObject render in editor.Renderers)
                        {
                            if (render is RenderablePath)
                            {
                                ((RenderablePath)render).TranslateDelta(positionDelta);
                            }
                            else
                            {
                                render.Transform.Position += positionDelta;
                                render.Transform.UpdateMatrix(true);
                            }
                        }
                    }
                }
            }*/

            if (ActiveEditor != null)
                ActiveEditor.ToolWindowDrawer?.Render();
        }

        private void UpdateLayoutEditor(ILayoutEditor editor, bool filterVisuals = true)
        {
            //Enable/Disable "Active" editors which determine to use shorts ie alt mouse click to spawn
            foreach (var ed in Editors)
            {
                ed.IsActive = ed == editor;
                foreach (var render in ed.Renderers)
                {
                    if (render is RenderablePath)
                        ((RenderablePath)render).IsActive = ed.IsActive;
                }
            }
            ActiveEditor = editor;

            Workspace.ActiveWorkspace.ReloadEditors();
            ReloadOutliner(filterVisuals);
        }


        public void ReloadOutliner(bool filterVisuals)
        {
            //   if (FilterEditorsInOutliner)
            //   Root.Children.Clear();

            if (filterVisuals)
            {
                //   Workspace.Outliner.DeselectAll();
                Scene.DeselectAll(GLContext.ActiveContext);

                foreach (var editor in Editors)
                    HideRenders(editor);
            }
            foreach (var editor in Editors)
            {
                if (editor.IsActive)
                {
                    editor.ReloadEditor();
                    //  if (FilterEditorsInOutliner)
                    //    Root.AddChild(editor.Root);
                }

                if (filterVisuals)
                    editor.Root.IsChecked = editor.IsActive;
                if (filterVisuals && editor.IsActive)
                {
                    foreach (var render in editor.Renderers)
                        render.IsVisible = true;
                }
            }

            GLContext.ActiveContext.UpdateViewport = true;
        }



        public override List<UIFramework.DockWindow> PrepareDocks()
        {
            List<UIFramework.DockWindow> windows = new List<UIFramework.DockWindow>();
            windows.Add(Workspace.Outliner);
            /*if (LightingEditorWindow != null)
                windows.Add(LightingEditorWindow);*/
            windows.Add(Workspace.PropertyWindow);
            windows.Add(Workspace.ConsoleWindow);
            windows.Add(Workspace.AssetViewWindow);
            windows.Add(Workspace.HelpWindow);
            windows.Add(Workspace.ToolWindow);
            windows.Add(Workspace.ViewportWindow);
            return windows;
        }

        /*public override bool OnFileDrop(string filePath)
        {
            if (filePath.EndsWith(".kcl"))
            {
                MapLoader.LoadColllsion(filePath);
                return true;
            }
            if (MapLoader.CollisionFile.OnFileDrop(filePath))
            {
                return true;
            }
            return false;
        }*/

        public override void AssetViewportDrop(AssetItem item, Vector2 screenCoords)
        {
            var asset = item as MapObjectAsset;
            if (asset == null)
                return;

            //((ObjectEditor)Editors[0]).OnAssetViewportDrop(asset.ObjID, screenCoords);
            ((ObjectEditor)Editors[0]).OnAssetViewportDrop(asset.Name, screenCoords);
        }

        private void HideRenders(ILayoutEditor editor)
        {
            foreach (var render in editor.Renderers)
            {
                render.IsVisible = false;
                if (render is IEditModeObject)
                {
                    Scene.DisableEditMode((ITransformableObject)render);
                    foreach (var part in ((IEditModeObject)render).Selectables)
                        part.CanSelect = false;
                }
                else
                    ((ITransformableObject)render).CanSelect = false;
            }
        }

        public override void OnMouseMove(MouseEventInfo mouseInfo)
        {
            foreach (var editor in this.Editors.Where(x => x.IsActive))
                editor.OnMouseMove(mouseInfo);
        }

        public override void OnMouseDown(MouseEventInfo mouseInfo)
        {
            foreach (var editor in this.Editors.Where(x => x.IsActive))
                editor.OnMouseDown(mouseInfo);
        }

        public override void OnMouseUp(MouseEventInfo mouseInfo)
        {
            foreach (var editor in this.Editors.Where(x => x.IsActive))
                editor.OnMouseUp(mouseInfo);
        }

        public override void OnKeyDown(KeyEventInfo keyInfo)
        {
            foreach (var editor in this.Editors.Where(x => x.IsActive))
                editor.OnKeyDown(keyInfo);
        }
    }
}

using GLFrameworkEngine;
using GLFrameworkEngine.UI;
using ImGuiNET;
using MapStudio.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;
using Toolbox.Core.ViewModels;

namespace SampleMapEditor.LayoutEditor
{
    public class ObjectEditor : ILayoutEditor, UIEditToolMenu
    {
        public string Name => "Object Editor";

        public StageLayoutPlugin MapEditor { get; set; }

        public bool IsActive { get; set; } = false;

        public IToolWindowDrawer ToolWindowDrawer => new MapObjectToolMenu(this);

        public List<IDrawable> Renderers { get; set; } = new List<IDrawable>();

        //public NodeBase Root { get; set; } = new NodeBase(TranslationSource.GetText("MAP_OBJECTS")) { HasCheckBox = true };
        public NodeBase Root { get; set; } = new NodeBase("Map Objects") { HasCheckBox = true };

        public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();

        //int SpawnObjectID = 1018;
        string SpawnObjectName = "RespawnPos";

        public List<NodeBase> GetSelected()
        {
            return Root.Children.Where(x => x.IsSelected).ToList();
        }

        static bool initIcons = false;
        //Loads the icons for map objects (once on init)
        static void InitIcons()
        {
            if (initIcons)
                return;

            initIcons = true;

            //Load icons for map objects
            string folder = System.IO.Path.Combine(Runtime.ExecutableDir, "Lib", "Images", "MapObjects");
            if (Directory.Exists(folder))
            {
                foreach (var imageFile in Directory.GetFiles(folder))
                {
                    IconManager.LoadTextureFile(imageFile, 32, 32);
                }
            }
        }


        public ObjectEditor(StageLayoutPlugin editor, List<Obj> objs)
        {
            MapEditor = editor;
            InitIcons();

            Root.Icon = MapEditorIcons.OBJECT_ICON.ToString();

            Init(objs);

            GlobalSettings.LoadDataBase();

            var addMenu = new MenuItemModel("ADD_OBJECT", AddObjectMenuAction);
            var commonItemsMenu = new MenuItemModel("OBJECTS");
            commonItemsMenu.MenuItems.Add(new MenuItemModel("SPAWNPOINT", () => AddObject("RespawnPos", true)));

            GLContext.ActiveContext.Scene.MenuItemsAdd.Add(addMenu);
            GLContext.ActiveContext.Scene.MenuItemsAdd.Add(commonItemsMenu);

            MenuItems.AddRange(GetEditMenuItems());
        }

        public List<MenuItemModel> GetToolMenuItems()
        {
            List<MenuItemModel> items = new List<MenuItemModel>();
            return items;
        }

        MapObjectSelector ObjectSelector;

        public void DrawEditMenuBar()
        {
            DrawObjectSpawner();

            if (ImguiCustomWidgets.MenuItemTooltip($"   {IconManager.ADD_ICON}   ", "ADD", InputSettings.INPUT.Scene.Create))
            {
                AddObjectMenuAction();
            }
            if (ImguiCustomWidgets.MenuItemTooltip($"   {IconManager.DELETE_ICON}   ", "REMOVE", InputSettings.INPUT.Scene.Delete))
            {
                MapEditor.Scene.BeginUndoCollection();
                RemoveSelected();
                MapEditor.Scene.EndUndoCollection();
            }
            if (ImguiCustomWidgets.MenuItemTooltip($"   {IconManager.COPY_ICON}   ", "COPY", InputSettings.INPUT.Scene.Copy))
            {
                CopySelected();
            }
            if (ImguiCustomWidgets.MenuItemTooltip($"   {IconManager.PASTE_ICON}   ", "PASTE", InputSettings.INPUT.Scene.Paste))
            {
                PasteSelected();
            }
        }

        public void DrawHelpWindow()
        {
            if (ImGuiNET.ImGui.CollapsingHeader("Objects", ImGuiNET.ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGuiHelper.BoldTextLabel(InputSettings.INPUT.Scene.Create, "Create Object.");
            }
        }



        private void DrawObjectSpawner()
        {
            //Selector popup window instance
            if (ObjectSelector == null)
            {
                var objects = GlobalSettings.ActorDatabase.Values.OrderBy(x => x.Name).ToList();
                ObjectSelector = new MapObjectSelector(objects);
                ObjectSelector.CloseOnSelect = true;
                //Update current spawn id when selection is changed // ??? ~~~ Remove comment ~~~
                // Update current spawn name when selection is changed
                ObjectSelector.SelectionChanged += delegate
                {
                    SpawnObjectName = ObjectSelector.GetSelectedID();
                };
            }
            // Current spawnable
            string resName = Obj.GetResourceName(SpawnObjectName);
            var pos = ImGui.GetCursorScreenPos();

            //Make the window cover part of the viewport
            var viewportHeight = GLContext.ActiveContext.Height;
            var spawnPopupHeight = viewportHeight;

            ImGui.SetNextWindowPos(new System.Numerics.Vector2(pos.X, pos.Y + 27));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, spawnPopupHeight));

            //Render popup window when opened
            var flags = ImGuiWindowFlags.NoScrollbar;
            if (ImGui.BeginPopup("spawnPopup", ImGuiWindowFlags.Popup | flags))
            {

                if (ImGui.BeginChild("spawnableChild", new System.Numerics.Vector2(300, spawnPopupHeight), false, flags))
                {
                    ObjectSelector.Render(false);
                }
                ImGui.EndChild();
                ImGui.EndPopup();
            }

            //Menu to open popup
            ImGui.PushItemWidth(150);
            if (ImGui.BeginCombo("##spawnableCB", resName))
            {
                ImGui.EndCombo();
            }
            if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(0))
            {
                if (ImGui.IsPopupOpen("spawnPopup"))
                    ImGui.CloseCurrentPopup();
                else
                {
                    ImGui.OpenPopup("spawnPopup");
                    ObjectSelector.SetSelectedID(SpawnObjectName);
                }
            }
            ImGui.PopItemWidth();
        }


        public List<MenuItemModel> GetEditMenuItems()
        {
            List<MenuItemModel> items = new List<MenuItemModel>();
            items.Add(new MenuItemModel($"   {IconManager.ADD_ICON}   ", AddObjectMenuAction));

            bool hasSelection = GetSelected().Count > 0;

            items.Add(new MenuItemModel($"   {IconManager.COPY_ICON}   ", CopySelected) { IsEnabled = hasSelection, ToolTip = $"Copy ({InputSettings.INPUT.Scene.Copy})" });
            items.Add(new MenuItemModel($"   {IconManager.PASTE_ICON}   ", PasteSelected) { IsEnabled = hasSelection, ToolTip = $"Paste ({InputSettings.INPUT.Scene.Paste})" });
            items.Add(new MenuItemModel($"   {IconManager.DELETE_ICON}   ", () =>
            {
                GLContext.ActiveContext.Scene.DeleteSelected();
            })
            { IsEnabled = hasSelection, ToolTip = $" Delete ({InputSettings.INPUT.Scene.Delete})" });

            return items;
        }


        public void ReloadEditor()
        {
            Root.Header = TranslationSource.GetText("MAP_OBJECTS");

            foreach (EditableObject render in Renderers)
            {
                UpdateObjectLinks(render);

                render.CanSelect = true;

                /*if (((Obj)render.UINode.Tag).IsSkybox)
                    render.CanSelect = false;*/
            }
        }

        void Init(List<Obj> objs)
        {
            Root.Children.Clear();
            Renderers.Clear();

            //Load the current tree list
            for (int i = 0; i < objs?.Count; i++)
                Add(Create(objs[i]));

            if (Root.Children.Any(x => x.IsSelected))
                Root.IsExpanded = true;
        }


        public void OnSave(StageDefinition stage)
        {
            stage.Objs = new List<Obj>();

            foreach (EditableObject render in Renderers)
            {
                var obj = (Obj)render.UINode.Tag;
                obj.Translate = new ByamlVector3F(
                    render.Transform.Position.X,
                    render.Transform.Position.Y,
                    render.Transform.Position.Z);
                obj.Rotate = new ByamlVector3F(
                    render.Transform.RotationEuler.X,
                    render.Transform.RotationEuler.Y,
                    render.Transform.RotationEuler.Z);
                obj.Scale = new ByamlVector3F(
                    render.Transform.Scale.X,
                    render.Transform.Scale.Y,
                    render.Transform.Scale.Z);
                stage.Objs.Add(obj);
            }
        }



        public void OnMouseDown(MouseEventInfo mouseInfo)
        {
            bool isActive = Workspace.ActiveWorkspace.ActiveEditor.SubEditor == this.Name;

            if (isActive && KeyEventInfo.State.KeyAlt && mouseInfo.LeftButton == OpenTK.Input.ButtonState.Pressed)
                AddObject(SpawnObjectName);
        }
        public void OnMouseUp(MouseEventInfo mouseInfo)
        {
        }
        public void OnMouseMove(MouseEventInfo mouseInfo)
        {
        }

        public void Add(EditableObject render, bool undo = false)
        {
            MapEditor.AddRender(render, undo);
        }

        public void Remove(EditableObject render, bool undo = false)
        {
            MapEditor.RemoveRender(render, undo);
        }



        /// <summary>
        /// When an object asset is drag and dropped into the viewport.
        /// </summary>
        //public void OnAssetViewportDrop(int id, Vector2 screenPosition)
        public void OnAssetViewportDrop(string actorName, Vector2 screenPosition)
        {
            var context = GLContext.ActiveContext;

            Quaternion rotation = Quaternion.Identity;
            //Spawn by drag/drop coordinates in 3d space.
            Vector3 position = context.ScreenToWorld(screenPosition.X, screenPosition.Y, 100);
            //Face the camera
            if (MapStudio.UI.GlobalSettings.Current.Asset.FaceCameraAtSpawn)
                rotation = Quaternion.FromEulerAngles(0, -context.Camera.RotationY, 0);
            //Drop to collision if used.
            if (context.EnableDropToCollision)
            {
                Quaternion rotateByDrop = Quaternion.Identity;
                CollisionDetection.SetObjectToCollision(context, context.CollisionCaster, screenPosition, ref position, ref rotateByDrop);
                if (context.TransformTools.TransformSettings.RotateFromNormal)
                    rotation *= rotateByDrop;
            }

            // Add the object with the dropped name and set the transform 
            var render = AddObject(actorName);
            var obj = render.UINode.Tag as Obj;

            //Set the dropped place based on where the current mouse is.
            render.Transform.Position = position;
            render.Transform.Rotation = rotation;
            render.Transform.UpdateMatrix(true);
            render.UINode.IsSelected = true;

            this.MapEditor.Scene.SelectionUIChanged?.Invoke(render.UINode, EventArgs.Empty);

            //Update the SRT tool if active
            GLContext.ActiveContext.TransformTools.UpdateOrigin();

            //Force the editor to display
            if (!IsActive)
            {
                IsActive = true;
                ((StageLayoutPlugin)Workspace.ActiveWorkspace.ActiveEditor).ReloadOutliner(true);
            }
        }


        public void OnKeyDown(KeyEventInfo keyInfo)
        {
            bool isActive = Workspace.ActiveWorkspace.ActiveEditor.SubEditor == this.Name;

            if (isActive && !keyInfo.KeyCtrl && keyInfo.IsKeyDown(InputSettings.INPUT.Scene.Create))
                AddObject(SpawnObjectName);
            if (keyInfo.IsKeyDown(InputSettings.INPUT.Scene.Copy) && GetSelected().Count > 0)
                CopySelected();
            if (keyInfo.IsKeyDown(InputSettings.INPUT.Scene.Paste))
                PasteSelected();
            if (keyInfo.IsKeyDown(InputSettings.INPUT.Scene.Dupe))
            {
                CopySelected();
                PasteSelected();
                copied.Clear();
            }
        }


        public void ExportModel()
        {
            Console.WriteLine("~ Called ObjectEditor.ExportModel() ~ [Commented Out]");
            /*ImguiFileDialog dlg = new ImguiFileDialog();
            dlg.SaveDialog = true;
            dlg.AddFilter(".dae", ".dae");
            if (dlg.ShowDialog())
                ExportModel(dlg.FilePath);*/
        }


        public void ExportModel(string filePath)
        {
            var scene = new IONET.Core.IOScene();
            foreach (ITransformableObject render in this.Renderers.Where(x => ((ITransformableObject)x).IsSelected))
            {
                var obj = ((IRenderNode)render).UINode.Tag as Obj;
                var modelFilePath = Obj.FindFilePath(Obj.GetResourceName(obj.ObjId));
                if (!File.Exists(modelFilePath))
                    continue;

                var resFile = new BfresLibrary.ResFile(modelFilePath);
                var model = CafeLibrary.ModelConversion.BfresModelExporter.FromGeneric(resFile, resFile.Models[0]);
                foreach (var mesh in model.Models[0].Meshes)
                    mesh.Transform = Matrix4Extension.ToNumerics(render.Transform.TransformMatrix);

                scene.Models.AddRange(model.Models);
                scene.Materials.AddRange(model.Materials);

                string folder = System.IO.Path.GetDirectoryName(filePath);
                foreach (var tex in resFile.Textures.Values)
                {
                    if (tex is BfresLibrary.WiiU.Texture)
                    {
                        var ftex = new FtexTexture(resFile, (BfresLibrary.WiiU.Texture)tex);
                        ftex.Export(System.IO.Path.Combine(folder, $"{tex.Name}.png"), new TextureExportSettings());
                    }
                }
            }
            IONET.IOManager.ExportScene(scene, filePath);
        }


    }

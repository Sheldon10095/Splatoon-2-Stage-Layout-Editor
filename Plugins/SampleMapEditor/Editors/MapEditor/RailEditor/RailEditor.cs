using GLFrameworkEngine;
using GLFrameworkEngine.UI;
using ImGuiNET;
using MapStudio.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core.ViewModels;

namespace SampleMapEditor.LayoutEditor
{
    public class RailEditor : ILayoutEditor//, UIEditToolMenu
    {
        public string Name => "Rail Editor";
        public StageLayoutPlugin MapEditor { get; set; }

        private IToolWindowDrawer PathSettingsWindow;

        public IToolWindowDrawer ToolWindowDrawer => PathSettingsWindow;

        public List<IDrawable> Renderers { get; set; } = new List<IDrawable>();

        public NodeBase Root { get; set; }

        public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();

        public bool IsActive { get; set; }

        
        private GlobalSettings.PathColor Color;

        public RailEditor(StageLayoutPlugin editor, GlobalSettings.PathColor color, List<MuRail> rails)
        {
            MapEditor = editor;
            PathSettingsWindow = new RailPathToolSettings(this);
            Root = new NodeBase(Name) { HasCheckBox = true };
            MapEditorIcons.ReloadIcons(Root, typeof(MuRail));

            Color = color;
            ReloadPaths(rails);

            var addMenu = new MenuItemModel(this.Name);
            GLContext.ActiveContext.Scene.MenuItemsAdd.Add(addMenu);

            addMenu.MenuItems.Add(new MenuItemModel("ADD_NORMAL", CreateLinearPath));
            addMenu.MenuItems.Add(new MenuItemModel("ADD_BEZIER", CreateBezierPathStandard));
            addMenu.MenuItems.Add(new MenuItemModel("ADD_CIRCLE", CreateBezierPathCircle));

            MenuItems = new List<MenuItemModel>();
            MenuItems.AddRange(addMenu.MenuItems);
        }




        public void DrawEditMenuBar()       // DIRECT COPY AND PASTE // MAY NEED LOOKED OVER
        {
            foreach (var item in MenuItems)
                ImGuiHelper.LoadMenuItem(item);

            var selected = (RenderablePath)this.Renderers.FirstOrDefault(x =>
                ((RenderablePath)x).IsSelected ||
                ((RenderablePath)x).EditMode);

            if (selected != null)
            {
                if (ImGui.Button(selected.EditMode ? "Object Mode" : "Edit Mode"))
                {
                    GLContext.ActiveContext.Scene.ToggleEditMode();
                }
            }

            if (selected != null && selected.EditMode)
            {
                Workspace.ActiveWorkspace.ViewportWindow.DrawPathDropdown();
            }
        }


        public void DrawHelpWindow()
        {
            if (ImGuiNET.ImGui.CollapsingHeader("Paths", ImGuiNET.ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGuiHelper.BoldTextLabel(InputSettings.INPUT.Scene.EditMode, "Edit Selected Path.");
                ImGuiHelper.BoldTextLabel(InputSettings.INPUT.Scene.Extrude, "Extrude Point.");
            }
        }



        public void ReloadPath(MuRail rail)
        {
            CreatePathObject(rail);
        }


        //public void ReloadPaths(IEnumerable<PathBase<TPath, TPoint>> groups)
        public void ReloadPaths(List<MuRail> rails)
        {
            if (rails != null)
            {
                Root.Children.Clear();
                foreach (var rail in rails)
                    CreatePathObject(rail);
            }
        }



        public void OnSave(StageDefinition stage)
        {
            stage.Rails = new List<MuRail>();

            foreach (var path in Root.Children)
            {
                var rail = path.Tag as MuRail;
                rail.RailPoints.Clear();    //rail.Points.Clear();

                foreach (RenderablePath.PointNode point in path.Children)
                    ApplyPoint(point.Point, rail, point.Tag as MuRailPoint);

                /*if (path.Tag is Path)
                    course.Paths.Add((Path)path.Tag);
                if (path.Tag is ObjPath)
                    course.ObjPaths.Add((ObjPath)path.Tag);
                if (path.Tag is JugemPath)
                    course.JugemPaths.Add((JugemPath)path.Tag);*/

                if (path.Tag is MuRail)
                    stage.Rails.Add((MuRail)path.Tag);
            }
        }


        private void ApplyPoint(RenderablePathPoint renderPoint, MuRail rail, MuRailPoint point)
        {
            rail.RailPoints.Add(point);

            point.Translate = new ByamlVector3F(
                renderPoint.Transform.Position.X,
                renderPoint.Transform.Position.Y,
                renderPoint.Transform.Position.Z);
            point.Rotate = new ByamlVector3F(
                renderPoint.Transform.Rotation.X,
                renderPoint.Transform.Rotation.Y,
                renderPoint.Transform.Rotation.Z);
            if (renderPoint.Transform.Scale != Vector3.One)
            {
                point.Scale = new ByamlVector3F(
                    renderPoint.Transform.Scale.X,
                    renderPoint.Transform.Scale.Y,
                    renderPoint.Transform.Scale.Z);
            }
            if (renderPoint.ParentPath.InterpolationMode == RenderablePath.Interpolation.Bezier)
            {
                var pathPoint = point as MuRailPoint;
                pathPoint.ControlPoints.Clear();
                pathPoint.ControlPoints.Add(new ByamlVector3F(
                    renderPoint.ControlPoint1.Transform.Position.X,
                    renderPoint.ControlPoint1.Transform.Position.Y,
                    renderPoint.ControlPoint1.Transform.Position.Z));
                pathPoint.ControlPoints.Add(new ByamlVector3F(
                    renderPoint.ControlPoint2.Transform.Position.X,
                    renderPoint.ControlPoint2.Transform.Position.Y,
                    renderPoint.ControlPoint2.Transform.Position.Z));
            }
        }



        private void CreatePathObject(MuRail rail)
        {
            RenderablePath renderer = null;
            //Reload existing rails
            foreach (RenderablePath render in Renderers)
            {
                if (render.UINode.Tag == rail)
                    renderer = render;
            }
            if (renderer == null)
            {
                renderer = new RenderablePath();
                this.Add(renderer);
            }

            //Reset points incase the path gets reloaded
            renderer.PathPoints.Clear();
            renderer.UINode.Children.Clear();

            LoadPath(renderer, rail);
        }



        private void PreparePath(RenderablePath path)
        {
            //Make the path create instances for property types
            path.PointUITagType = typeof(MuRailPoint);
            path.PathUITagType = typeof(MuRail);
            path.GetPointColor = (RenderablePathPoint pt) =>
            {
                var tag = pt.UINode.Tag;
                if (tag is MuRailPoint)
                {
                    var pointData = tag as MuRailPoint;
                    /*if (pointData.Prm1 != 0 || pointData.Prm2 != 0)
                        return new Vector4(1, 1, 0, 1);*/
                }
                return path.PointColor;
            };

            //Create the path tag instace for created paths
            path.UINode.Tag = Activator.CreateInstance(path.PathUITagType);
            MapEditorIcons.ReloadIcons(path.UINode, typeof(MuRail));

            // Setup the path default settings
            path.ConnectHoveredPoints = false;
            path.PointColor = new Vector4(Color.PointColor.X, Color.PointColor.Y, Color.PointColor.Z, 1.0f);
            path.ArrowColor = new Vector4(Color.ArrowColor.X, Color.ArrowColor.Y, Color.ArrowColor.Z, 1.0f);
            path.LineColor = new Vector4(Color.LineColor.X, Color.LineColor.Y, Color.LineColor.Z, 1.0f);
            // Only connect by the next points
            path.AutoConnectByNext = true;

            // Debug color changing setting for updating real time
            Color.OnColorChanged += delegate
            {
                path.PointColor = new Vector4(Color.PointColor.X, Color.PointColor.Y, Color.PointColor.Z, 1.0f);
                path.ArrowColor = new Vector4(Color.ArrowColor.X, Color.ArrowColor.Y, Color.ArrowColor.Z, 1.0f);
                path.LineColor = new Vector4(Color.LineColor.X, Color.LineColor.Y, Color.LineColor.Z, 1.0f);
            };
            path.AddCallback += delegate
            {
                Root.AddChild(path.UINode);
                Renderers.Add(path);
            };
            path.RemoveCallback += delegate
            {
                Root.Children.Remove(path.UINode);
                Renderers.Remove(path);
            };
            SetupProperties(path);
        }



        private void SetupProperties(RenderablePath path)
        {
            if (path.UINode.Tag is MuRail) // ???
            {
                //Update the renderer when properties are updated
                var pathProp = path.UINode.Tag as MuRail;
                path.UINode.TagUI.UIDrawer = null;
                path.UINode.TagUI.UIDrawer += delegate
                {
                    if (ImGuiNET.ImGui.Button(TranslationSource.GetText("EXPORT")))
                    {
                        var sfd = new ImguiFileDialog() { SaveDialog = true };
                        sfd.FileName = path.UINode.Header;
                        sfd.AddFilter(".curve", "");
                        if (sfd.ShowDialog("EXPORT_FILE"))
                        {
                            path.ExportAsFile(sfd.FilePath);
                        }
                    }
                    ImGuiNET.ImGui.SameLine();
                    if (ImGuiNET.ImGui.Button(TranslationSource.GetText("IMPORT")))
                    {
                        var sfd = new ImguiFileDialog();
                        sfd.FileName = path.UINode.Header;
                        sfd.AddFilter(".curve", "");
                        if (sfd.ShowDialog("IMPORT_FILE"))
                        {
                            path.CreateFromFile(sfd.FilePath);
                            GLContext.ActiveContext.UpdateViewport = true;
                        }
                    }
                };
                pathProp.PropertyChanged += delegate
                {
                    path.Loop = pathProp.IsClosed;
                    //if (pathProp.RailType == Path.RailInterpolation.Bezier)
                    if (pathProp.RailType == "Bezier")
                        path.InterpolationMode = RenderablePath.Interpolation.Bezier;
                    else
                        path.InterpolationMode = RenderablePath.Interpolation.Linear;

                    GLContext.ActiveContext.UpdateViewport = true;
                };
            }
        }



        public void ReloadEditor()
        {
            foreach (RenderablePath path in Renderers)
            {
                foreach (var part in path.PathPoints)
                    part.CanSelect = true; // !IsBaked;
            }
        }



        private void LoadPath(RenderablePath renderable, MuRail rail)
        {
            if (rail == null)
                return;

            renderable.PathPoints.Clear();

            if (rail is MuRail)
                renderable.Loop = (rail as MuRail).IsClosed;

            //Set the tag information of the path node
            renderable.UINode.Tag = rail;
            //Setup the tag properties
            SetupProperties(renderable);

            for (int i = 0; i < rail.RailPoints.Count; i++)
            {
                var pt = rail.RailPoints[i];
                // Set the transform of the point
                var position = new Vector3(pt.Translate.X, pt.Translate.Y, pt.Translate.Z);
                var point = renderable.CreatePoint(position);
                point.Transform.RotationEulerDegrees = new Vector3(pt.Rotate.X, pt.Rotate.Y, pt.Rotate.Z);
                //if (pt.Scale.HasValue) // ???
                point.Transform.Scale = new Vector3(pt.Scale.X, pt.Scale.Y, pt.Scale.Z); //new Vector3(pt.Scale.Value.X, pt.Scale.Value.Y, pt.Scale.Value.Z);
                point.Transform.UpdateMatrix(true);
                //Add the point to the list
                renderable.AddPoint(point);

                //Set the tag information of the node
                point.UINode.Tag = pt;

                //Configure path points
                if (pt is MuRailPoint)
                {
                    var pathPt = pt as MuRailPoint;
                    var handles = pathPt.ControlPoints;
                    //Configure control points if used
                    if (handles?.Count > 0)
                    {
                        //Set each control handle position in world space while updating all matrices
                        point.ControlPoint1.Transform.Position = new Vector3(handles[0].X, handles[0].Y, handles[0].Z);
                        point.ControlPoint2.Transform.Position = new Vector3(handles[1].X, handles[1].Y, handles[1].Z);
                        point.UpdateMatrices();
                    }
                    //Configure the interpolation method for the path
                    //if ((group as Path).RailType == Path.RailInterpolation.Bezier)
                    if (rail.RailType == "Bezier")
                        renderable.InterpolationMode = RenderablePath.Interpolation.Bezier;
                    else
                        renderable.InterpolationMode = RenderablePath.Interpolation.Linear;
                }

                // Connect each point from the previous point
                if (i != 0)
                {
                    renderable.PathPoints[i - 1].AddChild(point);
                }
            }
        }



        public List<NodeBase> GetSelected()
        {
            var selected = new List<NodeBase>();
            foreach (var groupNode in this.Root.Children)
            {
                if (groupNode.IsSelected)
                    selected.Add(groupNode);

                foreach (var pointNode in groupNode.Children)
                {
                    if (pointNode.IsSelected)
                        selected.Add(pointNode);
                }
            }

            return selected;
        }


        /*private void CreatePathFromFile(string filePath)
        {
            RenderablePath path = new RenderablePath();
            this.Add(path);
            path.CreateFromFile(filePath);
            ((MuRail)path.UINode.Tag).IsClosed = path.Loop;
            //Update UI properties from tag
            if (path.InterpolationMode == RenderablePath.Interpolation.Bezier)
                ((MuRail)path.UINode.Tag).RailType = "Bezier"; //Path.RailInterpolation.Bezier;
            else
                ((MuRail)path.UINode.Tag).RailType = "Linear"; // Path.RailInterpolation.Linear;
            PrepareObject(path);
        }*/


        private void CreateLinearPath()
        {
            RenderablePath path = new RenderablePath();
            this.Add(path);
            path.CreateLinearStandard(100);
            ((MuRail)path.UINode.Tag).IsClosed = false;
            ((MuRail)path.UINode.Tag).RailType = "Linear"; // Path.RailInterpolation.Linear;
            PrepareObject(path);

            if (!Root.IsChecked)
                Root.IsChecked = true;
        }


        private void CreateBezierPathCircle()
        {
            RenderablePath path = new RenderablePath();
            this.Add(path);
            path.CreateBezierCircle(20);
            ((MuRail)path.UINode.Tag).IsClosed = true;
            ((MuRail)path.UINode.Tag).RailType = "Bezier"; // Path.RailInterpolation.Bezier;
            PrepareObject(path);

            if (!Root.IsChecked)
                Root.IsChecked = true;
        }



        private void CreateBezierPathStandard()
        {
            RenderablePath path = new RenderablePath();
            this.Add(path);
            path.CreateBezierStandard(20);
            ((MuRail)path.UINode.Tag).IsClosed = false;
            ((MuRail)path.UINode.Tag).RailType = "Bezier"; // Path.RailInterpolation.Bezier;
            PrepareObject(path);

            if (!Root.IsChecked)
                Root.IsChecked = true;
        }

        private void PrepareObject(RenderablePath path)
        {
            var context = GLContext.ActiveContext;
            context.Scene.DeselectAll(context);

            path.Translate(context.Camera.GetViewPostion());
            path.IsSelected = true;
        }



        public void Add(RenderablePath path, bool undo = false)
        {
            //Add the tree node of the path
            PreparePath(path);
            MapEditor.AddRender(path, undo);
        }

        public void Remove(RenderablePath path)
        {
            MapEditor.RemoveRender(path);
        }

        public void Remove(MuRail rail) // path)
        {
            var render = this.Renderers.FirstOrDefault(x => ((RenderablePath)x).UINode.Tag == rail);
            if (render != null)
                MapEditor.RemoveRender(render);
        }



        public void DuplicateSelected()
        {
            var selected = Renderers.Where(x => ((RenderablePath)x).IsSelected).ToList();
            if (selected.Count == 0)
                return;

            GLContext.ActiveContext.Scene.BeginUndoCollection();
            GLContext.ActiveContext.Scene.DeselectAll(GLContext.ActiveContext);

            foreach (RenderablePath ob in selected)
            {
                RenderablePath duplicated = new RenderablePath();
                duplicated.InterpolationMode = ob.InterpolationMode;
                duplicated.Loop = ob.Loop;
                Add(duplicated, true);

                if (ob.UINode.Tag is MuRail)
                {
                    var prop = ob.UINode.Tag as MuRail;
                    ((MuRail)duplicated.UINode.Tag).IsClosed = prop.IsClosed;
                    //((MuRail)duplicated.UINode.Tag).Delete = prop.Delete;
                    ((MuRail)duplicated.UINode.Tag).RailType = prop.RailType;
                    //((MuRail)duplicated.UINode.Tag).UseAsObjPath = prop.UseAsObjPath;
                }

                foreach (var point in ob.PathPoints)
                {
                    var dupePoint = new RenderablePathPoint(duplicated, Vector3.Zero);
                    if (point.UINode.Tag is ICloneable)
                        dupePoint.UINode.Tag = ((ICloneable)point.UINode.Tag).Clone();
                    dupePoint.Transform = point.Transform.Clone();
                    dupePoint.ControlPoint1.Transform = point.ControlPoint1.Transform.Clone();
                    dupePoint.ControlPoint2.Transform = point.ControlPoint2.Transform.Clone();
                    duplicated.AddPoint(dupePoint);
                }
                duplicated.IsSelected = true;

            }

            GLContext.ActiveContext.Scene.EndUndoCollection();
        }



        public void RemoveSelected()
        {
            GLContext.ActiveContext.Scene.BeginUndoCollection();

            List<RenderablePath> removedPaths = new List<RenderablePath>();
            foreach (RenderablePath path in Renderers)
            {
                if (path.EditMode && path.PathPoints.Any(x => x.IsSelected))
                    path.RemoveSelected();
                else if (path.IsSelected)
                    removedPaths.Add(path);
            }

            if (removedPaths.Count > 0)
            {
                GLContext.ActiveContext.Scene.AddToUndo(
                    new EditableObjectDeletedUndo(GLContext.ActiveContext.Scene, removedPaths));
            }

            foreach (var path in removedPaths)
                Remove(path);

            GLContext.ActiveContext.Scene.EndUndoCollection();
        }


        public void OnMouseDown(MouseEventInfo mouseInfo) { }
        public void OnMouseUp(MouseEventInfo mouseInfo) { }
        public void OnMouseMove(MouseEventInfo mouseInfo) { }

        public void OnKeyDown(KeyEventInfo keyInfo)
        {
            if (keyInfo.IsKeyDown(InputSettings.INPUT.Scene.Delete))
                RemoveSelected();
            if (keyInfo.IsKeyDown(InputSettings.INPUT.Scene.Copy))
                DuplicateSelected();
        }
    }
}

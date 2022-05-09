using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFrameworkEngine;
using MapStudio.UI;
using Toolbox.Core.ViewModels;

namespace SampleMapEditor.LayoutEditor
{
    public interface ILayoutEditor
    {
        string Name { get; }

        StageLayoutPlugin MapEditor { get; set; }

        IToolWindowDrawer ToolWindowDrawer { get; }

        List<IDrawable> Renderers { get; set; }
        NodeBase Root { get; set; }
        List<MenuItemModel> MenuItems { get; set; }

        List<NodeBase> GetSelected();

        bool IsActive { get; set; }

        void ReloadEditor();

        void DrawHelpWindow();
        void DrawEditMenuBar();

        void RemoveSelected();

        void OnMouseMove(MouseEventInfo mouseInfo) { }
        void OnMouseDown(MouseEventInfo mouseInfo);
        void OnMouseUp(MouseEventInfo mouseInfo);
        void OnKeyDown(KeyEventInfo keyInfo);
        void OnSave(StageDefinition stage);
    }
}

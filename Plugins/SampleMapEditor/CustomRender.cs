using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFrameworkEngine;
using Toolbox.Core.ViewModels;

namespace SampleMapEditor
{
    /// <summary>
    /// Represents a custom renderer that can be transformed and manipulated.
    /// </summary>
    public class CustomRender : EditableObject, IColorPickable
    {
        UVSphereRender SphereDrawer;
        StandardMaterial Material;

        public CustomRender(NodeBase parent = null) : base(parent)
        {
            //Prepare our renderable sphere
            SphereDrawer = new UVSphereRender(20, 30, 30);
            //The gl framework includes some base materials to easily use
            Material = new StandardMaterial();
            //We can also apply some in engine textures
            Material.DiffuseTextureID = RenderTools.uvTestPattern.ID;
        }

        public void DrawColorPicking(GLContext context)
        {
            //Here we can draw under a color picking shader
            SphereDrawer.DrawPicking(context, this, Transform.TransformMatrix);
        }

        public override void DrawModel(GLContext context, Pass pass)
        {
            //Make sure to draw on the right pass!
            //These are used to sort out transparent ordering
            if (pass == Pass.OPAQUE)
                DrawOpaque(context);
        }

        private void DrawOpaque(GLContext context)
        {
            //Apply material
            Material.ModelMatrix = this.Transform.TransformMatrix;
            Material.Render(context);
            //Draw with a selection visual. 
            SphereDrawer.DrawWithSelection(context, this.IsSelected || this.IsHovered);
        }
    }


    public class CustomBoundingBoxRender : EditableObject, IColorPickable
    {
        UVSphereRender SphereDrawer;
        UVCubeRenderer CubeDrawer;
        StandardMaterial Material;

        public CustomBoundingBoxRender(NodeBase parent = null) : base(parent)
        {
            //Prepare our renderable sphere
            //SphereDrawer = new UVSphereRender(20, 30, 30);
            CubeDrawer = new UVCubeRenderer(10, OpenTK.Graphics.OpenGL.PrimitiveType.Lines);
            //The gl framework includes some base materials to easily use
            Material = new StandardMaterial();
            //We can also apply some in engine textures
            Material.DiffuseTextureID = RenderTools.uvTestPattern.ID;
        }

        public void DrawColorPicking(GLContext context)
        {
            //Here we can draw under a color picking shader
            CubeDrawer.DrawPicking(context, this, Transform.TransformMatrix);
        }

        public override void DrawModel(GLContext context, Pass pass)
        {
            //Make sure to draw on the right pass!
            //These are used to sort out transparent ordering
            if (pass == Pass.OPAQUE)
                DrawOpaque(context);
        }

        private void DrawOpaque(GLContext context)
        {
            //Apply material
            Material.ModelMatrix = this.Transform.TransformMatrix;
            Material.Render(context);
            //Draw with a selection visual. 
            CubeDrawer.DrawWithSelection(context, this.IsSelected || this.IsHovered);
        }
    }
}

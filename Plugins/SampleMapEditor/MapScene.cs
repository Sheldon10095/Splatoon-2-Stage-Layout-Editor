using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLFrameworkEngine;
using OpenTK;
using Toolbox.Core.ViewModels;
using MapStudio.UI;
using CafeLibrary.Rendering;
using CafeLibrary;
using Toolbox.Core.IO;
using System.IO;

namespace SampleMapEditor
{
    internal class MapScene
    {
        public void Setup(EditorLoader loader)
        {
            //Prepare a collision caster for snapping objects onto
            SetupSceneCollision();
            //Add some objects to the scene
            SetupObjects(loader);
        }

        /// <summary>
        /// Adds objects to the scene.
        /// </summary>
        private void SetupObjects(EditorLoader loader)
        {
            NodeBase objFolder = new NodeBase("Objs");
            //objFolder.HasCheckBox = true;
            loader.Root.AddChild(objFolder);
            NodeBase railsFolder = new NodeBase("Rails");
            //railsFolder.HasCheckBox = true;
            loader.Root.AddChild(railsFolder);

            bool HasModel(string mpath, out SARC s)
            {
                s = new SARC();
                s.Load(new MemoryStream(YAZ0.Decompress(mpath)));
                foreach (var f in s.files) if (f.FileName == "output.bfres") return true;
                return false;
            }

            foreach (var mapObj in loader.MapObjList)
            {
                /*NodeBase objNode = new NodeBase(mapObj["UnitConfigName"]);
                objNode.Icon = IconManager.MESH_ICON.ToString();
                objFolder.AddChild(objNode);*/

                string modelPath = loader.GetModelPathFromObject(mapObj);

                if (File.Exists(modelPath) && HasModel(modelPath, out SARC s))
                {
                    //SARC s = new SARC();
                    //s.Load(new MemoryStream(YAZ0.Decompress(modelPath)));
                    BfresRender o = new BfresRender(s.files.Find(f => f.FileName == "output.bfres").FileData, modelPath);
                    //o.Models.ForEach(model => { if (model.Name != mapObj["UnitConfigName"]) { model.IsVisible = false; Console.WriteLine($"Hiding model: {model.Name}"); } });

                    o.Models.ForEach(model =>
                    {
                        bool state = true;
                        if (model.Name != loader.GetActorFromObj(mapObj).FmdbName)
                        {
                            //model.IsVisible = false;
                            //Console.WriteLine($"Hiding model: {model.Name}");
                            state = false;
                            if ((model.Name.StartsWith("Fld_") &&
                                //!(model.Name.EndsWith("_Map") || model.Name.EndsWith("_drcmap"))))
                                model.Name.EndsWith("_DV")))
                            {
                                state = true;
                            }
                            //else state = true;
                        }

                        model.IsVisible = state;
                        if (!state)
                        {
                            Console.WriteLine($"Hiding model: {model.Name}");
                        }
                    });

                    //loader.GetActorFromObj(mapObj).FmdbName
                    //loader.Actors.Find(x => x.FmdbName == )

                    string fmdbname = loader.GetActorFromObj(mapObj).FmdbName;
                    if (fmdbname.StartsWith("Fld_Deli_"))
                    {
                        Console.WriteLine("Loading DeliTextures!");
                        var deliTex_sarc = new SARC();
                        string dtpath = EditorLoader.GetContentPath("Model/DeliTextures.Nin_NX_NVN.szs");
                        s.Load(new MemoryStream(YAZ0.Decompress(dtpath)));
                        BfresRender dt_bfres = new BfresRender(s.files.Find(f => f.FileName == "output.bfres").FileData, dtpath);
                        for (int i = 0; i < dt_bfres.Textures.Count; i++)
                        {
                            o.Textures.Add(dt_bfres.Textures.Keys.ElementAt(i), dt_bfres.Textures.Values.ElementAt(i));
                        }
                    }

                    objFolder.AddChild(o.UINode);
                    o.UINode.Header = mapObj["UnitConfigName"];
                    o.UINode.Icon = IconManager.MESH_ICON.ToString();
                    o.Transform.Position = EditorLoader.GetObjPos(mapObj);
                    o.Transform.Scale = EditorLoader.GetObjScale(mapObj);
                    o.Transform.RotationEulerDegrees = EditorLoader.GetObjRotation(mapObj);
                    o.Transform.UpdateMatrix(true);
                    loader.AddRender(o);
                }
                else
                {
                    TransformableObject o = new TransformableObject(objFolder);
                    //CustomBoundingBoxRender o = new CustomBoundingBoxRender(objFolder);
                    o.UINode.Header = mapObj["UnitConfigName"];
                    o.UINode.Icon = IconManager.MESH_ICON.ToString();
                    o.Transform.Position = EditorLoader.GetObjPos(mapObj);
                    o.Transform.Scale = EditorLoader.GetObjScale(mapObj);
                    o.Transform.RotationEulerDegrees = EditorLoader.GetObjRotation(mapObj);
                    //o.Color = new Vector4(0.5F, 0.5F, 0.5F, 0.5F);
                    o.Transform.UpdateMatrix(true);
                    //loader.AddRender(o);

                    //var min = o.Boundings.Box.Min;
                    //var max = o.Boundings.Box.Max;

                    //BoundingBoxRender bbox = new BoundingBoxRender(min, max);
                    
                    //loader.AddRender(o);
                }



                //Vector3F pos = 

                /*TransformableObject testOBJ = new TransformableObject(objFolder);
                testOBJ.UINode.Header = mapObj["UnitConfigName"];
                testOBJ.UINode.Icon = IconManager.MESH_ICON.ToString();
                testOBJ.Transform.Position = loader.GetObjPos(mapObj);
                testOBJ.Transform.Scale = loader.GetObjScale(mapObj);
                testOBJ.Transform.RotationEulerDegrees = loader.GetObjRotation(mapObj);
                testOBJ.Transform.UpdateMatrix(true);
                loader.AddRender(testOBJ);*/
                
            }

            /*//Console.WriteLine("Attempt to load model...");
            int i = 0;
            string path = loader.GetModelPathFromObject(loader.MapObjList[i]);
            //SARC s = new SARC();
            s.Load(new MemoryStream(YAZ0.Decompress(path)));
            BfresRender br = new BfresRender(s.files.Find(f => f.FileName == "output.bfres").FileData, path);
            br.Models.ForEach(model => { if (model.Name != loader.MapObjList[i]["UnitConfigName"]) model.IsVisible = false; });
            br.UINode.Header = "TESTING MODEL";
            br.UINode.Icon = IconManager.MESH_ICON.ToString();
            *//*br.Transform.Position = loader.GetObjPos(loader.MapObjList[i]);
            br.Transform.Scale = loader.GetObjScale(loader.MapObjList[i]);
            br.Transform.RotationEuler = loader.GetObjRotation(loader.MapObjList[i]);*//*
            br.Transform.Position = EditorLoader.GetObjPos(loader.MapObjList[i]);
            br.Transform.Scale = EditorLoader.GetObjScale(loader.MapObjList[i]);
            br.Transform.RotationEulerDegrees = EditorLoader.GetObjRotation(loader.MapObjList[i]);

            br.Transform.UpdateMatrix(true);
            loader.AddRender(br);*/

            //loader.model1.Root.Children[0].
            //CafeLibrary.FMDL fmdl = loader.model1.ModelFolder.Models[0];
            //loader.AddRender(fmdl.BfresWrapper.Renderer);
            //loader.AddRender(loader.model1.Root.Children[0].);

            //A folder to represent in the outliner UI
            NodeBase folder = new NodeBase("Objects");
            //Allow toggling visibility for the folder
            folder.HasCheckBox = true;
            //Add it to the root of our loader
            //It is important you use "AddChild" so the parent is applied
            loader.Root.AddChild(folder);
            //Icons can be obtained from the icon manager constants
            //These also are all from font awesome and can be used directly
            folder.Icon = IconManager.MODEL_ICON.ToString();

            //These are default transform cubes
            //You give it the folder you want to parent in the tree or make it null to not be present.
            TransformableObject obj = new TransformableObject(folder);
            //Name
            obj.UINode.Header = "Object1 lmao";
            obj.UINode.Icon = IconManager.MESH_ICON.ToString();
            //Give it a transform in the scene
            obj.Transform.Position = new Vector3(0, 10, 0);
            obj.Transform.Scale = new Vector3(1, 1, 1);
            obj.Transform.RotationEulerDegrees = new Vector3(0, 0, 0);
            //You need to force update it. This is not updated per frame to save on performance
            obj.Transform.UpdateMatrix(true);

            //Lastly add your object to the scene
            //loader.AddRender(obj);

            //Custom renderer
            CustomRender renderer = new CustomRender(folder);
            renderer.UINode.Icon = IconManager.MESH_ICON.ToString();
            renderer.UINode.Header = "Sphere";
            renderer.Transform.Position = new Vector3(-100, 0, 0);
            renderer.Transform.Scale = new Vector3(2.5f);
            renderer.Transform.UpdateMatrix(true);
            //loader.AddRender(renderer);
        }

        /// <summary>
        /// Creates a big plane which you can drop objects onto.
        /// </summary>
        private void SetupSceneCollision()
        {
            var context = GLContext.ActiveContext;

            float size = 2000;
            float height = 0;

            //Make a big flat plane for placing spaces on.
            context.CollisionCaster.Clear();
            context.CollisionCaster.AddTri(
                new Vector3(-size, height, size),
                new Vector3(0, height, -(size * 2)),
                new Vector3(size * 2, height, 0));
            context.CollisionCaster.AddTri(
                new Vector3(-size, height, -size),
                new Vector3(size * 2, height, 0),
                new Vector3(size * 2, height, size * 2));
            context.CollisionCaster.UpdateCache();
        }
    }
}

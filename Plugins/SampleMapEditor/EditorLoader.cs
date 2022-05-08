using System;
using System.IO;
using Toolbox.Core;
using MapStudio.UI;
using OpenTK;
using GLFrameworkEngine;
using CafeLibrary;
using Toolbox.Core.IO;
using OpenTK.Input;
using ByamlExt.Byaml;
using System.Collections.Generic;
using CafeLibrary.Rendering;

namespace SampleMapEditor
{
    /// <summary>
    /// Represents a class used for loading files into the editor.
    /// IFileFormat determines what files to use. FileEditor is used to store all the editor information.
    /// </summary>
    public class EditorLoader : FileEditor, IFileFormat
    {
        /// <summary>
        /// The description of the file extension of the plugin.
        /// </summary>
        public string[] Description => new string[] { "Map Data" };

        /// <summary>
        /// The extension of the plugin. This should match whatever file you plan to open.
        /// </summary>
        public string[] Extension => new string[] { "*.szs2" };

        /// <summary>
        /// Determines if the plugin can save or not.
        /// </summary>
        public bool CanSave { get; set; } = true;

        /// <summary>
        /// File info of the loaded file format.
        /// </summary>
        public File_Info FileInfo { get; set; }

        /// <summary>
        /// Determines when to use the map editor from a given file.
        /// You can check from file extension or check the data inside the file stream.
        /// The file stream is always decompressed if the given file has a supported ICompressionFormat like Yaz0.
        /// </summary>
        public bool Identify(File_Info fileInfo, Stream stream)
        {
            return fileInfo.Extension == ".szs2";
        }


        public static string GetContentPath(string relativePath)
        {
            if (File.Exists($"{PluginConfig.S2ModPath}/{relativePath}")) return $"{PluginConfig.S2ModPath}/{relativePath}";
            if (File.Exists($"{PluginConfig.S2AocPath}/{relativePath}")) return $"{PluginConfig.S2AocPath}/{relativePath}";
            if (File.Exists($"{PluginConfig.S2GamePath}/{relativePath}")) return $"{PluginConfig.S2GamePath}/{relativePath}";
            return relativePath;


            /*//Update first then base package.
            //if (File.Exists($"{ModOutputPath}\\{relativePath}")) return $"{ModOutputPath}\\{relativePath}";
            if (File.Exists($"{UpdatePath}\\{relativePath}")) return $"{UpdatePath}\\{relativePath}";
            if (File.Exists($"{GamePath}\\{relativePath}")) return $"{GamePath}\\{relativePath}";

            //4 individual DLCs. Each directory is divided by content and permissive info.
            if (File.Exists($"{AOCPath}\\0013\\{relativePath}")) return $"{AOCPath}\\0013\\{relativePath}";
            if (File.Exists($"{AOCPath}\\0015\\{relativePath}")) return $"{AOCPath}\\0015\\{relativePath}";
            if (File.Exists($"{AOCPath}\\0017\\{relativePath}")) return $"{AOCPath}\\0017\\{relativePath}";
            if (File.Exists($"{AOCPath}\\0019\\{relativePath}")) return $"{AOCPath}\\0019\\{relativePath}";

            return relativePath;*/
        }


        public class Actor
        {
            public string ClassName { get; set; }
            public string FmdbName { get; set; }
            public string JmpName { get; set; }
            public string LinkUserName { get; set; }
            public string Name { get; set; }
            public string ParamsFileBaseName { get; set; }
            public string ResJmpName { get; set; }
            public string ResName { get; set; }


            public Actor(dynamic actor)
            {
                ClassName = actor["ClassName"];
                FmdbName = actor["FmdbName"];
                JmpName = actor["JmpName"];
                LinkUserName = actor["LinkUserName"];
                Name = actor["Name"];
                ParamsFileBaseName = actor["ParamsFileBaseName"];
                try { ResJmpName = actor["ResJmpName"]; } catch { } //ResJmpName = actor["ResJmpName"];
                ResName = actor["ResName"];
            }

            public Actor()
            {
                ClassName = "Gachihoko";
                FmdbName = "Wsp_Shachihoko";
                JmpName = "";
                LinkUserName = "Gachihoko";
                Name = "ScrambleBombFlower";
                ParamsFileBaseName = "MapObj/ScrambleBombFlower";
                ResJmpName = "";
                ResName = "Wsp_Shachihoko";
            }
        }

        public List<Actor> Actors { get; set; } = new List<Actor>();


        public List<dynamic> MapObjList { get; set; } = new List<dynamic>();

        public static Vector3 GetObjPos(dynamic obj)
        {
            var t = obj["Translate"];
            return new Vector3(t["X"], t["Y"], t["Z"]);
        }
        public static Vector3 GetObjScale(dynamic obj)
        {
            var t = obj["Scale"];
            return new Vector3(t["X"], t["Y"], t["Z"]);
        }
        public static Vector3 GetObjRotation(dynamic obj)
        {
            var t = obj["Rotate"];
            return new Vector3(t["X"], t["Y"], t["Z"]);
        }


        private void ParseActorDb()
        {
            //string mushPackPath = $"{PluginConfig.S2GamePath}/Pack/Mush.release.pack";
            string mushPackPath = GetContentPath("Pack/Mush.release.pack");
            SARC mushSARC = new SARC();

            // Load Mush.release.pack
            using (FileReader r = new FileReader(mushPackPath))
            {
                mushSARC.Load(r.BaseStream);
            }

            // Find ActorDb
            BymlFileData actorDbByml = new BymlFileData();
            foreach (var file in mushSARC.Files)
            {
                if (file.FileName.Contains("ActorDb"))
                {
                    Console.WriteLine("Found ActorDb!");
                    if (Nisasyst.IsEncrypted(file.FileData))
                    {
                        actorDbByml = Nisasyst.DecryptByaml((SARC.FileEntry)file);
                    }
                    else
                    {
                        actorDbByml = ByamlFile.LoadN(new MemoryStream(file.AsBytes()));
                    }
                }
                Console.WriteLine($"{file.FileName} {(Nisasyst.IsEncrypted(file.FileData) ? "is" : "is not")} Nisasyst encrypted.");
            }

            if (actorDbByml == null) return;

            foreach (var node in actorDbByml.RootNode)
            {
                Actors.Add(new Actor(node));
                //Console.WriteLine(node["Name"]);
            }

            Console.WriteLine("Finished loading ActorDb.");
        }


        //
        public string GetModelPathFromUnitConfigName(string name)
        {
            Actor actor = Actors.Find(x => x.Name == name);
            if (actor == null) return null;
            if (actor.ResName == "") return null;
            //return $"{PluginConfig.S2GamePath}/Model/{actor.ResName}.Nin_NX_NVN.szs";
            return GetContentPath($"Model/{actor.ResName}.Nin_NX_NVN.szs");
        }

        public string GetModelPathFromObject(dynamic obj)
        {
            return GetModelPathFromUnitConfigName(obj["UnitConfigName"]);
        }

        public Actor GetActorFromObj(dynamic obj)
        {
            string ucName = obj["UnitConfigName"];
            return GetActorFromUnitConfigName(ucName);
        }

        public Actor GetActorFromUnitConfigName(string name)
        {
            Actor actor = Actors.Find(x => x.Name == name);
            return actor;
        }

        public BFRES model1 = new BFRES();


        /// <summary>
        /// Loads the given file data from a stream.
        /// </summary>
        public void Load(Stream stream)
        {
            Console.WriteLine(PluginConfig.S2GamePath);

            ParseActorDb();

            //SARC arc = (SARC)STFileLoader.OpenFileFormat(stream, FileInfo.FilePath);
            SARC arc = new SARC();
            arc.Load(stream);
            BymlFileData lytByml = ByamlFile.LoadN(arc.files[0].FileData, false);
            
            MapObjList.Clear();

            // Print out the name of each object that will be loaded
            foreach (var obj in lytByml.RootNode["Objs"])
            {
                Console.WriteLine(obj["UnitConfigName"]);
                float x = obj["Translate"]["X"];
                float y = obj["Translate"]["Y"];
                float z = obj["Translate"]["Z"];
                Console.WriteLine($"  Pos: {x}, {y}, {z}");
                MapObjList.Add(obj);
            }

            Console.WriteLine("Object list:");
            foreach (var obj in MapObjList)
            {
                Console.WriteLine($"  Object name: {obj["UnitConfigName"]}");
            }

            // (TESTING) Load the first object in the list
            int i = 0;
            string modelPath = GetModelPathFromObject(MapObjList[i]);
            Console.WriteLine($"Model Path: {modelPath}");


            /*SARC mdlFile = new SARC();
            byte[] tmp = YAZ0.Decompress(modelPath);
            using (var ms = new MemoryStream(tmp))
            {
                mdlFile.Load(ms);
            }*/

            //SARC mdlFile = new SARC();
            //mdlFile.Load(new MemoryStream(YAZ0.Decompress(modelPath)));
            //BfresRender r = new BfresRender(mdlFile.files.Find(x => x.FileName == "output.bfres").FileData, modelPath);

            
            //Console.WriteLine(mdlFile.files[0].FileName);
            //mdlFile.files.ForEach(f => Console.WriteLine(f.FileName));
            //model1 = (BFRES)mdlFile.files.Find(x => x.FileName == "output.bfres").OpenFile();


            //SARC modelFile = (SARC)STFileLoader.OpenFileFormat(modelPath);
            //modelFile.files.ForEach(f => Console.WriteLine(f));
            //BFRES res = (BFRES)modelFile.files[0].OpenFile();

            //model1 = res;

            //Console.WriteLine("!!!Break!!!");
            // Check if file is a Stage Layout Byml
            //BFRES a = new BFRES();
            //SARC arc = new SarcData(stream);

            //For this example I will show loading 3D objects into the scene
            MapScene scene = new MapScene();
            scene.Setup(this);
        }

        /// <summary>
        /// Saves the given file data to a stream.
        /// </summary>
        public void Save(Stream stream)
        {

        }

        //Extra overrides for FileEditor you can use for custom UI

        /// <summary>
        /// Draws the viewport menu bar usable for custom tools.
        /// </summary>
        public override void DrawViewportMenuBar()
        {

        }

        /// <summary>
        /// When an asset item from the asset windows gets dropped into the editor.
        /// You can configure your own asset category from the asset window and make custom asset items to drop into.
        /// </summary>
        public override void AssetViewportDrop(AssetItem item, Vector2 screenPosition)
        {
            //viewport context
            var context = GLContext.ActiveContext;

            //Screen coords can be converted into 3D space
            //By default it will spawn in the mouse position at a distance
            Vector3 position = context.ScreenToWorld(screenPosition.X, screenPosition.Y, 100);
            //Collision dropping can be used to drop these assets to the ground from CollisionCaster
            if (context.EnableDropToCollision)
            {
                Quaternion rot = Quaternion.Identity;
                CollisionDetection.SetObjectToCollision(context, context.CollisionCaster, screenPosition, ref position, ref rot);
            }
        }

        /// <summary>
        /// Checks for dropped files to use for the editor.
        /// If the value is true, the file will not be loaded as an editor if supported.
        /// </summary>
        public override bool OnFileDrop(string filePath)
        {
            return false;
        }
    }
}

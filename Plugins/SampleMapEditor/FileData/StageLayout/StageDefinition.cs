using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByamlExt.Byaml;
using Toolbox.Core;

namespace SampleMapEditor
{
    public class StageDefinition : IByamlSerializable
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        //For saving back in the same place
        private string originalPath;

        private BymlFileData BymlData;

        public StageDefinition()
        {
            BymlData = new BymlFileData()
            {
                byteOrder = Syroot.BinaryData.ByteOrder.LittleEndian,
                SupportPaths = false,
                Version = 3
            };
        }

        public StageDefinition(string fileName)
        {
            originalPath = fileName;
            Load(System.IO.File.OpenRead(fileName));
        }

        public StageDefinition(System.IO.Stream stream)
        {
            Load(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StageDefinition"/> class from the given stream.
        /// </summary>
        /// <param name="stream">The stream from which the instance will be loaded.</param>
        private void Load(System.IO.Stream stream)
        {

            BymlData = ByamlFile.LoadN(stream, true);
            //   Console.WriteLine($"Loaded byaml! {fileName}");
            ByamlSerialize.Deserialize(this, BymlData.RootNode);
               Console.WriteLine("Deserialized byaml!");

            // After loading all the instances, allow references to be resolved.
            /*Areas?.ForEach(x => x.DeserializeReferences(this));
            Clips?.ForEach(x => x.DeserializeReferences(this));
            ClipPattern?.AreaFlag?.ForEach(x => x.DeserializeReferences(this));
            EnemyPaths?.ForEach(x => x.DeserializeReferences(this));
            GCameraPaths?.ForEach(x => x.DeserializeReferences(this));
            GlidePaths?.ForEach(x => x.DeserializeReferences(this));
            GravityPaths?.ForEach(x => x.DeserializeReferences(this));
            ItemPaths?.ForEach(x => x.DeserializeReferences(this));
            JugemPaths?.ForEach(x => x.DeserializeReferences(this));
            LapPaths?.ForEach(x => x.DeserializeReferences(this));
            ObjPaths?.ForEach(x => x.DeserializeReferences(this));
            Paths?.ForEach(x => x.DeserializeReferences(this));
            PullPaths?.ForEach(x => x.DeserializeReferences(this));
            Objs?.ForEach(x => x.DeserializeReferences(this));
            ReplayCameras?.ForEach(x => x.DeserializeReferences(this));
            IntroCameras?.ForEach(x => x.DeserializeReferences(this));
            SteerAssistPaths?.ForEach(x => x.DeserializeReferences(this));
            RouteChanges?.ForEach(x => x.DeserializeReferences(this));*/

            Objs?.ForEach(x => x.DeserializeReferences(this));
            //Rails?.ForEach(x => x.DeserializeReferences(this));

            //Convert baked in tool obj paths to editable rail paths
            /*if (ObjPaths != null)
            {
                List<ObjPath> converted = ObjPaths.Where(x => x.BakedRailPath == true).ToList();
                if (converted.Count > 0 && Paths == null)
                    Paths = new List<Path>();

                foreach (var objPath in converted)
                {
                    var path = Path.ConvertFromObjPath(objPath);
                    Paths.Add(path);
                    ObjPaths.Remove(objPath);

                    //Link the obj path
                    foreach (var obj in Objs)
                    {
                        if (obj.ObjPath == objPath)
                        {
                            int ptIndex = -1;
                            if (obj.ObjPathPoint != null)
                                ptIndex = obj.ObjPath.Points.IndexOf(obj.ObjPathPoint);

                            obj.ObjPath = null;
                            obj.ObjPathPoint = null;
                            obj.Path = path;
                            if (ptIndex != -1 && ptIndex < path.Points.Count)
                                obj.PathPoint = path.Points[ptIndex];
                        }
                    }
                }
            }*/
        }

        public void Save() { this.Save(originalPath); }

        public void Save(string fileName)
        {
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                Save(stream);
        }

        public void Save(System.IO.Stream stream)
        {
            SaveMapObjList();

            //Convert editable rail paths to obj paths.
            /*List<Path> converted = Paths?.Where(x => x.UseAsObjPath == true).ToList();
            if (converted != null)
            {
                foreach (var path in converted)
                {
                    var objPath = ObjPath.ConvertFromPath(path);
                    ObjPaths.Add(objPath);

                    //Link the obj path
                    foreach (var obj in Objs)
                    {
                        if (obj.Path == path)
                        {
                            int ptIndex = -1;
                            if (obj.PathPoint != null)
                                ptIndex = obj.Path.Points.IndexOf(obj.PathPoint);

                            obj.ObjPath = objPath;
                            if (ptIndex != -1 && ptIndex < objPath.Points.Count)
                                obj.ObjPathPoint = objPath.Points[ptIndex];
                        }
                    }
                    Paths.Remove(path);
                }
            }*/

            /*if (ClipPattern != null && ClipPattern.AreaFlag?.Count > 0)
            {
                Clips = new List<Clip>();
                foreach (var clip in ClipPattern.AreaFlag)
                    Clips.Add(clip);
            }*/

            // Before saving all the instances, allow references to be resolved.
            /*Areas?.ForEach(x => x.SerializeReferences(this));
            Clips?.ForEach(x => x.SerializeReferences(this));
            ClipPattern?.AreaFlag?.ForEach(x => x.SerializeReferences(this));
            EnemyPaths?.ForEach(x => x.SerializeReferences(this));
            GCameraPaths?.ForEach(x => x.SerializeReferences(this));
            GlidePaths?.ForEach(x => x.SerializeReferences(this));
            GravityPaths?.ForEach(x => x.SerializeReferences(this));
            ItemPaths?.ForEach(x => x.SerializeReferences(this));
            JugemPaths?.ForEach(x => x.SerializeReferences(this));
            LapPaths?.ForEach(x => x.SerializeReferences(this));
            PullPaths?.ForEach(x => x.SerializeReferences(this));
            Objs?.ForEach(x => x.SerializeReferences(this));
            ReplayCameras?.ForEach(x => x.SerializeReferences(this));
            IntroCameras?.ForEach(x => x.SerializeReferences(this));
            Paths?.ForEach(x => x.SerializeReferences(this));
            SteerAssistPaths?.ForEach(x => x.SerializeReferences(this));
            RouteChanges?.ForEach(x => x.SerializeReferences(this));*/

            Objs?.ForEach(x => x.SerializeReferences(this));
            //Rails?.ForEach(x => x.SerializeReferences(this));

            BymlData.RootNode = ByamlSerialize.Serialize(this);
            ByamlFile.SaveN(stream, BymlData);

            //Re add converted obj paths back to rails for editors
            /*foreach (var path in converted)
            {
                if (!Paths.Contains(path))
                    Paths.Add(path);
            }*/
        }

        private void SaveMapObjList()
        {
            /*if (GlobalSettings.ObjDatabase.Count == 0)
                return;*/

            /*if (Objs == null)
            {
                this.MapObjResList = new List<string>();
                this.MapObjIdList = new List<int>();
                return;
            }*/

            //Order the obj list by ID from highest to smallest
            //This is very important for certain objs (like water boxes)
            
            //Objs = Objs.OrderByDescending(x => x.ObjId).ToList(); // Sort the object list

            /*List<string> resNameList = new List<string>();
            List<int> resIDList = new List<int>();
            foreach (var ob in Objs)
            {
                if (GlobalSettings.ObjDatabase.ContainsKey(ob.ObjId))
                {
                    List<string> names = GlobalSettings.ObjDatabase[ob.ObjId].ResNames;
                    for (int j = 0; j < names.Count; j++)
                    {
                        if (!resNameList.Contains(names[j]))
                            resNameList.Add(names[j]);
                    }
                }

                if (!resIDList.Contains(ob.ObjId))
                    resIDList.Add(ob.ObjId);
            }*/

            /*if (resNameList.Contains("Dokan1"))
            {
                resIDList.Add(1019); //Dokan1
            }

            if (resNameList.Contains("CmnGroupToad") ||
                resNameList.Contains("N64RTrain"))
            {
                resIDList.Add(1044); //KaraPillarBase
                resNameList.Add("CmnToad");
            }

            if (resNameList.Contains("KaraPillar"))
                resIDList.Add(9006); //KaraPillarBase
            if (resNameList.Contains("ItemBox"))
                resIDList.Add(9007); //ItemBoxFont*/

            //this.MapObjResList = resNameList.Reverse<string>().Distinct().ToList();
            //this.MapObjIdList = resIDList.OrderByDescending(x => x).Distinct().ToList();
        }


        // ---- PROPERTIES ---------------------------------------------------------------------------------------------
        //
        // NONE
        // 





        // ---- Objects ----

        /// <summary>
        /// Gets or sets the list of <see cref="Obj"/> instances.
        /// </summary>
        [ByamlMember("Objs")]
        public List<Obj> Objs { get; set; }


        // ---- Rails ----
#warning Uncomment this once Rail is defined!
        //[ByamlMember("Rails", Optional = true)]
        //public List<Rail> Rails { get; set; }




        public void DeserializeByaml(IDictionary<string, object> dictionary)
        {
            /*// ObjParams
            ObjParams = new List<int>();
            for (int i = 1; i <= 8; i++)
            {
                if (dictionary.TryGetValue("OBJPrm" + i.ToString(), out object objParam))
                {
                    ObjParams.Add((int)objParam);
                }
            }*/
        }

        public void SerializeByaml(IDictionary<string, object> dictionary)
        {
            /*if (HeadLight == CourseHeadLight.Off) HeadLight = null;

            // ObjParams
            for (int i = 1; i <= ObjParams.Count; i++)
            {
                dictionary["OBJPrm" + i.ToString()] = ObjParams[i - 1];
            }

            if (IsFirstLeft == true)
                FirstCurve = "left";
            else
                FirstCurve = "right";*/
        }
    }
}

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



        public bool Identify(File_Info fileInfo, Stream stream)
        {
            //Just load maps from checking the byaml extension atm.
            //return fileInfo.Extension == ".byaml";
            return fileInfo.Extension == ".szs" && !fileInfo.FileName.Contains("Nin_NX_NVN");
        }



        private bool IsNewProject = false;



        public override bool CreateNew()
        {
            //bool isSwitch = IsNewSwitch;

            IsNewProject = true;

            MapLoader = new MapLoader(this);
            //MapLoader.Init(this, isSwitch, model_path);
            MapLoader.Init(this);

            //Root.Header = "course_muunt.byaml";
            Root.Header = "Fld_Test01_Vss.byaml (HEADER)";
            Root.Tag = this;

            //FileInfo.FileName = "course_muunt.byaml";
            //FileInfo.FileName = "Fld_Test01_Vss.byaml";
            FileInfo.FileName = "Fld_Test01_Vss.szs";

            //Setup(MapLoader);

            return true;
        }







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
    }
}

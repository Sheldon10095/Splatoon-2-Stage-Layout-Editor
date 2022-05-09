using ByamlExt.Byaml;
using CafeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMapEditor
{
    public class ActorDefinitionDb
    {
        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        private BymlFileData BymlData;

        public ActorDefinitionDb()
        {
            BymlData = new BymlFileData()
            {
                byteOrder = Syroot.BinaryData.ByteOrder.LittleEndian,
                SupportPaths = false,
                Version = 3,
            };
        }


        public ActorDefinitionDb(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Load(stream);
            }
        }


        public ActorDefinitionDb(Stream stream)
        {
            Load(stream);
        }


        // ---- PROPERTIES ---------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Gets or sets the list of <see cref="ActorDefinition"/> instances in this database.
        /// </summary>
        public List<ActorDefinition> Definitions
        {
            get;
            set;
        }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Saves the definitions into the file with the given name.
        /// </summary>
        /// <param name="fileName">The name of the file in which the definitions will be stored.</param>
        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Save(stream);
            }
        }

        /// <summary>
        /// Saves the definitions into the the given stream.
        /// </summary>
        /// <param name="stream">The stream in which the definitions will be stored.</param>
        public void Save(Stream stream)     // ???
        {
            BymlData.RootNode = ByamlSerialize.Serialize(this);
            ByamlFile.SaveN(stream, BymlData);
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private void Load(Stream stream)
        {
            //BymlData = ByamlFile.LoadN(stream, true);
            //ByamlSerialize.Deserialize(this, BymlData.RootNode);

            SARC mushy = new SARC();
            mushy.Load(stream);

            foreach (var file in mushy.Files)
            {
                if (file.FileName.Contains("ActorDb"))
                {
                    Console.WriteLine("Found ActorDb!");
                    if (Nisasyst.IsEncrypted(file.FileData))
                    {
                        BymlData = Nisasyst.DecryptByaml((SARC.FileEntry)file);
                    }
                    else
                    {
                        BymlData = ByamlFile.LoadN(new MemoryStream(file.AsBytes()));
                    }
                }
                Console.WriteLine($"{file.FileName} {(Nisasyst.IsEncrypted(file.FileData) ? "is" : "is not")} Nisasyst encrypted.");    // DEBUG PRINT
            }

            if (BymlData == null) { Console.WriteLine("Failed to find ActorDb!"); return; }

            ByamlSerialize.Deserialize(this, BymlData.RootNode);

            Console.WriteLine("Finished loading ActorDb.");
        }
    }
}

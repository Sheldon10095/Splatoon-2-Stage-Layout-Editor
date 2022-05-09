using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Toolbox.Core;

namespace SampleMapEditor
{
    [ByamlObject]
    public class ActorDefinition
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        [ByamlMember]
        [BindGUI("Class Name")]
        public string ClassName
        {
            get => className;
            set => SetField(ref className, value);
        }
        private string className;


        [ByamlMember]
        [BindGUI("Fmdb Name")]
        public string FmdbName
        {
            get => fmdbName;
            set => SetField(ref fmdbName, value);
        }
        private string fmdbName;


        [ByamlMember]
        [BindGUI("Jmp Name")]
        public string JmpName
        {
            get => jmpName;
            set => SetField(ref jmpName, value);
        }
        private string jmpName;


        [ByamlMember]
        [BindGUI("Link User Name")]
        public string LinkUserName
        {
            get => linkUserName;
            set => SetField(ref linkUserName, value);
        }
        private string linkUserName;


        [ByamlMember]
        [BindGUI("Name")]
        public string Name
        {
            get => name;
            set => SetField(ref name, value);
        }
        private string name;


        [ByamlMember]
        [BindGUI("Params File Base Name")]
        public string ParamsFileBaseName
        {
            get => paramsFileBaseName;
            set => SetField(ref paramsFileBaseName, value);
        }
        private string paramsFileBaseName;


        [ByamlMember]
        [BindGUI("Res Jmp Name")]
        public string ResJmpName
        {
            get => resJmpName;
            set => SetField(ref resJmpName, value);
        }
        private string resJmpName;


        [ByamlMember]
        [BindGUI("Res Name")]
        public string ResName
        {
            get => resName;
            set => SetField(ref resName, value);
        }
        private string resName;



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

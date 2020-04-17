using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using PluginInterface;
using DSOFile;

namespace Lab01
{
    public class Plugin : IPlugin
    {
        public Plugin(string Path)
        { SetPlugin(Path); }

        public static string GetTypenameFromEnum(PluginType T)
        {
            switch (T)
            {
                case PluginType.Cryptography:
                    return "Cryptography";
                default:
                    return "Unknown";
            }
        }

        private IPlugin internalPlugin;
        private string myPath;
        private PluginType myType = PluginType.Unknown;
        public string GetPath { get { return myPath; } }
        public string Filename { get { return new FileInfo(myPath).Name; } }
        public PluginType Type
        {
            get
            {
                if (internalPlugin == null)
                {
                    return PluginType.Unknown;
                }
                return myType;
            }
        }
        public string Name
        {
            get
            {
                if (internalPlugin == null)
                {
                    return "Unknown";
                }
                return internalPlugin.Name;
            }
        }
        public string Version
        {
            get
            {
                if (internalPlugin == null)
                {
                    return "Unknown";
                }
                return internalPlugin.Version;
            }
        }
        public string Author
        {
            get
            {
                if (internalPlugin == null)
                {
                    return "Unknown";
                }
                return internalPlugin.Author;
            }
        }
        public override string ToString()
        { return string.Format("{0}: {1}: {2}: {3}: {4}", GetTypenameFromEnum(myType), Filename, Name, Version, Author); }

        public static byte[] ActivatePlugin(Plugin plugin,byte[] _data,bool way)
        {
            switch (plugin.Type)
            {
                case PluginType.Cryptography:
                    byte[] result=null;
                    if (way)
                    {
                        result=((ICrypography)plugin.internalPlugin).Encrypt(_data);
                    }
                    else { 
                        result=((ICrypography)plugin.internalPlugin).Decrypt(_data);   
                    }
                    return result;
                default:
                    IPlugin plugin_IPlugin = plugin as IPlugin;
                    return null;
            }
        }

        public static int FindPlugin(string filename)
        {
            string myPath = Directory.GetCurrentDirectory() + "/Plugins";
            string plugin_name = GetCustomFileProperty(filename);
            if (plugin_name == null|| plugin_name=="")
            {
                return 0;
            }
            if (!Directory.Exists(myPath))
            {
                return -1;
            }
            foreach (string f in Directory.GetFiles(myPath))
            {
                FileInfo fi = new FileInfo(f);

                if (fi.Extension.Equals(".dll") && fi.Name.Equals(plugin_name))
                {
                    Form1._curr_Plugin= new Plugin(f);
                    return 1;
                }   
            }
            return -1;
        }

        public static void SetCustomFileProperty(string filename,string pluginname)
        {
            OleDocumentProperties myFile = new DSOFile.OleDocumentProperties();
            myFile.Open(filename, false, DSOFile.dsoFileOpenOptions.dsoOptionDefault);
            object Value = pluginname;
            foreach (DSOFile.CustomProperty property in myFile.CustomProperties)
            {
                if (property.Name == "MyPlugin")
                {
                    property.set_Value(Value);
                    myFile.Close(true);
                    return;
                }
            }
            myFile.CustomProperties.Add("MyPlugin", ref Value);
            myFile.Save();
            myFile.Close(true);
        }

        public static string GetCustomFileProperty(string filename)
        {
            OleDocumentProperties myFile = new DSOFile.OleDocumentProperties();
            myFile.Open(filename, false, DSOFile.dsoFileOpenOptions.dsoOptionDefault);
            foreach (DSOFile.CustomProperty property in myFile.CustomProperties)
            {
                if (property.Name == "MyPlugin")
                {
                    string pluginname=property.get_Value();
                    myFile.Close(true);
                    return pluginname;
                }
            }
            return null;
        }

        public bool SetPlugin(string PluginFile)
        {
            Assembly asm;
            PluginType pt = PluginType.Unknown;
            Type PluginClass = null;

            if (!File.Exists(PluginFile))
            {
                return false;
            }
            asm = Assembly.LoadFile(PluginFile);

            if (asm != null)
            {
                myPath = PluginFile;
                foreach (Type type in asm.GetTypes())
                {
                    if (type.IsAbstract) continue;
                    object[] attrs = type.GetCustomAttributes(typeof(PluginAttribute), true);
                    if (attrs.Length > 0)
                    {
                        foreach (PluginAttribute pa in attrs)
                        {
                            pt = pa.Type;
                        }
                        PluginClass = type;
                        if (pt == PluginType.Unknown)
                        {
                            return false;
                        }
                        internalPlugin =(IPlugin)Activator.CreateInstance(PluginClass);
                        myType = pt;
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
    }
}

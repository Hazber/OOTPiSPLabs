using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System.Reflection;

namespace Lab01
{
    public abstract class Creator
    {
        public abstract void FileSave(string filename, List<MenuItem> MyMenu);
        public abstract List<MenuItem> FileOpen(string filename);
    }
    public class BinFileCreator:Creator
    {
        public override void FileSave(string filename, List<MenuItem> MyMenu)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, MyMenu);
            }
        }
        public override List<MenuItem> FileOpen(string filename)
        {
            List<MenuItem> MyMenu;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                 MyMenu= (List<MenuItem>)formatter.Deserialize(fs);
            }
            return MyMenu;
        }
    }

    public class JsonFileCreator : Creator
    {
        public override void FileSave(string filename, List<MenuItem> MyMenu)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string Serialized = JsonConvert.SerializeObject(MyMenu, settings);
            using (StreamWriter jsfile = File.CreateText(filename))
            {
                jsfile.WriteLine(Serialized);
            }
        }
        public override List<MenuItem> FileOpen(string filename)
        {
            string Serialized;
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            using (StreamReader jsfile = File.OpenText(filename))
            {
                Serialized = jsfile.ReadLine();
            }
            List<MenuItem> MyMenu = JsonConvert.DeserializeObject<List<MenuItem>>(Serialized, settings);
            return MyMenu;
        }
    }

    public class TextFileCreator: Creator
    {
        public override void FileSave(string filename, List<MenuItem> MyMenu)
        {
            string Serialized = null;
            foreach (MenuItem item in MyMenu)
            {
                MakeClass(ref Serialized, item);
                Serialized += "};";
            }
            Serialized += "$";
            using (StreamWriter txtfile = File.CreateText(filename))
            {
                txtfile.WriteLine(Serialized);
            }
        }

        public override List<MenuItem> FileOpen(string filename)
        {
            string Serialized = null;
            using (StreamReader file = File.OpenText(filename))
            {
                Serialized = file.ReadLine();
            }
            List<MenuItem> LocalMenu = new List<MenuItem>();
            while (Serialized != "$")
            {
                LocalMenu.Add(ParseSerialized(ref Serialized));
            }
            return LocalMenu;
        }


        private static void MakeClass(ref string str, object obj)
        {
            MenuItem item = (MenuItem)obj;
            Type type = Type.GetType("Lab01." + item.Category.ToString(), false, true);
            str += type.ToString() + "{";
            int i = 0;
            foreach (FieldInfo param in type.GetFields())
            {
                if (param.FieldType.ToString().IndexOf("Lab01.") > -1 && param.FieldType.ToString().IndexOf("+") <= -1)
                {
                    MakeClass(ref str, param.GetValue(item));
                    str += "},";
                }
                else
                {
                    str += param.FieldType.ToString() + ":" + param.GetValue(item) + ",";
                }
            }
        }
        private static Type FieldType(ref string str, int ind)
        {
            Type objtype = Type.GetType(str.Substring(0, ind), false, true);
            str = str.Remove(0, ind + 1);
            return objtype;
        }

        private static string GetStringValue(ref string str)
        {
            int ind = str.IndexOf(',');
            string s = str.Substring(0, ind);
            str = str.Remove(0, ind + 1);
            return s;
        }

        private static MenuItem ParseSerialized(ref string str)
        {
            object cat = null;
            int index0 = str.IndexOf('{');
            Type objtype = FieldType(ref str, index0);
            FieldInfo[] fields = objtype.GetFields();
            Type[] types = new Type[fields.Length - 1];
            object[] values = new object[fields.Length - 1];
            int i = 0;
            foreach (var param in fields)
            {
                int index1 = str.IndexOf(':');
                int index2 = str.IndexOf('{');
                if (index1 < index2 || index2 == -1)
                {
                    types[i] = FieldType(ref str, index1);
                    string value_str = GetStringValue(ref str);
                    if (types[i].Name == "Int32")
                        values[i] = Convert.ToInt32(value_str);
                    else if (types[i].Name == "String")
                        values[i] = value_str;
                    else if (types[i].Name == "Double")
                        values[i] = Convert.ToDouble(value_str);
                    else if (types[i].Name == "Boolean")
                        values[i] = Convert.ToBoolean(value_str);
                    else
                    {
                        if (types[i].Name != "TCategorys")
                            values[i] = Enum.Parse(types[i], value_str);
                        else
                        {
                            cat = Enum.Parse(types[i], value_str);
                            i--;
                        }
                    }
                }
                else
                {
                    values[i] = ParseSerialized(ref str);
                }
                i++;
            }
            str = str.Remove(0, 2);
            object obj = Activator.CreateInstance(objtype, values);
            MenuItem menuItem = (MenuItem)obj;
            menuItem.Category = (MenuItem.TCategorys)cat;
            return menuItem;
        }
    }
}

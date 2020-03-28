using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace Lab01
{
    class FormCreate
    {
        static int X = 20;
        static int Y = 40;
        static void This_is_Enum(Type Fieldtype,Form2 form)
        {

            ComboBox myCombobox = new ComboBox();
            form.Controls.Add(myCombobox);
            myCombobox.Top = Y;
            myCombobox.Left = X;
            foreach (string i in Enum.GetNames(Fieldtype))
            {
                myCombobox.Items.Add(i);
            }
            X = 20;
            Y = Y + 50;
        }

        static void AddLabel(string LabelName,Form2 form)
        {
            Label myLab = new Label();
            form.Controls.Add(myLab);
            myLab.Text = LabelName;
            myLab.Top = Y;
            myLab.Left = X;
            myLab.Width = 100;
            X = X + 140;
        }

        static void This_is_Field(string type, Form2 form)
        {
            if (type == "Boolean")
            {
                CheckBox myCheckBox = new CheckBox();
                form.Controls.Add(myCheckBox);
                myCheckBox.Top = Y;
                myCheckBox.Left = X;
            }
            else
            {
                TextBox myTextbox = new TextBox();
                form.Controls.Add(myTextbox);
                myTextbox.Top = Y;
                myTextbox.Left = X;
                myTextbox.Width = 150;
            }
            X = 20;
            Y = Y + 50;
        }
        public static void This_is_Object(Type type, Form2 form)
        {
            ComboBox myCombobox = new ComboBox();
            myCombobox.Name = "Comb1." + type.Name;
            form.Controls.Add(myCombobox);
            myCombobox.Top = Y;
            myCombobox.Left = X;
            MenuItem.TCategorys cat = (MenuItem.TCategorys)Enum.Parse(typeof(MenuItem.TCategorys),type.Name);
            foreach (MenuItem item in Form1.MyMenu.FindAll(x => x.Category == cat))
            {
                myCombobox.Items.Add(item.ItemName);
            }   
            Button but1 = new Button();
            but1.Name = "But1."+type.Name;
            form.Controls.Add(but1);
            but1.Top = Y;
            but1.Left = X+200;
            but1.Width = 70;
            but1.Text = "Add";
            Button but2 = new Button();
            but2.Name = "But2." + type.Name;
            form.Controls.Add(but2);
            but2.Top = Y;
            but2.Left = X +300;
            but2.Width = 100;
            but2.Text = "Edit_Preview";
            but1.Click += new System.EventHandler(Form2.But1_Click);
            but2.Click += new System.EventHandler(Form2.But2_Click);
            X = 20;
            Y = Y + 50;

        }
   
        public static void CreateComponents(string type,Form2 form)
        {
            form.Num_of_Params = 0;
            form.myType = Type.GetType(type, false, true);
            foreach (var param in form.myType.GetFields())
            {
                if (param.FieldType.Name != "TCategorys")
                {
                    string fieldType = $"{param.FieldType}";
                    AddLabel($"{param.Name}",form);
                    if (fieldType.IndexOf('+') > 0)
                        This_is_Enum(param.FieldType,form);
                    else if (fieldType.IndexOf("Lab01.") > -1)
                        This_is_Object(param.FieldType, form);
                    else This_is_Field(param.FieldType.Name, form);
                    form.Num_of_Params++;
                }    
            }
            Y = 40;
        }
        public static object[] GetParams(Type myType, Form2 form)
        {
            Type[] types = new Type[form.Num_of_Params];
            object[] values = new object[form.Num_of_Params];
            int i = 0;
            foreach (var param in form.myType.GetFields())
            {
                if (param.FieldType.Name != "TCategorys")
                {
                    types[i] = param.FieldType;
                    i++;
                }
            }
            i = 0;
            for (int a = 0; a < form.Controls.Count; a++)
            {
                if (form.Controls[a] is TextBox)
                {
                    TextBox textBox = (TextBox)form.Controls[a];
                    if (types[i].Name == "Int32")
                    {
                        if (Int32.TryParse(textBox.Text, out int res))
                            values[i] = res;
                        else
                        {
                            MessageBox.Show("Unable to get Int32 value");
                            return null;
                        }
                    }
                    if (types[i].Name == "String")
                        values[i] = textBox.Text;
                    if (types[i].Name == "Double")
                    {
                        try
                        {
                            double res = Convert.ToDouble(textBox.Text);
                            values[i] = res;
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("Unable to get Double value");
                            return null;
                        }
                    }
                    i++;
                }

                if (form.Controls[a] is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)form.Controls[a];
                    if (comboBox.SelectedItem != null)
                    {
                        if (comboBox.Name.IndexOf("Comb1.") > -1)
                        {
                            values[i] = Form1.MyMenu.Find(x => x.ItemName == comboBox.SelectedItem.ToString());
                        }
                        else
                            values[i] = Enum.Parse(types[i], comboBox.SelectedItem.ToString());
                    }    
                    else
                    {
                        MessageBox.Show("One of the categiries was not chosen");
                        return null;
                    }
                    i++;
                }
                if (form.Controls[a] is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)form.Controls[a];
                    if (checkBox.Checked) values[i] = true;
                    else values[i] = false;
                    i++;
                }
            }
            return values;
        }

        public static MenuItem CreateItem(object[] values, Type myType)
        {
            object obj = Activator.CreateInstance(myType, values);
            return (MenuItem)obj;
        }

        public static void InitComponents(int index, Type type, Form2 form)
        {
            object res = Form1.MyMenu.ElementAt<object>(index);
            object[] values = new object[form.Num_of_Params];
            int i = 0;
            foreach (FieldInfo param in type.GetFields())
            {
                if (param.FieldType.Name!= "TCategorys")
                {
                    values[i] = param.GetValue(res);
                    i++;
                }      
            }
            i = 0;
            for (int a = 0; a < form.Controls.Count; a++)
            {

                if (form.Controls[a] is TextBox)
                {
                    TextBox textBox = (TextBox)form.Controls[a];
                    textBox.Text = values[i].ToString();
                    i++;
                }

                if (form.Controls[a] is ComboBox)
                {
                    ComboBox comboBox = (ComboBox)form.Controls[a];
                    if (comboBox.Name.IndexOf("Comb1.") > -1)
                    {
                        MenuItem obj = (MenuItem)values[i];
                        comboBox.Text = obj.ItemName;
                    }
                    else
                        comboBox.Text=values[i].ToString();
                    i++;
                }
                if (form.Controls[a] is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)form.Controls[a];
                    checkBox.Checked = (bool)values[i];
                    i++;
                }
            }
        }
    }
}

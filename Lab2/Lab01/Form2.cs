using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
    public partial class Form2 : Form
    {
        delegate void ShowMethod(Form2 form);
        static Form2 ParentForm;
        Type myType;
        int Num_of_Params;
        int fieldInd;
        static int X = 20;
        static int Y = 40;
        
        Form1.TOperationType Op;
        
        public Form2(string type, int index, Form1.TOperationType op)
        {
            InitializeComponent();
            Op = op;
            this.button1.Text = op.ToString();
            fieldInd = index;
            CreateComponents(type, this);
            if (op != Form1.TOperationType.add)
            {
                InitComponents(index, myType, this);
            }
        }
        static void AddItem(Type type, Form2 form)
        {
            object[] values = GetParams(type, form);
            if (values != null)
            {
                MenuItem obj = CreateItem(values, type);
                if (type.Name == "Combo")
                {
                    Combo combo = (Combo)obj;
                    combo.Calories = combo.ComboBurger.Calories + combo.ComboDrink.Calories + combo.ComboGarneer.Calories;
                    obj = combo;
                }
                if (!Form1.MyMenu.Exists(x => x.ItemName == obj.ItemName && x.Price == obj.Price && x.Calories == obj.Calories))
                {
                    Form1.MyMenu.Add(obj);
                    AddLinetoListView();
                }
            }
        }

        static void EditItem(int index, Type type, Form2 form)
        {
            object[] values = GetParams(type, form);
            if (values != null)
            {
                MenuItem obj = CreateItem(GetParams(type, form), type);
                if (type.Name == "Combo")
                {
                    Combo combo = (Combo)obj;
                    combo.Calories = combo.ComboBurger.Calories + combo.ComboDrink.Calories + combo.ComboGarneer.Calories;
                    Form1.MyMenu.RemoveAt(index);
                    Form1.MyMenu.Insert(index, obj);
                }
                else
                {
                    Form1.MyMenu.RemoveAt(index);
                    Form1.MyMenu.Insert(index, obj);
                }
            }
        }
        static void DeleteItem(int index)
        {
            Form1.MyMenu.RemoveAt(index);
            Form1.MainForm.listView1.Items.RemoveAt(index);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (Op)
            {
                case Form1.TOperationType.add:
                    AddItem(myType, this);
                    break;
                case Form1.TOperationType.edit:
                    EditItem(fieldInd, myType, this);
                    break;
                case Form1.TOperationType.delete:
                    DeleteItem(fieldInd);
                    break;
            }
            ShowMethod show = ShowList;
            show.Invoke(this);
            show += reNewComboBox;
            if (ParentForm != null)
                show.Invoke(ParentForm);
            else
                show.Invoke(this);
            this.Close();
        }

        public static void But1_Click(object sender, EventArgs e)
        {
            string typename = String.Concat("Lab01.", ((sender as Button).Name).Remove(0, 5));
            Form2 form = (Form2)(sender as Button).Parent;
            string controlName = String.Concat("Comb1.", ((sender as Button).Name).Remove(0, 5));
            Control[] controls = (form.Controls).Find(controlName, false);
            ParentForm = (Form2)(sender as Button).Parent;//
            Form2 AddForm = new Form2(typename, 0, Form1.TOperationType.add);
            AddForm.Show();
        }

        public static void But2_Click(object sender, EventArgs e)
        {
            string typename = String.Concat("Lab01.", ((sender as Button).Name).Remove(0, 5));
            Form2 form = (Form2)(sender as Button).Parent;
            string controlName = String.Concat("Comb1.", ((sender as Button).Name).Remove(0, 5));
            Control[] controls = (form.Controls).Find(controlName, false);
            if ((controls[0] as ComboBox).SelectedItem != null)
            {
                int index = Form1.MyMenu.FindIndex(x => x.ItemName == (controls[0] as ComboBox).SelectedItem.ToString());
                ParentForm = (Form2)(sender as Button).Parent;//
                Form2 AddForm = new Form2(typename, index, Form1.TOperationType.edit);
                AddForm.Show();
            }
        }
       
        static void This_is_Enum(Type Fieldtype, Form2 form)
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

        static void AddLabel(string LabelName, Form2 form)
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
         static void This_is_Object(Type type, Form2 form)
        {
            ComboBox myCombobox = new ComboBox();
            myCombobox.Name = "Comb1." + type.Name;
            form.Controls.Add(myCombobox);
            myCombobox.Top = Y;
            myCombobox.Left = X;
            MenuItem.TCategorys cat = (MenuItem.TCategorys)Enum.Parse(typeof(MenuItem.TCategorys), type.Name);
            foreach (MenuItem item in Form1.MyMenu.FindAll(x => x.Category == cat))
            {
                myCombobox.Items.Add(item.ItemName);
            }
            Button but1 = new Button();
            but1.Name = "But1." + type.Name;
            form.Controls.Add(but1);
            but1.Top = Y;
            but1.Left = X + 200;
            but1.Width = 70;
            but1.Text = "Add";
            Button but2 = new Button();
            but2.Name = "But2." + type.Name;
            form.Controls.Add(but2);
            but2.Top = Y;
            but2.Left = X + 300;
            but2.Width = 100;
            but2.Text = "Edit_Preview";
            but1.Click += new System.EventHandler(Form2.But1_Click);
            but2.Click += new System.EventHandler(Form2.But2_Click);
            X = 20;
            Y = Y + 50;

        }

        static int Get_TextBox(int i, TextBox textBox, Type[] types, object[] values)
        {
            if (types[i].Name == "Int32")
            {
                if (Int32.TryParse(textBox.Text, out int res))
                    values[i] = res;
                else
                {
                    MessageBox.Show("Unable to get Int32 value");
                    return -1;
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
                    return -1;
                }
            }
            return 0;
        }

        static int Get_ComboBox(int i, ComboBox comboBox, Type[] types, object[] values)
        {
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
                return -1;
            }
            return 0;
        }

        static int Get_CheckBox(int i, CheckBox checkBox, Type[] types, object[] values)
        {
            if (checkBox.Checked) values[i] = true;
            else values[i] = false;
            return 0;
        }

        static void CreateComponents(string type, Form2 form)
        {
            form.Num_of_Params = 0;
            form.myType = Type.GetType(type, false, true);
            foreach (var param in form.myType.GetFields())
            {
                if (param.FieldType.Name != "TCategorys")
                {
                    string fieldType = $"{param.FieldType}";
                    AddLabel($"{param.Name}", form);
                    if (fieldType.IndexOf('+') > 0)
                        This_is_Enum(param.FieldType, form);
                    else if (fieldType.IndexOf("Lab01.") > -1)
                        This_is_Object(param.FieldType, form);
                    else This_is_Field(param.FieldType.Name, form);
                    form.Num_of_Params++;
                }
            }
            Y = 40;
        }
        static object[] GetParams(Type myType, Form2 form)
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
                    if (Get_TextBox(i, (TextBox)form.Controls[a], types, values) < 0) return null;
                    i++;
                }

                if (form.Controls[a] is ComboBox)
                {
                    if (Get_ComboBox(i, (ComboBox)form.Controls[a], types, values) < 0) return null;
                    i++;
                }
                if (form.Controls[a] is CheckBox)
                {
                    Get_CheckBox(i, (CheckBox)form.Controls[a], types, values);
                    i++;
                }
            }
            return values;
        }

        static MenuItem CreateItem(object[] values, Type myType)
        {
            object obj = Activator.CreateInstance(myType, values);
            return (MenuItem)obj;
        }


        static void InitComponents(int index, Type type, Form2 form)
        {
            object res = Form1.MyMenu.ElementAt<object>(index);
            object[] values = new object[form.Num_of_Params];
            int i = 0;
            foreach (FieldInfo param in type.GetFields())
            {
                if (param.FieldType.Name != "TCategorys")
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
                        comboBox.Text = values[i].ToString();
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
        static void ShowList(Form2 form)
        {
            int i = 0;
            foreach (MenuItem item in Form1.MyMenu)
            {
                Form1.MainForm.listView1.Items[i].SubItems[0].Text = item.ItemName;
                Form1.MainForm.listView1.Items[i].SubItems[1].Text = item.Category.ToString();
                Form1.MainForm.listView1.Items[i].SubItems[2].Text = item.Calories.ToString();
                Form1.MainForm.listView1.Items[i].SubItems[3].Text = item.Price.ToString();
                i++;
            }
        }
        static void reNewComboBox(Form2 form)
        {
            for (int a = 0; a < form.Controls.Count; a++)
            {
                if (form.Controls[a] is ComboBox)
                {
                    ComboBox comboBox = form.Controls[a] as ComboBox;
                    if (comboBox.Name == "Comb1.Burger" || comboBox.Name == "Comb1.Garneer" || comboBox.Name == "Comb1.Drink_Desert")
                    {
                        object savedtext = comboBox.SelectedItem;
                        comboBox.Items.Clear();
                        MenuItem.TCategorys cat = (MenuItem.TCategorys)Enum.Parse(typeof(MenuItem.TCategorys), comboBox.Name.Remove(0, 6));
                        foreach (MenuItem item in Form1.MyMenu.FindAll(x => x.Category == cat))
                        {
                            (form.Controls[a] as ComboBox).Items.Add(item.ItemName);
                        }
                        if (savedtext != null && comboBox.Items.Contains(savedtext))
                        {
                            comboBox.SelectedItem = savedtext;
                        }
                        else
                            comboBox.Text = "";
                    }
                }
            }
        }

        public static void AddLinetoListView()
        {
            ListViewItem lvi = new ListViewItem();
            ListViewItem.ListViewSubItem lvsi1 = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lvsi2 = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lvsi3 = new ListViewItem.ListViewSubItem();
            lvi.SubItems.Add(lvsi1);
            lvi.SubItems.Add(lvsi2);
            lvi.SubItems.Add(lvsi3);
            Form1.MainForm.listView1.Items.Add(lvi);
        }
    }
}


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
        public Type myType;
        public int Num_of_Params;
        Form1.TOperationType Op;
        int fieldInd;
        public Form2(string type, int index, Form1.TOperationType op)
        {
            InitializeComponent();
            Op = op;
            this.button1.Text = op.ToString();
            fieldInd = index;
            FormCreate.CreateComponents(type, this);
            if (op != Form1.TOperationType.add)
            {
                FormCreate.InitComponents(index, myType, this);
            }
        }
        static void AddItem(Type type, Form2 form)
        {
            object[] values = FormCreate.GetParams(type, form);
            if (values != null)
            {
                MenuItem obj = FormCreate.CreateItem(values, type);
                if (type.Name == "Combo")
                {
                    Combo combo = (Combo)obj;
                    combo.Calories = combo.ComboBurger.Calories + combo.ComboDrink.Calories + combo.ComboGarneer.Calories;
                    obj = combo;
                }
                if (!Form1.MyMenu.Exists(x => x.ItemName == obj.ItemName && x.Price == obj.Price && x.Calories == obj.Calories))
                {
                    Form1.MyMenu.Add(obj);
                    MyDelegetes.AddLinetoListView();
                }
            }
        }

        static void EditItem(int index, Type type, Form2 form)
        {
            object[] values = FormCreate.GetParams(type, form);
            if (values != null)
            {
                MenuItem obj = FormCreate.CreateItem(FormCreate.GetParams(type, form), type);
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
            ShowMethod show = MyDelegetes.ShowList;
            show.Invoke(this);
            show += MyDelegetes.reNewComboBox;
            if (MyDelegetes.ParentForm != null)
                show.Invoke(MyDelegetes.ParentForm);
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
            MyDelegetes.ParentForm = (Form2)(sender as Button).Parent;//
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
                MyDelegetes.ParentForm = (Form2)(sender as Button).Parent;//
                Form2 AddForm = new Form2(typename, index, Form1.TOperationType.edit);
                AddForm.Show();
            }
        }
    }
}

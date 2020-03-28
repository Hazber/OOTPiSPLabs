using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Lab01
{
    public class MyDelegetes
    {
        public static Form2 ParentForm;
        public static void ShowList(Form2 form)
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
        public static void reNewComboBox(Form2 form)
        {
            for (int a = 0; a < form.Controls.Count; a++)
            {
                if (form.Controls[a] is ComboBox)
                {
                    ComboBox comboBox = form.Controls[a] as ComboBox;
                    if (comboBox.Name == "Comb1.Burgers" || comboBox.Name == "Comb1.Garneers" || comboBox.Name == "Comb1.Drinks_Deserts")
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

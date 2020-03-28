using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
    public partial class Form1 : Form
    {
        public enum TOperationType { add, edit, delete, browse };
        public static List<MenuItem> MyMenu = new List<MenuItem>();
        public static Form1 MainForm=null;
        public Form1()
        {
            InitializeComponent();
            MainForm = this;
            ColumnHeader header1, header2, header3, header4;
            header1 = new ColumnHeader();
            header2 = new ColumnHeader();
            header3 = new ColumnHeader();
            header4 = new ColumnHeader();

            header1.Text = "Item Name";
            header1.Width = 400;
            header2.Text = "Category";
            header2.Width = 250;
            header3.Text = "Calories";
            header3.Width = 100;
            header4.Text = "Price";
            header4.Width = 150;

            listView1.Columns.Add(header1);
            listView1.Columns.Add(header2);
            listView1.Columns.Add(header3);
            listView1.Columns.Add(header4);

            listView1.View = View.Details;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string typename = String.Concat("Lab01.", comboBox1.SelectedItem.ToString());
                Form2 AddForm = new Form2(typename, 0, TOperationType.add);
                AddForm.Show();
            }
            else MessageBox.Show("Choose the category to add");
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (int index in listView1.SelectedIndices)
            {
                string typename = String.Concat("Lab01.", listView1.Items[index].SubItems[1].Text);
                Form2 AddForm = new Form2(typename, index, TOperationType.edit);
                AddForm.Show();
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (int index in listView1.SelectedIndices)
            {
                string typename = String.Concat("Lab01.", listView1.Items[index].SubItems[1].Text);
                Form2 AddForm = new Form2(typename, index, TOperationType.delete);
                AddForm.Show();
            }
        }

        private void описаниеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (int index in listView1.SelectedIndices)
            {
                string typename = String.Concat("Lab01.", listView1.Items[index].SubItems[1].Text);
                Form2 AddForm = new Form2(typename, index, TOperationType.browse);
                AddForm.Show();
            }
        }
    }
}

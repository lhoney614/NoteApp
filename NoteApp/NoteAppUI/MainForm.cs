using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NoteApp;
using Enum = System.Enum;

namespace NoteAppUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            comboBox1.DataSource = Enum.GetValues(typeof(NoteCategory));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Note note = new Note(DateTime.Now);

            note.Title = textBox1.Text;
            note.Text = textBox2.Text;
            note.IsChanged = DateTime.Now;

            string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/NoteApp/";

            Project serialize = new Project {Notes = {note}};
            ProjectManager.SaveToFile(serialize, defaultPath);

            Project deserialize = ProjectManager.LoadFromFile(defaultPath);

        }
    }
}

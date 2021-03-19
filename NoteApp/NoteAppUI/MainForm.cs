using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NoteApp;
using Enum = System.Enum;

namespace NoteAppUI
{
    public partial class MainForm : Form
    {
        private Project _project = new Project();
        Note note = new Note();

        public MainForm()
        {
            InitializeComponent();
           
            //Источником данных для списка является класс-перечисление "Категория заметки"
            comboBox1.DataSource = Enum.GetValues(typeof(NoteCategory));
            label1.Text = "";

            //Отображение времени создания и изменения заметки
            textBox3.Text = note.IsCreated.ToLongTimeString();
            textBox4.Text = note.IsChanged.ToLongTimeString();
        }

        /// <summary>
        /// Обработка события нажатия на кнопку "ОК"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Заносится название заметки
            try
            {
                note.Title = textBox1.Text;
                label1.Text = "";
            }
            catch (ArgumentException exception)
            {
                label1.Text = exception.Message;
                label1.ForeColor = Color.Red;
            }
            
            //Заносится содержимое заметки
            note.Text = textBox2.Text;

            //Меняет значение категории заметки
            note.Category = (NoteCategory)Enum.Parse(typeof(NoteCategory), comboBox1.Text);

            //Время последнего изменения обновляется
            textBox4.Text = note.IsChanged.ToLongTimeString();

            _project.Notes.Add(note);

            //Сохранение файла
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Загрузка из файла
            _project = ProjectManager.LoadFromFile(ProjectManager.FileName);
            note = _project.Notes[0];

            comboBox1.SelectedItem = note.Category;

            textBox1.Text = note.Title;
            textBox2.Text = note.Text;
            textBox3.Text = note.IsCreated.ToLongTimeString();
            textBox4.Text = note.IsChanged.ToLongTimeString();
        }
    }
}

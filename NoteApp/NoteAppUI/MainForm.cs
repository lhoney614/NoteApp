﻿using System;
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
        Note note = new Note(DateTime.Now);

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

            //Время последнего изменения обновляется
            textBox4.Text = note.IsChanged.ToLongTimeString();

            //Путь к папке, лежащей в "Мои документы", где будет храниться файл NoteApp.notes
            string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/NoteApp/";

            //Сохранение файла
            Project serialize = new Project {Notes = {note}};
            ProjectManager.SaveToFile(serialize, defaultPath);

            //Загрузка из файла
            Project deserialize = ProjectManager.LoadFromFile(defaultPath);
        }
    }
}

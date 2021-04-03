using System;
using System.Drawing;
using System.Windows.Forms;
using NoteApp;

namespace NoteAppUI
{
    /// <summary>
    /// Форма NoteForm, выполняющая редактирование и добавление заметки
    /// </summary>
    public partial class NoteForm : Form
    {
        /// <summary>
        /// Создание локальной переменной типа Note
        /// Исопльзуется для создания и изменения заметки
        /// </summary>
        private Note _note = new Note();

        /// <summary>
        /// Создание доступной для другой формы переменной типа Note
        /// </summary>
        public Note Note
        {
            get => _note;
            set
            {
                _note = value;
                DisplayNote();
            }
        }

        /// <summary>
        /// Загрузка формы NoteForm
        /// </summary>
        public NoteForm()
        {
            InitializeComponent();

            //Источником данных для списка является класс-перечисление "Категория заметки"
            CategoryComboBox.DataSource = Enum.GetValues(typeof(NoteCategory));
        }

        /// <summary>
        /// Реализация отображения содержимого формы EditNote
        /// в зависимости от содержимого переданной заметки:
        /// либо для редактирования, либо создание новой
        /// </summary>
        public void DisplayNote()
        {
            //Обработка передачи пустой заметки для ее создания
            if (_note == null)
            {
                TimeCreatedComboBox.Text = DateTime.Now.ToShortDateString() +
                                           @" " + DateTime.Now.ToLongTimeString();
                TimeChangedComboBox.Text = DateTime.Now.ToShortDateString() +
                                           @" " + DateTime.Now.ToLongTimeString();
                TitleBox.Text = @"<Без названия>";
                return;
            }

            //Обработка передачи заметки для редактирования
            TitleBox.Text = _note.Title;
            TextBox.Text = _note.Text;
            CategoryComboBox.SelectedItem = _note.Category;
            TimeCreatedComboBox.Text = _note.IsCreated.ToShortDateString() +
                             @" " + _note.IsCreated.ToLongTimeString();
            TimeChangedComboBox.Text = _note.IsChanged.ToShortDateString() +
                             @" " + _note.IsChanged.ToLongTimeString();
        }

        /// <summary>
        /// Изменения содержимого TitleBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _note.Title = TitleBox.Text;
            }
            catch
            {
                //Обработка исключения, если длина названия больше 50 символов
                TitleBox.BackColor = Color.LightCoral;
            }
        }

        /// <summary>
        /// Изменене выбранного элемента CategoryComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _note.Category = (NoteCategory)CategoryComboBox.SelectedItem;
        }

        /// <summary>
        /// Изменения содержимого TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            _note.Text = TextBox.Text;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку "ОК"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            if (TitleBox.Text.Length > 50)
            {
                MessageBox.Show(@"Длина названия не должна превышать 50 символов");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Обработка события нажатия на кнопку "Cancel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

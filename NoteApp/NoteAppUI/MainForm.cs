using System;
using System.ComponentModel;
using System.Windows.Forms;
using NoteApp;
using Enum = System.Enum;

namespace NoteAppUI
{
    /// <summary>
    /// Форма MainForm, значащаяся как главное окно
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Создание переменной типа Project
        /// Предназначена для  загрузки из файла
        /// и последующего сохранения в файл всех заметок
        /// </summary>
        private readonly Project _project;

        /// <summary>
        /// Переменная, являющаяся источником данных
        /// всех заметок для NotesListBox
        /// </summary>
        private BindingList<Note> _notes;

        /// <summary>
        /// Загрузка формы MainForm
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //Загрузка заметок из файла
            _project = ProjectManager.LoadFromFile(ProjectManager.FileName);

            //Источником данных для списка категорий
            //является класс-перечисление "Категория заметки"
            categoryBox.DataSource = Enum.GetValues(typeof(NoteCategory));

            //Занесение списка заметок в компонент NotesListBox
            //с помощью привязки данных
            UpdateNotesListBox();
            NotesListBox.DisplayMember = "Title";
        }

        /// <summary>
        /// Обновление содержимого компонента NotesListBox
        /// </summary>
        private void UpdateNotesListBox()
        {
            _notes = _project.Notes;
            NotesListBox.DataSource = _notes;
        }

        /// <summary>
        /// Обработка события нажатия на кнопку "New"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewNoteButton_Click(object sender, EventArgs e) => AddNote();

        /// <summary>
        /// Реализация алгоритма создания новой заметки
        /// через форму NoteForm
        /// </summary>
        private void AddNote()
        {
            //Создание экземпляра формы EditNote
            var addForm = new NoteForm();
            addForm.ShowDialog();

            //Обработка закрытия формы NoteForm
            if (addForm.DialogResult == DialogResult.OK)
            {
                //Передача данных из формы NoteForm в форму MainForm
                //Обновление списка заметок
                var addedNote = addForm.Note;
                
                _project.Notes.Add(addedNote);

            }
            else return;

            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }
        
        /// <summary>
        /// Обработка события нажатия на кнопку "Edit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditNoteButton_Click(object sender, EventArgs e) => EditNote();

        /// <summary>
        /// Реализация алгоритма редактирования заметки
        /// </summary>
        private void EditNote()
        {
            var selectedIndex = NotesListBox.SelectedIndex;

            //Обработка исключения, если ни одна заметка не выбрана
            if (selectedIndex == -1)
            {
                MessageBox.Show(@"Не выбрана запись для редактирования", @"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedNote = _project.Notes[selectedIndex];

                //Передача форме NoteForm данных выбранной заметки
                var editForm = new NoteForm {Note = selectedNote};
                editForm.ShowDialog();

                //Обработка закрытия формы EditNote
                if (editForm.DialogResult == DialogResult.OK)
                {
                    var editedNote = editForm.Note;

                    //Вставка измененной заметки в список с
                    //последующим удалением старой версии заметки
                    _project.Notes.RemoveAt(selectedIndex);
                    _project.Notes.Insert(selectedIndex, editedNote);
                }

                else return;
            }
            catch
            {
                MessageBox.Show(@"Запись не найдена", @"Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }

        /// <summary>
        /// Обработка события нажатия на кнопку "Remove"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveNoteButton_Click(object sender, EventArgs e) => RemoveNote();

        /// <summary>
        /// Реализация алгоритма удаления заметки
        /// </summary>
        private void RemoveNote()
        {
            var selectedIndex = NotesListBox.SelectedIndex;

            //Обработка события, если заметка не выбрана
            if (selectedIndex == -1)
            {
                MessageBox.Show(@"Не выбрана запись для удаления", @"Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Ожидание подтверждения удаления заметки пользователем
            var dialogResult = MessageBox.Show(@"Вы действительно хотите удалить запись?", 
                @"Удаление записи", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            //Обработка положительного ответа
            if (dialogResult == DialogResult.OK)
            {
                //Удаление заметки
                _project.Notes.RemoveAt(selectedIndex);
            }
            
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }

        /// <summary>
        /// Обработка события изменения выбранной заметки
        /// Отображение содержания заметки в форме MainForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedNote = (Note)NotesListBox.SelectedItem;

            //Обработка события, если список пуст
            if (NotesListBox.SelectedItem == null)
            {
                TitleLabel.Text = @"Без названия";
                SelectedCategoryLabel.Text = @"";
                TextBox.Text = @"";
                TimeCreated.Text = @"";
                TimeChanged.Text = @"";
                return;
            }

            TitleLabel.Text = selectedNote.Title;
            SelectedCategoryLabel.Text = selectedNote.Category.ToString();
            TextBox.Text = selectedNote.Text;
            TimeCreated.Text = ToFormattedTime(selectedNote.IsCreated);
            TimeChanged.Text = ToFormattedTime(selectedNote.IsChanged);
        }

        private string ToFormattedTime(DateTime time)
        {
            return time.ToShortDateString() + @" " + time.ToLongTimeString();
        }

        /// <summary>
        /// Обработка события выбора содержимого StripMenu
        /// варианта "Exit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
            this.Close();
        }


        /// <summary>
        /// Обработка события выбора содержимого StripMenu
        /// варианта "Add Note"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addNoteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddNote();
        }

        /// <summary>
        /// Обработка события выбора содержимого StripMenu
        /// варианта "Edit Note"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editNoteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EditNote();
        }

        /// <summary>
        /// Обработка события выбора содержимого StripMenu
        /// варианта "Remove Note"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeNoteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RemoveNote();
        }

        /// <summary>
        /// Обработка события выбора содержимого StripMenu
        /// варианта "About"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }
    }
}

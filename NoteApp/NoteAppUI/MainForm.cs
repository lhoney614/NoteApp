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
        private readonly Project _project;
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
            _notes = new BindingList<Note>(_project.Notes);
            NotesListBox.DataSource = _notes;
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
        /// через форму EditForm
        /// </summary>
        private void AddNote()
        {
            //Создание экземпляра формы EditNote
            var addForm = new EditForm();
            addForm.ShowDialog();

            //Обработка закрытия формы EditForm
            if (addForm.DialogResult == DialogResult.OK)
            {
                //Передача данных из формы EditForm в форму MainForm
                //Обновление списка заметок
                var addedNote = addForm.Note;
                
                _project.Notes.Add(addedNote);
                UpdateNotesListBox();

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

                //Передача форме EditForm данных выбранной заметки
                var editForm = new EditForm {Note = selectedNote};
                editForm.ShowDialog();

                //Обработка закрытия формы EditNote
                if (editForm.DialogResult == DialogResult.OK)
                {
                    var editedNote = editForm.Note;

                    //Вставка измененной заметки в список с
                    //последующим удалением старой версии заметки
                    _project.Notes.Insert(selectedIndex, editedNote);
                    _project.Notes.RemoveAt(selectedIndex + 1);

                    //Обновление списка заметок
                    UpdateNotesListBox();
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
                //Удаление заметки и обновление списка заметок
                _project.Notes.RemoveAt(selectedIndex);
                UpdateNotesListBox();
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
            if (selectedNote == null)
            {
                TitleLabel.Text = @"Без названия";
                TextBox.Text = @"";
                TimeCreatedLabel.Text = @"";
                TimeChangedLabel.Text = @"";
                return;
            }

            TitleLabel.Text = selectedNote.Title;
            SelectedCategoryLabel.Text = selectedNote.Category.ToString();
            TextBox.Text = selectedNote.Text;
            TimeCreatedLabel.Text = selectedNote.IsCreated.ToShortDateString() + @" " + selectedNote.IsCreated.ToLongTimeString();
            TimeChangedLabel.Text = selectedNote.IsChanged.ToShortDateString() + @" " + selectedNote.IsChanged.ToLongTimeString();
        }

        /// <summary>
        /// Обработка события выбора содержимого StripMenu
        /// варианта "Exit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
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

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
        /// Переменная, хранящая отсортированный список
        /// всех заметок
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
            AddCategoryToCategoryBox();
            
            //При загрузке изначально высвечивается категория "All"
            categoryBox.SelectedIndex = 0;

            //Занесение списка заметок в компонент NotesListBox
            //с помощью привязки данных
            UpdateNotesListBox();
            NotesListBox.DisplayMember = "Title";

            //Присвоение индекса текущей заметки, которая
            //была открыта при предыдущем сеансе
            if (_notes.Count != 0)
            {
                NotesListBox.SelectedIndex = _project.CurrentNoteIndex;
            }
        }

        /// <summary>
        /// Создание списка категорий в comboBox
        /// </summary>
        private void AddCategoryToCategoryBox()
        {
            categoryBox.Items.Add("All");
            categoryBox.Items.Add(Enum.ToObject(typeof(NoteCategory), 0));
            categoryBox.Items.Add(Enum.ToObject(typeof(NoteCategory), 1));
            categoryBox.Items.Add(Enum.ToObject(typeof(NoteCategory), 2));
            categoryBox.Items.Add(Enum.ToObject(typeof(NoteCategory), 3));
            categoryBox.Items.Add(Enum.ToObject(typeof(NoteCategory), 4));
            categoryBox.Items.Add(Enum.ToObject(typeof(NoteCategory), 5));
            categoryBox.Items.Add(Enum.ToObject(typeof(NoteCategory), 6));
        }

        /// <summary>
        /// Обновление содержимого компонента NotesListBox
        /// </summary>
        private void UpdateNotesListBox()
        {
            _notes = _project.Notes;
            
            //Для категории "All"
            if (categoryBox.SelectedIndex == 0)
            {
                _notes = _project.SortByEdited(_notes);
            }
            //Для выбранной категории
            else
            {
                _notes = _project.SortByEditedAndCategory(_notes, (NoteCategory)categoryBox.SelectedItem);
            }

            NotesListBox.DataSource = _notes;
        }

        /// <summary>
        /// Обновление значения индекса текущей заметки
        /// </summary>
        private void UpdateCurrentNoteIndex()
        {
            if (NotesListBox.Items.Count != 0)
            {
                _project.CurrentNoteIndex = NotesListBox.SelectedIndex;
            }
            else
            {
                _project.CurrentNoteIndex = 0;
            }
        }

        /// <summary>
        /// Обновление показываемой информации о заметке
        /// при пустом списке заметок
        /// </summary>
        private void UpdateEmptyListBox()
        {
            TitleLabel.Text = @"Без названия";
            SelectedCategoryLabel.Text = @"";
            TextBox.Text = @"";
            TimeCreated.Text = @"";
            TimeChanged.Text = @"";
        }

        /// <summary>
        /// Возвращает значение времени
        /// в определенном формате
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private string ToFormattedTime(DateTime time)
        {
            return time.ToShortDateString() + @" " + time.ToShortTimeString();
        }

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

            UpdateNotesListBox();
            UpdateCurrentNoteIndex();
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }

        /// <summary>
        /// Реализация алгоритма редактирования заметки
        /// </summary>
        private void EditNote()
        {
            var selectedIndex = NotesListBox.SelectedIndex;

            //Обработка исключения, если ни одна заметка не выбрана
            if (selectedIndex == -1)
            {
                MessageBox.Show(@"Note for editing is not selected", @"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedNote = _notes[selectedIndex];
                var notesSelectedIndex = _project.Notes.IndexOf(selectedNote);

                //Передача форме NoteForm данных выбранной заметки
                var editForm = new NoteForm {Note = selectedNote};
                editForm.ShowDialog();

                //Обработка закрытия формы EditNote
                if (editForm.DialogResult == DialogResult.OK)
                {
                    var editedNote = editForm.Note;
                    

                    //Вставка измененной заметки в список с
                    //последующим удалением старой версии заметки
                    _project.Notes.RemoveAt(notesSelectedIndex);
                    _project.Notes.Insert(notesSelectedIndex, editedNote);
                }

                else return;
            }
            catch
            {
                MessageBox.Show(@"Note not found", @"Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateNotesListBox();
            UpdateCurrentNoteIndex();
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }

        /// <summary>
        /// Реализация алгоритма удаления заметки
        /// </summary>
        private void RemoveNote()
        {
            var selectedIndex = NotesListBox.SelectedIndex;
            var selectedNote = _notes[selectedIndex];
            var notesSelectedIndex = _project.Notes.IndexOf(selectedNote);

            //Обработка события, если заметка не выбрана
            if (selectedIndex == -1)
            {
                MessageBox.Show(@"Note for deletion not selected", @"Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Ожидание подтверждения удаления заметки пользователем
            var dialogResult = MessageBox.Show(@"Are you sure you want to delete the note?", 
                @"Deleting a note", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            //Обработка положительного ответа
            if (dialogResult == DialogResult.OK)
            {
                //Удаление заметки
                _project.Notes.RemoveAt(notesSelectedIndex);
            }

            UpdateNotesListBox();
            UpdateCurrentNoteIndex();
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);

            if (_notes.Count == 0)
            {
                UpdateEmptyListBox();
            }
        }

        /// <summary>
        /// Обработка события нажатия на кнопку "New"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewNoteButton_Click(object sender, EventArgs e) => AddNote();
        
        /// <summary>
        /// Обработка события нажатия на кнопку "Edit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditNoteButton_Click(object sender, EventArgs e) => EditNote();

        /// <summary>
        /// Обработка события нажатия на кнопку "Remove"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveNoteButton_Click(object sender, EventArgs e) => RemoveNote();
        
        /// <summary>
        /// Обработка события изменения выбранной категории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void categoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNotesListBox();
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

            //Обработка события, если список не пуст
            if (NotesListBox.SelectedItem != null)
            {
                TitleLabel.Text = selectedNote.Title;
                SelectedCategoryLabel.Text = selectedNote.Category.ToString();
                TextBox.Text = selectedNote.Text;
                TimeCreated.Text = ToFormattedTime(selectedNote.IsCreated);
                TimeChanged.Text = ToFormattedTime(selectedNote.IsChanged);
                return;
            }
            
            UpdateEmptyListBox();
        }
        
        /// <summary>
        /// Обработка события нажатия кнопки "Delete"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotesListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                RemoveNote();
            }
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
            var aboutForm = new AboutForm();
            aboutForm.Show();
        }

        /// <summary>
        /// Обработка события выбора содержимого StripMenu
        /// варианта "Exit"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UpdateCurrentNoteIndex();
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
            this.Close();
        }

        /// <summary>
        /// Обработка закрытия формы через значок "крестика"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateCurrentNoteIndex();
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }
    }
}

using System;
using System.ComponentModel;
using System.Windows.Forms;
using NoteApp;
using Enum = System.Enum;

namespace NoteAppUI
{
    public partial class MainForm : Form
    {
        private readonly Project _project;
        private BindingList<Note> _notes;

        public MainForm()
        {
            InitializeComponent();
            _project = ProjectManager.LoadFromFile(ProjectManager.FileName);
            categoryBox.DataSource = Enum.GetValues(typeof(NoteCategory));
            _notes = new BindingList<Note>(_project.Notes);
            NotesListBox.DataSource = _notes;
            NotesListBox.DisplayMember = "Title";
        }

        private void UpdateNotesListBox()
        {
            _notes = _project.Notes;
            NotesListBox.DataSource = _notes;
        }

        private void NewNoteButton_Click(object sender, EventArgs e) => AddNote();

        private void AddNote()
        {
            var addForm = new EditForm();
            addForm.ShowDialog();

            if (addForm.DialogResult == DialogResult.OK)
            {
                var addedNote = addForm.NoteData;
                
                _project.Notes.Add(addedNote);
                UpdateNotesListBox();

            }
            else return;

            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }
        
        private void EditNoteButton_Click(object sender, EventArgs e) => EditNote();

        private void EditNote()
        {
            var selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show(@"Не выбрана запись для редактирования", @"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedNote = _project.Notes[selectedIndex];

                var editForm = new EditForm {NoteData = selectedNote};
                editForm.ShowDialog();

                if (editForm.DialogResult == DialogResult.OK)
                {
                    var editedNote = editForm.NoteData;

                    _project.Notes.Insert(selectedIndex, editedNote);
                    _project.Notes.RemoveAt(selectedIndex + 1);
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

        private void RemoveNoteButton_Click(object sender, EventArgs e) => RemoveNote();

        private void RemoveNote()
        {
            var selectedIndex = NotesListBox.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show(@"Не выбрана запись для удаления", @"Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dialogResult = MessageBox.Show(@"Вы действительно хотите удалить запись?", 
                @"Удаление записи", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.OK)
            {
                _project.Notes.RemoveAt(selectedIndex);
                UpdateNotesListBox();
            }
            
            ProjectManager.SaveToFile(_project, ProjectManager.FileName);
        }

        private void NotesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedNote = (Note) NotesListBox.SelectedItem;

            TitleLabel.Text = selectedNote.Title;
            SelectedCategoryLabel.Text = selectedNote.Category.ToString();
            TextBox.Text = selectedNote.Text;
            TimeCreatedLabel.Text = selectedNote.IsCreated.ToShortDateString() + @" " + selectedNote.IsCreated.ToLongTimeString();
            TimeChangedLabel.Text = selectedNote.IsChanged.ToShortDateString() + @" " + selectedNote.IsChanged.ToLongTimeString();
        }

        private void FileButton_Click(object sender, EventArgs e)
        {
           FileButton.ContextMenuStrip.Show(FileButton, new System.Drawing.Point(0, 20));
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            EditButton.ContextMenuStrip.Show(EditButton, new System.Drawing.Point(0, 20));
        }

        private void HelpAboutButton_Click(object sender, EventArgs e)
        {
            HelpAboutButton.ContextMenuStrip.Show(HelpAboutButton, new System.Drawing.Point(0, 20));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void addNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNote();
        }

        private void editNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditNote();
        }

        private void removeNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveNote();
        }
    }
}

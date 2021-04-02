using System;
using System.Windows.Forms;
using NoteApp;

namespace NoteAppUI
{
    public partial class EditForm : Form
    {
        private Note _noteData = new Note();

        public Note NoteData
        {
            get => _noteData;
            set
            {
                _noteData = value;
                DisplayNote();
            }
        }

        public EditForm()
        {
            InitializeComponent();
            //Источником данных для списка является класс-перечисление "Категория заметки"
            CategoryComboBox.DataSource = Enum.GetValues(typeof(NoteCategory));
        }

        public void DisplayNote()
        {
            if (_noteData == null)
            {
                TimeCreatedComboBox.Text = DateTime.Now.ToShortDateString() +
                                           @" " + DateTime.Now.ToLongTimeString();
                TimeChangedComboBox.Text = DateTime.Now.ToShortDateString() +
                                           @" " + DateTime.Now.ToLongTimeString();
                TitleBox.Text = @"Без названия";
            }

            TitleBox.Text = _noteData.Title;
            TextBox.Text = _noteData.Text;
            CategoryComboBox.SelectedItem = _noteData.Category;
            TimeCreatedComboBox.Text = _noteData.IsCreated.ToShortDateString() +
                             @" " + _noteData.IsCreated.ToLongTimeString();
            TimeChangedComboBox.Text = _noteData.IsChanged.ToShortDateString() +
                             @" " + _noteData.IsChanged.ToLongTimeString();
        }

        private void TitleBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _noteData.Title = TitleBox.Text;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void CategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _noteData.Category = (NoteCategory)CategoryComboBox.SelectedItem;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            _noteData.Text = TextBox.Text;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        
    }
}

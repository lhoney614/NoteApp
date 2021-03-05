using System;

namespace NoteApp
{
    /// <summary>
    /// Хранит название, категорию и текст заметки,
    /// а также время ее создания и последнего изменения
    /// </summary>
    public class Note
    {
        private string _title = "Без названия";
        private NoteCategory _category;
        private string _text;
        private DateTime _isCreated;
        private DateTime _isChanged;
    }
}
using System.Collections.Generic;

namespace NoteApp
{
    /// <summary>
    /// Хранит список всех заметок
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Возвращает список текущих заметок
        /// </summary>
        public List<Note> Notes { get; private set; } = new List<Note>();
    }
}
using System.Collections.Generic;
using System.ComponentModel;

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
        public BindingList<Note> Notes { get; private set; } = new BindingList<Note>();
    }
}
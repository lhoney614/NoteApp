using System.ComponentModel;
using System.Linq;

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

        /// <summary>
        /// Возвращает или задает текущий индекс в ListBox
        /// просматрвиаемой заметки
        /// </summary>
        public int CurrentNoteIndex { get; set; }

        /// <summary>
        /// Возвращает отсортированный в порядке убывания
        /// даты изменения заметки список
        /// </summary>
        /// <param name="notSortedList"></param>
        /// <returns></returns>
        public BindingList<Note> SortByEdited(BindingList<Note> notSortedList)
        {
            return new BindingList<Note>(notSortedList.OrderByDescending(item => item.IsChanged).ToList());
        }

        /// <summary>
        /// Возвращается отсортированный в порядке убывания
        /// даты изменения заметки список определенной категории
        /// </summary>
        /// <param name="notSortedList"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public BindingList<Note> SortByEditedAndCategory(BindingList<Note> notSortedList, NoteCategory category)
        {
            return new BindingList<Note>(notSortedList.Where(item => item.Category == category)
                .OrderByDescending(item => item.IsChanged).ToList());
        }
    }
}
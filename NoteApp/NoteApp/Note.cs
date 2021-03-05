using System;

namespace NoteApp
{
    /// <summary>
    /// Хранит название, категорию и текст заметки,
    /// а также время ее создания и последнего изменения
    /// </summary>
    public class Note : ICloneable
    {
        private string _title = "Без названия";
        private NoteCategory _category;
        private string _text;
        private readonly DateTime _isCreated = DateTime.Now;
        private DateTime _isChanged;


        /// <summary>
        /// Инициализация заметки
        /// </summary>
        /// <param name="dateTime">Время изменения</param>
        public Note(DateTime dateTime)
        {
            this.IsChanged = dateTime;
        }

        /// <summary>
        /// Название заметки
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("Длина названия не должна превышать 50 символов");
                }

                if (value != String.Empty)
                {
                    _title = value;
                }

                IsChanged = DateTime.Now;
            }
        }

        ///<summary>
        /// Категория заметки
        ///</summary>
        public NoteCategory Category
        {
            get
            {
                return _category;
            }

            set
            {
                _category = value;
                IsChanged = DateTime.Now;
            }
        }

        /// <summary>
        /// Текст заметки
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;
                IsChanged = DateTime.Now;
            }
        }

        /// <summary>
        /// Время создания заметки
        /// </summary>
        public DateTime IsCreated
        {
            get
            {
                return _isCreated;
            }
        }

        /// <summary>
        /// Время последнего изменения заметки
        /// </summary>
        public DateTime IsChanged
        {
            get
            {
                return _isChanged;
            }

            set
            {
                _isChanged = DateTime.Now;
            }
        }

        /// <summary>
        /// Метод клонирования объекта
        /// </summary>
        /// <returns>Возвращение копии объекта</returns>
        public object Clone()
        {
            return new Note(DateTime.Now)
            {
                Title = this._title,
                Text = this._text,
                Category = this._category,
                IsChanged = this._isChanged
            };
        }
        
    }
}
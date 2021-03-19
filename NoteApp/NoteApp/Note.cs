using System;

namespace NoteApp
{
    /// <summary>
    /// Хранит название, категорию и текст заметки,
    /// а также время ее создания и последнего изменения
    /// </summary>
    public class Note : ICloneable
    {
        /// <summary>
        /// Название заметки
        /// </summary>
        private string _title = "Без названия";

        /// <summary>
        /// Категория заметки
        /// </summary>
        private NoteCategory _category;

        /// <summary>
        /// Текст заметки
        /// </summary>
        private string _text;

        /// <summary>
        /// Время создания заметки. По умолчанию: только для чтения
        /// </summary>
        private readonly DateTime _isCreated = DateTime.Now;

        /// <summary>
        /// Время изменения файла
        /// </summary>
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
        /// Возвращает или задает значения "Название заметки"
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

                if (value != string.Empty)
                {
                    _title = value;
                }

                IsChanged = DateTime.Now;
            }
        }

        ///<summary>
        ///Возвращает или задает значения "Категория заметки"
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
        /// Возвращает или задает значения "Текст заметки"
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
        /// Возвращает значения "Время создания заметки"
        /// </summary>
        public DateTime IsCreated
        {
            get
            {
                return _isCreated;
            }
        }

        /// <summary>
        /// Возвращает или задает значения "Время последнего изменения"
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
        /// <inheritdoc cref="ICloneable"/>
        /// </summary>
        /// <returns>Возвращает копию объекта</returns>
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
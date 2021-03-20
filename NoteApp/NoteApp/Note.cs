﻿using System;
using Newtonsoft.Json;

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
        private DateTime _isCreated = DateTime.Now;

        /// <summary>
        /// Время изменения файла
        /// </summary>
        private DateTime _isChanged;


        /// <summary>
        /// Возвращает или задает значения "Название заметки"
        /// </summary>
        public string Title
        {
            get => _title;

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
            get => _category;

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
            get => _text;

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
            get => _isCreated;
            private set => _isCreated = value;
        }

        /// <summary>
        /// Конструктор класса Note
        /// </summary>
        public Note()
        {

        }

        /// <summary>
        /// Конструктор класса Note для сериализации
        /// </summary>
        /// <param name="title">Не более 50 символов</param>
        /// <param name="category"></param>
        /// <param name="text"></param>
        /// <param name="isCreated"></param>
        /// <param name="isChanged"></param>
        [JsonConstructor]
        public Note(string title, NoteCategory category, string text, DateTime isCreated, DateTime isChanged)
        {
            Title = title;
            Category = category;
            Text = text;
            IsCreated = isCreated;
            IsChanged = isChanged;
        }

        /// <summary>
        /// Возвращает или задает значения "Время последнего изменения"
        /// </summary>
        public DateTime IsChanged
        {
            get => _isChanged;

            private set => _isChanged = value;
        }
        
        /// <summary>
        /// <inheritdoc cref="ICloneable"/>
        /// </summary>
        /// <returns>Возвращает копию объекта</returns>
        public object Clone()
        {
            return new Note()
            {
                Title = this._title,
                Text = this._text,
                Category = this._category,
                IsChanged = this._isChanged
            };
        }
        
    }
}
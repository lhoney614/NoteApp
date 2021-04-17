using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace NoteApp.UnitTests
{
    /// <summary>
    /// Определение классов юнит-тестирования
    /// </summary>
    [TestFixture]
    public class NoteTest
    {
        private Note _note;

        /// <summary>
        /// Метод, выполняющийся каждый раз перед запуском теста
        /// Создает экзеп=
        /// </summary>
        [SetUp]
        public void InitNote()
        {
            _note = new Note
            {
                Title = "Название заметки",
                Category = NoteCategory.Home,
                Text = "Здесь должен быть текст"
            };
        }
        
        /// <summary>
        /// Проверка на ввод правильного названия заметки
        /// </summary>
        [Test(Description = "Позитивный тест геттера Title")]
        public void TestTitleGet_CorrectValue()
        {
            //Ожидаемое название заметки
            var expected = _note.Title;

            //Присваиваем полю "Название заметки" текст
            _note.Title = expected;

            //Присваиваем полю, хранящему название
            //заметки в реальном времени новое значение
            var actual = _note.Title;

            //Сравниваем ожидаемое значение с тем, что получили
            Assert.AreEqual(expected, actual,
                "Геттер Title возвращает неправильное название заметки");
        }

        /// <summary>
        /// Проверка на ввод пустой строки в название заметки
        /// </summary>
        [Test(Description = "Тест геттера Title с пустым значением")]
        public void TestTitleGet_EmptyValue()
        {
            //Ожидаемое название заметки
            var expected = "Без названия";

            //Присваиваем пустое значение
            _note.Title = "";

            //Присваиваем значение из поля "Нзавание заметки"
            var actual = _note.Title;

            //Сравниваем ожидаемое значение с полученным
            Assert.AreEqual(expected, actual,
                "Геттер Title возвращает неверное название заметки при пустом значении");
        }

        /// <summary>
        /// Проверка на ввод названия заметки более 50 символов
        /// </summary>
        [Test(Description = "Тест геттера Title с длиной значения" +
                            "поля, превышающего 50 символов")]
        public void TestTitleGet_TooLongValue()
        {
            var source = "Слишком длинное название поля," +
                         "превышающее 50 символов и выбрасывающее" +
                         "исключение";

            Assert.Throws<ArgumentException>(
                //Анонимный метод
                () =>
                {
                    //Тестируемый код
                    _note.Title = source;
                }, "Должно возникать исключение, если длина названия" +
                   "более 50 символов");
        }

        /// <summary>
        /// Проверка на ввод любого текста
        /// </summary>
        [Test(Description = "Тест геттера Text")]
        public void TestTextGet_Value()
        {
            var expected = _note.Text;

            _note.Text = expected;

            var actual = _note.Text;

            Assert.AreEqual(expected, actual, 
                "Геттер Text возвращает неправильное значение");
        }

        /// <summary>
        /// Проврека на НЕизменение даты создания заметки
        /// </summary>
        [Test(Description = "Тест геттера IsCreated")]
        public void TestIsCreated_ChangeValue()
        {
            var expected = _note.IsCreated;

            _note.Text = _note.Text;

            var actual = _note.IsCreated;

            Assert.AreEqual(expected, actual, 
                "Геттер IsCreated возвращает неправильное значение");
        }

        /// <summary>
        /// Проверка на изменение даты изменения заметки
        /// </summary>
        [Test(Description = "Тест геттера IsChanged")]
        public void TestIsChanged_ChangeValue()
        {
            var firstValue = _note.IsChanged;

            _note.Text = _note.Text;

            var secondValue = _note.IsChanged;

            Assert.AreNotEqual(firstValue, secondValue,
                "Геттер IsChanged возвращает неправильное значение");
        }

        /// <summary>
        /// Проверка на правильность клонирования
        /// </summary>
        [Test(Description = "Тест клонирования ICloneable")]
        public void TestClone_ReturnsSameClone()
        {
            var expected = (Note)_note.Clone();

            Assert.AreEqual(expected.Title, _note.Title);
            Assert.AreEqual(expected.Text, _note.Text);
            Assert.AreEqual(expected.Category, _note.Category);
            Assert.AreEqual(expected.IsChanged, _note.IsChanged);
        }
    }
}

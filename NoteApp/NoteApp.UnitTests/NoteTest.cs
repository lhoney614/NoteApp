using System;
using System.ComponentModel;
using NUnit.Framework;

namespace NoteApp.UnitTests
{
    /// <summary>
    /// Определение классов юнит-тестирования
    /// </summary>
    [TestFixture]
    public class NoteTest
    {
        /// <summary>
        /// Экземпляр заметки
        /// </summary>
        private Note _sourceNote;

        /// <summary>
        /// Переменная, хранящая время создания заметки
        /// </summary>
        private readonly DateTime _createdTime = new DateTime(2021, 04, 18, 17, 00, 00);

        /// <summary>
        /// Переменная, хранящая время изменения заметки
        /// </summary>
        private readonly DateTime _changedTime = new DateTime(2021, 04, 18, 17, 05, 00);

        /// <summary>
        /// Метод, выполняющийся каждый раз перед запуском теста
        /// Создает экземпляр заметки
        /// </summary>
        [SetUp]
        public void MakeSourceNote()
        {
            _sourceNote = new Note
            {
                Title = "Здесь должен быть текст",
                Category = NoteCategory.Home,
                Text = "Название заметки",
                IsCreated = _createdTime,
                IsChanged = _changedTime
            };
        }
        
        [Test(Description = "Позитивный тест геттера Title")]
        public void Title_GetCorrectValue()
        {
            //Setup
            var expected = "Здесь должен быть текст";

            //Act
            var actual = _sourceNote.Title;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Позитивный тест сеттера Title")]
        public void Title_SetRightValue()
        {
            //Setup
            var expected = _sourceNote.Title;

            //Act
            _sourceNote.Title = expected;
            var actual = _sourceNote.Title;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Тест сеттера Title с пустым значением")]
        public void Title_SetEmptyValue()
        {
            //Setup
            var expected = "Без названия";

            //Act
            _sourceNote.Title = "";
            var actual = _sourceNote.Title;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test(Description = "Тест сеттера Title с длиной значения" +
                            "поля, превышающего 50 символов")]
        public void Title_SetTooLongValue()
        {
            //Setup
            var source = "Слишком длинное название поля," +
                         "превышающее 50 символов и выбрасывающее" +
                         "исключение";

            //Assert
            Assert.Throws<ArgumentException>(
                () =>
                {
                    _sourceNote.Title = source;
                });
        }

        [Test(Description = "Тест геттера Category")]
        public void Category_GetRightCategory()
        {
            //Setup
            var expected = NoteCategory.Home;

            //Act
            var actual = _sourceNote.Category;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Тест сеттера Category")]
        public void Category_SetRightValue()
        {
            //Setup
            var expected = _sourceNote.Category;

            //Act
            _sourceNote.Category = expected;
            var actual = _sourceNote.Category;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test(Description = "Тест геттера Text")]
        public void Text_GetRightText()
        {
            //Setup
            var expected = "Название заметки";

            //Act
            var actual = _sourceNote.Text;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Тест сеттера Text")]
        public void Text_SetRightValue()
        {
            //Setup
            var expected = _sourceNote.Text;

            //Act
            _sourceNote.Text = expected;
            var actual = _sourceNote.Text;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test(Description = "Тест геттера IsCreated")]
        public void IsCreated_GetRightCreatedTime()
        {
            //Setup
            var expected = _createdTime;

            //Act
            var actual = _sourceNote.IsCreated;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Тест сеттера IsCreated")]
        public void IsCreated_SetRightCreatedTime()
        {
            //Setup
            var expected = _sourceNote.IsCreated;

            //Act
            _sourceNote.IsCreated = expected;
            var actual = _sourceNote.IsCreated;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test(Description = "Тест геттера IsChanged")]
        public void IsChanged_GetRightChangedTime()
        {
            //Setup
            var expected = _changedTime;

            //Act
            var actual = _sourceNote.IsChanged;
            
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Тест сеттера IsChanged")]
        public void IsChanged_SetRightChangedTime()
        {
            //Setup
            var expected = _sourceNote.IsChanged;

            //Act
            _sourceNote.IsChanged = expected;
            var actual = _sourceNote.IsChanged;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test(Description = "Тест клонирования ICloneable")]
        public void TestClone_ReturnsSameClone()
        {
            //Act
            var expected = (Note)_sourceNote.Clone();

            //Assert
            Assert.AreEqual(expected, _sourceNote);
        }
    }
}

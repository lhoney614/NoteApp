using System;
using System.IO;
using NUnit.Framework;

namespace NoteApp.UnitTests
{
    [TestFixture]
    public class ProjectManagerTest
    {
        /// <summary>
        /// Путь к правильному файлу
        /// </summary>
        private const string CorrectProjectFileName = @"TestData\correctProject.json";

        /// <summary>
        /// Путь к поврежденному файлу
        /// </summary>
        private const string CorruptedProjectFileName = @"TestData\corruptedProject.json";

        /// <summary>
        /// Путь для сохранения файла
        /// </summary>
        private const string OutputProjectFileName = @"Output\savedFile.json";

        /// <summary>
        /// Создается объект проекта с двумя заметками в нем
        /// </summary>
        /// <returns></returns>
        private Project GetCorrectProject()
        {
            var correctProject = new Project();

            //Переменные хранящие время из эталонного файла
            var firstTime = DateTime.Parse("2021-04-18T12:13:16.470348+07:00");
            var secondTime = DateTime.Parse("2021-04-18T12:13:16.4713462+07:00");

            correctProject.Notes.Add(new Note()
            {
                Title = "New",
                Text = "Text",
                Category = NoteCategory.Other,
                IsCreated = firstTime,
                IsChanged = firstTime
            });
            correctProject.Notes.Add(new Note()
            {
                Title = "Older",
                Text = "Another text",
                Category = NoteCategory.Finance,
                IsCreated = secondTime,
                IsChanged = secondTime
            });

            return correctProject;
        }

        [Test(Description = "Позитивный тест сериализации проекта")]
        public void SaveToFile_SaveCorrectProject_ProjectSavedCorrectly()
        {
            //Setup
            var savingProject = GetCorrectProject();
            Directory.Delete(@"Output", true);

            //Act
            ProjectManager.SaveToFile(savingProject, OutputProjectFileName);
            
            //Assert
            var actual = File.ReadAllText(OutputProjectFileName);
            var expected = File.ReadAllText(CorrectProjectFileName);

            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Позитивный тест десериализации проекта")]
        public void LoadFromFile_LoadCorrectProject_ProjectLoadedCorrectly()
        {
            //Setup
            var expectedProject = GetCorrectProject();

            //Act
            var actualProject = ProjectManager.LoadFromFile(CorrectProjectFileName);

            //Assert
            Assert.AreEqual(expectedProject.Notes.Count, actualProject.Notes.Count);

            Assert.Multiple(() =>
            {
                for (var i = 0; i < expectedProject.Notes.Count; i++)
                {
                    var expected = expectedProject.Notes[i];
                    var actual = actualProject.Notes[i];

                    Assert.AreEqual(expected, actual);
                }
            });

        }

        [Test(Description = "Тест десериализации поврежденного файла")]
        public void LoadFromFile_LoadNotCorrectProject_ProjectLoadedNotCorrectly()
        {
            //Act
            var actualProject = ProjectManager.LoadFromFile(CorruptedProjectFileName);

            //Assert
            Assert.AreEqual(actualProject.Notes.Count, 0);
            Assert.NotNull(actualProject);
        }

        [Test(Description = "Тест десериализации несуществующего файла")]
        public void LoadFromFile_LoadNonExistentProject_ProjectLoadedNotCorrectly()
        {
            //Act
            var actualProject = ProjectManager.LoadFromFile(@"TestData\Project.json");

            //Assert
            Assert.AreEqual(actualProject.Notes.Count, 0);
            Assert.NotNull(actualProject);
        }
    }
}
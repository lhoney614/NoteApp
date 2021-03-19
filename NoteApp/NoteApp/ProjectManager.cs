using System;
using System.IO;
using Newtonsoft.Json;

namespace NoteApp
{
    public static class ProjectManager
    {
        /// <summary>
        /// Название файла для сохранений и загрузки
        /// </summary>
        public static readonly string FileName = 
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) 
            + "/NoteApp/NoteApp.notes";

        /// <summary>
        /// Сериализация (сохранение файла)
        /// </summary>
        /// <param name="project">сериализуемый объект</param>
        /// <param name="filename">Название файла</param>
        public static void SaveToFile(Project project, string filename)
        {
            //Создается экземпляр сериализатора
            JsonSerializer serializer = new JsonSerializer();

            //Открывается поток для записи в файл с указанием пути
            using (StreamWriter writer = new StreamWriter(filename))
            {
                using (JsonTextWriter textWriter = new JsonTextWriter(writer))
                {
                    //Вызывается сериализация и передается файл, 
                    //который нужно сериализовать
                    serializer.Serialize(textWriter, project);
                }
            }
        }

        /// <summary>
        /// Десериализация (загрузка файла)
        /// </summary>
        /// <param name="filename">Название файла</param>
        /// <returns></returns>
        public static Project LoadFromFile(string filename)
        {
            //Создается переменная, которая будет хранить
            //результат десериализации
            Project project = null;

            if (File.Exists(filename))
            {
                //Создается экземпляр сериализатора
                JsonSerializer serializer = new JsonSerializer();

                //Открывается поток для чтения из файла с указанием пути
                using (StreamReader reader = new StreamReader(filename))
                {
                    using (JsonTextReader textReader = new JsonTextReader(reader))
                    {
                        //Вызывается десериализация и явно
                        //преобразуется результат в целевой тип данных
                        project = serializer.Deserialize<Project>(textReader);

                        if (project == null)
                        {
                            return new Project();
                        }
                    }
                }
            }
            return project;
        }
    }
}
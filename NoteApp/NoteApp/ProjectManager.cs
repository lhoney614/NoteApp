using System.IO;
using Newtonsoft.Json;

namespace NoteApp
{
    public class ProjectManager
    {
        /// <summary>
        /// Название файла для сохранений и загрузки
        /// </summary>
        private const string FileName = "NoteApp.notes";

        public static void SaveToFile(Project project, string path)
        {
            Directory.CreateDirectory(path);
            path += FileName;
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter writer = new StreamWriter(path))
            {
                using (JsonTextWriter textWriter = new JsonTextWriter(writer))
                {
                    serializer.Serialize(textWriter, project);
                }
            }
        }

        public static Project LoadFromFile(string path)
        {
            path += FileName;

            Project project;
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader reader = new StreamReader(path))
            {
                using (JsonTextReader textReader = new JsonTextReader(reader))
                {
                    project = (Project) serializer.Deserialize<Project>(textReader);
                    if (project == null)
                    {
                        project = new Project();
                    }

                    return project;

                }
            }
        }
    }
}
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

        public static void SaveToFile(Project project, string filename)
        {
            Directory.CreateDirectory(filename);
            filename += FileName;
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter writer = new StreamWriter(filename))
            {
                using (JsonTextWriter textWriter = new JsonTextWriter(writer))
                {
                    serializer.Serialize(textWriter, project);
                }
            }
        }

        public static Project LoadFromFile(string filename)
        {
            filename += FileName;

            Project project;
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader reader = new StreamReader(filename))
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
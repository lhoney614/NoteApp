using NUnit.Framework;

namespace NoteApp.UnitTests
{
    [TestFixture]
    public class ProjectTest
    {
        private Project _notes = new Project();
        private Note _note = new Note();

        [Test]
        public void Project_NotNull()
        {
            _notes.Notes.Add(_note);

            Assert.NotNull(_notes);
        }
    }
}
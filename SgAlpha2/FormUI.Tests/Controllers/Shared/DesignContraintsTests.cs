using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;

namespace FormUI.Tests.Controllers.Shared
{
    [TestFixture]
    public class DesignContraintsTests
    {
        [Test]
        public void All_files_in_web_should_be_in_csproj()
        {
            var csproj = new XmlDocument();
            csproj.Load(@"..\..\..\FormUI\FormUI.csproj");

            var fileAttributes = csproj.SelectNodes("//*[@Include]");

            var relevantElements = new string[] { "Compile", "Content", "None", };

            var csprojFiles = fileAttributes
                .Cast<XmlElement>()
                .Where(e => relevantElements.Contains(e.Name))
                .Select(e => e.Attributes["Include"].Value)
                .ToList();

            var filesOnDisk = Directory.GetFiles(@"..\..\..\FormUI", "*.*", SearchOption.AllDirectories)
                .Select(f => f.Remove(0, @"..\..\..\FormUI\".Length))
                .Where(f => !f.StartsWith(@"bin\") && !f.StartsWith(@"obj\") && !f.Contains(".csproj"))
                .ToList();

            var filesMissingFromCsproj = filesOnDisk.Where(f => !csprojFiles.Contains(f)).ToList();

            // We have observed that the packaging that MSBuild uses on the CI server will not package css files (for example) that
            // are in source-control, but not in the .csproj.  This test ensures we don't accidentally miss some.

            if (filesMissingFromCsproj.Count > 0)
                Assert.Fail($"The following files are on disk, but not in the .csproj:  {string.Join(", ", filesMissingFromCsproj)}\n\nPlease add them to the csproj to ensure they are packaged and deployed correctly");
        }
    }
}

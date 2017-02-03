﻿using System;
using System.IO;
using System.Linq;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace FormUI.Tests.Shared
{
    [TestFixture]
    public class FormUiContraintsTests
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

        [Test]
        public void Verify_csproj_files_excluded_are_known()
        {
            var csproj = new XmlDocument();
            csproj.Load(@"..\..\..\FormUI\FormUI.csproj");

            var includedFiles = csproj.SelectNodes("//*[@Include]")
                .Cast<XmlElement>();

            var filesNotPackagedThatShouldBe = includedFiles
                .Where(fileElement => NotPackagedButShouldBe(fileElement))
                .Select(e => e.Attributes["Include"].Value)
                .ToList();

            // We have observed that the packaging that MSBuild uses on the CI server will not package a .cshtml file
            // if Visual Studio has added it as a <None/> element in the .csproj (instead of a <Content/> element)
            // This test ensures that we know about all <None/> content elements

            if (filesNotPackagedThatShouldBe.Count > 0)
                Assert.Fail($"The following files are inside <None ... /> elements in the .csproj: \n\n{string.Join("\n", filesNotPackagedThatShouldBe)}\n\nPlease verify if they need to have their type changed, or if NotPackagedButShouldBe() needs modified");
        }

        [Test]
        public void NotPackagedButShouldBe_VerifyElements()
        {
            Func<string, XmlElement> createElement = s =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(s);
                return doc.DocumentElement;
            };

            // false means the file are correct in the .csproj
            NotPackagedButShouldBe(createElement("<Content Include='AboutYou.cshtml' />")).Should().BeFalse(".cshtml files included as content will get deployed");
            NotPackagedButShouldBe(createElement("<Compile Include='AboutYou.cs' />")).Should().BeFalse("any compiled files are implicitly packaged in the assemblies");
            NotPackagedButShouldBe(createElement("<None Include='Scripts\\jquery.validate-vsdoc.js' />")).Should().BeFalse("vsdoc.js files can be excluded from package");
            NotPackagedButShouldBe(createElement("<None Include='compilerconfig.json' />")).Should().BeFalse("config for Web Compiler extension does not need packaged");
            NotPackagedButShouldBe(createElement("<None Include='compilerconfig.json.defaults' />")).Should().BeFalse("config for Web Compiler extension does not need packaged");

            // true means the file is incorrect in the .csproj
            NotPackagedButShouldBe(createElement("<None Include='AboutYou.cshtml' />")).Should().BeTrue(".cshtml files should not be a None element");
        }

        private bool NotPackagedButShouldBe(XmlElement fileElement)
        {
            if (fileElement.Name != "None")
                return false;

            var file = fileElement.Attributes["Include"].Value;
            file = Path.GetFileName(file).ToLower();

            if (file.StartsWith("compilerconfig.json"))
                return false;

            if (file.EndsWith("vsdoc.js") || file.EndsWith("intellisense.js"))
                return false;

            if (file == "Project_Readme.html".ToLower())
                return false;

            return true;
        }
    }
}

using System;
using System.IO;
using JustTag2;
using JustTag2.Tagging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaggingTests
{
    [TestClass]
    public class SetTagsTests
    {
        private const string TEMPLATE_PATH = "../../TestFolderTemplate";
        private const string TEST_PATH = "./TestFolder";

        [TestInitialize]
        public void RestoreTemplateFolder()
        {
            // Delete the test folder if it exists
            if (Directory.Exists(TEST_PATH))
                Directory.Delete(TEST_PATH, true);

            // Restore it.
            FileSystemInfo dir = new DirectoryInfo(TEMPLATE_PATH);
            dir.Copy(TEST_PATH);
        }

        [TestMethod]
        public void MyTestMethod()
        {

        }
    }
}

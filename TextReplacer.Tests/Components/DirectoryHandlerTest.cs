using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextReplacer.Components
{
    [TestClass]
    public class DirectoryHandlerTest
    {
        [TestMethod]
        public void Should_CreateHandler()
        {
            // Arrange
            // Act
            var cut = new DirectoryHandler(Environment.CurrentDirectory);

            // Assert
            Assert.IsNotNull(cut);
        }

        [TestMethod]
        [DeploymentItem(@"Components\TestDir", "TestDir")]
        public void Should_ProcessOneFile()
        {
            // Arrange
            var cut = new DirectoryHandler("TestDir");

            // Act
            var numberOfProcessedFiles = cut.Process();

            // Assert
            Assert.AreEqual(1, numberOfProcessedFiles);
        }

        [TestMethod]
        [DeploymentItem(@"Components\TestDir", "TestDir2")]
        public void Should_ProcessFileAndCreateBak()
        {
            // Arrange
            var cut = new DirectoryHandler("TestDir2");

            // Act
            var numberOfProcessedFiles = cut.Process();

            // Assert
            var bakFile = Directory.EnumerateFiles("TestDir2", "*.*")
                .Select(s => Path.GetFileName(s))
                .SingleOrDefault(s => Path.GetExtension(s) == ".bak");
            Assert.AreEqual("ToBeProcessed.bak", bakFile);
        }
    }
}
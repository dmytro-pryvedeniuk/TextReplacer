using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TextReplacer.Components
{
    [TestClass]
    public class TextReplacerTest
    {
        [TestMethod]
        public void Should_CreateReplacer()
        {
            // Arrange
            // Act
            var cut = new Replacer();

            // Assert
            Assert.IsNotNull(cut);
        }

        [TestMethod]
        public void Should_ReplaceText()
        {
            // Arrange
            var inputText = "Trisoft";
            var outputText = "SDL Trisoft";
            var template = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<test title=\"{0} will be updated\">" +
                "<line1>{0} has been renamed to SDL Trisoft</line1>" +
                "</test>";
            var inputString = string.Format(template, inputText);
            var expectedString = string.Format(template, outputText);
            var cut = new Replacer();

            // Act
            var actualString = cut.Process(inputString);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Should_ReplaceText_When_AttributeForInnerNodeIsUsed()
        {
            // Arrange
            var inputText = "Trisoft";
            var outputText = "SDL Trisoft";
            var template = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<test title=\"{0} will be updated\">" +
                "<line1 attr1=\"{0} is SDL Trisoft\">{0} has been renamed to SDL Trisoft</line1>" +
                "</test>";
            var inputString = string.Format(template, inputText);
            var expectedString = string.Format(template, outputText);
            var cut = new Replacer();

            // Act
            var actualString = cut.Process(inputString);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Should_ReplaceText_WhenTwoWhiteSpacesAreUsedBetweenSDLAndTrisoft()
        {
            // Arrange
            var inputText = "SDL  Trisoft";
            var outputText = "SDL  SDL Trisoft";
            var template = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<test title=\"{0} will be updated\">" +
                "</test>";
            var inputString = string.Format(template, inputText);
            var expectedString = string.Format(template, outputText);
            var cut = new Replacer();

            // Act
            var actualString = cut.Process(inputString);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Should_PreserveWhiteSpaces()
        {
            // Arrange
            var inputText = "Trisoft";
            var outputText = "SDL Trisoft";
            var template = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> " +
                "<test title=\"{0} will be updated\"> " +
                "</test>";
            var inputString = string.Format(template, inputText);
            var expectedString = string.Format(template, outputText);
            var cut = new Replacer();

            // Act
            var actualString = cut.Process(inputString);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void Should_NotReplaceText_When_NoMatchFound()
        {
            // Arrange
            var template = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> " +
                "<test title=\"It will be updated\"> " +
                "</test>";
            var cut = new Replacer();

            // Act
            var res = cut.Process(new ReplaceArgs {Input = template });

            // Assert
            Assert.IsFalse(res.IsProcessed);
        }

        [TestMethod]
        public void Should_ReturnError_When_TextIsNotValidXml()
        {
            // Arrange
            var template = "I am not an xml";
            var cut = new Replacer();

            // Act
            var res = cut.Process(new ReplaceArgs { Input = template });

            // Assert: XmlException
            StringAssert.StartsWith(res.Error, "Invalid xml");
        }

        public void Should_NotProcessFile_When_TextIsNotValidXml()
        {
            // Arrange
            var template = "I am not an xml";

            // Act
            var cut = new Replacer();
            var res = cut.Process(new ReplaceArgs { Input = template });

            // Arrange
            Assert.IsFalse(res.IsProcessed);
        }
    }
}
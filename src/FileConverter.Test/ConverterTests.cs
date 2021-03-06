﻿namespace FileConverter.Test
{
    using System;
    using System.IO;
    using System.Reflection;
    using Moq;
    using NUnit.Framework;

    public class ConverterTests
    {
        #region Fields

        private Mock<IConfiguration> _moqConfiguration;

        private Converter _converter;

        private string _directory;

        #endregion

        #region Methods

        [SetUp]
        public void Setup()
        {
            this._directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this._moqConfiguration = new Mock<IConfiguration>();
            this._moqConfiguration.SetupGet(x => x.PluginLocation).Returns(this._directory);
            this._converter = new Converter(this._moqConfiguration.Object);
        }

        [Test]
        public void Process_WhenNoInputFileIsPassed_ThrowsFileNotFoundException()
        {
            // Arrange
            string input = string.Empty;
            string output = "test.json";

            // Act

            // Assert
            Assert.ThrowsAsync<FileNotFoundException>(() => this._converter.Process(input, output));
        }


        [Test]
        public void Process_WhenNoInputFileIsPassed_CheckFileNotFoundReturnMessage()
        {
            // Arrange
            string input = string.Empty;
            string output = "test.json";

            // Act

            // Assert
            FileNotFoundException result = Assert.ThrowsAsync<FileNotFoundException>(() => this._converter.Process(input, output));
            Assert.That(result.Message, Is.EqualTo("Input file not found. Please enter a file!"));
        }

        [Test]
        public void Process_WhenInputFileIsPassedWithIncorrectExtension_ThrowsArgumentException()
        {
            // Arrange
            string input = "Documents/IncorrectExtension.txt";
            string output = "test.json";

            // Act

            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => this._converter.Process(input, output));
        }

        [Test]
        public void Process_WhenFileIsPassedWithIncorrectExtension_ThrowArgumentException()
        {
            // Arrange
            string input = "Documents/IncorrectExtension.txt";
            string output = "test.json";

            // Act

            // Assert
            ArgumentException result = Assert.ThrowsAsync<ArgumentException>(() => this._converter.Process(input, output));
            Assert.That(result.Message, Is.EqualTo("No content found in file!"));
        }

        [Test]
        public void Process_WhenGivenEmptyOutputPath_ThrowFileNotFoundException()
        {
            // Arrange
            string input = string.Empty;
            string output = string.Empty;

            // Act

            // Assert
            Assert.ThrowsAsync<FileNotFoundException>(() => this._converter.Process(input, output));
        }

        [Test]
        public void Process_WhenOutputFileIsPassedWithIncorrectExtension_CheckInvalidExtensionReturnMessage()
        {
            // Arrange
            string input = "Documents/Valid.csv";
            string output = "test.txt";

            // Act

            // Assert
            NotSupportedException result = Assert.ThrowsAsync<NotSupportedException>(() => this._converter.Process(input, output));
            Assert.That(result.Message, Is.EqualTo("Conversion process not found!"));
        }

        [Test]
        public void Process_WhenInvalidOutputFileExtensionIsPassed_ThrowFileNotFoundException()
        {
            // Arrange
            string input = "Documents/Valid.csv";
            string output = "output.jpg";

            // Act

            // Assert
            NotSupportedException result = Assert.ThrowsAsync<NotSupportedException>(() => this._converter.Process(input, output));
            Assert.That(result.Message, Is.EqualTo("Conversion process not found!"));
        }


        [Test]
        public void Process_WhenOutputFileNotIsPassed_ThrowFileNotFoundException()
        {
            // Arrange
            string input = "Documents/Valid.csv";
            string output = string.Empty;

            // Act

            // Assert
            FileNotFoundException result = Assert.ThrowsAsync<FileNotFoundException>(() => this._converter.Process(input, output));
            Assert.That(result.Message, Is.EqualTo("Output file not found. Please enter a file!"));
        }

        [Test]
        public void Process_WhenInvalidCsvIsPassed_ShouldThrowInvalidDataException()
        {
            // Arrange
            string input = "Documents/InValid.csv";
            string output = "output.json";

            // Act

            // Assert
            InvalidDataException result = Assert.ThrowsAsync<InvalidDataException>(() => this._converter.Process(input, output));
            Assert.That(result.Message, Is.EqualTo("CSV data validation failed!"));
        }

        [Test]
        public void Process_WhenInValidDelimiterIsPassed_ShouldThrowInvalidDataException()
        {
            // Arrange
            string input = "Documents/Valid.csv";
            string output = "output.json";
            char delimiter = ';';
            // Act

            // Assert
            InvalidDataException result = Assert.ThrowsAsync<InvalidDataException>(() =>
            {
                this._converter.MetaData = delimiter;
                return this._converter.Process(input, output);
            });
            Assert.That(result.Message, Is.EqualTo("Invalid delimiter!"));
        }

        #endregion
    }
}
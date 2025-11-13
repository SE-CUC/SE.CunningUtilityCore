using NUnit.Framework;
using IngameScript.Tests.Mocks;
using System.Text.RegularExpressions;

namespace IngameScript.Tests
{
    [TestFixture]
    public class SurfaceLoggerTests
    {
        private MockTextSurface _surface;
        private SurfaceLogger _logger;
        private LoggerConfig _config;

        [SetUp]
        public void SetUp()
        {
            _surface = new MockTextSurface();
            _config = new LoggerConfig { LogLevel = LogLevel.Debug };
            _logger = new SurfaceLogger(_config, _surface);
        }

        [Test]
        public void LogsDebugMessage()
        {
            _logger.Debug("This is a debug message");
            Assert.That(_surface.Lines.Count, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(_surface.Lines[0], @"\[DEBUG\] This is a debug message"));
        }

        [Test]
        public void LogsInfoMessage()
        {
            _logger.Info("This is an info message");
            Assert.That(_surface.Lines.Count, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(_surface.Lines[0], @"\[INFO\] This is an info message"));
        }

        [Test]
        public void LogsErrorMessage()
        {
            _logger.Error("This is an error message");
            Assert.That(_surface.Lines.Count, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(_surface.Lines[0], @"\[ERROR\] This is an error message"));
        }

        [Test]
        public void IgnoresDebugMessageWhenLogLevelIsInfo()
        {
            _config.LogLevel = LogLevel.Info;
            _logger.Debug("This should be ignored");
            Assert.That(_surface.Lines.Count, Is.EqualTo(0));
        }

        [Test]
        public void IgnoresInfoMessageWhenLogLevelIsError()
        {
            _config.LogLevel = LogLevel.Error;
            _logger.Info("This should be ignored");
            Assert.That(_surface.Lines.Count, Is.EqualTo(0));
        }

        [Test]
        public void LogsErrorMessageWhenLogLevelIsError()
        {
            _config.LogLevel = LogLevel.Error;
            _logger.Error("This should be logged");
            Assert.That(_surface.Lines.Count, Is.EqualTo(1));
            Assert.That(Regex.IsMatch(_surface.Lines[0], @"\[ERROR\] This should be logged"));
        }
    }
}

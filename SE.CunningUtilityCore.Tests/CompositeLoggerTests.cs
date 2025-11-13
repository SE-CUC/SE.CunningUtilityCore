using NUnit.Framework;
using System.Collections.Generic;
using IngameScript.Tests.Mocks;

namespace IngameScript.Tests
{
    [TestFixture]
    public class CompositeLoggerTests
    {
        private CompositeLogger _compositeLogger;
        private MockLogger _logger1;
        private MockLogger _logger2;

        [SetUp]
        public void SetUp()
        {
            _logger1 = new MockLogger();
            _logger2 = new MockLogger();
            _compositeLogger = new CompositeLogger(_logger1, _logger2);
        }

        [Test]
        public void ForwardsLogMessagesToAllLoggers()
        {
            _compositeLogger.Info("Test message");

            Assert.That(_logger1.Messages.Count, Is.EqualTo(1));
            Assert.That(_logger1.Messages[0], Is.EqualTo("[INFO] Test message"));

            Assert.That(_logger2.Messages.Count, Is.EqualTo(1));
            Assert.That(_logger2.Messages[0], Is.EqualTo("[INFO] Test message"));
        }
    }
}

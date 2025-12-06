using NUnit.Framework;
using IngameScript;
using System;
using System.Collections.Generic;

namespace IngameScript.Tests
{
    [TestFixture]
    public class CommandSystemTests
    {
        private CommandService _service;
        private MockLogger _logger;

        private class MockLogger : ILogger
        {
            public List<string> Logs = new List<string>();
            public void Debug(string text) => Logs.Add($"DEBUG: {text}");
            public void Error(string text) => Logs.Add($"ERROR: {text}");
            public void Error(Exception e, string text = "") => Logs.Add($"ERROR: {text} {e.Message}");
            public void Info(string text) => Logs.Add($"INFO: {text}");
            public void Write(LogLevel level, string text) => Logs.Add($"{level}: {text}");
        }

        private class MockCommand : ICommand
        {
            public string Name { get; set; }
            public string HelpText => "Mock Command";
            public Action<string, Action<string>> OnExecute { get; set; }

            public void Execute(string arguments, Action<string> reply)
            {
                OnExecute?.Invoke(arguments, reply);
            }
        }

        [SetUp]
        public void Setup()
        {
            _logger = new MockLogger();
            _service = new CommandService(_logger);
        }

        [Test]
        public void ArgumentReader_ParsesQuotesCorrectly()
        {
            string input = "arg1 \"arg 2 with spaces\" arg3";
            var reader = new ArgumentReader(input);

            Assert.AreEqual("arg1", reader.Next());
            Assert.AreEqual("arg 2 with spaces", reader.Next());
            Assert.AreEqual("arg3", reader.Next());
            Assert.IsNull(reader.Next());
        }

        [Test]
        public void ArgumentReader_ParsesAdjacentQuotes()
        {
            string input = "\"one\"\"two\"";
            var reader = new ArgumentReader(input);
            Assert.AreEqual("one", reader.Next());
            Assert.AreEqual("two", reader.Next());
        }

        [Test]
        public void CommandService_DispatchesToRegisteredCommand()
        {
            bool executed = false;
            var cmd = new MockCommand 
            { 
                Name = "test", 
                OnExecute = (args, reply) => 
                {
                    executed = true;
                    Assert.AreEqual("foo bar", args);
                    reply("pong");
                }
            };

            _service.Register(cmd);
            
            string replyMsg = null;
            _service.Handle("test foo bar", msg => replyMsg = msg);

            Assert.IsTrue(executed);
            Assert.AreEqual("pong", replyMsg);
        }

        [Test]
        public void CommandService_HandlesExceptionsGracefully()
        {
            var cmd = new MockCommand
            {
                Name = "crash",
                OnExecute = (args, reply) => throw new InvalidOperationException("Boom")
            };

            _service.Register(cmd);

            string replyMsg = null;
            Assert.DoesNotThrow(() => _service.Handle("crash", msg => replyMsg = msg));
            
            StringAssert.StartsWith("Error: Boom", replyMsg);
            // Verify logger received error
            Assert.IsTrue(_logger.Logs.Exists(l => l.Contains("Error executing command")));
        }

        [Test]
        public void CommandService_UnknownCommand_RepliesUnknown()
        {
            string replyMsg = null;
            _service.Handle("unknown_check", msg => replyMsg = msg);
            
            StringAssert.StartsWith("Unknown command", replyMsg);
        }
    }
}

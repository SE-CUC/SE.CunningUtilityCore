using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace IngameScript
{
    public class TestRunner
    {
        public static void Main(string[] args)
        {
            var assemblyPath = args.Length > 0 ? args[0] : "../SE.CunningUtilityCore.Tests/bin/Debug/SE.CunningUtilityCore.Tests.dll";
            var assembly = Assembly.LoadFrom(assemblyPath);
            var testFixtures = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(TestFixtureAttribute)).Any());

            int testsRun = 0;
            int testsPassed = 0;

            foreach (var fixture in testFixtures)
            {
                Console.WriteLine($"[Fixture] {fixture.Name}");
                var instance = Activator.CreateInstance(fixture);
                var setupMethod = fixture.GetMethods().FirstOrDefault(m => m.GetCustomAttributes(typeof(SetUpAttribute)).Any());
                var testMethods = fixture.GetMethods().Where(m => m.GetCustomAttributes(typeof(TestAttribute)).Any());

                foreach (var test in testMethods)
                {
                    testsRun++;
                    try
                    {
                        setupMethod?.Invoke(instance, null);
                        test.Invoke(instance, null);
                        Console.WriteLine($"  [PASS] {test.Name}");
                        testsPassed++;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"  [FAIL] {test.Name}");
                        Console.WriteLine(e.InnerException?.Message);
                        Console.WriteLine(e.InnerException?.StackTrace);
                    }
                }
            }

            Console.WriteLine($"\nTests run: {testsRun}, Passed: {testsPassed}, Failed: {testsRun - testsPassed}");
        }
    }
}

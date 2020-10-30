using System;

namespace LeastRecentCache
{
    public class MockLogger :ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
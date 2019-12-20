using System;

namespace CSGOStats.Infrastructure.Messaging.Tests.Model
{
    public class TestData
    {
        public DateTime Date { get; }

        public TimeSpan Time { get; }

        public TestData(DateTime date, TimeSpan time)
        {
            Date = date;
            Time = time;
        }
    }
}
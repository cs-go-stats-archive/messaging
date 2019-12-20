using System;

namespace CSGOStats.Infrastructure.Messaging.Extensions
{
    public static class ValidationExtensions
    {
        public static T NotNull<T>(this T instance, string argumentName)
            where T : class => instance ?? throw new ArgumentNullException(argumentName);

        public static int Positive(this int instance, string argumentName) =>
            instance > 0 ? instance : throw new ArgumentOutOfRangeException(argumentName);

        public static int LessThan(this int instance, int upperBound, string argumentName) =>
            instance <= upperBound ? instance : throw new ArgumentOutOfRangeException(argumentName);
    }
}
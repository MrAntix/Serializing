using System;
using System.Linq;
using Antix.Serializing.Tests.Models;
using Testing;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class benchmarks
    {
        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void simple_object()
        {
            var sut = GetBuilder()
                .Create();

            var obj = new Simple();

            Run("simple", () => sut.Serialize(obj), 1000, 10000, 100000);
        }

        [Fact]
        public void simple_collection()
        {
            var sut = GetBuilder()
                .Create();

            var obj = new Builder<Simple>().Build(10);

            Run("simple", () => sut.Serialize(obj), 1000, 10000);
        }

        [Fact]
        public void simple_collection_large()
        {
            var sut = GetBuilder()
                .Create();

            var obj = new Builder<Simple>().Build(100);

            Run("simple", () => sut.Serialize(obj), 1000, 10000);
        }

        static void Run(
            int iterations, string notes, Action action)
        {
            var time = Benchmark.Run(iterations, action);
            Console.WriteLine(
                "{0}: {1} {2}x {3}",
                time,
                TimeSpan.FromTicks(time.Ticks/iterations),
                iterations,
                notes);
        }

        static void Run(
            string notes, Action action, int iterations,
            params int[] moreIterations)
        {
            foreach (var i in new[] {iterations}.Union(moreIterations))
            {
                Run(i, notes, action);
            }
        }
    }
}
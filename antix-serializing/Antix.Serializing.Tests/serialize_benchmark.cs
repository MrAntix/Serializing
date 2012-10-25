using System;
using System.Linq;
using Antix.Serializing.Tests.Models;
using Antix.Testing;
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

            Run("simple_object", () => sut.Serialize(obj), 1000, 10000, 100000);
        }

        [Fact]
        public void simple_collection()
        {
            var sut = GetBuilder()
                .Create();

            var obj = new Builder<Simple>().Build(10);

            Run("simple_collection", () => sut.Serialize(obj), 1000, 10000);
        }

        [Fact]
        public void simple_collection_large()
        {
            var sut = GetBuilder()
                .Create();

            var obj = new Builder<Simple>().Build(100);

            Run("simple_collection_large", () => sut.Serialize(obj), 1000, 10000);
        }

        static void Run(
            string notes, Action action,
            params int[] iterations)
        {
            Console.WriteLine(notes);
            var results = Benchmark.Run(action, iterations);
            foreach (var result in results.Results)
            {
                Console.WriteLine(result);
            }
            Console.WriteLine("totals");
            Console.WriteLine(results);
            Console.WriteLine("---   ---   ---   ---   ---   ---   ---   ---");
        }
    }
}
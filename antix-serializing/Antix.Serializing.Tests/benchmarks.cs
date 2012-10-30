using System;
using System.Linq;
using Antix.Serializing.Abstraction.Builders;
using Antix.Serializing.Builders;
using Antix.Serializing.Tests.Models;
using Antix.Testing;
using Xunit;

namespace Antix.Serializing.Tests
{
    public class benchmarks
    {
        readonly Action<ComplexNested> _assignComplex = o =>
                                                            {
                                                                o.StringValue =
                                                                    TestData.Text.WithLetters()
                                                                            .WithRange(10, 20)
                                                                            .Build();
                                                                o.IntegerArrayValue =
                                                                    TestData.Integer.Build(10).ToArray();
                                                                o.DateValue = TestData.DateTime.Build();
                                                            };

        static ISerializerBuilder GetBuilder()
        {
            return new SerializerBuilder();
        }

        [Fact]
        public void simple_object()
        {
            var sut = GetBuilder()
                .Build();

            var obj = new Simple();

            Run("simple_object", () => sut.Serialize(obj), 1000, 10000, 100000);
        }

        [Fact]
        public void simple_collection()
        {
            var sut = GetBuilder()
                .Build();

            var obj = new Builder<Simple>().Build(10);

            Run("simple_collection", () => sut.Serialize(obj), 1000, 10000);
        }

        [Fact]
        public void simple_collection_large()
        {
            var sut = GetBuilder()
                .Build();

            var obj = new Builder<Simple>().Build(100);

            Run("simple_collection_large", () => sut.Serialize(obj), 1000, 10000);
        }

        [Fact]
        public void complex_object()
        {
            var sut = GetBuilder()
                .Build();

            var obj = new Builder<ComplexNested>().Build(_assignComplex);

            Run("complex_object", () => Console.WriteLine(sut.Serialize(obj)), 1000, 10000, 100000);
        }

        [Fact]
        public void complex_collection()
        {
            var sut = GetBuilder()
                .Build();

            var obj = new Builder<ComplexNested>().Build(10, _assignComplex);

            Run("complex_collection", () => sut.Serialize(obj), 1000, 10000);
        }

        [Fact]
        public void complex_collection_large()
        {
            var sut = GetBuilder()
                .Build();

            var obj = new Builder<ComplexNested>().Build(100, _assignComplex);

            Run("complex_collection_large", () => sut.Serialize(obj), 1000, 10000);
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
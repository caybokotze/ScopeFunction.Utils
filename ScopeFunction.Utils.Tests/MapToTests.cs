#nullable enable
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace ScopeFunction.Utils.Tests
{
    [TestFixture]
    public class MapToTests
    {
        [TestFixture]
        public class WhenMappingProperties
        {
            [Test]
            public void ShouldCopyValues()
            {
                // arrange
                var randomId = GetRandomInt(1000);
                var foo = GetRandom<Foo>();
                var secondFoo = new Foo
                {
                    Id = randomId,
                    Additional = GetRandomString(),
                    Details = new FooDetails
                    {
                        Details = GetRandomString()
                    },
                    Name = GetRandomString()
                };
                // act
                var result = foo.MapTo(secondFoo);
                // arrange
                Expect(secondFoo.Name).To.Equal(foo.Name);
                Expect(secondFoo.Id).To.Equal(foo.Id);
                Expect(secondFoo.Additional).To.Equal(foo.Additional);
                Expect(secondFoo.Details?.Details).To.Equal(foo.Details?.Details);
                Expect(foo).To!.Deep.Equal(result);
            }
        }


        [TestFixture]
        public class WhenMappingWithOverrides
        {
            [Test]
            public void WhenPropertiesAreGetAndSet()
            {
                // arrange
                var randomId = GetRandomInt(1000);
                var foo = GetRandom<Foo>();
                var secondFoo = new Foo
                {
                    Id = randomId,
                    Additional = GetRandomString(),
                    Details = new FooDetails
                    {
                        Details = GetRandomString()
                    },
                    Name = GetRandomString()
                };
                // act
                var fooId = foo.Id;
                foo.MapTo(secondFoo, f => { return new[] { nameof(f.Id) }; });
                // arrange
                Expect(secondFoo.Name).To.Equal(foo.Name);
                Expect(secondFoo.Id).To.Equal(randomId);
                Expect(secondFoo.Id).To.Not.Equal(fooId);
                Expect(secondFoo.Details?.Details).To.Equal(foo.Details?.Details);
            }

            [TestFixture]
            public class WhenPropertiesAreInit
            {
                [Test]
                [Repeat(10)]
                public void ShouldStillReflectivelyMapValues()
                {
                    // arrange
                    var randomId = GetRandomInt(1000);
                    var foo = GetRandom<Foo>();
                    var fooInit = new InitFooDto
                    {
                        Id = randomId
                    };
                    // act
                    foo.MapTo(fooInit, f => { return new[] { nameof(f.Id) }; });
                    // arrange
                    Expect(fooInit.Name).To.Equal(foo.Name);
                    Expect(fooInit.Id).To.Equal(randomId);
                    Expect(fooInit.Details?.Details).To.Equal(foo.Details?.Details);
                }
            }

            [TestFixture]
            public class WhenPropertiesAreReadonly
            {
                [Test]
                public void ShouldNotMapValuesThatAreReadonly()
                {
                    // arrange
                    var randomId = GetRandomInt(1000);
                    var randomName = GetRandomString();
                    var foo = GetRandom<Foo>();
                    var fooReadOnly = new ReadOnlyFooDto(randomId, randomName);
                    // act
                    var result = foo.MapTo(fooReadOnly, f => { return new[] { nameof(f.Id) }; });
                    // arrange
                    Expect(fooReadOnly.Name).To.Equal(randomName);
                    Expect(fooReadOnly.Id).To.Equal(randomId);
                    Expect(fooReadOnly.Details?.Details).To.Equal(foo.Details?.Details);

                    Expect(result).To.Deep.Equal(new ReadOnlyFooDto(fooReadOnly.Id, fooReadOnly.Name)
                    {
                        Details = foo.Details
                    });
                }
            }

            [TestFixture]
            public class WhenMappingPropertiesThatDontExistInDestination
            {
                [Test]
                public void ShouldNotThrowAndMapOtherValues()
                {
                    // arrange
                    var randomId = GetRandomInt(1000);
                    var foo = GetRandom<Foo>();
                    var fooInit = new InitFooDto
                    {
                        Id = randomId
                    };
                    // act
                    var result = foo.MapTo(fooInit, f =>
                    {
                        return new[]
                        {
                            nameof(f.Id),
                            nameof(f.Additional)
                        };
                    });
                    // arrange
                    Expect(fooInit.Name).To.Equal(foo.Name);
                    Expect(fooInit.Id).To.Equal(randomId);
                    Expect(fooInit.Details?.Details).To.Equal(foo.Details?.Details);

                    Expect(result).To.Deep.Equal(new InitFooDto
                    {
                        Id = randomId,
                        Details = fooInit.Details,
                        Name = fooInit.Name
                    });
                }
            }
        }
    }
}

public class Foo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Additional { get; set; }
    public FooDetails? Details { get; set; }
}

public class FooDetails
{
    public string? Details { get; set; }
}

public class ReadOnlyFooDto
{
    public ReadOnlyFooDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public int Id { get; }
    public string Name { get; }
    
    public FooDetails? Details { get; init; }
}

public class InitFooDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    
    public FooDetails? Details { get; init; }
}
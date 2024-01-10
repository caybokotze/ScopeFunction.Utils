using System;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;

namespace ScopeFunction.Utils.Tests;

[TestFixture]
public class StringHelperTests
{
    [TestFixture]
    public class RemoveSpecialCharacters
    {
        [TestFixture]
        public class WhenStringContainsSpecialCharacters
        {
            
        }

        [TestFixture]
        public class WhenStringDoesNotContainSpecialCharacters
        {
            
        }

        [TestFixture]
        public class WhenNumbersShouldBeRemoved
        {
            
        }

        [TestFixture]
        public class WhenDiacriticsShouldBeRemoved
        {
            [Test]
            public void ShouldReplaceDiacritic()
            {
                // arrange
                var originalString = "ééé";
                var expectedString = "eee";
                
                // act
                var result = originalString.RemoveSpecialCharacters(options: o => o.WithoutDiacritics());
                // assert
                Expect(result).To.Equal(expectedString);
            }

            [TestFixture]
            public class WhenCombinedWithDiacriticsAndNormalCharacters
            {
                [Test]
                public void ShouldReplaceOnlyDiacritics()
                {
                    // arrange
                    var originalString = "éabcëőṃM̈m̈";
                    var expectedString = "eabceomMm";
                    // act
                    var result = originalString.RemoveSpecialCharacters(options: o =>
                    {
                        o.WithoutDiacritics();
                        o.WithCharacterMapping('ṃ', 'm');
                        o.WithoutCharacter('?');
                    });
                    // assert
                    Expect(result).To.Equal(expectedString);
                }               
            }
        }
    }
}
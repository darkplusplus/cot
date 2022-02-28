using System;
using Xunit;

namespace dpp.cot.Tests
{
    public class CotTypeTests
    {
        [Theory]
        [InlineData(Helpers.SimplePayload, "a-.-A-M")]
        [InlineData(Helpers.SimplePayload, "a-h-A-M")]
        [InlineData(Helpers.SimplePayload, CotPredicates.air)]
        [InlineData(Helpers.EudPayload, CotPredicates.ground)]
        public void PredicateTest(string corpus, string predicate)
        {
            var evt = cot.Event.Parse(corpus);

            Assert.True(evt.IsA(predicate));
        }

        [Theory]
        [InlineData(Helpers.SimplePayload, "UTILITY (MEDIUM)")]
        [InlineData(Helpers.EudPayload, "COMBAT")]
        public void DescriptionTest(string corpus, string description)
        {
            var evt = cot.Event.Parse(corpus);

            Assert.Equal(description, evt.GetDescription());
        }
    }
}

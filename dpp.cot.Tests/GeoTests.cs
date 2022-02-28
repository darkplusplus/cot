using System;
using Xunit;

namespace dpp.cot.Tests
{
    public class GeoTests
    {
        [Fact]
        public void DistanceExtensionInMilesTest()
        {
            var evt = new cot.Event();
            evt.Point.Lat = 36.12;
            evt.Point.Lon = -86.67;

            var m = evt.GetDistanceMiles(33.94, -118.4);
            Assert.Equal(1793.57342023, Math.Round(m, 8));
        }

        [Fact]
        public void DistanceExtensionInKilometersTest()
        {
            var evt = new cot.Event();
            evt.Point.Lat = 36.12;
            evt.Point.Lon = -86.67;

            var km = evt.GetDistanceKilometers(33.94, -118.4);
            Assert.Equal(2887.25995061, Math.Round(km, 8));
        }
    }
}

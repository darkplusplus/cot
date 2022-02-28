using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dpp.cot
{
    public static class Extensions
    {
        public static bool IsA(string p, string t)
        {
            var regex = new Regex(p);
            return regex.Matches(t).Count > 0;
        }

        public static bool IsA(this Event e, string p)
        {
            var regex = new Regex(p);
            return regex.Matches(e.Type).Count > 0;
        }

        private static double GetDistanceTo(double R, double lat1, double lon1, double lat2, double lon2)
        {
            // implementaion of the Haversine distance formula

            var rLat1 = Math.PI * lat1 / 180.0;
            var rLat2 = Math.PI * lat2 / 180.0;
            var rLon1 = Math.PI * lon1 / 180.0;
            var rLon2 = Math.PI * lon2 / 180.0;

            var dLat = rLat2 - rLat1;
            var dLon = rLon2 - rLon1;

            var d = Math.Asin(Math.Sqrt(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(rLat1) * Math.Cos(rLat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2)));
            return 2 * R * d;
        }

        public static double GetDistanceMiles(this Event e, double lat, double lon)
        {
            var rMiles = 3958.8;
            return GetDistanceTo(rMiles, e.Point.Lat, e.Point.Lon, lat, lon);
        }
        public static double GetDistanceKilometers(this Event e, double lat, double lon)
        {
            var rKilometers = 6372.8;
            return GetDistanceTo(rKilometers, e.Point.Lat, e.Point.Lon, lat, lon);
        }
    }
}

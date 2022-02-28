using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dpp.cot.Tests
{
    public static class Helpers
    {
        public const string SimplePayload = @"<event version=""2.0"" uid=""J-01334"" type=""a-h-A-M-F-U-M"" time=""2005-04-05T11:43:38.07Z"" start=""2005-04-05T11:43:38.07Z"" stale=""2005-04-05T11:45:38.07Z""><detail></detail><point lat=""30.0090027"" lon=""-85.9578735"" ce=""45.3"" hae=""-42.6"" le=""99.5"" /></event>";
        public const string EudPayload = @"<event version='2.0' uid='ANDROID-ASDFASDFASDF' type='a-f-G-U-C' time='2022-02-02T22:22:22.222Z' start='2022-02-02T22:22:22.222Z' stale='2022-02-02T22:32:22.222Z' how='h-e'><point lat='-0.00123456789012345' lon='-0.00123456789012345' hae='9999999.0' ce='9999999.0' le='9999999.0' /><detail><takv device='SOMSANG FOOBAR' platform='ATAK-CIV' os='25' version='1.23.4-56789.56789-CIV'/><contact endpoint='192.168.1.2:4242:tcp' callsign='SUPERMAN'/><uid Droid='SUPERMAN'/><precisionlocation geopointsrc='USER' altsrc='???'/><__group name='Blue' role='Team Member'/><status battery='45'/><track speed='0.0' course='0.0'/></detail></event>";
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace dpp.cot
{
    internal static class CotProtoMapper
    {
        public static ProtoTakMessage ToProto(Message message)
        {
            return new ProtoTakMessage
            {
                TakControl = ToProto(message.Control),
                CotEvent = ToProto(message.Event),
                SubmissionTime = message.SubmissionTime.HasValue ? ToUnixMilliseconds(message.SubmissionTime.Value) : 0,
                CreationTime = message.CreationTime.HasValue ? ToUnixMilliseconds(message.CreationTime.Value) : 0,
            };
        }

        public static Message FromProto(ProtoTakMessage message)
        {
            return new Message
            {
                Control = FromProto(message.TakControl),
                Event = FromProto(message.CotEvent),
                SubmissionTime = FromUnixMillisecondsOrDefault(message.SubmissionTime),
                CreationTime = FromUnixMillisecondsOrDefault(message.CreationTime),
            };
        }

        private static ProtoTakControl ToProto(TakControl control)
        {
            if (control == null)
            {
                return null;
            }

            return new ProtoTakControl
            {
                MinProtoVersion = control.minProtoVersion,
                MaxProtoVersion = control.maxProtoVersion,
            };
        }

        private static TakControl FromProto(ProtoTakControl control)
        {
            if (control == null)
            {
                return new TakControl();
            }

            return new TakControl
            {
                minProtoVersion = control.MinProtoVersion,
                maxProtoVersion = control.MaxProtoVersion,
            };
        }

        private static ProtoCotEvent ToProto(Event evt)
        {
            if (evt == null)
            {
                return null;
            }

            return new ProtoCotEvent
            {
                Type = evt.Type ?? "",
                Access = evt.Access ?? "",
                Qos = evt.Qos ?? "",
                Opex = evt.Opex ?? "",
                Caveat = evt.Caveat ?? "",
                ReleaseableTo = evt.ReleaseableTo ?? "",
                Uid = evt.Uid ?? "",
                SendTime = ToUnixMilliseconds(evt.Time),
                StartTime = ToUnixMilliseconds(evt.Start),
                StaleTime = ToUnixMilliseconds(evt.Stale),
                How = evt.How ?? "",
                Lat = evt.Point?.Lat ?? 0,
                Lon = evt.Point?.Lon ?? 0,
                Hae = evt.Point?.Hae ?? 0,
                Ce = evt.Point?.Ce ?? 0,
                Le = evt.Point?.Le ?? 0,
                Detail = ToProto(evt.Detail),
            };
        }

        private static Event FromProto(ProtoCotEvent evt)
        {
            if (evt == null)
            {
                return null;
            }

            return new Event
            {
                Type = evt.Type ?? "",
                Access = evt.Access ?? "",
                Qos = evt.Qos ?? "",
                Opex = evt.Opex ?? "",
                Caveat = evt.Caveat ?? "",
                ReleaseableTo = evt.ReleaseableTo ?? "",
                Uid = evt.Uid ?? "",
                Time = FromUnixMilliseconds(evt.SendTime),
                Start = FromUnixMilliseconds(evt.StartTime),
                Stale = FromUnixMilliseconds(evt.StaleTime),
                How = evt.How ?? "",
                Point = new Point
                {
                    Lat = evt.Lat,
                    Lon = evt.Lon,
                    Hae = evt.Hae,
                    Ce = evt.Ce,
                    Le = evt.Le,
                },
                Detail = FromProto(evt.Detail),
            };
        }

        private static ProtoDetail ToProto(Detail detail)
        {
            if (detail == null)
            {
                return null;
            }

            var xmlDetail = SerializeResidualDetail(detail, out var overriddenElementNames);
            detail.xmlDetail = xmlDetail;

            return new ProtoDetail
            {
                XmlDetail = xmlDetail,
                Contact = overriddenElementNames.Contains("contact") ? null : detail.Contact,
                Group = overriddenElementNames.Contains("__group") ? null : detail.Group,
                PrecisionLocation = overriddenElementNames.Contains("precisionlocation") ? null : detail.PrecisionLocation,
                Status = overriddenElementNames.Contains("status") ? null : detail.Status,
                Takv = overriddenElementNames.Contains("takv") ? null : detail.Takv,
                Track = overriddenElementNames.Contains("track") ? null : detail.Track,
            };
        }

        private static Detail FromProto(ProtoDetail detail)
        {
            if (detail == null)
            {
                return null;
            }

            var additionalElements = ParseResidualDetail(detail.XmlDetail);
            var overriddenElementNames = GetOverrideElementNames(additionalElements);

            return new Detail
            {
                xmlDetail = detail.XmlDetail ?? "",
                Contact = overriddenElementNames.Contains("contact") ? null : detail.Contact,
                Group = overriddenElementNames.Contains("__group") ? null : detail.Group,
                PrecisionLocation = overriddenElementNames.Contains("precisionlocation") ? null : detail.PrecisionLocation,
                Status = overriddenElementNames.Contains("status") ? null : detail.Status,
                Takv = overriddenElementNames.Contains("takv") ? null : detail.Takv,
                Track = overriddenElementNames.Contains("track") ? null : detail.Track,
                AdditionalElements = additionalElements,
            };
        }

        private static ulong ToUnixMilliseconds(DateTime value)
        {
            var utc = value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            };

            return (ulong)new DateTimeOffset(utc).ToUnixTimeMilliseconds();
        }

        private static DateTime FromUnixMilliseconds(ulong value)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds((long)value).UtcDateTime;
        }

        private static DateTime? FromUnixMillisecondsOrDefault(ulong value)
        {
            return value == 0 ? null : FromUnixMilliseconds(value);
        }

        private static string SerializeResidualDetail(Detail detail, out HashSet<string> overriddenElementNames)
        {
            var residualElements = new List<XmlElement>();

            if (detail.AdditionalElements != null && detail.AdditionalElements.Length > 0)
            {
                residualElements.AddRange(detail.AdditionalElements.Where(e => e != null));
            }

            AddResidualTypedElement(residualElements, "contact", detail.Contact, HasAdditionalAttributes(detail.Contact?.AdditionalAttributes), WriteContactAttributes);
            AddResidualTypedElement(residualElements, "__group", detail.Group, HasAdditionalAttributes(detail.Group?.AdditionalAttributes), WriteGroupAttributes);
            AddResidualTypedElement(residualElements, "precisionlocation", detail.PrecisionLocation, HasAdditionalAttributes(detail.PrecisionLocation?.AdditionalAttributes), WritePrecisionLocationAttributes);
            AddResidualTypedElement(residualElements, "status", detail.Status, HasAdditionalAttributes(detail.Status?.AdditionalAttributes), WriteStatusAttributes);
            AddResidualTypedElement(residualElements, "takv", detail.Takv, HasAdditionalAttributes(detail.Takv?.AdditionalAttributes), WriteTakvAttributes);
            AddResidualTypedElement(residualElements, "track", detail.Track, HasAdditionalAttributes(detail.Track?.AdditionalAttributes), WriteTrackAttributes);

            overriddenElementNames = GetOverrideElementNames(residualElements.ToArray());

            if (residualElements.Count > 0)
            {
                return string.Concat(residualElements.Select(e => e.OuterXml));
            }

            return detail.xmlDetail ?? "";
        }

        private static bool HasAdditionalAttributes(XmlAttribute[] attributes)
        {
            return attributes != null && attributes.Length > 0;
        }

        private static void AddResidualTypedElement<T>(List<XmlElement> residualElements, string elementName, T element, bool keepAsXml, Action<XmlElement, T> writeAttributes)
            where T : class
        {
            if (!keepAsXml || element == null)
            {
                return;
            }

            var document = new XmlDocument();
            var xmlElement = document.CreateElement(elementName);
            writeAttributes(xmlElement, element);
            residualElements.Add(xmlElement);
        }

        private static void WriteContactAttributes(XmlElement element, Contact contact)
        {
            WriteStringAttribute(element, "endpoint", contact.Endpoint);
            WriteStringAttribute(element, "callsign", contact.Callsign);
            WriteAdditionalAttributes(element, contact.AdditionalAttributes);
        }

        private static void WriteGroupAttributes(XmlElement element, Group group)
        {
            WriteStringAttribute(element, "name", group.Name);
            WriteStringAttribute(element, "role", group.Role);
            WriteAdditionalAttributes(element, group.AdditionalAttributes);
        }

        private static void WritePrecisionLocationAttributes(XmlElement element, PrecisionLocation precisionLocation)
        {
            WriteStringAttribute(element, "geopointsrc", precisionLocation.Geopointsrc);
            WriteStringAttribute(element, "altsrc", precisionLocation.Altsrc);
            WriteAdditionalAttributes(element, precisionLocation.AdditionalAttributes);
        }

        private static void WriteStatusAttributes(XmlElement element, Status status)
        {
            element.SetAttribute("battery", XmlConvert.ToString(status.Battery));
            WriteAdditionalAttributes(element, status.AdditionalAttributes);
        }

        private static void WriteTakvAttributes(XmlElement element, Takv takv)
        {
            WriteStringAttribute(element, "device", takv.Device);
            WriteStringAttribute(element, "platform", takv.Platform);
            WriteStringAttribute(element, "os", takv.Os);
            WriteStringAttribute(element, "version", takv.Version);
            WriteAdditionalAttributes(element, takv.AdditionalAttributes);
        }

        private static void WriteTrackAttributes(XmlElement element, Track track)
        {
            element.SetAttribute("speed", XmlConvert.ToString(track.Speed));
            element.SetAttribute("course", XmlConvert.ToString(track.Course));
            WriteAdditionalAttributes(element, track.AdditionalAttributes);
        }

        private static void WriteStringAttribute(XmlElement element, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                element.SetAttribute(name, value);
            }
        }

        private static void WriteAdditionalAttributes(XmlElement element, XmlAttribute[] additionalAttributes)
        {
            if (additionalAttributes == null)
            {
                return;
            }

            foreach (var attribute in additionalAttributes)
            {
                if (attribute == null)
                {
                    continue;
                }

                var imported = (XmlAttribute)element.OwnerDocument.ImportNode(attribute, deep: true);
                element.Attributes.Append(imported);
            }
        }

        private static XmlElement[] ParseResidualDetail(string xmlDetail)
        {
            if (string.IsNullOrWhiteSpace(xmlDetail))
            {
                return Array.Empty<XmlElement>();
            }

            var doc = new XmlDocument();
            doc.LoadXml($"<detail>{xmlDetail}</detail>");

            return doc.DocumentElement?
                .ChildNodes
                .OfType<XmlElement>()
                .Select(e => (XmlElement)e.CloneNode(deep: true))
                .ToArray() ?? Array.Empty<XmlElement>();
        }

        private static System.Collections.Generic.HashSet<string> GetOverrideElementNames(XmlElement[] additionalElements)
        {
            var names = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);

            if (additionalElements == null)
            {
                return names;
            }

            foreach (var element in additionalElements)
            {
                if (element != null)
                {
                    names.Add(element.Name);
                }
            }

            return names;
        }
    }
}

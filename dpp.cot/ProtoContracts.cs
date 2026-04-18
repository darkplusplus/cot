using ProtoBuf;

namespace dpp.cot
{
    [ProtoContract]
    internal sealed class ProtoTakMessage
    {
        [ProtoMember(1, Name = @"takControl")]
        public ProtoTakControl TakControl { get; set; }

        [ProtoMember(2, Name = @"cotEvent")]
        public ProtoCotEvent CotEvent { get; set; }

        [ProtoMember(3, Name = @"submissionTime")]
        public ulong SubmissionTime { get; set; }

        [ProtoMember(4, Name = @"creationTime")]
        public ulong CreationTime { get; set; }
    }

    [ProtoContract]
    internal sealed class ProtoTakControl
    {
        [ProtoMember(1)]
        public uint MinProtoVersion { get; set; }

        [ProtoMember(2)]
        public uint MaxProtoVersion { get; set; }

    }

    [ProtoContract]
    internal sealed class ProtoCotEvent
    {
        [ProtoMember(1, Name = @"type")]
        public string Type { get; set; } = "";

        [ProtoMember(2, Name = @"access")]
        public string Access { get; set; } = "";

        [ProtoMember(3, Name = @"qos")]
        public string Qos { get; set; } = "";

        [ProtoMember(4, Name = @"opex")]
        public string Opex { get; set; } = "";

        [ProtoMember(16, Name = @"caveat")]
        public string Caveat { get; set; } = "";

        [ProtoMember(17, Name = @"releaseableTo")]
        public string ReleaseableTo { get; set; } = "";

        [ProtoMember(5, Name = @"uid")]
        public string Uid { get; set; } = "";

        [ProtoMember(6, Name = @"sendTime")]
        public ulong SendTime { get; set; }

        [ProtoMember(7, Name = @"startTime")]
        public ulong StartTime { get; set; }

        [ProtoMember(8, Name = @"staleTime")]
        public ulong StaleTime { get; set; }

        [ProtoMember(9, Name = @"how")]
        public string How { get; set; } = "";

        [ProtoMember(10, Name = @"lat")]
        public double Lat { get; set; }

        [ProtoMember(11, Name = @"lon")]
        public double Lon { get; set; }

        [ProtoMember(12, Name = @"hae")]
        public double Hae { get; set; }

        [ProtoMember(13, Name = @"ce")]
        public double Ce { get; set; }

        [ProtoMember(14, Name = @"le")]
        public double Le { get; set; }

        [ProtoMember(15, Name = @"detail")]
        public ProtoDetail Detail { get; set; }
    }

    [ProtoContract]
    internal sealed class ProtoDetail
    {
        [ProtoMember(1, Name = @"xmlDetail")]
        public string XmlDetail { get; set; } = "";

        [ProtoMember(2, Name = @"contact")]
        public Contact Contact { get; set; }

        [ProtoMember(3, Name = @"group")]
        public Group Group { get; set; }

        [ProtoMember(4, Name = @"precisionLocation")]
        public PrecisionLocation PrecisionLocation { get; set; }

        [ProtoMember(5, Name = @"status")]
        public Status Status { get; set; }

        [ProtoMember(6, Name = @"takv")]
        public Takv Takv { get; set; }

        [ProtoMember(7, Name = @"track")]
        public Track Track { get; set; }
    }
}

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MultiFinger.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FigureType
    {
        [EnumMember(Value = "point")]
        Point = 0,
        [EnumMember(Value = "line")]
        Line = 1,
    }
}

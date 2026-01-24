using System.Text.Json.Serialization;
using be_lspmpi.Models;

namespace be_lspmpi.Json
{
    [JsonSerializable(typeof(User[]))]
    [JsonSerializable(typeof(User))]
    [JsonSerializable(typeof(Role))]
    [JsonSerializable(typeof(UserProfile))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
}
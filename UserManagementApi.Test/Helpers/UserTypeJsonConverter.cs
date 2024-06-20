using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserManagementAPI.Models;

namespace UserManagementAPI.Test.Helpers
{

    public class UserTypeJsonConverter : JsonConverter<UserType>
    {
        public override UserType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return Enum.TryParse<UserType>(value, true, out var userType) ? userType : UserType.Client;
        }

        public override void Write(Utf8JsonWriter writer, UserType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
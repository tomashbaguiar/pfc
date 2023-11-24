using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Template.Api.Models;

public class Template
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string? Name { get; set; } = null!;

    [BsonElement("Rule")]
    public string? Rule { get; set; } = null!;
}
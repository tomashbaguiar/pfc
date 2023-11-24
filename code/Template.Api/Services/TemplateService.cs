using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Template.Api.Settings;

namespace Template.Api.Services;

public class TemplateService
{
    private readonly IMongoCollection<Models.Template> _templatesCollection;

    public TemplateService(IOptions<TemplateStorageDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

        _templatesCollection = mongoDatabase.GetCollection<Models.Template>(
            settings.Value.TemplatesCollectionName);
    }

    public async Task<Models.Template?> GetAsync(string name) =>
        await _templatesCollection.Find(x => x.Name == name)
            .FirstOrDefaultAsync();
    
    public async Task CreateAsync(Models.Template template) =>
        await _templatesCollection.InsertOneAsync(template);

    public async Task UpdateAsync(string name, Models.Template template) =>
        await _templatesCollection.ReplaceOneAsync(x => x.Name == name, template);
    
    public async Task RemoveAsync(string name) =>
        await _templatesCollection.DeleteOneAsync(x => x.Name == name);
}
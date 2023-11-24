namespace Template.Api.Settings;

public class TemplateStorageDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string TemplatesCollectionName { get; set; } = null!;
}
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyTicket.Infrastructure.Localization;
public class JsonStringLocalizer : IStringLocalizer
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializer _serializer = new();

    public JsonStringLocalizer(IDistributedCache cache)
    {
        _cache = cache;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var actualValue = this[name];
            return !actualValue.ResourceNotFound
                ? new LocalizedString(name, string.Format(actualValue.Value, arguments))
                : actualValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var filePath = $"Localization/{Thread.CurrentThread.CurrentCulture.Name}.json";

        using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader streamReader = new(stream);
        using JsonTextReader reader = new(streamReader);

        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName)
                continue;

            var key = reader.Value as string;
            reader.Read();
            var value = _serializer.Deserialize<string>(reader);
            yield return new LocalizedString(key, value);
        }
    }

    private string GetFilePath()
    {
        var fileName = $"{Thread.CurrentThread.CurrentCulture.Name}.json";
        var rootPath = AppContext.BaseDirectory;
        return Path.Combine("/Users/zeyneb/Projects/MyTicket/Infrastructure/MyTicket.Infrastructure/Localization", fileName);
    }

    private string GetString(string key)
    {
        var fullFilePath = GetFilePath();

        if (!File.Exists(fullFilePath))
        {
            return $"[{key} not found in {Thread.CurrentThread.CurrentCulture.Name}]";
        }

        var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        var cacheValue = _cache.GetString(cacheKey);

        if (!string.IsNullOrEmpty(cacheValue))
            return cacheValue;

        try
        {
            var result = GetValueFromJSON(key, fullFilePath);

            if (!string.IsNullOrEmpty(result))
            {
                _cache.SetString(cacheKey, result);
                return result;
            }
        }
        catch (Exception ex)
        {
            // Log exception
            return $"[{key} not found]";
        }

        return $"[{key} not found]";
    }

    private string GetValueFromJSON(string propertyName, string filePath)
    {
        try
        {
            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader reader = new(stream);
            var json = JObject.Parse(reader.ReadToEnd());

            return json[propertyName]?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading localization file {filePath}: {ex.Message}");
            return string.Empty;
        }
    }
}


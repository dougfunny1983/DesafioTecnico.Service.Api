using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesafioTecnico.Service.DotNet8.InfrastructureCore.Util
{
    public static class JsonFileReader
    {
        public static async Task<IEnumerable<T>> ReadJsonFileAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var jsonData = await File.ReadAllTextAsync(filePath);

            var data = JsonSerializer.Deserialize<IEnumerable<T>>(jsonData);

            return data;
        }

        public static async Task<List<T>> ReadJsonFileAsListAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
            return data;
        }
    }
}

using Newtonsoft.Json;
using SpecFlowPdfReader.Models;

namespace SpecFlowPdfReader.Helpers
{
    public static class JsonHelper
    {
        public static PathModel? GetValues()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string jsonFilePath = Path.Combine(currentDirectory, "Resourses", "Paths.json");

            Console.Write(jsonFilePath);
            if (!File.Exists(jsonFilePath))
                return null;

            string json = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<PathModel>(json);
        }
    }
}
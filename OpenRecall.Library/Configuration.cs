using System.Text.Json;

namespace OpenRecall.Library
{
    public class Configuration
    {
        private const string ConfigurationFileName = "config.json";

        public string OpenAiApiKey { get; set; } = string.Empty;
        public int SnapshotInterval { get; set; }
        public int ActivitySnapshotThreashold { get; set; }

        public static Configuration Load()
        {
            if (!File.Exists(ConfigurationFileName))
            {
                throw new FileNotFoundException("Configuration file not found.");
            }

            var json = File.ReadAllText(ConfigurationFileName);
            var config = JsonSerializer.Deserialize<Configuration>(json) ?? throw new Exception("Failed to deserialize configuration file.");

            if (string.IsNullOrWhiteSpace(config.OpenAiApiKey))
            {
                throw new Exception("OpenAI API key is missing.");
            }

            return config;
        }
    }
}

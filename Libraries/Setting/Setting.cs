using System.Text.Json;
using System.Text.Json.Serialization;

namespace TunnelServer.Libraries.Setting
{
    public class SettingLoader
    {
        private readonly Logger.Logger _logger;
        private readonly string _filePath; // Removed nullable since we validate it in constructor
        private AppConfig? _config;

        /// <summary>
        /// Gets the logging configuration.
        /// </summary>
        public LoggingConfig? Logging { get; private set; }

        /// <summary>
        /// Gets the local configuration.
        /// </summary>
        public LocalConfig? Local { get; private set; }

        /// <summary>
        /// Gets the server configuration.
        /// </summary>
        public ServerConfig? Server { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingLoader"/> class.
        /// </summary>
        /// <param name="jsonFilePath">The path to the JSON configuration file.</param>
        /// <param name="logger">The logger instance for logging errors and information.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="jsonFilePath"/> or <paramref name="logger"/> is null.</exception>
        public SettingLoader(string jsonFilePath, Logger.Logger logger)
        {
            ArgumentNullException.ThrowIfNull(jsonFilePath, nameof(jsonFilePath));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _filePath = jsonFilePath.Trim();
            _logger = logger;
        }

        /// <summary>
        /// Loads and updates the application settings from the specified JSON file.
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown when the configuration file does not exist.</exception>
        /// <exception cref="JsonException">Thrown when the JSON file cannot be deserialized.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the configuration data is invalid or incomplete.</exception>
        public void UpdateSettings()
        {
            try
            {
                // Check if the configuration file exists
                if (!File.Exists(_filePath))
                {
                    throw new FileNotFoundException($"Setting file not found at: {_filePath}");
                }

                // Read the JSON content from the file
                string jsonContent = File.ReadAllText(_filePath);

                // Configure JSON deserialization options
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Ignore case sensitivity for property names
                    Converters = { new JsonStringEnumConverter() } // Support string-to-enum conversion (e.g., for LogLevel)
                };

                // Deserialize the JSON content into AppConfig
                _config = JsonSerializer.Deserialize<AppConfig>(jsonContent, options)
                    ?? throw new JsonException("Failed to deserialize the configuration file.");

                // Validate and assign configuration properties
                if (_config.Logging == null || _config.Local == null || _config.Server == null)
                {
                    throw new InvalidOperationException("Configuration file is missing required sections (Logging, Local, or Server).");
                }

                Logging = _config.Logging;
                Local = _config.Local;
                Server = _config.Server;
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error($"Configuration file error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.Error($"Failed to parse configuration: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error while loading configuration: {ex.Message}");
                throw;
            }
        }
    }
}
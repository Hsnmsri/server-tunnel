// using System;
// using System.IO;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace TunnelServer.Libraries.Setting
// {
//     public class AppSetting
//     {
//         private string FilePath;
//         public LocalConfiguration LocalConfig { get; set; } = new LocalConfiguration();
//         public ServerConfiguration ServerConfig { get; set; } = new ServerConfiguration();
//         public LoggingConfiguration LoggingConfig { get; set; } = new LoggingConfiguration();

//         public AppSetting(string filePath)
//         {
//             this.FilePath = filePath;
//         }

//         public async Task<bool> UpdateSettingsAsync(string? filePath = null)
//         {
//             try
//             {
//                 filePath = filePath ?? this.FilePath;

//                 if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
//                 {
//                     Console.WriteLine($"⚠ Warning: Settings file not found: {filePath}");
//                     return false;
//                 }

//                 // Read the file asynchronously
//                 string jsonString = await File.ReadAllTextAsync(filePath);

//                 // Deserialize data safely
//                 JsonSerializerOptions options = new JsonSerializerOptions
//                 {
//                     PropertyNameCaseInsensitive = true
//                 };

//                 AppConfiguration? settings;
//                 try
//                 {
//                     settings = JsonSerializer.Deserialize<AppConfiguration>(jsonString, options);
//                 }
//                 catch (JsonException ex)
//                 {
//                     Console.WriteLine($"❌ Error: Invalid JSON format in settings file: {ex.Message}");
//                     return false;
//                 }

//                 if (settings == null)
//                 {
//                     Console.WriteLine("❌ Error: Failed to fully load settings file!");
//                     return false;
//                 }

//                 // Convert string Level to LoggingLevel enum
//                 if (!Enum.TryParse(settings.Logging?.Level, true, out LoggingLevel loggingLevel))
//                 {
//                     loggingLevel = LoggingLevel.Debug; // Default to Debug if conversion fails
//                 }

//                 // Assign values with null checks
//                 this.LocalConfig = settings.Local ?? new LocalConfiguration();
//                 this.ServerConfig = settings.Server ?? new ServerConfiguration();
//                 this.LoggingConfig = settings.Logging ?? new LoggingConfiguration { Level = loggingLevel };

//                 // Update the file path in case it was changed
//                 this.FilePath = filePath;

//                 Console.WriteLine("✅ Settings successfully updated!");
//                 return true;
//             }
//             catch (Exception error)
//             {
//                 Console.WriteLine($"❌ Error loading settings file: {error.Message}");
//                 return false;
//             }
//         }
//     }

//     public class AppConfiguration
//     {
//         public LocalConfiguration? Local { get; set; }
//         public ServerConfiguration? Server { get; set; }
//         public LoggingConfiguration? Logging { get; set; }
//     }

//     public class LocalConfiguration
//     {
//         public string Ip { get; set; } = "0.0.0.0";  // Default value to avoid null
//         public int Port { get; set; } = 8000;       // Default value to avoid null
//     }

//     public class ServerConfiguration
//     {
//         public string Ip { get; set; } = "0.0.0.0";  // Default value to avoid null
//         public int Port { get; set; } = 8000;       // Default value to avoid null
//     }

//     public class LoggingConfiguration
//     {
//         public LoggingLevel Level { get; set; } = LoggingLevel.Debug; // Default value to avoid null
//     }

//     public enum LoggingLevel
//     {
//         None = 1,
//         Debug = 2,
//         Warning = 3,
//         Error = 4,
//     }
// }

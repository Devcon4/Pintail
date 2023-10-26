// Model for returning settings to client
public record SettingDto(string? AppName);

// Model matching AppSettings.json
public class SettingOptions {
  public string AppName { get; set; } = "Kite Dashboard";
}
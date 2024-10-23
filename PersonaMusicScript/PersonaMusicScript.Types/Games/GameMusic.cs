namespace PersonaMusicScript.Types.Games;

public class GameMusic
{
    public Game Game { get; set; }

    public int Version { get; set; }

    public string DefaultBaseReplacementPath { get; set; } = string.Empty;

    public string? DefaultEncoder { get; set; }

    public List<Song> Songs { get; set; } = new();
}

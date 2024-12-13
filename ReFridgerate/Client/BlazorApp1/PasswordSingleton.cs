namespace BlazorApp1;

public sealed class PasswordSingleton {
    public string Key { get; set; } = "#"; 
    public string Cx { get; set; } = "#"; 
    private static readonly Lazy<PasswordSingleton> lazy =
        new Lazy<PasswordSingleton>(() => new PasswordSingleton());

    public static PasswordSingleton Instance { get { return lazy.Value; } }

    private PasswordSingleton()
    {}
}
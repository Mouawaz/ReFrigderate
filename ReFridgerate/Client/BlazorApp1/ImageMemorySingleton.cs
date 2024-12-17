namespace BlazorApp1;

public sealed class ImageMemorySingleton {
    private Dictionary<string, string?> ImageMemory;
    private static readonly Lazy<ImageMemorySingleton> lazy =
        new Lazy<ImageMemorySingleton>(() => new ImageMemorySingleton());  
    public static ImageMemorySingleton Instance { get { return lazy.Value; } }
    public ImageMemorySingleton() {
        ImageMemory = new();
    }
    public bool containsKey(string key) {
        return ImageMemory.ContainsKey(key);
    }
    public void add(string key, string value) {
        ImageMemory.Add(key, value);
    }
    public string get(string key) {
        return ImageMemory[key] ?? "https://www.google.com/imgres?q=error%20png&imgurl=https%3A%2F%2Fe7.pngegg.com%2Fpngimages%2F285%2F84%2Fpng-clipart-computer-icons-error-super-8-film-angle-triangle-thumbnail.png&imgrefurl=https%3A%2F%2Fwww.pngegg.com%2Fen%2Fsearch%3Fq%3Derror&docid=Pwxq0JhpzuoCXM&tbnid=VMz0C3m3-GsgGM&vet=12ahUKEwi57c2c7qSKAxUhm_0HHTtzG6MQM3oECBgQAA..i&w=348&h=348&hcb=2&ved=2ahUKEwi57c2c7qSKAxUhm_0HHTtzG6MQM3oECBgQAA";
    }
}
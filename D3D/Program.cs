namespace D3D;

public static class Program
{
    public static void Main(string[] args)
    {
        using var game = new Core();
        game.Run();
    }
}

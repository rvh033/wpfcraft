namespace wpfcraftserver
{
    public class WPFCraftServer()
    {

        string Ver = "0.1";
        Server Server;

        public static void Main(string[] args)
        {
            WPFCraftServer p = new();
            p.Start(args);
        }

        void Start(string[] args)
        {
            Console.WriteLine($"Starting WPFCraft Server version {Ver}");
            Console.WriteLine($"Copyright RVH Productions 2026");
            Console.WriteLine($"Licensed under MIT\n\n");
            Server = new(this, "127.0.0.1", 8888);
        }
    }
}

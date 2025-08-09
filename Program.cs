using System.Threading.Tasks;

namespace RyeBot
{
    // Program entry point
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}

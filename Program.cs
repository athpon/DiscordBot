using System;

namespace TestDiscord
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunAsyn().GetAwaiter().GetResult();
        }
    }
}

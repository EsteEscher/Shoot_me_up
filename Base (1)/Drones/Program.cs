using Player.Helpers;

namespace Player
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            Player p1 = new Player(GameSpace.WIDTH / 2, GameSpace.HEIGHT / 2, "Goku");
            List<Ennemis> army = new List<Ennemis>();
            for (int i = 0; i < 10; i++)
                army.Add(new Ennemis(RandomHelpers.Next(10, GameSpace.WIDTH - 30), 10));


            // Démarrage
            Application.Run(new GameSpace(p1, army));


        }
    }
}
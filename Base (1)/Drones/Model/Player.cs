using Player.Helpers;
using Game.Properties;

namespace Player
{
    // Cette partie de la classe Drone définit ce qu'est un drone par un modèle numérique
    public class Player
    {
        public int hp;                                // Ces Pv                      
        public int x;                                 // Position en X depuis la gauche de l'espace aérien
        public int y;                                 // Position en Y depuis le haut de l'espace aérien
        private Random _alea = new Random();

        // Constructeur
        public Player(int x, int y, string name)
        {
            Random alea = new Random();
            this.x = x;
            this.y = y;
            hp = 3; // La charge initiale de la batterie est choisie aléatoirement
        }

        // Cette méthode calcule le nouvel état dans lequel le drone se trouve après
        // que 'interval' millisecondes se sont écoulées
        public void Update(int interval)
        {
        }

        // Choisit vitesse
        public void ChangeDirection(int side)
        {
            if (side == 1)
                x += 5;
            if (side == 2)
                y -= 5;
            if (side == 3)
                x -= 5;
            if (side == 4)
                y += 5;

            //Barriere invisible
            if (x <= 0)
                x = 0;
            if (x >= GameSpace.WIDTH - 100)
                x = GameSpace.WIDTH - 100;
            if (y <= 0)
                y = 0;
            if (y >= GameSpace.HEIGHT - 100)
                y = GameSpace.HEIGHT - 100;
        }

        /// //////////////////////////////////////////////////////////////////////////////
        //  
        //  Ce qui suit appartient à la vue, pas au modèle.
        //  Il aurait été préférable de séparer la déclaration de la classe Drone en deux,
        //  Nous regroupons tout ici pour simplifier
        //  
        /// //////////////////////////////////////////////////////////////////////////////

        private Pen droneBrush = new Pen(new SolidBrush(Color.Purple), 3);

        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.Goku, x, y, 125, 100);
            drawingSpace.Graphics.DrawString($"{this}", TextHelpers.drawFont, TextHelpers.writingBrush, x + 5, y - 25);
        }

        // De manière textuelle
        public override string ToString()
        {
            return $"Goku ({((int)((double)hp / 1000 * 100)).ToString()}%)";
        }


    }
}

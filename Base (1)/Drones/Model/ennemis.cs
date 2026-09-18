using Game.Properties;
using Player.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    // Cette partie de la classe Drone définit ce qu'est un drone par un modèle numérique
    public class Ennemis
    {
        public int hp;                                // Ces Pv                      
        public int x;                                 // Position en X depuis la gauche de l'espace aérien
        public int y;                                 // Position en Y depuis le haut de l'espace aérien
        private Random _alea = new Random();

        // Constructeur
        public Ennemis(int x, int y, string name)
        {
            Random alea = new Random();
            this.x = x;
            this.y = y;
            hp = 3; // Leur Pv
        }

        // Cette méthode calcule le nouvel état dans lequel le drone se trouve après
        // que 'interval' millisecondes se sont écoulées
        public void Update(int interval)
        {
        }

        // Choisit une nouvelle vitesse aléatoirement
        public void ChangeDirection(int side)
        {
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
            drawingSpace.Graphics.DrawImage(Resources.saib, x, y, 125, 100);
            drawingSpace.Graphics.DrawString($"{this}", TextHelpers.drawFont, TextHelpers.writingBrush, x + 5, y - 25);
        }

        // De manière textuelle
        public override string ToString()
        {
            return $"Saibaman ({((int)((double)hp / 1000 * 100)).ToString()}%)";
        }


    }
}

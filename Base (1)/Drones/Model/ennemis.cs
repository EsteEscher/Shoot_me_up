using Game.Properties;
using Player.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace Player
{
    // Cette partie de la classe Drone définit ce qu'est un drone par un modèle numérique
    public class Ennemis
    {
        public int hp;                                // Ces Pv                      
        private int _x;                                 // Position en X depuis la gauche de l'espace aérien
        private int _y;                                 // Position en Y depuis le haut de l'espace aérien
        public const int HEIGHTSIZE = 62;
        public const int WIDTHSIZE = 50;
        private int _targetX;
        public State state = State.TRAVELLING;
        private Player _player;

        public enum State { TRAVELLING, SEEKING, STOPPING, EXPLOSING }


        public int Y { get => _y; set => _y = value; }
        public int X { get => _x; set => _x = value; }

        // Constructeur
        public Ennemis(int x, int y, Player player)
        {
            Random alea = new Random();
            this.X = x;
            this.Y = y;
            hp = 3; // Leur Pv
            this._player = player;
        }

        // Cette méthode calcule le nouvel état dans lequel le drone se trouve après
        // que 'interval' millisecondes se sont écoulées
        public void Update(int interval)
        {
            if (_y <= GameSpace.HEIGHT - 200)
            {
                state = State.TRAVELLING;
                _y += 5;
            
            }
            if (_y >= GameSpace.HEIGHT - 200)
            {
                state = State.SEEKING;
                if (_player.x < _x)
                {
                    _x--;
                }
                if (_player.y < _y)
                {
                    _y--;
                }
                if (_player.x > _x)
                {
                    _x++;
                }
                if (_player.y > _y)
                {
                    _y++;
                }
            }
            if (_x == _player.x && _y == _player.y)
            {
                state = State.STOPPING;
                state = State.EXPLOSING;
                return;
            }
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


        // De manière graphique
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.saib, _x, _y, HEIGHTSIZE, WIDTHSIZE);
        }


    }
}

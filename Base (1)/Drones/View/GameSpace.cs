using System.Runtime.InteropServices;

namespace Player
{
    // La classe AirSpace représente le territoire au dessus duquel les drones peuvent voler
    // Il s'agit d'un formulaire (une fenêtre) qui montre une vue 2D depuis en dessus
    // Il n'y a donc pas de notion d'altitude qui intervient

    public partial class GameSpace : Form
    {
        public static readonly int WIDTH = 1200;        // Dimensions of the airspace
        public static readonly int HEIGHT = 600;

        // La flotte est l'ensemble des drones qui évoluent dans notre espace aérien
        private Player _player;
        private List<Ennemis> _ennemis= new List<Ennemis>();
        private List<Tirs> _bullets = new List<Tirs>();

        BufferedGraphicsContext currentContext;
        BufferedGraphics gamespace;

        // Initialisation de l'espace aérien avec un certain nombre de drones
        public GameSpace(Player player, List<Ennemis> ennemis, List<Tirs> bullets)
        {
            InitializeComponent();
            ClientSize = new Size(WIDTH, HEIGHT);

            // Gets a reference to the current BufferedGraphicsContext
            currentContext = BufferedGraphicsManager.Current;
            // Creates a BufferedGraphics instance associated with this form, and with
            // dimensions the same size as the drawing surface of the form.
            gamespace = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            this._player = player;
            this._ennemis = ennemis;
            this._bullets = bullets;
        }

        // Affichage de la situation actuelle
        private void Render()
        {
            gamespace.Graphics.Clear(Color.AliceBlue);

            _player.Render(gamespace);

            foreach (Ennemis ennemis in _ennemis)
            {
                ennemis.Render(gamespace);
            }

            gamespace.Render();
        }

        // Calcul du nouvel état après que 'interval' millisecondes se sont écoulées
        private void Update(int interval)
        {
            _player.Update(interval);

            for (int i = _ennemis.Count -1; i >= 0; i--)
            {
                _ennemis[i].Update(interval);

                if (_ennemis[i].state == global::Player.Ennemis.State.DEAD)
                {
                    _ennemis.RemoveAt(i);
                }
            }

        }

        // Méthode appelée à chaque frame
        private void NewFrame(object sender, EventArgs e)
        {
            this.Update(ticker.Interval);
            this.Render();
        }

        //Mouvement
        private void AirSpace_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.D:
                    _player.ChangeDirection(1);
                    break;

                case Keys.Up:
                case Keys.W:
                    _player.ChangeDirection(2);
                    break;

                case Keys.Left:
                case Keys.A:
                    _player.ChangeDirection(3);
                    break;

                case Keys.Down:
                case Keys.S:
                    _player.ChangeDirection(4);
                    break;

                case Keys.LButton:
                case Keys.Space:
                    _bullets.Add(new Tirs(_player.x, _player.y,5));
                    break;
            }
        }
    }
}
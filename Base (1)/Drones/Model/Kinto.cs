using Game.Properties;

namespace Player
{
    public class Kinto
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; } = 90;
        public int Height { get; private set; } = 50;

        public const int MAX_HP = 3;
        public int Hp { get; set; } = MAX_HP;

        // Le nuage est détruit si ses PV tombent à 0
        public bool IsDestroyed => Hp <= 0;

        // Timer de réapparition en millisecondes (15 secondes = 15 000 ms)
        private const int RESPAWN_DELAY = 15000;
        private int _respawnTimer = 0;

        public Kinto(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void TakeDamage(int damage = 1)
        {
            if (IsDestroyed) return;

            Hp -= damage;
            if (Hp <= 0)
            {
                Hp = 0;
                _respawnTimer = RESPAWN_DELAY; // Enclenche le décompte de 15 secondes
            }
        }

        public void Update(int interval)
        {
            if (IsDestroyed)
            {
                _respawnTimer -= interval;
                if (_respawnTimer <= 0)
                {
                    // Régénération complète du nuage Kinto !
                    Hp = MAX_HP;
                    _respawnTimer = 0;
                }
            }
        }

        public void Render(BufferedGraphics drawingSpace)
        {
            // S'il est détruit, on ne le dessine pas pendant son temps de recharge
            if (IsDestroyed) return;

            drawingSpace.Graphics.DrawImage(Resources.FlyingNimbus, X, Y, Width, Height);
        }
    }
}
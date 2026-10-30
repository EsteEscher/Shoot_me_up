using Game.Properties;

namespace Player
{
    public class Kinto
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; } = 100;
        public int Height { get; private set; } = 60;

        public const int MAX_HP = 3;
        public int Hp { get; set; } = MAX_HP;

        // Le nuage est détruit si ses PV tombent à 0
        public bool IsDestroyed => Hp <= 0;

        // Timer de réapparition
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
                _respawnTimer = RESPAWN_DELAY;
            }
        }

        public void Update(int interval)
        {
            if (IsDestroyed)
            {
                _respawnTimer -= interval;
                if (_respawnTimer <= 0)
                {
                    // Régénération complete de la barriere
                    Hp = MAX_HP;
                    _respawnTimer = 0;
                }
            }
        }

        public void Render(BufferedGraphics drawingSpace)
        {
            if (IsDestroyed) return;

            drawingSpace.Graphics.DrawImage(Resources.FlyingNimbus, X, Y, Width, Height);
        }
    }
}
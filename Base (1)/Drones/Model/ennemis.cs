using Game.Properties;
using Player.Helpers;

namespace Player
{
    public class Ennemis
    {
        public const int POINTS_VALUE = 100;
        public const int HEIGHTSIZE = 70;
        public const int WIDTHSIZE = 60;

        public int hp;
        private int _x;
        private int _y;
        public State state = State.TRAVELLING;
        private Player _player;
        private int _explosionTimer = 0;
        private int _shootCooldown = RandomHelpers.Next(10, 40);

        public enum State { TRAVELLING, SEEKING, STOPPING, EXPLOSING, DEAD }

        public int Y { get => _y; set => _y = value; }
        public int X { get => _x; set => _x = value; }

        public Ennemis(int x, int y, Player player)
        {
            this.X = x;
            this.Y = y;
            hp = 3;
            this._player = player;
            state = State.TRAVELLING;
        }

        public void Update(int interval, List<Kinto> kintos)
        {
            if (state == State.EXPLOSING || state == State.DEAD)
            {
                _explosionTimer++;
                if (_explosionTimer >= 5)
                {
                    state = State.DEAD;
                }
                return;
            }

            int nextX = _x;
            int nextY = _y;

            if (_y <= GameSpace.HEIGHT - 200)
            {
                state = State.TRAVELLING;
                nextY += 5;
            }
            else
            {
                state = State.SEEKING;
                if (_player.x < _x) nextX--;
                if (_player.x > _x) nextX++;
                if (_player.y < _y) nextY--;
                if (_player.y > _y) nextY++;
            }

            Rectangle nextEnnemiRect = new Rectangle(nextX, nextY, WIDTHSIZE, HEIGHTSIZE);

            bool blocked = false;
            foreach (Kinto kinto in kintos)
            {
                if (kinto.IsDestroyed) continue;

                Rectangle kintoRect = new Rectangle(kinto.X, kinto.Y, kinto.Width, kinto.Height);
                if (nextEnnemiRect.IntersectsWith(kintoRect))
                {
                    blocked = true;
                    kinto.TakeDamage(1);
                    break;
                }
            }

            if (!blocked)
            {
                _x = nextX;
                _y = nextY;
            }

            if (Math.Abs(_x - _player.x) < 20 && Math.Abs(_y - _player.y) < 20)
            {
                state = State.EXPLOSING;
                _player.hp--;
            }
        }

        public TirEnnemi? TryShoot()
        {
            // Ne tire pas s'il explose, s'il est mort, OU s'il a atteint la zone basse (SEEKING) !
            if (state == State.EXPLOSING || state == State.DEAD || _y >= GameSpace.HEIGHT - 200)
                return null;

            _shootCooldown--;
            if (_shootCooldown <= 0)
            {
                _shootCooldown = RandomHelpers.Next(90, 220);

                double spawnX = _x + WIDTHSIZE / 2.0;
                double spawnY = _y + HEIGHTSIZE;

                double targetX = _player.x + 30;
                double targetY = _player.y + 20;

                return new TirEnnemi(spawnX, spawnY, targetX, targetY);
            }

            return null;
        }

        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(state != State.EXPLOSING ? Resources.saib : Resources.boom, _x, _y, WIDTHSIZE, HEIGHTSIZE);
        }
    }
}
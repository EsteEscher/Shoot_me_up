using Shoot_me_up.Helpers;
using Game.Properties;

namespace Shoot_me_up
{
    public class Player
    {
        public int hp;
        public int x;
        public int y;

        public Player(int startX, int startY, string name)
        {
            x = startX;
            y = startY;
            hp = 3;
        }

        public void Update(int interval)
        {
        }

        public void ChangeDirection(int side, List<Kinto> kintos)
        {
            int nextX = x;
            int nextY = y;

            if (side == 1)
                nextX += 10;
            if (side == 2)
                nextY -= 10;
            if (side == 3)
                nextX -= 10;
            if (side == 4)
                nextY += 10;

            // Barrières horizontales
            if (nextX <= 0)
                nextX = 0;
            if (nextX >= GameSpace.WIDTH - 100)
                nextX = GameSpace.WIDTH - 100;
            // Barrières verticales
            if (nextY <= GameSpace.HEIGHT - 200)
                nextY = GameSpace.HEIGHT - 200;
            if (nextY >= GameSpace.HEIGHT - 100)
                nextY = GameSpace.HEIGHT - 100;

            Rectangle nextPlayerRect = new Rectangle(nextX + 10, nextY + 10, 60, 70);

            bool blocked = false;
            foreach (Kinto kinto in kintos)
            {
                if (kinto.IsDestroyed) continue;

                Rectangle kintoRect = new Rectangle(kinto.X, kinto.Y, kinto.Width, kinto.Height);
                if (nextPlayerRect.IntersectsWith(kintoRect))
                {
                    blocked = true;
                    break;
                }
            }

            if (!blocked)
            {
                x = nextX;
                y = nextY;
            }
        }

        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.Goku, x - 30, y - 10, 125, 100);
        }
    }
}
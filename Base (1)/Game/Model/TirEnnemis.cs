using Game.Properties;
using System.Drawing.Drawing2D;

namespace Shoot_me_up
{
    public class TirEnnemi
    {
        private const double SPEED = 7.0; // Vitesse du projectile ennemi
        private double _x;
        private double _y;
        private double _dirx;
        private double _diry;
        private float _angle;

        public int SizeX { get; private set; } = 25;
        public int SizeY { get; private set; } = 25;

        public double X => _x;
        public double Y => _y;

        // Sortie de l'écran par n'importe quel côté
        public bool IsOutOfBounds => _x < -50 || _x > GameSpace.WIDTH + 50 || _y < -50 || _y > GameSpace.HEIGHT + 50;

        public TirEnnemi(double startX, double startY, double targetX, double targetY)
        {
            _x = startX;
            _y = startY;

            double deltaX = targetX - startX;
            double deltaY = targetY - startY;

            double distance = MathHelpers.Distance(startX, startY, targetX, targetY);

            if (distance > 0)
            {
                _dirx = (deltaX / distance) * SPEED;
                _diry = (deltaY / distance) * SPEED;
            }
            else
            {
                _dirx = 0;
                _diry = SPEED; // Vers le bas par défaut
            }

            // Orientation du tir vers sa cible
            _angle = (float)(Math.Atan2(deltaY, deltaX) * 180.0 / Math.PI) - 90f;
        }

        public void Update(int interval)
        {
            _x += _dirx;
            _y += _diry;
        }

        public void Render(BufferedGraphics drawingSpace)
        {
            Graphics g = drawingSpace.Graphics;
            GraphicsState state = g.Save();

            g.TranslateTransform((float)_x + SizeX / 2f, (float)_y + SizeY / 2f);
            g.RotateTransform(_angle);
            g.DrawImage(Resources.enemyshot, -SizeX / 2f, -SizeY / 2f, SizeX, SizeY);
            g.Restore(state);
        }
    }
}
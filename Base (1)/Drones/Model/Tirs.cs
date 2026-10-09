using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace Player
{
    public class Tirs
    {
        private const int _SPEED = 10;
        private double _x;
        private double _y;
        private int _bulletsizex;
        private int _bulletsizey;
        private double _dirx;
        private double _diry;
        private float _angle;
        public double X => _x;
        public double Y => _y;
        public int SizeX => _bulletsizex;
        public int SizeY => _bulletsizey;


        public bool IsOutOfBounds => _y < -50 ||_x < -50 || _x > GameSpace.WIDTH || _y > GameSpace.HEIGHT +50;

        public Tirs(double startx, double starty,double targetx, double targety, int bulletsizex, int bulletsizey)
        {
            _x = startx;
            _y = starty;
            _bulletsizex = bulletsizex;
            _bulletsizey = bulletsizey;

            double deltax = targetx - startx;
            double deltay = targety - starty;

            double distance = MathHelpers.Distance(startx, starty, targetx, targety);

            if (distance > 0)
            {
                _dirx = (deltax / distance) * _SPEED;
                _diry = (deltay / distance) * _SPEED;
            }
            else
            {
                _dirx = 0;
                _diry = -_SPEED;
            }
            _angle = (float)(Math.Atan2(deltay, deltax) * 180 / Math.PI) + 90f;
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
            g.TranslateTransform((float)_x + _bulletsizex / 2f, (float)_y + _bulletsizey /2f);
            drawingSpace.Graphics.DrawImage(Resources.allyshot, (float)_x, (float)_y, _bulletsizex, _bulletsizey);
        }
    }
}

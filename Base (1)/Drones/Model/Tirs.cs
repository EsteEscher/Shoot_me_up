using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public bool IsOutOfBounds => _y < -50;

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

        }

        public void Update(int interval)
        {
            _x += _dirx;
            _y += _diry;
        }
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.allyshot, (float)_x, (float)_y, _bulletsizex, _bulletsizey);
        }
    }
}

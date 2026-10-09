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
        private int _x;
        private int _y;
        private int _bulletsizex;
        private int _bulletsizey;
        private double _dirx;
        private double _diry;


        public bool IsOutOfBounds => _y < -50;

        public Tirs(int startx, int starty,int targetx, int targety, int bulletsizex, int bulletsizey)
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
            _y -= _SPEED;
        }
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.allyshot, _x, _y, _bulletsizex, _bulletsizey);
        }
    }
}

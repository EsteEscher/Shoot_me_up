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
        private int _bulletsize;

        public Tirs(int x, int y, int bulletsize)
        {
            _x = x;
            _y = y;
            _bulletsize = bulletsize;
        }
        public void Render(BufferedGraphics drawingSpace)
        {
            drawingSpace.Graphics.DrawImage(Resources.allyshot, GameSpace.WIDTH / 2, GameSpace.HEIGHT / 2, 100, 100);
        }
    }
}

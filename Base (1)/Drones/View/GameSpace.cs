using Player.Helpers;

namespace Player
{
    public partial class GameSpace : Form
    {
        public static readonly int WIDTH = 1200;
        public static readonly int HEIGHT = 600;

        private int _waveNumber = 1;
        private int _waveCooldown = 0;
        private int _score = 0;
        private bool _isGameOver = false;

        private const int PLAYER_SHOOT_DELAY = 500;
        private int _playerShootCooldown = 0;

        private Player _player;
        private List<Ennemis> _ennemis = new List<Ennemis>();
        private List<Tirs> _bullets = new List<Tirs>();
        private List<Kinto> _kintos = new List<Kinto>();
        private List<TirEnnemi> _enemyBullets = new List<TirEnnemi>();

        BufferedGraphicsContext currentContext;
        BufferedGraphics gamespace;

        public GameSpace(Player player, List<Ennemis> ennemis, List<Tirs> bullets)
        {
            InitializeComponent();
            ClientSize = new Size(WIDTH, HEIGHT);

            currentContext = BufferedGraphicsManager.Current;
            gamespace = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            this._player = player;
            this._ennemis = ennemis;
            this._bullets = bullets;

            InitializeKintos();
        }

        private void InitializeKintos()
        {
            _kintos.Clear();
            int kintoY = HEIGHT - 200;
            int spacing = WIDTH / 6;

            for (int i = 1; i <= 5; i++)
            {
                _kintos.Add(new Kinto((i * spacing) - 45, kintoY));
            }
        }

        private void RestartGame()
        {
            _player.hp = 3;
            _player.x = WIDTH / 2;
            _player.y = HEIGHT - 100;

            _score = 0;
            _waveNumber = 1;
            _waveCooldown = 0;
            _playerShootCooldown = 0;
            _isGameOver = false;

            _bullets.Clear();
            _enemyBullets.Clear();
            _ennemis.Clear();

            InitializeKintos();
            SpawnWave();
        }

        private void Render()
        {
            gamespace.Graphics.Clear(Color.AliceBlue);

            _player.Render(gamespace);

            foreach (Tirs tir in _bullets)
            {
                tir.Render(gamespace);
            }

            foreach (Ennemis ennemis in _ennemis)
            {
                ennemis.Render(gamespace);
            }

            foreach (Kinto kinto in _kintos)
            {
                kinto.Render(gamespace);
            }

            foreach (TirEnnemi enemyTir in _enemyBullets)
            {
                enemyTir.Render(gamespace);
            }

            // HUD
            gamespace.Graphics.DrawString($"Vague : {_waveNumber}", TextHelpers.drawFont, TextHelpers.writingBrush, 20, 20);
            gamespace.Graphics.DrawString($"Score : {_score} pts", TextHelpers.drawFont, TextHelpers.writingBrush, 20, 40);
            gamespace.Graphics.DrawString($"PV Goku : {_player.hp}", TextHelpers.drawFont, TextHelpers.writingBrush, 20, 60);

            string reloadStatus = _playerShootCooldown <= 0 ? "Tir : PRÊT" : "Tir : RECHARGE...";
            Brush statusBrush = _playerShootCooldown <= 0 ? Brushes.Green : Brushes.OrangeRed;
            gamespace.Graphics.DrawString(reloadStatus, TextHelpers.drawFont, statusBrush, 20, 80);

            // Écran de Game Over
            if (_isGameOver)
            {
                gamespace.Graphics.FillRectangle(TextHelpers.darkOverlayBrush, 0, 0, WIDTH, HEIGHT);

                string overText = "GAME OVER";
                string scoreText = $"Score Final : {_score} pts (Vague {_waveNumber})";
                string restartText = "Appuie sur 'R' pour retenter ta chance !";

                SizeF overSize = gamespace.Graphics.MeasureString(overText, TextHelpers.bigFont);
                SizeF scoreSize = gamespace.Graphics.MeasureString(scoreText, TextHelpers.mediumFont);
                SizeF restartSize = gamespace.Graphics.MeasureString(restartText, TextHelpers.drawFont);

                gamespace.Graphics.DrawString(overText, TextHelpers.bigFont, TextHelpers.redBrush, (WIDTH - overSize.Width) / 2, HEIGHT / 2 - 80);
                gamespace.Graphics.DrawString(scoreText, TextHelpers.mediumFont, TextHelpers.whiteBrush, (WIDTH - scoreSize.Width) / 2, HEIGHT / 2 - 10);
                gamespace.Graphics.DrawString(restartText, TextHelpers.drawFont, TextHelpers.whiteBrush, (WIDTH - restartSize.Width) / 2, HEIGHT / 2 + 40);
            }

            gamespace.Render();
        }

        private void Update(int interval)
        {
            if (_isGameOver) return;

            if (_player.hp <= 0)
            {
                _player.hp = 0;
                _isGameOver = true;
                return;
            }

            // Décompte du cooldown en millisecondes réelles
            if (_playerShootCooldown > 0)
            {
                _playerShootCooldown -= interval;
                if (_playerShootCooldown < 0)
                    _playerShootCooldown = 0;
            }

            _player.Update(interval);

            // 1. Tirs alliés
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Tirs tir = _bullets[i];
                tir.Update(interval);

                if (tir.IsOutOfBounds)
                {
                    _bullets.RemoveAt(i);
                    continue;
                }

                bool hit = false;
                Rectangle bulletRect = new Rectangle((int)tir.X, (int)tir.Y, tir.SizeX, tir.SizeY);

                for (int k = _kintos.Count - 1; k >= 0; k--)
                {
                    Kinto kinto = _kintos[k];
                    if (kinto.IsDestroyed) continue;

                    Rectangle kintoRect = new Rectangle(kinto.X, kinto.Y, kinto.Width, kinto.Height);
                    if (bulletRect.IntersectsWith(kintoRect))
                    {
                        kinto.TakeDamage(1);
                        hit = true;
                        break;
                    }
                }

                if (!hit)
                {
                    for (int j = _ennemis.Count - 1; j >= 0; j--)
                    {
                        Ennemis ennemi = _ennemis[j];
                        if (ennemi.state == Ennemis.State.EXPLOSING || ennemi.state == Ennemis.State.DEAD) continue;

                        Rectangle ennemiRect = new Rectangle(ennemi.X - 5, ennemi.Y - 5, Ennemis.WIDTHSIZE + 10, Ennemis.HEIGHTSIZE + 10);
                        if (bulletRect.IntersectsWith(ennemiRect))
                        {
                            ennemi.hp--;
                            if (ennemi.hp <= 0)
                            {
                                ennemi.state = Ennemis.State.EXPLOSING;
                                _score += Ennemis.POINTS_VALUE;
                            }
                            hit = true;
                            break;
                        }
                    }
                }

                if (hit)
                {
                    _bullets.RemoveAt(i);
                }
            }

            // 2. Ennemis
            for (int i = _ennemis.Count - 1; i >= 0; i--)
            {
                _ennemis[i].Update(interval, _kintos);

                if (_ennemis[i].state == Ennemis.State.DEAD)
                {
                    _ennemis.RemoveAt(i);
                }
            }

            // 3. Tirs ennemis
            foreach (Ennemis ennemi in _ennemis)
            {
                TirEnnemi? nouveauTir = ennemi.TryShoot();
                if (nouveauTir != null)
                {
                    _enemyBullets.Add(nouveauTir);
                }
            }

            // 4. Déplacement et collisions tirs ennemis
            for (int i = _enemyBullets.Count - 1; i >= 0; i--)
            {
                TirEnnemi tir = _enemyBullets[i];
                tir.Update(interval);

                if (tir.IsOutOfBounds)
                {
                    _enemyBullets.RemoveAt(i);
                    continue;
                }

                bool hit = false;
                Rectangle bulletRect = new Rectangle((int)tir.X, (int)tir.Y, tir.SizeX, tir.SizeY);

                for (int k = _kintos.Count - 1; k >= 0; k--)
                {
                    Kinto kinto = _kintos[k];
                    if (kinto.IsDestroyed) continue;

                    Rectangle kintoRect = new Rectangle(kinto.X, kinto.Y, kinto.Width, kinto.Height);
                    if (bulletRect.IntersectsWith(kintoRect))
                    {
                        kinto.TakeDamage(1);
                        hit = true;
                        break;
                    }
                }

                if (!hit)
                {
                    Rectangle playerRect = new Rectangle(_player.x, _player.y, 80, 80);
                    if (bulletRect.IntersectsWith(playerRect))
                    {
                        _player.hp--;
                        hit = true;
                    }
                }

                if (hit)
                {
                    _enemyBullets.RemoveAt(i);
                }
            }

            // 5. Vagues d'ennemis
            if (_ennemis.Count == 0)
            {
                _waveCooldown++;

                if (_waveCooldown >= 60)
                {
                    _waveNumber++;
                    SpawnWave();
                    _waveCooldown = 0;
                }
            }

            // 6. Régénération des Kintos
            foreach (Kinto kinto in _kintos)
            {
                kinto.Update(interval);
            }
        }

        private void SpawnWave()
        {
            int enemiesCount = 4 + (_waveNumber * 2);

            for (int i = 0; i < enemiesCount; i++)
            {
                int spawnX = RandomHelpers.Next(30, WIDTH - Ennemis.WIDTHSIZE - 30);
                int spawnY = -RandomHelpers.Next(20, 150);

                Ennemis nouvelEnnemi = new Ennemis(spawnX, spawnY, _player);
                _ennemis.Add(nouvelEnnemi);
            }
        }

        private void NewFrame(object sender, EventArgs e)
        {
            this.Update(ticker.Interval);
            this.Render();
        }

        private void GameSpace_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isGameOver)
            {
                if (e.KeyCode == Keys.R)
                {
                    RestartGame();
                }
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.D:
                    _player.ChangeDirection(1, _kintos);
                    break;

                case Keys.Up:
                case Keys.W:
                    _player.ChangeDirection(2, _kintos);
                    break;

                case Keys.Left:
                case Keys.A:
                    _player.ChangeDirection(3, _kintos);
                    break;

                case Keys.Down:
                case Keys.S:
                    _player.ChangeDirection(4, _kintos);
                    break;

                case Keys.Space:
                    if (_playerShootCooldown <= 0)
                    {
                        Point mousePos = this.PointToClient(Cursor.Position);
                        int startx = _player.x + 15;
                        int starty = _player.y;

                        _bullets.Add(new Tirs(startx, starty, mousePos.X, mousePos.Y, 40, 40));
                        _playerShootCooldown = PLAYER_SHOOT_DELAY; // 200 ms avant le tir suivant
                    }
                    break;
            }
        }
    }
}
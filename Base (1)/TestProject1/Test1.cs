using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Goku = Player.Player;
using Saibamen = Player.Ennemis;
using Obstacle = Player.Kinto;
using Calc = Player.MathHelpers;

namespace Game.Tests
{
    [TestClass]
    public class GameUnitTests
    {
        [TestMethod]
        public void Player_InitialState_ShouldHaveThreeHp()
        {
            Goku player = new Goku(100, 100, "Goku");
            Assert.AreEqual(3, player.hp, "Goku doit débuter avec 3 PV.");
        }

        [TestMethod]
        public void Kinto_TakeDamage_ShouldBecomeDestroyedWhenHpReachesZero()
        {
            Obstacle kinto = new Obstacle(200, 200);
            kinto.TakeDamage(1);
            Assert.IsFalse(kinto.IsDestroyed);

            kinto.TakeDamage(2);
            Assert.IsTrue(kinto.IsDestroyed);
            Assert.AreEqual(0, kinto.Hp);
        }

        [TestMethod]
        public void Kinto_Respawn_ShouldRegenerateAfter15Seconds()
        {
            Obstacle kinto = new Obstacle(200, 200);
            kinto.TakeDamage(Obstacle.MAX_HP);
            Assert.IsTrue(kinto.IsDestroyed);

            kinto.Update(14000);
            Assert.IsTrue(kinto.IsDestroyed);

            kinto.Update(1000);
            Assert.IsFalse(kinto.IsDestroyed);
            Assert.AreEqual(Obstacle.MAX_HP, kinto.Hp);
        }

        [TestMethod]
        public void Ennemi_InitialHpAndPoints_ShouldMatchSpecifications()
        {
            Goku player = new Goku(100, 500, "Goku");
            Saibamen ennemi = new Saibamen(100, 50, player);

            Assert.AreEqual(3, ennemi.hp);
            Assert.AreEqual(100, Saibamen.POINTS_VALUE);
        }

        [TestMethod]
        public void MathHelpers_Distance_ShouldCalculateEuclideanDistance()
        {
            double distance = Calc.Distance(0, 0, 3, 4);
            Assert.AreEqual(5.0, distance, 0.001);
        }
    }
}
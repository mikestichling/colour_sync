using NUnit.Framework;
using ColourSync.Domain;
using ColourSync.Domain.Enums;
using System;

namespace ColourSync.Tests {
    public class PlayerTests
    {
        [Test]
        public void GivenANewPlayer_WhenInstantiatingIt_ItShouldBeValid()
        {
            var player = new Player("bob");

            Assert.IsNotNull(player);
            
        }

        [Test]
        public void GivenANewPlayer_WhenInstantiatingIt_ItShouldAnEmptyListOfMoves()
        {
            var player = new Player("bob");

            Assert.IsEmpty(player.Moves);
        }

        [Test]
        public void GivenANewPlayer_WhenInstantiatingIt_ItShouldHaveAName()
        {
            var player = new Player("bob");

            Assert.AreEqual("bob", player.Name);
        }

        [Test]
        public void GivenANewPlayer_WhenMakingAMove_ItShouldSaveTheMoveAndTimestamp()
        {
            var player = new Player("bob");
            var timestamp = DateTime.Now;
            player.MakeMove(Moves.Blue, timestamp);
            Assert.AreEqual(Moves.Blue, player.Moves[0].ChosenMove);
            Assert.AreEqual(timestamp, player.Moves[0].Timestamp);
        }
    }
}

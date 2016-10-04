using System;
using ColourSync.Domain;
using ColourSync.Domain.Enums;
using NUnit.Framework;

namespace ColourSync.Tests {
    public class MoveTests
    {
        [Test]
        public void GivenANewMove_WhenInstantiatingIt_ItShouldBeValid()
        {
            var move = new Move(Moves.Blue, DateTime.Now, new Player("bob", Guid.NewGuid()));
            
            Assert.IsNotNull(move);
        }

        [Test]
        public void GivenANewMove_WhenPlayingAMove_ItShouldSaveTheMove()
        {
            var move = new Move(Moves.Blue, DateTime.Now, new Player("bob", Guid.NewGuid()));
            
            Assert.AreEqual(Moves.Blue, move.ChosenMove);
        }

        [Test]
        public void GivenANewMove_WhenPlayingAMoveWithAGivenTimestamp_ItShouldSaveTheTimestamp()
        {
            var timestamp = DateTime.Now;
            var move = new Move(Moves.Blue, timestamp, new Player("bob", Guid.NewGuid()));

            Assert.AreEqual(timestamp, move.Timestamp);
        }
    }
}
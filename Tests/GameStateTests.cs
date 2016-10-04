using NUnit.Framework;
using ColourSync.Domain;
using ColourSync.Domain.Enums;
using System;

namespace ColourSync.Tests {
    public class GameStateTests
    {
        [Test]
        public void GivenANewGameState_WhenInstantiatingIt_ItShouldBeValid()
        {
            var state = new GameState();

            Assert.IsNotNull(state);
        }

        [Test]
        public void GivenANewGameState_WhenAddingNewTablesBySupplyingAName_ItShouldAddItToTheTablesList()
        {
            var state = new GameState();

            state.AddTable("abcd");

            Assert.IsNotEmpty(state.Tables);
        }

        [Test]
        public void GivenANewGameState_WhenAddingNewTablesBySupplyingAContructedTable_ItShouldAddItToTheTablesList()
        {
            var state = new GameState();

            state.AddTable(new Table(123));

            Assert.IsNotEmpty(state.Tables);
            Assert.AreEqual("zxtv", state.Tables[0].Name);
        }

        [Test]
        public void GivenANewGameState_WhenRemovingATablesByName_ItShouldRemoveItFromTheTablesList()
        {
            var state = new GameState();

            state.AddTable(new Table(123));
            state.RemoveTable("zxtv");
            Assert.IsEmpty(state.Tables);
        }

        [Test]
        public void GivenANewGameStateWithATableAndPlayers_WhenMakingAMoveForAPlayer_ItShouldSaveThatState()
        {
            var state = new GameState();
            state.AddTable("abcd");
            state.Tables[0].AddPlayer(new Player("bob", Guid.NewGuid()));
            var timestamp = DateTime.Now;

            state.MakeMoveForPlayer("abcd", "bob", Moves.Blue, timestamp);

            Assert.AreEqual(timestamp, state.Tables[0].Players[0].Moves[0].Timestamp);
            Assert.AreEqual(Moves.Blue, state.Tables[0].Players[0].Moves[0].ChosenMove);
        } 

        [Test]
        public void GivenANewGameState_WhenJoiningATableThatDoesntExist_ItShouldCreateTheTable()
        {
            var state = new GameState();
            state.JoinTable("abcd", "bob", Guid.NewGuid());

            Assert.AreEqual(1, state.Tables.Count);
            Assert.AreEqual(1, state.Tables[0].Players.Count);
        }

        [Test]
        public void GivenANewGameState_WhenLeavingATableThatExists_ItShouldUpdateThePlayerList()
        {
            var id = Guid.NewGuid();
            var state = new GameState();
            state.JoinTable("abcd", "bob", Guid.NewGuid());
            state.JoinTable("abcd", "bob2", id);

            state.LeaveTable("abcd", id);

            Assert.AreEqual(1, state.Tables.Count);
            Assert.AreEqual(1, state.Tables[0].Players.Count);

        }
    }
}

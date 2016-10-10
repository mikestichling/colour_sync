using NUnit.Framework;
using ColourSync.Domain;
using ColourSync.Domain.Enums;
using System;

namespace ColourSync.Tests {
    public class TableTests
    {
        [Test]
        public void GivenANewTable_WhenInstantiatingIt_ItShouldBeValid()
        {
            var table = new Table(123);

            Assert.IsNotNull(table);
        }

        [Test]
        public void GivenANewTable_WhenInstantiatingIt_ItShouldBeInstantiatedWithADefaultRandomTableName()
        {
            var table = new Table(123);

            Assert.IsFalse(string.IsNullOrEmpty(table.Name));
            Assert.IsTrue(table.Name.Length == 4);
            Assert.AreEqual("zxtv", table.Name);
        }

        [Test]
        public void GivenANewTable_WhenInstantiatingItWithATableName_ItShouldBeInstantiatedWithATheSuppliedTableName()
        {
            var table = new Table("abcd");

            Assert.IsFalse(string.IsNullOrEmpty(table.Name));
            Assert.IsTrue(table.Name.Length == 4);
            Assert.AreEqual("abcd", table.Name);
        }

        [Test]
        public void GivenANewTable_WhenInstantiatingIt_ItShouldHaveAnEmptyListOfPlayers()
        {
            var table = new Table("abcd");
           
            Assert.IsEmpty(table.Players);
        }

        [Test]
        public void GivenANewTable_WhenAddingAPlayer_ItShouldAddThemToThePlayersList()
        {
            var table = new Table("abcd");

            table.AddPlayer(new Player("bob", Guid.NewGuid()));
           
            Assert.IsNotEmpty(table.Players);
            Assert.AreEqual("bob", table.Players[0].Name);
        }

        [Test]
        public void GivenANewTable_WhenRemovingAPlayerByName_ItShouldRemoveThemFromThePlayersList()
        {
            var id = Guid.NewGuid();
            var table = new Table("abcd");
            table.AddPlayer(new Player("bob", id));
            
            table.RemovePlayer(id);
           
            Assert.IsEmpty(table.Players);
        }

        [Test]
        public void GivenANewTable_WhenRemovingAPlayerByNameThatDoesntExist_ItShouldThrowAnException()
        {
            var table = new Table("abcd");
            table.AddPlayer(new Player("bob", Guid.NewGuid()));
            Assert.That(() => { table.RemovePlayer(Guid.NewGuid()); }, Throws.InvalidOperationException);
        }

        [Test]
        public void GivenANewTable_WhenStartingTheGame_ItShouldSetTheRoundToOne()
        {
            var table = new Table("abcd");
            table.StartGame(123);

            Assert.AreEqual(1, table.CurrentRound);
        }

        [Test]
        public void GivenANewTable_WhenAdvancingToTheNextRound_ItShouldAdvanceTheRoundByOne()
        {
            var table = new Table("abcd");
            table.StartGame(123);
            table.NextRound(124);

            Assert.AreEqual(2, table.CurrentRound);
        }

        [Test]
        public void GivenANewTable_WhenAdvancingToTheNextRound_ItShouldOnlyAdvanceIfAllPlayersHaveMadeAMoveForThatRound()
        {
            var table = new Table("abcd");
            table.AddPlayer(new Player("bob", Guid.NewGuid()));
            table.AddPlayer(new Player("bob2", Guid.NewGuid()));
            table.StartGame(123);
            Assert.That(() => { table.NextRound(124);}, Throws.InvalidOperationException);
        }

        [Test]
        public void GivenANewTable_WhenAdvancingToTheNextRound_ItShouldAdvanceIfAllPlayersHaveMadeAMoveForThatRound()
        {
            var table = new Table("abcd");
            table.AddPlayer(new Player("bob", Guid.NewGuid()));
            table.AddPlayer(new Player("bob2", Guid.NewGuid()));
            table.Players[0].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[1].MakeMove(Moves.Blue, DateTime.Now);
            table.StartGame(123);
            table.NextRound(124);
            Assert.AreEqual(2, table.CurrentRound);
        }

        [Test]
        public void GivenANewTable_WhenCheckingHowManyPlayersStillNeedToMakeAMoveWhenNoneHaveMadeAMove_ItShouldReturnTwo()
        {
            var table = new Table("abcd");
            table.AddPlayer(new Player("bob", Guid.NewGuid()));
            table.AddPlayer(new Player("bob2", Guid.NewGuid()));
            
            table.StartGame(123);
            Assert.AreEqual(2, table.PlayersNeedingToMove);
        }

        [Test]
        public void GivenANewTable_WhenCheckingHowManyPlayersStillNeedToMakeAMoveWhenAllHaveMadeAMove_ItShouldReturnZero()
        {
            var table = new Table("abcd");
            table.AddPlayer(new Player("bob", Guid.NewGuid()));
            table.AddPlayer(new Player("bob2", Guid.NewGuid()));
            
            table.StartGame(123);

            table.Players[0].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[1].MakeMove(Moves.Blue, DateTime.Now);

            Assert.AreEqual(0, table.PlayersNeedingToMove);
        }

        [Test]
        public void GivenANewTable_WhenCreatingItWithUppercaseLetters_ItShoulsChangeItToLowerCase()
        {
            var table = new Table("ABCD");

            Assert.AreEqual("abcd", table.Name);
        }

        [Test]
        public void GivenATableWithSixPlayers_WhenFindingTheSlowestPlayer_ItShouldReturnJane()
        {
            var table = new Table("ABCD");
            table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Player2", Guid.NewGuid()));
            table.AddPlayer(new Player("Player3", Guid.NewGuid()));
            table.AddPlayer(new Player("Player4", Guid.NewGuid()));
            table.AddPlayer(new Player("Player5", Guid.NewGuid()));
            table.AddPlayer(new Player("Jane", Guid.NewGuid()));

            table.StartGame(123);

            table.Players[0].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[1].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[2].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[3].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[4].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[5].MakeMove(Moves.Blue, DateTime.Now);

            Assert.AreEqual("Jane", table.SlowestPlayer.Name);
        }

        [Test]
        public void GivenATableWithSixPlayers_WhenFindingThePlayersThatPickedTheMostPickedColour_ItShouldReturnBobJaneAndJohn()
        {
            var table = new Table("ABCD");
            table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Player2", Guid.NewGuid()));
            table.AddPlayer(new Player("Player3", Guid.NewGuid()));
            table.AddPlayer(new Player("John", Guid.NewGuid()));
            table.AddPlayer(new Player("Bob", Guid.NewGuid()));
            table.AddPlayer(new Player("Jane", Guid.NewGuid()));

            table.StartGame(123);

            table.Players[0].MakeMove(Moves.Yellow, DateTime.Now);
            table.Players[1].MakeMove(Moves.Green, DateTime.Now);
            table.Players[2].MakeMove(Moves.Red, DateTime.Now);
            table.Players[3].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[4].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[5].MakeMove(Moves.Blue, DateTime.Now);

            Assert.AreEqual(3, table.Loosers.Count);
            Assert.AreEqual("John", table.Loosers[0].Name);
            Assert.AreEqual("Bob", table.Loosers[1].Name);
            Assert.AreEqual("Jane", table.Loosers[2].Name);
        }

        [Test]
        public void GivenATableWithSixPlayers_WhenFindingThePlayersThatPickedTheMostPickedColourAndThereIsATie_ItShouldReturnBobJaneJohnAndJake()
        {
            var table = new Table("ABCD");
            table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Player2", Guid.NewGuid()));
            table.AddPlayer(new Player("Jake", Guid.NewGuid()));
            table.AddPlayer(new Player("John", Guid.NewGuid()));
            table.AddPlayer(new Player("Bob", Guid.NewGuid()));
            table.AddPlayer(new Player("Jane", Guid.NewGuid()));

            table.StartGame(123);

            table.Players[0].MakeMove(Moves.Yellow, DateTime.Now);
            table.Players[1].MakeMove(Moves.Green, DateTime.Now);
            table.Players[2].MakeMove(Moves.Red, DateTime.Now);
            table.Players[3].MakeMove(Moves.Red, DateTime.Now);
            table.Players[4].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[5].MakeMove(Moves.Blue, DateTime.Now);

            Assert.AreEqual(4, table.Loosers.Count);
            Assert.AreEqual("Jake", table.Loosers[0].Name);
            Assert.AreEqual("John", table.Loosers[1].Name);
            Assert.AreEqual("Bob", table.Loosers[2].Name);
            Assert.AreEqual("Jane", table.Loosers[3].Name);
        }

        [Test]
        public void GivenATableWithSixPlayers_WhenFindingTheSinglePlayerThatPickedTheJoker_ItShouldReturnBobJaneJohnJakeAndBilly()
        {
            var table = new Table("ABCD");
            table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Billy", Guid.NewGuid()));
            table.AddPlayer(new Player("Jake", Guid.NewGuid()));
            table.AddPlayer(new Player("John", Guid.NewGuid()));
            table.AddPlayer(new Player("Bob", Guid.NewGuid()));
            table.AddPlayer(new Player("Jane", Guid.NewGuid()));

            table.StartGame(123);

            table.Players[0].MakeMove(Moves.Joker, DateTime.Now);
            table.Players[1].MakeMove(Moves.Green, DateTime.Now);
            table.Players[2].MakeMove(Moves.Red, DateTime.Now);
            table.Players[3].MakeMove(Moves.Red, DateTime.Now);
            table.Players[4].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[5].MakeMove(Moves.Blue, DateTime.Now);

            Assert.AreEqual(5, table.JokerLoosers.Count);
            Assert.AreEqual("Billy", table.JokerLoosers[0].Name);
            Assert.AreEqual("Jake", table.JokerLoosers[1].Name);
            Assert.AreEqual("John", table.JokerLoosers[2].Name);
            Assert.AreEqual("Bob", table.JokerLoosers[3].Name);
            Assert.AreEqual("Jane", table.JokerLoosers[4].Name);
        }

        [Test]
        public void GivenATableWithSixPlayers_WhenFindingTheTwoOrMorePlayersThatPickedTheJoker_ItShouldReturnBobJaneJohn()
        {
            var table = new Table("ABCD");
            table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Player2", Guid.NewGuid()));
            table.AddPlayer(new Player("Player3", Guid.NewGuid()));
            table.AddPlayer(new Player("John", Guid.NewGuid()));
            table.AddPlayer(new Player("Bob", Guid.NewGuid()));
            table.AddPlayer(new Player("Jane", Guid.NewGuid()));

            table.StartGame(123);

            table.Players[0].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[1].MakeMove(Moves.Green, DateTime.Now);
            table.Players[2].MakeMove(Moves.Red, DateTime.Now);
            table.Players[3].MakeMove(Moves.Joker, DateTime.Now);
            table.Players[4].MakeMove(Moves.Joker, DateTime.Now);
            table.Players[5].MakeMove(Moves.Joker, DateTime.Now);

            Assert.AreEqual(3, table.JokerLoosers.Count);
            Assert.AreEqual("John", table.JokerLoosers[0].Name);
            Assert.AreEqual("Bob", table.JokerLoosers[1].Name);
            Assert.AreEqual("Jane", table.JokerLoosers[2].Name);
        }

        [Test]
        public void GivenATableWithTwoPlayer_WhenStartingAGame_ItShouldReturnTheConfigurationForThatGame()
        {
            var table = new Table("ABCD");
            table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Player2", Guid.NewGuid()));

            var config = table.StartGame(123);

            Assert.AreEqual(2, config.Colours.Count);
            Assert.AreEqual("darkred", config.Colours[0]);
            Assert.AreEqual("deepskyblue", config.Colours[1]);
        }

        [Test]
        public void GivenATableWithSixPlayer_WhenStartingAGame_ItShouldReturnTheConfigurationForThatGame()
        {
            var table = new Table("ABCD");
             table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Player2", Guid.NewGuid()));
            table.AddPlayer(new Player("Player3", Guid.NewGuid()));
            table.AddPlayer(new Player("John", Guid.NewGuid()));
            table.AddPlayer(new Player("Bob", Guid.NewGuid()));
            table.AddPlayer(new Player("Jane", Guid.NewGuid()));

            var config = table.StartGame(123);

            Assert.AreEqual(5, config.Colours.Count);
            Assert.AreEqual("darkred", config.Colours[0]);
            Assert.AreEqual("deepskyblue", config.Colours[1]);
            Assert.AreEqual("teal", config.Colours[2]);
            Assert.AreEqual("lightcoral", config.Colours[3]);
            Assert.AreEqual("red", config.Colours[4]);
        }

        [Test]
        public void GivenATableWithSixPlayer_WhenAdvancingToTheNextRound_ItShouldReturnTheConfigurationForThatGame()
        {
            var table = new Table("ABCD");
            table.AddPlayer(new Player("Player1", Guid.NewGuid()));
            table.AddPlayer(new Player("Player2", Guid.NewGuid()));
            table.AddPlayer(new Player("Player3", Guid.NewGuid()));
            table.AddPlayer(new Player("John", Guid.NewGuid()));
            table.AddPlayer(new Player("Bob", Guid.NewGuid()));
            table.AddPlayer(new Player("Jane", Guid.NewGuid()));

            var config = table.StartGame(123);

            table.Players[0].MakeMove(Moves.Blue, DateTime.Now);
            table.Players[1].MakeMove(Moves.Green, DateTime.Now);
            table.Players[2].MakeMove(Moves.Red, DateTime.Now);
            table.Players[3].MakeMove(Moves.Joker, DateTime.Now);
            table.Players[4].MakeMove(Moves.Joker, DateTime.Now);
            table.Players[5].MakeMove(Moves.Joker, DateTime.Now);

            config = table.NextRound(124);

            Assert.AreEqual(5, config.Colours.Count);
            Assert.AreEqual("magenta", config.Colours[0]);
            Assert.AreEqual("lightseagreen", config.Colours[1]);
            Assert.AreEqual("red", config.Colours[2]);
            Assert.AreEqual("yellow", config.Colours[3]);
            Assert.AreEqual("deepskyblue", config.Colours[4]);
        }
    }
}

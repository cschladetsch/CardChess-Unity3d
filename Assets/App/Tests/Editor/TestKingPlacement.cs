using NUnit.Framework;

namespace App
{
    [TestFixture]
    public class TestKingPlacement
    {

        [Test]
        public void TestKing()
        {
            var arbiter = new Arbiter();
            //var d0 = arbiter.NewModel<Model.Deck>();
            var a3 = arbiter.NewEntity<Model.Player, EColor, Model.IDeck, Agent.Player>(EColor.White, null);

            ////var board = new Model.Board();
            ////board.Create(8, 8);

            //var d1 = arbiter.NewModel<Model.Deck>();

            //var p0 = arbiter.NewModel<Model.Player, EColor, Model.IDeck>(EColor.White, d0);
            //var p1 = arbiter.NewModel<Model.Player, EColor, Model.IDeck>(EColor.Black, d1);

            //var a0 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(p0);
            //var a1 = arbiter.NewAgent<Agent.Player, Model.IPlayer>(p1);


            //arbiter.SetPlayers(a0, a1);
            //arbiter.NewGame();
            //arbiter.StartGame();
        }
    }
}

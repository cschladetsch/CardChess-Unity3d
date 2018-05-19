using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using App.Common;
using App.Model;

namespace UnitTests
{
    [TestFixture]
    class TestBoard
    {
        [Test]
        public void TestBoardPiecePlacement()
        {
            var reg = new Registry();
            var board = reg.New<BoardModel>(8, 8);
            var w = reg.New<PlayerModel>(EColor.White);
        }
    }
}

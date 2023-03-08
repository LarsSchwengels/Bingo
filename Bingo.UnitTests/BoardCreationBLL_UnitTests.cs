using Bingo.BusinessLogic;
using Bingo.Exceptions;
using Bingo.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Bingo.Entities;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.UnitTests
{
    public class BoardCreationBLL_UnitTests
    {
        private IBoardCreationBLL _sut;
        private Mock<IGameRepo> _gameRepo;

        [SetUp]
        public void Setup()
        {
            _gameRepo = new Mock<IGameRepo>();
            _sut = new BoardCreationBLL(new GameBLL(_gameRepo.Object));
        }

        [TestCase("Kumbh Sans", "Amagno-Orange", "Amagno-Blau", "Gelb", "Mein Bingo", CustomBoard.BoardSize.Big, "FC Bayern München\r\nBorussia Dortmund\r\n1. FC Union Berlin\r\n" +
            "RB Leipzig\r\nSC Freiburg\r\nSG Eintracht Frankfurt\r\n1. FSV Mainz 05\r\nVfL Wolfsburg\r\nSV Werder Bremen\r\nVfL Borussia Mönchengladbach\r\nBayer 04 Leverkusen\r\n" +
            "1. FC Köln\r\nFC Augsburg\r\nHertha BSC Berlin\r\nVfB Stuttgart\r\nTSG 1899 Hoffenheim\r\nVfL Bochum\r\nFC Schalke 04")]
        [TestCase("Khumb Sans", "Amagno-Orange", "Amagno-Blau", "Gelb", "Mein Bingo", CustomBoard.BoardSize.Medium, "FC Bayern München\r\nBorussia Dortmund\r\n1. FC Union Berlin\r\n" +
            "RB Leipzig\r\nSC Freiburg\r\nSG Eintracht Frankfurt\r\n1. FSV Mainz 05\r\nVfL Wolfsburg\r\nSV Werder Bremen\r\nVfL Borussia Mönchengladbach\r\nBayer 04 Leverkusen\r\n" +
            "1. FC Köln\r\nFC Augsburg\r\nHertha BSC Berlin\r\nVfB Stuttgart\r\nTSG 1899 Hoffenheim\r\nVfL Bochum\r\nFC Schalke 04")]
        public void CreateGame_NotEnoughEntries_Throws(string font, string fontColour, string gridColour, string backgroundColour, string name, CustomBoard.BoardSize size, string words)
        {
            Action action = () => _sut.CreateGame(font, fontColour, gridColour, backgroundColour, name, size, words, true);

            action.Should().Throw<EntryCountException>();
            _gameRepo.Verify(g => g.SaveCurrent(It.IsAny<CustomBoard>()), Times.Never);
        }

        [TestCase("Khumb Sans", "Amagno-Orange", "Amagno-Blau", "Gelb", "Mein Bingo", CustomBoard.BoardSize.Small, "FC Bayern München\r\nBorussia Dortmund\r\n1. FC Union Berlin\r\n" +
            "RB Leipzig\r\nSC Freiburg\r\nSG Eintracht Frankfurt\r\n1. FSV Mainz 05\r\nVfL Wolfsburg\r\nSV Werder Bremen\r\nVfL Borussia Mönchengladbach\r\nBayer 04 Leverkusen\r\n" +
            "1. FC Köln\r\nFC Augsburg\r\nHertha BSC Berlin\r\nVfB Stuttgart\r\nTSG 1899 Hoffenheim\r\nVfL Bochum\r\nFC Schalke 04")]
        public void CreateGame_CorrectInput_CreatesAndSavesGame(string font, string fontColour, string gridColour, string backgroundColour, string name, CustomBoard.BoardSize size, string words)
        {
            var result = _sut.CreateGame(font, fontColour, gridColour, backgroundColour, name, size, words, true);

            result.Should().BeTrue();
            _gameRepo.Verify(g => g.SaveCurrent(It.IsAny<CustomBoard>()), Times.Once);
        }
    }
}
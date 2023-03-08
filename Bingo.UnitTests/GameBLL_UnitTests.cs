using Bingo.BusinessLogic;
using Bingo.Entities;
using Bingo.Repositories;
using FluentAssertions;
using Moq;

namespace Bingo.UnitTests
{
    public class Tests
    {
        private IGameBLL _sut;
        private Mock<IGameRepo> _gameRepo;
        private CustomBoard _board;
        private List<string> _wordList = new List<string>()
                {
                    "Bayern München",
                    "Borussia Dortmund",
                    "Union Berlin",
                    "RB Leipzig",
                    "SC Freiburg",
                    "Eintracht Frankfurt",
                    "FSV Mainz 05",
                    "VfL Wolfsburg",
                    "Bayer Leverkusen",
                    "Borussia Mönchengladbach",
                    "Werder Bremen",
                    "FC Köln",
                    "FC Augsburg",
                    "Hertha Berlin",
                    "VfB Stuttgart",
                    "1899 Hoffenheim",
                    "Schalke 04",
                    "VfL Bochum"
                };

        [SetUp]
        public void Setup()
        {
            _gameRepo = new Mock<IGameRepo>();
            _sut = new GameBLL(_gameRepo.Object);
            _board = new CustomBoard("Khumb Sans", "Yellow", "Red", "Black", "Mein Board", CustomBoard.BoardSize.Small, _wordList, true);
            _sut.SetBoard(_board);
        }

        [Test]
        public void GetBoard_Called_Returns()
        {
            var result = _sut.GetBoard();

            result.Should().BeEquivalentTo(_board);
        }

        [Test]
        public void DeleteCurrentSave_Calls_DeleteExistingSave()
        {
            _sut.DeleteCurrentSave();

            _gameRepo.Verify(g => g.DeleteExistingSave(), Times.Once);
        }

        [Test]
        public void SetBoard_CorrectInput_SetsNewBoard()
        {
            var exp = new CustomBoard("Inter SemiBold", "Black", "Red", "Green", "Test Board", CustomBoard.BoardSize.Small, _wordList, true);

            _sut.SetBoard(exp);

            var res = _sut.GetBoard();

            res.Should().Be(exp);
        }

        [Test]
        public void RandomizeBoard_Called_RandomizesEntries()
        {
            var exp = new List<string>();
            var exp_Squares = _sut.GetBoard().Squares;
            foreach (var item in exp_Squares)
            {
                exp.Add(item.Text);
            }

            _sut.RandomizeBoard();

            var res = new List<string>();
            var res_Squares = _sut.GetBoard().Squares;
            foreach (var item in res_Squares)
            {
                res.Add(item.Text);
            }

            res.Should().NotBeEquivalentTo(exp);
        }

        [Test]
        public void CheckOrUncheckSquare_SquareChecked_UnchecksSquare()
        {
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[2]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[2]);
            _sut.GetBoard().Squares[2].IsChecked.Should().BeFalse();
        }

        [Test]
        public void CheckOrUncheckSquare_SquareUnchecked_ChecksSquare()
        {
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[4]);

            _sut.GetBoard().Squares[4].IsChecked.Should().BeTrue();
        }

        [Test]
        public void CheckForBingo_NoBingo_ReturnsBingoCount()
        {
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[3]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[2]);
            var result = _sut.CheckForBingo();

            result.Should().Be(0);
        }

        [Test]
        public void CheckForBingo_HorizontalBingo_ReturnsBingoCount()
        {
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[3]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[2]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[1]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[0]);
            var result = _sut.CheckForBingo();

            result.Should().Be(1);
        }

        [Test]
        public void CheckForBingo_DiagonalBingo_ReturnsBingoCount()
        {
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[0]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[5]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[10]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[15]);
            var result = _sut.CheckForBingo();

            result.Should().Be(1);
        }

        [Test]
        public void CheckForBingo_VerticalBingo_ReturnsBingoCount()
        {
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[2]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[6]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[10]);
            _sut.CheckOrUncheckSquare(_sut.GetBoard().Squares[14]);
            var result = _sut.CheckForBingo();

            result.Should().Be(1);
        }
    }
}
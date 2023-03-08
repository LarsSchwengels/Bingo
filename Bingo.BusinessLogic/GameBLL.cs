using Bingo.Entities;
using Bingo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bingo.Entities.CustomBoard;

namespace Bingo.BusinessLogic
{
    public class GameBLL : IGameBLL
    {
        private IGameRepo gameRepo;
        private CustomBoard? board;

        public GameBLL(IGameRepo gameRepo)
        {
            this.gameRepo = gameRepo;

            if (File.Exists("../CurrentSave.json"))
            {
                board = gameRepo.GetExistingSave();
            }
        }

        #region Get/Save Board
        public CustomBoard GetBoard()
        {
            return board;
        }
        public void DeleteCurrentSave()
        {
            board = null;
            gameRepo.DeleteExistingSave();
        }
        #endregion

        #region PreGame
        public void SetBoard(CustomBoard customBoard)
        {
            board = customBoard;
            RandomizeBoard();
        }
        public void RandomizeBoard()
        {
            UncheckSquares();
            var unusedWords = new List<string>();
            foreach (var item in board.WordList)
            {
                unusedWords.Add(item.ToString());
            }
            var rnd = new Random();

            double size = board.Squares.Count;

            for (int i = 0; i < board.Squares.Count; i++)
            {
                var square = board.Squares[i];
                var randomNumber = rnd.Next(unusedWords.Count);
                square.Text = unusedWords[randomNumber];
                square.Column = i % (int)Math.Sqrt(size);
                square.Row = i / (int)Math.Sqrt(size);
                unusedWords.RemoveAt(randomNumber);
            }
            gameRepo.SaveCurrent(board);
        }
        private void UncheckSquares()
        {
            foreach (var item in board.Squares)
            {
                item.IsChecked = false;
            }
        }
        #endregion

        #region GameLogic
        public int CheckForBingo()
        {
            if (board == null)
            {
                return -1;
            }
            int bingos = 0;

            switch(board.Size)
            {
                case BoardSize.Small:
                    bingos += VerticalBingos(4) + HorizontalBingos(4) + DiagonalBingos(4);
                    break;
                case BoardSize.Medium:
                    bingos += VerticalBingos(5) + HorizontalBingos(5) + DiagonalBingos(5);
                    break;
                case BoardSize.Big:
                    bingos += VerticalBingos(6) + HorizontalBingos(6) + DiagonalBingos(6);
                    break;
            }

            board.BingoCount = bingos;
            gameRepo.SaveCurrent(board);
            return bingos;
        }
        private int DiagonalBingos(int size)
        {
            int bingos = 0;
            int diagonalSquares = 0;

            for (int i = 0; i < size; i++)
            {
                Square square = board.Squares.First(s => s.Row == i && s.Column == i);

                if (square.IsChecked)
                {
                    diagonalSquares++;
                }
                else
                {
                    break;
                }
            }

            if (diagonalSquares == size)
            {
                bingos++;
            }
            diagonalSquares = 0;

            for (int i = 0; i < size; i++)
            {
                Square square = board.Squares.First(s => s.Row == size - i - 1 && s.Column == i);

                if (square.IsChecked)
                {
                    diagonalSquares++;
                }
                else
                {
                    break;
                }
            }

            if (diagonalSquares == size)
            {
                bingos++;
            }

            return bingos;
        }
        private int HorizontalBingos(int size)
        {
            int bingos = 0;
            int horizontalSquares = 0;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Square square = board.Squares.First(s => s.Row == r && s.Column == c);
                    
                    if (square.IsChecked)
                    {
                        horizontalSquares++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (horizontalSquares == size)
                {
                    bingos++;
                }

                horizontalSquares = 0;
            }

            return bingos;
        }
        private int VerticalBingos(int size)
        {
            int bingos = 0;
            int verticalSquares = 0;

            for (int c = 0; c < size; c++)
            {
                for (int r = 0; r < size; r++)
                {
                    Square square = board.Squares.First(s => s.Row == r && s.Column == c);
                    if (square.IsChecked)
                    {
                        verticalSquares++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (verticalSquares == size)
                {
                    bingos++;
                }
                verticalSquares = 0;
            }

            return bingos;
        }
        public void CheckOrUncheckSquare(Square square)
        {
            square.IsChecked =! square.IsChecked;
            gameRepo.SaveCurrent(board);
        }
        #endregion
    }
}

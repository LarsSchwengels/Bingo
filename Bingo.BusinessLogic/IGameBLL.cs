using Bingo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.BusinessLogic
{
    public interface IGameBLL
    {
        void SetBoard(CustomBoard customBoard);
        int CheckForBingo();
        void CheckOrUncheckSquare(Square square);
        void RandomizeBoard();
        void DeleteCurrentSave();
        CustomBoard GetBoard();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bingo.Entities.CustomBoard;

namespace Bingo.BusinessLogic
{
    public interface IBoardCreationBLL
    {
        bool CreateGame(string font, string fontColour, string gridColour, string backgroundColour,
            string name, BoardSize size, string words, bool isLightMode);
    }
}

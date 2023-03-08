using Bingo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.Repositories
{
    public interface IGameRepo
    {
        void SaveCurrent(CustomBoard? board);
        CustomBoard? GetExistingSave();
        void DeleteExistingSave();
    }
}

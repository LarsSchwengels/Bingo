using Bingo.Entities;
using Bingo.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bingo.Entities.CustomBoard;

namespace Bingo.BusinessLogic
{
    public class BoardCreationBLL : IBoardCreationBLL
    {
        private IGameBLL gameBLL;
        public BoardCreationBLL(IGameBLL gameBLL)
        {
            this.gameBLL = gameBLL;
        }

        public bool CreateGame(string font, string fontColour, string gridColour, string backgroundColour,
            string name, BoardSize size, string words, bool isLightMode)
        {
            try
            {
                var wordList = GetWordList(words);
                var board = new CustomBoard(font, fontColour, gridColour, backgroundColour, name, size, wordList, isLightMode);
                gameBLL.SetBoard(board);
                return true;
            }
            catch (EntryCountException)
            {
                throw;
            }
        }

        private List<string> GetWordList(string words)
        {
            try
            {
                var wordArray = words.Split(Environment.NewLine);
                var wordList = new List<string>();

                foreach (var str in wordArray)
                {
                    if (wordList.Contains(str))
                    {
                        throw new EntryIsDoubledException($"Der Eintrag {str} ist doppelt vorhanden. Es darf allerdings kein Eintrag mehr als einmal vorkommen.");
                    }
                    if (!string.IsNullOrWhiteSpace(str) && !string.IsNullOrEmpty(str))
                    {
                        wordList.Add(str);
                    }
                }

                return wordList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

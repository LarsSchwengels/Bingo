using Bingo.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bingo.Entities
{
    public class CustomBoard
    {
        private string? _font;
        private string? _fontColour;
        private string? _gridColour;
        private string? _backgroundColour;
        private List<Square> _squares = new List<Square>();
        private string? _name;
        private BoardSize _size;
        private List<string> _wordList;
        private int _bingoCount;
        private bool _isLightMode;

        public string Font { get { return _font; } set { _font = value; } }
        public string FontColour { get { return _fontColour; } set { _fontColour = value; } }
        public string GridColour { get { return _gridColour; } set { _gridColour = value; } }
        public string BackgroundColour { get { return _backgroundColour; } set { _backgroundColour = value; } }
        public List<Square> Squares { get { return _squares; } set { _squares = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public BoardSize Size { get { return _size; } set { _size = value; } }
        public List<string> WordList { get { return _wordList; } set { _wordList = value; } }
        public int BingoCount { get { return _bingoCount; } set { _bingoCount = value; } }
        public bool IsLightMode { get { return _isLightMode; } set { _isLightMode = value; } }

        public CustomBoard(string font, string fontColour, string gridColour, string backgroundColour, 
            string name, BoardSize size, List<string> wordList, bool isLightMode)
        {
            _font = font;
            _fontColour = fontColour;
            _gridColour = gridColour;
            _backgroundColour = backgroundColour;
            _name = name;
            _size = size;
            _wordList = wordList;
            _bingoCount = 0;
            _isLightMode = isLightMode;
            CreateSquares();
        }

        [JsonConstructorAttribute]
        public CustomBoard(string font, string fontColour, string gridColour, string backgroundColour,
            List<Square> squares, string name, BoardSize size, List<string> wordList, int bingoCount, bool isLightMode)
        {
            _font = font;
            _fontColour = fontColour;
            _gridColour = gridColour;
            _backgroundColour = backgroundColour;
            _name = name;
            _size = size;
            _wordList = wordList;
            _bingoCount = bingoCount;
            _squares = squares;
            _isLightMode = isLightMode;
        }

        public enum BoardSize
        {
            Small,
            Medium,
            Big
        }


        private void CreateSquares()
        {
            int squareCount = GetSquareCount();

            for (int i = 0; i < Math.Sqrt(squareCount); i++)
            {
                for (int j = 0; j < Math.Sqrt(squareCount); j++)
                {
                    var square = new Square(i, j);
                    _squares.Add(square);
                }
            }
        }

        private int GetSquareCount()
        {
            int count = 0;
            switch (_size)
            {
                case BoardSize.Small:
                    if (_wordList.Count < 16)
                    {
                        throw new EntryCountException("Bei dieser Feldgröße müssen mindestens 16 Einträge vorhanden sein, um das Bingo zu starten.");
                    }
                    count = 16;
                    break;
                case BoardSize.Medium:
                    if (_wordList.Count < 25)
                    {
                        throw new EntryCountException("Bei dieser Feldgröße müssen mindestens 25 Einträge vorhanden sein, um das Bingo zu starten.");
                    }
                    count = 25;
                    break;
                case BoardSize.Big:
                    if (_wordList.Count < 36)
                    {
                        throw new EntryCountException("Bei dieser Feldgröße müssen mindestens 36 Einträge vorhanden sein, um das Bingo zu starten.");
                    }
                    count = 36;
                    break;
            }
            return count;
        }
    }
}
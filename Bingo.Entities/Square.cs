using System.Text.Json.Serialization;

namespace Bingo.Entities
{
    public class Square
    {
        private string _text;
        private bool _isChecked;
        private int _column;
        private int _row;

        public string Text { get { return _text; } set { _text = value; } }
        public bool IsChecked { get { return _isChecked; } set { _isChecked = value; } }
        public int Column { get { return _column; } set { _column = value; } }
        public int Row { get { return _row; } set { _row = value; } }

        public Square(int column, int row)
        {
            _text = string.Empty;
            _isChecked = false;
            _column = -1;
            _row = -1;
        }

        [JsonConstructorAttribute]
        public Square(string text, bool isChecked, int column, int row)
        {
            _text = text;
            _isChecked = isChecked;
            _column = column;
            _row = row;
        }
    }
}
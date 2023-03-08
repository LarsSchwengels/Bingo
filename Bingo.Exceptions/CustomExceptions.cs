namespace Bingo.Exceptions
{
    public class ColorException : Exception
    {
        public ColorException(string message) : base(message) { }
    }

    public class EntryCountException : Exception
    {
        public EntryCountException(string message) : base(message) { }
    }

    public class FileException : Exception
    {
        public FileException(string message) : base(message) { }
    }

    public class EntryIsDoubledException : Exception
    {
        public EntryIsDoubledException(string message) : base(message) { }
    }

    public class EmptyBoardNameException : Exception
    {
        public EmptyBoardNameException(string message) : base(message) { }
    }
}
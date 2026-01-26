using YourMovieDB.Models;

namespace YourMovieDB.OperationContracts.Interfaces
{
    public interface IDraw
    {
        void Box(List<Selection> loadedMenu, int selection);
        void ColorWrite(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black, bool lineBreak = true);
        void InfoBox(string text);
        void LeftSpace(int objectWidth);
        void Logo();
    }
}
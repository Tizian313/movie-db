using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YourMovieDB.Models
{
    public class Globals
    {
        public const string moviesJsonPath = "Movies.json";
        public const string personsJsonPath = "Persons.json";

        public readonly static List<ConsoleKey> UpButtons = [ConsoleKey.UpArrow, ConsoleKey.W, ConsoleKey.NumPad8];
        public readonly static List<ConsoleKey> DownButtons = [ConsoleKey.DownArrow, ConsoleKey.S, ConsoleKey.NumPad2];
        public readonly static List<ConsoleKey> SelectButtons = [ConsoleKey.Enter, ConsoleKey.Spacebar];
        public readonly static List<ConsoleKey> BackButtons = [ConsoleKey.Escape, ConsoleKey.Backspace];
    }
}

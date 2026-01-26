using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMovieDB.Models;
using YourMovieDB.OperationContracts.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace YourMovieDB.OperationContracts
{
    public class Draw : IDraw
    {

        public void InfoBox(string text)
        {
            Console.WriteLine();
            LeftSpace(text.Length + 4);
            ColorWrite("▕▛" + new string('▀', text.Length) + "▜▏", ConsoleColor.White);
            LeftSpace(text.Length + 4);
            ColorWrite("▕▌" + text + "▐▏", ConsoleColor.White);
            LeftSpace(text.Length + 4);
            ColorWrite("▕▙" + new string('▄', text.Length) + "▟▏", ConsoleColor.White);

            return;
        }

        public void Box(List<Selection> loadedMenu, int selection)
        {
            ConsoleColor color;

            for (int i = 0; i < loadedMenu.Count; i++)
            {
                Console.WriteLine();
                int nameLength = loadedMenu[i].Name.Length;
                LeftSpace(nameLength + 4);

                if (i == selection)
                {
                    color = ConsoleColor.White;
                    ColorWrite("╭" + new string('─', nameLength + 2) + "╮", color);
                    LeftSpace(nameLength + 8);
                    ColorWrite("> │ " + loadedMenu[i].Name + " │ <", color);
                }
                else
                {
                    color = ConsoleColor.Gray;
                    ColorWrite("╭" + new string('─', nameLength + 2) + "╮", color);
                    LeftSpace(nameLength + 4);
                    ColorWrite("│ " + loadedMenu[i].Name + " │", color);
                }
                LeftSpace(nameLength + 4);
                ColorWrite("╰" + new string('─', nameLength + 2) + "╯ ", color);
            }
        }

        public void Logo()
        {
            List<string> logo = ["🭇🭆🭂███████████████████████████████████🭍🭑🬼",
                                 "🭅██🭐 🭖██🭡 🭅█▍ 🬼🭖██🭡🭇 🮉█      🭖█  ███████🭐",
                                 "████🭐 🭖🭡 🭅██▍ 🭐 🭖🭡 🭅 🮉█  ██🭐 🮈█  ████████",
                                 "█████🭐🭢🭗🭅███▍ █🭐🭢🭗🭅█ 🮉█  ███ 🮈█  🭗   🭖███",
                                 "██████  ████▍ ██🭏🭄██ 🮉█  ██🭡 🮈█  🭒█🭌 🮈███",
                                 "🭖█████  ████▍ ██████ 🮉█      🭅█  🬼   🭅██🭡",
                                 "🭢🭧🭓███████████████████████████████████🭞🭜🭗"];

            Console.WriteLine();

            foreach (string line in logo)
            {
                LeftSpace(41);
                ColorWrite(line, ConsoleColor.DarkYellow, ConsoleColor.Black);
            }
            Console.ResetColor();
        }

        public void LeftSpace(int objectWidth) // Could be changed to take string and print left space plus text
        {
            int size = (Console.WindowWidth - objectWidth) / 2;

            if (size < 0)
            {
                size = 0;
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(new string(' ', size));
        }

        public void ColorWrite(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black, bool lineBreak = true)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            if (lineBreak)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }

            Console.ResetColor();
        }
    }
}

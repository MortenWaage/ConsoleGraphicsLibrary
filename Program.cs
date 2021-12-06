using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using GUILib;

namespace ConsoleGraphicsLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            GUI.UseGUI();

            var position_1 = new Vector2(60, 25);
            var position_2 = new Vector2(20, 10);
            var position_3 = new Vector2(30, 15);
            var position_4 = new Vector2(40, 20);

            var size_1 = new Vector2(30, 16);
            var size_2 = new Vector2(30, 16);
            var size_3 = new Vector2(30, 16);
            var size_4 = new Vector2(60, 16);

            var element_1 = new GUIElement(position_1, size_1, Color.Black, Color.DarkRed, ConsoleColor.White);
            var element_2 = new GUIElement(position_2, size_2, Color.Black, Color.DarkGreen, ConsoleColor.Green);
            var element_3 = new GUIElement(position_3, size_3, Color.Peru, Color.Gray, ConsoleColor.DarkBlue);
            var element_4 = new GUIElement(position_4, size_4, Color.Black, Color.CornflowerBlue, ConsoleColor.Blue);

            var message_body_1 = new string[]
            {
                "Hello this is a message",
                "Designed to test multi line text blocks",
                "in the Graphics Library.",
            };
            var message_body_2 = new string[]
            {
                "Hello this is a message designed to test what happens when the letters exceed the total space available on the element that the text is being written to.",
                "Using multiple lines, because why not.",
            };

            GUI.AddElement(element_1);
            GUI.AddElement(element_2);
            GUI.AddElement(element_3);
            GUI.AddElement(element_4);

            element_1.SetText(message_body_2);

            GUI.Refresh();

            while (true)
            {
                GUI.ElementController.NextElement.SetActive();
                GUI.WaitForInput();
            }
        }
    }
}

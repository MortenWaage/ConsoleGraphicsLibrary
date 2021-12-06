using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

#region HOW TO USE THE GRAPHICAL USER INTERFACE
/*
 *  HOW TO USE
 *
 *  To use the GUI-Interface, instantiate a GUI using
 *  GUI.UseGUI();
 *  
 *  To draw to and get input from the GUI
 *  use the follow two methods:
 *  
 *  -- GUI.WriteLine(EntryType, string);
 *  where EntryType specifies where on the GUI the message will appear.
 *  the message can be passed in as either a string or if multiple lines are needed, a string array.
 *
 *  -- GUI.ReadLine();
 *
 *  If not GUI has been instantiated, these two methods will behave like
 *  Console.WriteLine(); and Console.ReadLine();
 *
 *  Further customization is done by adjusting properties in ScreenData.cs
 *
 */
#endregion

namespace GUILib
{
    public static class GUI
    {
        private static bool UsingGui => GUIController.Instance != null;
        public static IEnumerable<string> Buffer => GUIController.Instance.ScreenBuffer.Buffer;
        public static IEnumerable<string> Text => GUIController.Instance.ScreenBuffer.Text;
        public static IEnumerable<GUIElement> Elements => GUIController.Instance.ElementController.GUIElements;
        public static IEnumerable<string> InputBuffer => GUIController.Instance.ScreenBuffer.InputArea;
        public static ElementController ElementController => GUIController.Instance.ElementController;

        public static void UseGUI()
        {
            if (UsingGui) return;

            GUIController.Instance = new GUIController
            {
                EventLog          = new EventLog(),
                ScreenBuffer      = new ScreenBuffer(),
                Renderer          = new GUIRenderer(),
                ElementController = new ElementController()
            };
        }

        #region Create and Remove screen Elements
        public static void AddElement(GUIElement element)
        {
            if (!UsingGui) return;
            GUIController.Instance.ElementController.AddElement(element);
        }
        public static void AddElement(Vector2 position, Vector2 size)
        {
            if (!UsingGui) return;
            GUIController.Instance.ElementController.AddElement(position, size, ScreenData.DefaultElementColor, ScreenData.DefaultBorderColor, BorderStyle.Default);
        }
        public static void AddElement(Vector2 position, Vector2 size, Color backgroundColor, Color borderColor)
        {
            if (!UsingGui) return;
            GUIController.Instance.ElementController.AddElement(position, size, backgroundColor, borderColor, BorderStyle.Default);
        }

        public static void RefreshAllElements()
        {
            GUIController.Instance.ElementController.RefreshAllElements();
        }
        public static void RemoveElement(GUIElement element)
        {
            if (!UsingGui) return;
            GUIController.Instance.ScreenBuffer.RemoveFromBuffer(element);
            GUIController.Instance.ElementController.RemoveElement(element);
            GUIController.Instance.ElementController.RefreshAllElements();
        }
        #endregion

        #region Write and ReadLine Methods - User Interacts with these
        public static void WriteLine(EntryType type, string message)
        {
            if (!UsingGui)
            {
                WriteWithoutGUI(new string[] { message }, false);
                return;
            }

            GUIController.Instance.EventLog.AddEntry(type, message);
        }
        public static void WriteLine(string[] messages)
        {
            if (!UsingGui)
            {
                WriteWithoutGUI(messages, true);
                return;
            }

            GUIController.Instance.EventLog.AddEntry(EntryType.Command, messages);
        }
        public static void WriteLine(string message)
        {
            if (!UsingGui)
            {
                WriteWithoutGUI(new string[] { message }, false);
                return;
            }

            GUIController.Instance.EventLog.AddEntry(EntryType.Command, message);
        }
        public static void WriteLine(EntryType type, string message, bool update)
        {
            if (!UsingGui)
            {
                WriteWithoutGUI(new string[] { message }, false);
                return;
            }

            GUIController.Instance.EventLog.AddEntry(type, message);
            if (!update) return;
            Refresh();
        }
        public static void WriteLine(EntryType type, string[] messages, bool update)
        {
            if (!UsingGui)
            {
                WriteWithoutGUI(messages, true);
                return;
            }

            GUIController.Instance.EventLog.AddEntry(type, messages);
            if (!update) return;
            Refresh();
        }
        public static void WriteLine(EntryType type, string[] messages)
        {
            if (!UsingGui)
            {
                WriteWithoutGUI(messages, true);
                return;
            }

            GUIController.Instance.EventLog.AddEntry(type, messages);
            Refresh();
        }
        static void WriteWithoutGUI(string[] messages, bool usingStringArray)
        {
            if (usingStringArray) foreach (var message in messages) Console.WriteLine(message);
            else Console.Write(messages[0]);
        }

        public static void WaitForInput()
        {
            Console.ReadLine();
        }
        public static string ReadLine()
        {
            return !UsingGui ? Console.ReadLine() : GUIController.Instance.EventLog.ReadLine();
        }
        public static void AddInputData(int x, int y, int length)
        {
            if (length <= 0) return;
            //GUIController.Instance.ScreenBuffer.AddInputData(x, y, length);
        }
        #endregion

        #region Methods to Refresh and Build the screen Buffer
        public static void Refresh()
        {
            if (!UsingGui) return;
            //-- Fills the DrawBuffer with the updated data
            GUIController.Instance.ScreenBuffer.BuildScreenBuffer();
            //-- Draws screen from the newly built buffer
            GUIController.Instance.Renderer.DrawScreenBuffer();
        }
        #endregion

        public static void ClearConsole()
        {
            if (UsingGui) return;
            Console.Clear();
        }

        public static void SortDepthNewActiveElement(GUIElement element)
        {
            ElementController.SortDepthNewActiveElement(element);
        }

        public static void Preview()
        {
            GUIController.Instance.Renderer.Preview();
        }
    }
}
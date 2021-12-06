using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace GUILib
{
    public class GUIElement
    {
        private int _depth = 0;
        public int Depth
        {
            get => _depth;
            set
            {
                RefreshElement = true;
                _depth = value;
            }
        }
        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                GUIController.Instance.ScreenBuffer.RemoveFromBuffer(this);
                GUI.RefreshAllElements();
            }
        }
        public Vector2 Size;
        public ConsoleColor TextColor = ScreenData.DefaultTextColor;
        public Color BackgroundColor;
        public Color BorderColor;
        public BorderStyle BorderStyle = BorderStyle.Default;
        private string[] _text;
        public IEnumerable<string> Text => _text;
        public bool RefreshElement { get; set; }
        internal bool RefreshText { get; set; }

        public void SetActive()
        {
            GUI.SortDepthNewActiveElement(this);
            GUI.Refresh();
        }
        
        void SetAttributes()
        {

        }

        void SetPropertiesOrDefault(Vector2 pos, Vector2 size, Color? bgColor, Color? borderColor, ConsoleColor? textColor, BorderStyle? bStyle)
        {
            Position = pos;
            Size = size;

            _text = Array.Empty<string>();
            RefreshElement = true;

            BackgroundColor = (Color)        (bgColor     ?? ScreenData.DefaultBackgroundColor);
            BorderColor     = (Color)        (borderColor ?? ScreenData.DefaultBorderColor);
            TextColor       = (ConsoleColor) (textColor   ?? ScreenData.DefaultTextColor);
            BorderStyle     = (BorderStyle)  (bStyle      ?? BorderStyle.Default);
        }

        public GUIElement(Vector2 position, Vector2 size)
        {
            SetPropertiesOrDefault(position, size, null, null, null, null);
        }
        public GUIElement(Vector2 position, Vector2 size, Color borderColor)
        {
            SetPropertiesOrDefault(position, size, null, borderColor, null, null);
        }
        public GUIElement(Vector2 position, Vector2 size, Color backgroundColor, Color borderColor, BorderStyle style)
        {
            SetPropertiesOrDefault(position, size, backgroundColor, borderColor, null, style);
        }
        public GUIElement(Vector2 position, Vector2 size, BorderStyle style)
        {
            SetPropertiesOrDefault(position, size, null, null, null, style);
        }
        public GUIElement(Vector2 position, Vector2 size, Color backgroundColor, Color borderColor, ConsoleColor textColor, BorderStyle style)
        {
            SetPropertiesOrDefault(position, size, backgroundColor, borderColor, textColor, style);
        }
        public GUIElement(Vector2 position, Vector2 size, Color backgroundColor, Color borderColor, ConsoleColor textColor)
        {
            SetPropertiesOrDefault(position, size, backgroundColor, borderColor, textColor, null);
        }
        public GUIElement(Vector2 position, Vector2 size, Color backgroundColor, Color borderColor)
        {
            SetPropertiesOrDefault(position, size, backgroundColor, borderColor, null, null);
        }

        public void SetText(string message)
        {
            RefreshText = true;

            SetText(new string[] { message });
            RefreshElement = true;
        }
        public void SetText(string[] messages)
        {
            RefreshText = true;

            var msgLength = messages.Length;
            var availableArea = (int)Size.Y - (int)ScreenData.BorderWidth.Y;
            var size = Math.Min(msgLength, availableArea);

           _text = new string[availableArea];

            for (var i = 0; i < size; i++)
                AddText(messages[i], i);
        }
        private void AddText(string message, int index)
        {
            _text[index] = message;
        }

        public void SetAutoDepth()
        {
            if (Depth == 0) Depth = GUI.ElementController.ElementCount;
        }
    }
}

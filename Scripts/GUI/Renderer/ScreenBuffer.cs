using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Numerics;
using static GUILib.ScreenData;

namespace GUILib
{
    class ScreenBuffer
    {
        private int ScreenWidth  => ScreenData.ScreenWidth;
        private int ScreenHeight => ScreenData.ScreenHeight;

        private readonly int BufferSize;

        public ScreenBuffer()
        {
            BufferSize = ScreenWidth * ScreenHeight;

            _zBuffer            = new int    [ScreenWidth, ScreenHeight];
            _textBuffer         = new string [ScreenWidth, ScreenHeight];
            _screenBuffer       = new string [ScreenWidth, ScreenHeight];
            _previousDrawCall   = new string [ScreenWidth, ScreenHeight];
            _consoleBuffer      = new string [BufferSize]; //--Actual Data drawn to screen
        }

        private int[,] _zBuffer;
        private string[,] _textBuffer;
        private readonly string[,] _screenBuffer;
        private readonly string[,] _previousDrawCall;
        private string[] _consoleBuffer;
        private string[] _consoleText;
        private string[] _consoleInputArea;
        public IEnumerable<string> Buffer    => _consoleBuffer;
        public IEnumerable<string> Text      => _consoleText;
        public IEnumerable<string> InputArea => _consoleInputArea;

        public void BuildScreenBuffer()
        {
            //-- Clear the Z Buffer
            WipeZBuffer();

            //-- Sort Elements by Z Value
            SortElements();

            //-- Load Elements to buffer if in need of an update
            BufferAddElements();

            //-- Fills empty cells with the background
            LoadEmptyCells();

            //-- Fills the console buffer with data that changed since previous draw call.
            PushElements();
        }

        #region Refresh the buffer with data queued for Update

        public void BufferAddElements()
        {
            foreach (var element in GUI.Elements)
            {
                if (!element.RefreshElement) continue;
                element.RefreshElement = false;

                var left = (int)element.Position.X;
                var right = left + (int)element.Size.X;
                var top = (int)element.Position.Y;
                var bottom = top + (int)element.Size.Y;

                LoadElementToBuffer(left, right, top, bottom, element);
            }
        }
        private void LoadElementToBuffer(int left, int right, int top, int bottom, GUIElement element)
        {
            var background = element.BackgroundColor;
            var border = element.BorderColor;
            var style = element.BorderStyle;
            var depth = element.Depth;

            for (var y = top; y < bottom; y++)
            for (var x = left; x < right; x++)
            {
                if (style == BorderStyle.None)
                {
                    UpdateBufferCell(background, x, y, Border.None);
                    continue;
                }

                if (x < 0 || y < 0 || x >= ScreenWidth || y >= ScreenHeight) continue;
                if (_zBuffer[x, y] > depth) continue;
                _zBuffer[x, y] = depth;

                if      (x == left && y == top)                                          UpdateBufferCell(border, x, y, Border.TopLeft);
                else if (x == right - 1 && y == top)                                     UpdateBufferCell(border, x, y, Border.TopRight);
                else if (x == left && y == bottom - 1)                                   UpdateBufferCell(border, x, y, Border.BottomLeft);
                else if (x == right - 1 && y == bottom - 1)                              UpdateBufferCell(border, x, y, Border.BottomRight);
                else if ((x == left || x == right - 1) && (y != top && y != bottom - 1)) UpdateBufferCell(border, x, y, Border.Vertical);
                else if ((x != left && x != right - 1) && (y == top || y == bottom - 1)) UpdateBufferCell(border, x, y, Border.Horizontal);
                else UpdateBufferCell(background, x, y, Border.None);
            }

            void UpdateBufferCell(Color cellColor, int x, int y, Border type)
            {
                var symbol = style switch
                {
                    BorderStyle.Default => BorderDefault[type],
                    BorderStyle.Heart   => BorderHeart[type],
                    BorderStyle.Simple  => BorderSimple[type],
                    _ => ' '
                };

                var color = Colors.GetColor(cellColor);
                //if (_textBuffer[x, y] != null) return;
                _screenBuffer[x, y] = "\x1b[48;5;" + color + $"m{symbol}";
            }
        }
        void BufferAddText(GUIElement element)
        {
            var width  = (int)BorderWidth.X;
            var height = (int)BorderWidth.Y;

            var left   = (int)(element.Position.X + width + TextPadding.X);
            var right  = (int)(element.Size.X + element.Position.X - width - TextPadding.X);
            var top    = (int)(element.Position.Y + height + TextPadding.Y);
            var bottom = (int)(element.Size.Y + element.Position.Y - height - TextPadding.Y);
            
            LoadTextToBuffer(left, right, top, bottom, element);
            
        }

        void LoadTextToBuffer(int left, int right, int top, int bottom, GUIElement element)
        {
            var text = element.Text;
            if (text == null) return;
            var depth = element.Depth;

            var color = Colors.GetColor(element.BackgroundColor);
            int x = left, y = top;

            foreach (var message in text)
            {
                if (y >= bottom || message == null) continue;

                var index = 0;
                var wordLength = 0;

                foreach (var character in message)
                {
                    CalculateWordLength();

                    if (!CharFitsOnScreen()) continue;

                    if (_zBuffer[x, y] == depth)
                        _textBuffer[x, y] = "\x1b[48;5;" + color + $"m{character}";

                    x++;

                    void CalculateWordLength()
                    {
                        if (character == ' ')
                        {
                            for (var i = index + 1; i < message.Length; i++)
                            {
                                if (message[i] != ' ' && i < message.Length - 1) continue;
                                wordLength = i - index;
                                i = message.Length;
                            }
                        }
                        index++;
                    }

                    bool CharFitsOnScreen()
                    {
                        if (y >= ScreenHeight - 1 || y >= bottom) return false;
                        if (x + wordLength >= right || x + wordLength >= ScreenWidth - 1)
                        {
                            if (character == ' ') return false;
                            x = left; y++;
                        }

                        wordLength = 0;
                        return true;
                    }
                }
                x = left; y++;
            }
        }
        #endregion

        #region MyRegion

        public void RemoveFromBuffer(GUIElement element)
        {
            var left      = (int)element.Position.X;
            var right = left + (int)element.Size.X;
            var top       = (int)element.Position.Y;
            var bottom = top + (int)element.Size.Y;

            for (var y = top; y < bottom; y++)
            for (var x = left; x < right; x++)
            {
                if (x < 0 || y < 0 || x >= ScreenWidth || y >= ScreenHeight) continue;
                _screenBuffer[x, y] = null;
                _textBuffer[x, y] = null;
            }
        }

        private void WipeZBuffer()
        {
            _zBuffer = new int[ScreenWidth, ScreenHeight];
        }

        private void SortElements()
        {
            GUIController.Instance.ElementController.SortElementsDescending();
        }


        public void BuildTextBuffer(GUIElement element)
        {
            SetElementBuffer(element);
            //ClearTextBuffer();
            BufferAddText(element);
            PushText(element);
        }
        public void ClearTextBuffer()
        {
            _textBuffer = new string[ScreenWidth, ScreenHeight];
        }
        private void SetElementBuffer(GUIElement element)
        {
            var width  = (int) element.Size.X;
            var height = (int) element.Size.Y;
            _textBuffer = new string[width, height];
        }

        #endregion

        private void PushElements()
        {
            _consoleBuffer = new string[BufferSize];

            var width = ScreenWidth;
            var height = ScreenHeight;
            var index = 0;

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++, index++)
            {
                if (_previousDrawCall[x, y] == _screenBuffer[x, y]) continue;
                _consoleBuffer[index] = _screenBuffer[x, y];
            }
        }

        void PushText(GUIElement element)
        {
            var width   = (int) element.Size.X;
            var height  = (int) element.Size.Y;
            var size = width * height;
            
            _consoleText = new string[size];
            var index = 0;

            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++, index++)
            {
                _consoleText[index] = _textBuffer[x, y];
            }
        }
        private void LoadEmptyCells()
        {
            for (var y = 0; y < ScreenHeight; y++)
            for (var x = 0; x < ScreenWidth; x++)
            {
                //if (_zBuffer[x, y] > 0) continue;
                if (_screenBuffer[x, y] != null /*|| _textBuffer[x, y] != null*/) continue;

                var color = Colors.GetColor(ScreenData.DefaultBackgroundColor);
                _screenBuffer[x, y] = "\x1b[48;5;" + color + $"m{ScreenData.Dithering3}";
            }
        }
    }
}

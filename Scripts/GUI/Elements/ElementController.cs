using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GUILib
{
    public class ElementController
    {
        public GUIElement NextElement => _guiElements[^1];

        private List<GUIElement> _guiElements;

        public IEnumerable<GUIElement> GUIElements => _guiElements;

        public int ElementCount => _guiElements?.Count ?? 0;

        public void SortElementsDescending()
        {
            _guiElements = _guiElements.OrderByDescending(z => z.Depth).ToList();
        }
        public void SortElementsAscending()
        {
            _guiElements = _guiElements.OrderBy(z => z.Depth).ToList();
        }
        public void AddElement(Vector2 position, Vector2 size, Color backgroundColor, Color borderColor, BorderStyle style)
        {
            var element = new GUIElement(position, size, backgroundColor, borderColor, style);
            _guiElements ??= new List<GUIElement>();
            _guiElements.Add(element);
            element.SetAutoDepth();
        }
        public void AddElement(GUIElement element)
        {
            _guiElements ??= new List<GUIElement>();
            _guiElements.Add(element);
            element.SetAutoDepth();
        }
        public void RemoveElement(GUIElement newElement)
        {
            _guiElements.Remove(newElement);
        }

        public void RefreshAllElements()
        {
            if (_guiElements == null) return;

            foreach (var element in _guiElements)
                element.RefreshElement = true;
        }

        public void SortDepthNewActiveElement(GUIElement element)
        {
            var minDepth = _guiElements.Count;

            SortElementsAscending();

            for (var i = 0; i < minDepth; i++)
            {
                var depth = i + 1;
                _guiElements[i].Depth = depth;
                _guiElements[i].RefreshElement = false;
                _guiElements[i].SetText(TestMessage(i, depth, false));
            }

            element.Depth = minDepth + 1;
            element.SetText(TestMessage(minDepth-1, minDepth, true));
        }

        string[] TestMessage(int index, int depth, bool isActiveElement)
        {
            var isActive = isActiveElement ? "SELECTED " : "";
            return new string[]
            {
                $"{isActive}Element Pos in List {index}",
                $"Element Depth is {depth}",
                $"",
                $"",
                $"",
                $"",
                $"",
                $"",
                $"",
                $"",
                $"Element Pos in List {index}",
                $"Element Depth is {depth}",
            };
        }
    }
}
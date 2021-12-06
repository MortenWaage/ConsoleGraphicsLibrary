using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace GUILib
{
    public enum EntryType
    {
        Command,
        Combat,
        Header,
    }
    class EventLog
    {
        private readonly List<List<string[]>> _lists;
        private readonly List<string[]> _commands;
        private List<string[]> _combat;
        private readonly List<string[]> _header;

        public EventLog()
        {
            _lists = new List<List<string[]>>();

            _commands = new List<string[]>();
            _combat = new List<string[]>();
            _header = new List<string[]>();

            _lists.Add(_commands);
            _lists.Add(_combat);
            _lists.Add(_header);
        }

        void ClearAll()
        {
            foreach (var list in _lists)
                list.Clear();
        }

        public void AddEntry(EntryType type, string message)
        {
            var messages = new string[] { message };
            AddEntry(type, messages);
        }
        public void AddEntry(EntryType type, string[] messages)
        {
            switch (type)
            {
                case EntryType.Command:   if (_commands.Count > 0) _commands[0] = messages; else _commands.Add(messages); break;
                case EntryType.Combat:    AddCombatEntry(messages);                                                       break;
                case EntryType.Header:    _header.Add(messages);                                                          break;
                default:                                                                                                  break;
            }
        }

        private void AddCombatEntry(string[] messages)
        {
            if (_combat.Count > 3)_combat = new List<string[]>();
            _combat.Add(messages);
        }
        public string ReadLine()
        {
            var x = (int) ScreenData.InputPosition.X;
            var y = (int) ScreenData.InputPosition.Y;

            Console.SetCursorPosition(x,y);
            var input = Console.ReadLine();
            var length = input?.Length ?? -1;
            GUI.AddInputData(x, y, length);
            GUI.Refresh();
            Console.SetCursorPosition(x, y);
            return input;
        }
    }
}

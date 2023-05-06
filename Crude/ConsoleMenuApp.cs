using System;
using System.Collections.Generic;

namespace BooksDb
{
    public record MenuItem(char Symbol, string Caption, Action Function);

    public class ConsoleMenu
    {
        public string Name { get; }
        public List<MenuItem> Items { get; } = new List<MenuItem>();
        public ConsoleMenu? Parent { get; set; }

        private char _nextSymbol = '1';

        public ConsoleMenu(string name)
        {
            Name = name;
        }

        public void RegisterMenuItem(string caption, Action func)
        {
            Items.Add(new MenuItem(_nextSymbol, caption, func));
            _nextSymbol = _nextSymbol == '9' ? 'a' : ++_nextSymbol;
        }
    }

    public class ConsoleMenuApp
    {
        private readonly ConsoleMenu _mainMenu = new("MainMenu");
        private readonly List<ConsoleMenu> _menus = new();
        private ConsoleMenu _currentMenu;

        private bool _running = true;
        private bool _setupFinished;

        public string GetCurrentMenu() => _currentMenu.Name;

        public ConsoleMenuApp()
        {
            _menus.Add(_mainMenu);
            _currentMenu = _mainMenu;
        }

        public void RegisterSubmenu(ConsoleMenu menu, ConsoleMenu submenu, string caption)
        {
            if (submenu.Parent != null) return;

            menu.RegisterMenuItem(caption, () => _currentMenu = submenu);
            submenu.Parent = menu;
        }

        public void AddExitToMenu(ConsoleMenu menu)
        {
            menu.RegisterMenuItem("Exit", () => _currentMenu = menu.Parent);
        }

        public void Setup()
        {
            if (_setupFinished) return;
            AppSetup();
            _mainMenu.RegisterMenuItem("Exit", Exit);
            _setupFinished = true;
        }

        protected virtual void AppSetup()
        {
        }

        protected virtual void AppExit()
        {
        }

        public void DisplayMenu()
        {
            Console.WriteLine("Book database");
            Console.WriteLine("=================");
            foreach (MenuItem item in _currentMenu.Items)
            {
                Console.WriteLine($"{item.Symbol}. {item.Caption}");
            }
        }

        public void HandleOp()
        {
            Console.Write("Choose an operation: ");
            char c = Console.ReadKey().KeyChar;
            Console.WriteLine();

            MenuItem item = _currentMenu.Items.Find(it => it.Symbol == c);

            if (item != null) item.Function();
            else Console.WriteLine("Unknown operation");

            Console.WriteLine();
        }

        public void Run()
        {
            while (_running)
            {
                DisplayMenu();
                HandleOp();
            }

            AppExit();
        }

        protected void Exit()
        {
            _running = false;
        }
    }
}
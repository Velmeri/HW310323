using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HW310323
{
    public class Editor
    {
        private StringBuilder text; // текущий текст
        private List<Memento> history; // история изменений
        public int currentIndex; // текущий индекс в истории изменений

        public Editor()
        {
            text = new StringBuilder();
            history = new List<Memento>();
            currentIndex = 0;
        }

        public void Start()
        {
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey(true);

                // Если пользователь нажал Ctrl + Z, выполняем операцию отмены
                if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 && keyInfo.Key == ConsoleKey.Z)
                {
                    Undo();
                    Console.Clear();
                    Console.Write(text);
                }
                // Если пользователь нажал Ctrl + Y, выполняем операцию повтора
                else if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 && keyInfo.Key == ConsoleKey.Y)
                {
                    Redo();
                    Console.Clear();
                    Console.Write(text);
                }
                // Иначе добавляем введенный символ в текущий текст
                else
                {
                    text.Append(keyInfo.KeyChar);
                    currentIndex++;
                    Console.Clear();
                    Console.Write(text);
                }

                // Сохраняем текущее состояние в истории изменений
                history.Add(new Memento(text.ToString()));

                // Если количество сохраненных изменений превысило 256, удаляем самое старое
                if (history.Count > 256)
                {
                    history.RemoveAt(0);
                }
            }
        }

        public void Undo()
        {
            // Если есть, что отменять, переходим к предыдущему сохраненному состоянию
            if (currentIndex > 0)
            {
                currentIndex--;
                Memento memento = history[currentIndex];
                text = new StringBuilder(memento.GetSavedText());
            }
        }

        public void Redo()
        {
            // Если есть, что повторять, переходим к следующему сохраненному состоянию
            if (currentIndex < history.Count - 1)
            {
                currentIndex++;
                Memento memento = history[currentIndex];
                text = new StringBuilder(memento.GetSavedText());
            }
        }
    }

    public class Memento
    {
        readonly private string savedText; // сохраненный текст

        public Memento(string text)
        {
            savedText = text;
        }

        public string GetSavedText()
        {
            return savedText;
        }
    }
}

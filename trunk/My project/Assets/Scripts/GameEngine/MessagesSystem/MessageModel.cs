using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.MessagesSystem
{
    public class MessageModel : IMessageModel, IDisposable
    {
        public string Message {
            get;
            set;
        }

        public string Count {
            get;
            set;
        }
        
        public Color Color {
            get;
            set;
        }
        
        public string Text {
            get;
            set;
        }

        private readonly List<IDisposable> _disposables = new();

        public MessageModel(string text, Color color, string count )
        {
            Text = text;
            Color = color;
            Count = count;
            Message = Text + Count;
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable.Dispose();
        }
    }
}
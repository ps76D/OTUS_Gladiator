using UnityEngine;

namespace GameEngine.MessagesSystem
{
    public interface IMessageModel
    {

        string Message {
            get;
            set;
        }
        string Text {
            get;
            set;
        }
        
        string Count {
            get;
            set;
        }

        Color32 Color {
            get;
            set;
        }
    }
}
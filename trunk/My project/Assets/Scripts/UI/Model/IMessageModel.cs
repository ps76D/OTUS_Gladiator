using UnityEngine;
using UnityEngine.Localization;

namespace UI.Model
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

        Color Color {
            get;
            set;
        }
    }
}
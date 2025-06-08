using UI.Infrastructure;

namespace UI.Model
{
    public class WinPopupModel : IWinPopupModel
    {
        private readonly UIManager _uiManager;
        public WinPopupModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
}
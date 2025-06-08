using UI.Infrastructure;

namespace UI.Model
{
    public class LosePopupModel : ILosePopupModel
    {
        private readonly UIManager _uiManager;
        public LosePopupModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
}
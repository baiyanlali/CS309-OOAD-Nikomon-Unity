

namespace GamePlay.UIFramework
{
    public class ContextBase
    {
        public UIType ViewType { get; private set; }
        public ViewBase ViewBase { get; private set; }
        public ContextBase(UIType viewType)
        {
            ViewType = viewType;
            ViewBase = UIManager.GetUI(viewType).GetComponent<ViewBase>();
        }
    }
}
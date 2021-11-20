using GamePlay.UI.UIFramework;

namespace GamePlay.UI.SaveUI
{
    public class SavingSignatureUI:BaseUI,IUIAnimator
    {
        public override UILayer Layer => UILayer.Top;
        public override bool IsOnly => false;
        public override bool IsBlockPlayerControl => false;

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
        }

        public void OnEnterAnimator()
        {
            
        }

        public void OnExitAnimator()
        {
            
        }
    }
}
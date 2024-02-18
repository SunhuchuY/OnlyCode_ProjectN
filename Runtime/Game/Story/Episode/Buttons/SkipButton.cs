
namespace Story
{
    public class SkipButton : PopUpBase
    {
        protected override void Confirm()
        {
            GameManager.Instance.storyManager.Skip();
        }
    }
}
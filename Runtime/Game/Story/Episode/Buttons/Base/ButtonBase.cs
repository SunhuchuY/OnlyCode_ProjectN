using UnityEngine;
using UnityEngine.UI;

namespace Story
{
    [System.Serializable]
    public abstract class ButtonBase : MonoBehaviour
    {
        [SerializeField] private StoryManager _storyManager;
        [SerializeField] private Button _button;

        protected Button button => _button;
        protected StoryManager storyManager => _storyManager;

        protected virtual void Start()
        {
            ButtonAddListener();
        }

        protected abstract void ButtonAddListener();
    }
}


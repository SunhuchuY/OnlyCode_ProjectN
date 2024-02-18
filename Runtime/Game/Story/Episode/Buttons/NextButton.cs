
using UnityEngine;
using UnityEngine.UI;

namespace Story
{
    public class NextButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(() => 
            {
                GameManager.Instance.storyManager.Next();
            });
        }
    }
}

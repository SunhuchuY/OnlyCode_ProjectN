using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Story
{
    public class StoryUI : MonoBehaviour
    {
        private const float TEXT_ANIMATION_DELAY = 0.07f;
        private const float Character_FADEIN_DURATION = 1.4f;

        private const string BackgroundsPath = "Sprite/Background/Story";
        private const string CharactersPath = "Sprite/Illustration";

        [SerializeField] private Image CharacterImage;
        [SerializeField] private Image BackgroundImage;
        [SerializeField] private TMP_Text ContextText;
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private TMP_Text SkipSummaryText;

        public Sequence typingSequence { get; private set; }

        public void InitSummaryText(string summary)
        {
            SkipSummaryText.text = summary;
        }

        public void Show(StoryData.StoryDialogue dialogue)
        {
            Sprite characterSprite = Resources.Load<Sprite>(CharactersPath + "/" + dialogue.CharacterImageName);

            if (characterSprite == null)
            {
                CharacterImage.enabled = false;
            }
            else
            {
                CharacterImage.enabled = true;
                CharacterImage.sprite = characterSprite;

                // 이전 캐릭터 이름과 같지 않다면 페이드인을 진행합니다.
                if (NameText.text != dialogue.CharacterName)
                {   
                    CharacterImage.DOFade(1, Character_FADEIN_DURATION).From(0);
                }
            }

            PlayTextAnimation(dialogue.Text);

            NameText.text = dialogue.CharacterName;
            BackgroundImage.sprite = Resources.Load<Sprite>(BackgroundsPath + "/" + dialogue.UseBackgroundImageName);
        }

        private void PlayTextAnimation(string text)
        {
            ContextText.text = ""; 
            typingSequence = DOTween.Sequence();

            int textLength = text.Length; 
            int currentLength = 0;

            typingSequence.Append(DOTween.To(() => currentLength, x => currentLength = x, textLength, textLength * TEXT_ANIMATION_DELAY)
                   .OnUpdate(() =>
                   {
                       ContextText.text = text.Substring(0, currentLength); 
                   })
                   .SetEase(Ease.Linear)); 

            typingSequence.OnComplete(() => 
            {
                ContextText.text = text;
                typingSequence.Kill();
            });

            typingSequence.Play();
        }

        public void SkipTextAnimation()
        {
            typingSequence.Complete();
        }
    }
}
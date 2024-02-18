using BackEnd;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Story
{
    public class StoryManager
    {
        private StoryUI UI;
        private StoryData data;
        private int currentOrder = 0;

        public async UniTaskVoid Initialize(int episode)
        {
            while (GameObject.FindObjectOfType<StoryUI>() == null)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(0.1f));
            }

            UI = GameObject.FindObjectOfType<StoryUI>();
            SetupStory(episode - 1);
        }

        private void SetupStory(int epsidoeIDX)
        {
            data = StoryReader.StoryRead(epsidoeIDX);
            currentOrder = 0;

            UI.InitSummaryText(data.Summary);
            Next();
        }

        public void Next()
        {
            if (currentOrder == data.Dialogue.Count)
            {
                Complete();
                return;
            }

            if (UI.typingSequence != null && UI.typingSequence.IsPlaying())
            {
                UI.SkipTextAnimation();
                return;
            }

            UI.Show(data.Dialogue[currentOrder++]);
        }

        private void Complete()
        {
            SceneLoader.LoadSceneAsync(SceneLoader.SceneType.mainTest);
        }

        public void Skip()
        {
            Complete();
        }
    }
}


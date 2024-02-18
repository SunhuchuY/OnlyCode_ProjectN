
using System.Collections.Generic;

namespace Story
{
    public class StoryData
    {
        public string Summary { get; private set; }
        public List<StoryDialogue> Dialogue { get; private set; }

        public StoryData(string summary, List<StoryDialogue> dialogue)
        {
            Summary = summary;
            Dialogue = dialogue;
        }
        public class StoryDialogue
        {
            public int Order { get; private set; }
            public string CharacterImageName { get; private set; }
            public string UseBackgroundImageName { get; private set; }
            public string CharacterName { get; private set; }
            public string Text { get; private set; }

            public StoryDialogue(int order, string characterImageName, string useBackgroundImageName, string characterName, string text)
            {
                Order = order;
                CharacterImageName = characterImageName;
                UseBackgroundImageName = useBackgroundImageName;
                CharacterName = characterName;
                Text = text;
            }
        }
    }
}

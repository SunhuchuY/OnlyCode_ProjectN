using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    public static class StoryReader
    {
        private const string JsonPath = "Table/storyData";

        public static StoryData StoryRead(int episodeIDX)
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>(JsonPath);

            if (jsonTextAsset == null)
            {
                Debug.LogError($"Failed to load JSON file at path: {JsonPath}");
                throw new Exception($"Failed to load JSON file at path: {JsonPath}");
            }

            string json = jsonTextAsset.text;

            List<StoryData> dats = JsonConvert.DeserializeObject<List<StoryData>>(json);
            if (episodeIDX >= 0 && episodeIDX < dats.Count)
            {
                return dats[episodeIDX];
            }
            else
            {
                Debug.LogError($"Episode {episodeIDX} is out of range.");
                throw new Exception($"Episode {episodeIDX} is out of range.");
            }
        }
    }

}

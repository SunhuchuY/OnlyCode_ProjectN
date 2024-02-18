using UnityEngine;

namespace Admob
{
    public class Access : MonoBehaviour
    {
        public static Access Instance { get; private set; }

        public Admob.RewardAds RewardAds { get; private set; }


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            InstatiateAds();
        }

        public void InstatiateAds()
        {
            RewardAds = new RewardAds();
        }
    }
}


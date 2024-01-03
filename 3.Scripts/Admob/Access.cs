using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
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
        }
    }

    public class RewardAds
    {
#if UNITY_ANDROID
        private const string _rewardAdUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif
        private RewardedAd _rewardedAd;


        public RewardAds()
        {
            AdsInitialize();
        }

        public void Show(Action callbackMethod)
        {
            _rewardedAd?.Show((Reward reward) =>
            {
                callbackMethod.Invoke();
            });
        }

        private void AdsInitialize()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(_rewardAdUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                });

        }

    }
}


using GoogleMobileAds.Api;
using System;
using UnityEngine;


namespace Admob
{
    public class RewardAds
    {
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
        private RewardedAd _rewardedAd;


        public RewardAds()
        {
            AdsInitialize();
        }

        public void Show(Action rewardMethod)
        {
            _rewardedAd?.Show((Reward reward) =>
            {
                rewardMethod.Invoke();
                AdsInitialize();
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
            RewardedAd.Load(_adUnitId, adRequest,
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

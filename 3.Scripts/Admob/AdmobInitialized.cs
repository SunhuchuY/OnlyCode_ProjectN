using GoogleMobileAds.Api;
using UnityEngine;

namespace Admob
{
    public class AdmobInitialized : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
            });
        }
    }

}


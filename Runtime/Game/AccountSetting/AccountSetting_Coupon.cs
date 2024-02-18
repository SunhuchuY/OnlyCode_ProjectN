using UnityEngine;
using TMPro;
using LitJson;
using System;
using System.Numerics;

public class AccountSetting_Coupon : MonoBehaviour, IAccountSetting_UI_Component
{
    [SerializeField] private TMP_InputField Input;
    
    const string completeInfo = "쿠폰이 정상적으로 처리되었습니다.";
    const string unableInfo = "존재하지 않은 쿠폰코드거나\n이미 사용한 쿠폰코드입니다.";

    public void Confirm()
    {
        if (!BackEnd.Backend.IsLogin)
            return;

        string code = Input.text;
        var bro = BackEnd.Backend.Coupon.UseCoupon(code);

        if (bro.IsSuccess())
        {
            GameManager.Instance.commonUI.ToastMessage(completeInfo);
            JsonData json = bro.GetReturnValuetoJSON();
            CheckReward(json);
        }
        else
        {
            GameManager.Instance.commonUI.ToastMessage(unableInfo);
        }
    }

    private void CheckReward(JsonData json)
    {
        int len = json["itemObject"].Count;

        if (len == 0)
            return;

        for (int i = 0; i < len; i++)
        {
            BigInteger itemCount = BigInteger.Parse(json["itemObject"][i]["itemCount"].ToString());

            if (json["itemObject"][i]["item"].ContainsKey("Currency"))
            {
                string reward = json["itemObject"][i]["item"]["Currency"].ToString();
                
                if (reward == "Diamond")
                {
                    GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Diamond, itemCount);
                }

                else if (reward == "Gold")
                {
                    GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.Gold, itemCount);
                }

                else if (reward == "MagicStone")
                {
                    GameManager.Instance.userDataManager.ModifierCurrencyValue(CurrencyType.MagicStone, itemCount);
                }
            }
        }

        GameManager.Instance.userDataManager.Update();
    }
}


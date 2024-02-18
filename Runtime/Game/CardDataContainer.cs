using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CardDataContainer", menuName = "ScriptableObjects/CardDataContainer", order = 1)]
public class CardDataContainer : ScriptableObject
{
    public CardData[] cards = new CardData[20];
}

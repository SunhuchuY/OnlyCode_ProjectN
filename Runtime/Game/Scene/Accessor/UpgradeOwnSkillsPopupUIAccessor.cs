using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeOwnSkillsPopupUIAccessor : MonoBehaviour
{
    [Serializable]
    public class SkillUpgradePopup
    {
        [SerializeField] private Image titleSkillImage;
        [SerializeField] private TMP_Text titleNameText;
        [SerializeField] private TMP_Text titleLevelText;

        [SerializeField] private TMP_Text currentLevelText;
        [SerializeField] private TMP_Text currentCostText;
        [SerializeField] private TMP_Text currentExplanationText;

        [SerializeField] private TMP_Text nextLevelText;
        [SerializeField] private TMP_Text nextCostText;
        [SerializeField] private TMP_Text nextExplanationText;

        [SerializeField] private Image needSkillImage;
        [SerializeField] private Image needGoldImage;
        [SerializeField] private TMP_Text needSkillText;
        [SerializeField] private TMP_Text needGoldText;
        [SerializeField] private TMP_Text levelUpText;
        [SerializeField] private Button levelUpButton;
        [SerializeField] private Button skillUseButton;

        public Image TitleSkillImage => titleSkillImage;
        public TMP_Text TitleNameText => titleNameText;
        public TMP_Text TitleLevelText => titleLevelText;

        public TMP_Text CurrentLevelText => currentLevelText;
        public TMP_Text CurrentCostText => currentCostText;
        public TMP_Text CurrentExplanationText => currentExplanationText;

        public TMP_Text NextLevelText => nextLevelText;
        public TMP_Text NextCostText => nextCostText;
        public TMP_Text NextExplanationText => nextExplanationText;

        public Image NeedSkillImage => needSkillImage;
        public Image NeedGoldImage => needGoldImage;
        public TMP_Text NeedSkillText => needSkillText;
        public TMP_Text NeedGoldText => needGoldText;
        public TMP_Text LevelUpText => levelUpText;
        public Button LevelUpButton => levelUpButton;
        public Button SkillUseButton => skillUseButton;

    }
    public SkillUpgradePopup skillUpgradePopup;

    
    [SerializeField] private RectTransform skillScollViewParent;
    public RectTransform SkillScollViewParent => skillScollViewParent;

    
    [SerializeField] private Button quitButton;
    public Button QuitButton => quitButton;

}
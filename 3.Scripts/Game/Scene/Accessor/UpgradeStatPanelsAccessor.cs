using UnityEngine;
using UnityEngine.UI;

public class UpgradeStatPanelsAccessor : MonoBehaviour
{
    [SerializeField] private Button upgradeSkillsMenuButton;
    [SerializeField] private Button upgradeAttackMenuButton;
    [SerializeField] private Button upgradeDefenseMenuButton;
    [SerializeField] private Button upgradeHealthMenuButton;
    [SerializeField] private RectTransform upgradableSkillsPlaceholder;
    [SerializeField] private RectTransform upgradableAttackAttributesPlaceholder;
    [SerializeField] private RectTransform upgradableDefenseAttributesPlaceholder;
    [SerializeField] private RectTransform upgradableHealthAttributesPlaceholder;
    [SerializeField] private RectTransform upgradableSkillsPanel;
    [SerializeField] private RectTransform upgradableAttackAttributesPanel;
    [SerializeField] private RectTransform upgradableDefenseAttributesPanel;
    [SerializeField] private RectTransform upgradableHealthAttributesPanel;

    public Button UpgradeSkillsMenuButton => upgradeSkillsMenuButton;
    public Button UpgradeAttackMenuButton => upgradeAttackMenuButton;
    public Button UpgradeDefenseMenuButton => upgradeDefenseMenuButton;
    public Button UpgradeHealthMenuButton => upgradeHealthMenuButton;
    public RectTransform UpgradableSkillsPlaceholder => upgradableSkillsPlaceholder;
    public RectTransform UpgradableAttackAttributesPlaceholder => upgradableAttackAttributesPlaceholder;
    public RectTransform UpgradableDefenseAttributesPlaceholder => upgradableDefenseAttributesPlaceholder;
    public RectTransform UpgradableHealthAttributesPlaceholder => upgradableHealthAttributesPlaceholder;
    public RectTransform UpgradableSkillsPanel => upgradableSkillsPanel;
    public RectTransform UpgradableAttackAttributesPanel => upgradableAttackAttributesPanel;
    public RectTransform UpgradableDefenseAttributesPanel => upgradableDefenseAttributesPanel;
    public RectTransform UpgradableHealthAttributesPanel => upgradableHealthAttributesPanel;
}
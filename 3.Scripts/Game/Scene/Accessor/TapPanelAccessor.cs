using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TapPanelAccessor : MonoBehaviour
{
    [SerializeField] private RectTransform wrappingPanel;
    [SerializeField] private Button toggleExpandButton;
    [SerializeField] private PlayerHealthAndManaPanelAccessor playerHealthAndManaPanelAccessor;
    [SerializeField] private PlayerSkillsCanUseAccessor playerSkillsCanUseAccessor;
    [FormerlySerializedAs("upgradableStatPanelsAccessor")] [SerializeField] private UpgradeStatPanelsAccessor upgradeStatPanelsAccessor;

    public RectTransform WrappingPanel => wrappingPanel;
    public Button ToggleExpandButton => toggleExpandButton;
    public PlayerHealthAndManaPanelAccessor PlayerHealthAndManaPanelAccessor => playerHealthAndManaPanelAccessor;
    public PlayerSkillsCanUseAccessor PlayerSkillsCanUseAccessor => playerSkillsCanUseAccessor;
    public UpgradeStatPanelsAccessor UpgradeStatPanelsAccessor => upgradeStatPanelsAccessor;

    // public void Awake()
    // {
    //     if (wrappingPanel == null)
    //     {
    //         wrappingPanel =
    //             gameObject.DescendantsAndSelf().First(x => x.name == "Wrapping Panel").GetComponent<RectTransform>();
    //     }
    //
    //     if (toggleExpandButton == null)
    //     {
    //         toggleExpandButton =
    //             gameObject.DescendantsAndSelf().First(x => x.name == "Toggle Expand Button").GetComponent<Button>();
    //     }
    //
    //     if (playerHealthAndManaPanelAccessor == null)
    //     {
    //         playerHealthAndManaPanelAccessor =
    //             gameObject.DescendantsAndSelf().OfComponent<PlayerHealthAndManaPanelAccessor>().First();
    //     }
    //
    //     if (playerSkillsCanUseAccessor == null)
    //     {
    //         playerSkillsCanUseAccessor =
    //             gameObject.DescendantsAndSelf().OfComponent<PlayerSkillsCanUseAccessor>().First();
    //     }
    // }
}
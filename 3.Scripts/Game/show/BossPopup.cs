using UnityEngine;

public class BossPopup : MonoBehaviour
{
    [SerializeField] GameObject bossPopUpObject;

    private Animator anim;

    private void Start()
    {
        anim = bossPopUpObject.GetComponent<Animator>();
        // anim.GetBehaviour<AnimationStateEventReceiver>().OnStateExitEvent += OnStateExit;

        Hide();
    }

    private void OnStateExit(AnimatorStateInfo _stateInfo, int _layer)
    {
        if (_stateInfo.IsName("Popup"))
            bossPopUpObject.SetActive(false);
    }

    public void Show()
    {
        bossPopUpObject.SetActive(true);
        anim.Rebind();
        anim.Update(0);
    }

    public void Hide()
    {
        bossPopUpObject.SetActive(false);
    }
}
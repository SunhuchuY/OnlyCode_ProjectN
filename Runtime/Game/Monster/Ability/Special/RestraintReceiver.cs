using System.Collections;
using UnityEngine;

public class RestraintReceiver : MonoBehaviour
{
    public void ApplyRestraint(float duration)
    {
        Collider2D col = GetComponent<Collider2D>();
        StartCoroutine(Restraint(duration, col));
    }

    IEnumerator Restraint(float duration, Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // TODO: 플레이어 스킬을 억제하는 기능을 넣으세요.
        }
        else if (col.CompareTag("Friend"))
        {
            // TODO: Friend 대상들을 속박하는 기능을 넣으세요
        }

        yield return new WaitForSeconds(duration);
        Destroy(this);
    }
}
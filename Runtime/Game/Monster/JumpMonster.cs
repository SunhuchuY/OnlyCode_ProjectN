using DG.Tweening;
using System.Collections;
using UnityEngine;

public class JumpMonster : MonoBehaviour
{
    readonly private float bugfixc_CoolTime = 1f;

    public bool isJumpMoveStop = true;

    private string isAttack = "isAttack";
    private Monster monster;

    private bool isJumping = false;
    private bool isProgressRun_Animation = false;

    private float jumpMove_Duration = 2f;
    private float bugfixc_CurTime = 0f;
    public float moveSpeed = 3f;

    [SerializeField] private Sprite prepareJump_Sprite, Jumping_Sprite, arrive_Sprite;
    [SerializeField] private Sprite[] run_Sprite;

    [SerializeField] private JumpMonsterAssist jumpMonsterAssist;
    [SerializeField] private GameObject quakeAnim_Prefab;


    private void Start()
    {
        monster = transform.parent.GetComponent<Monster>();
    }

    private void Update()
    {
        if (isProgressRun_Animation == false)
        {
            StartCoroutine(RunAnimation());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isJumping == false)
            {
                isJumping = true;

                //monster.targetObj = collision.transform;
                StartCoroutine(JumpAction_Animation());
            }
        }
    }

    IEnumerator JumpAction_Animation()
    {
        yield return new WaitForSeconds(3f); // 딜레이

        monster.GetComponent<SpriteRenderer>().sprite = prepareJump_Sprite;

        //jumpMonsterAssist.transform.position = monster.targetObj.position;
        jumpMonsterAssist.gameObject.SetActive(true);
        jumpMonsterAssist.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
        jumpMonsterAssist.GetComponent<SpriteRenderer>().DOFade(0.6f, 1f) // 1초 동안 20%에서 60%로 투명도 변경
            .SetLoops(-1, LoopType.Yoyo);

        yield return new WaitForSeconds(3f); // 점프 대기

        monster.GetComponent<SpriteRenderer>().sprite = Jumping_Sprite;
        //monster.transform.DOMove(monster.targetObj.position, jumpMove_Duration);
        jumpMonsterAssist.gameObject.SetActive(false);

        yield return new WaitForSeconds(jumpMove_Duration); // 점프중

        monster.GetComponent<SpriteRenderer>().sprite = arrive_Sprite;
        //Instantiate(quakeAnim_Prefab, monster.targetObj.position, Quaternion.identity);
        jumpMonsterAssist.Attack();

        isJumping = false;
    }

    IEnumerator RunAnimation()
    {
        float delay = 0.05f;
        isProgressRun_Animation = true;

        for (int i = 0; i < run_Sprite.Length; i++)
        {
            monster.GetComponent<SpriteRenderer>().sprite = run_Sprite[i];
            yield return new WaitForSeconds(delay);
        }

        isProgressRun_Animation = false;
    }
}
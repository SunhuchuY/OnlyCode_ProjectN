using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill : MonoBehaviour
{
    [HideInInspector] public int skillNumber = 1, dotDamage = 5;
    [HideInInspector] private float attackAmount = 40f, debuffAmount, defenseAmount;
    [HideInInspector] private float slowSpeed = 0.5f;
    [HideInInspector] public float DestroyDelayTime = 4f;
    
    public float GetattackAmount()
    {
        int _skillLevel = 1; // temp: 임시적으로 스킬 레벨을 1로 설정합니다.
        return GameManager.Instance.userDataManager.userData.CurrentLevel * (attackAmount * _skillLevel);
    }


    public async Task SkillInitialize(int _attackAmount, int skillNum, float destrpyDuration = 0)
    {
        attackAmount = _attackAmount;
        skillNumber = skillNum;
        await Task.Delay((int)(destrpyDuration * 1000));
        Destroy(gameObject);
    }

    private IEnumerator skill15_Coroutine(float delay, int N)
    {
        // skill 21번도
        for (int i = 0; i < N; i++)
        {
            SpriteRenderer spriteRenderer = GameManager.Instance.bulletController.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // SpriteRenderer의 bounds를 이용하여 Sprite의 크기 가져오기
                Bounds spriteBounds = spriteRenderer.bounds;

                // 랜덤한 위치 계산
                Vector3 randomPosition = new Vector3(
                    Random.Range(spriteBounds.min.x, spriteBounds.max.x),
                    Random.Range(spriteBounds.min.y, spriteBounds.max.y),
                    0f);

                transform.GetChild(i).position = randomPosition;
                transform.GetChild(i).gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // ȭ�� ������ ������ ������Ʈ�� ����
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;

        if (collision.CompareTag("Monster"))
        {
            Monster collision_MonsterScript = collision.GetComponent<Monster>();

            int level;
            float debuff;

            //if(attackAmount > 0)
                //collision_MonsterScript.GetDamage(GetattackAmount(), true);

            switch (skillNumber)
            {
                case 1:
                    // level = GameManager.Instance.skillTreeManager.CurCardStates[skillNumber].level;
                    // debuff = GameManager.Instance.skillTreeManager.CardDataContainer.cards[skillNumber].DebuffAmount(level);
                    //collision_MonsterScript.MoveSpeedDown(debuff, DestroyDelayTime);
                    break;

                case 2:
                    //collision_MonsterScript.GetDamage(GetattackAmount(), true);
                    gameObject.SetActive(false);
                    break;

                case 5:
                    //collision_MonsterScript.GetDamage(GetattackAmount(), true);
                    break;

                case 8:
                    // knock back
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
using UnityEngine;

public class RangedAbilityPenetrate : BaseRangedAbility
{
    float Speed;
    int SpreadCount;
    float AddAngle;

    public void ParameterInitialize(float speed, int spreadCount, float addAngle)
    {
        Speed = speed;
        SpreadCount = spreadCount;
        AddAngle = addAngle;
        IsParameterInitialized = true;
    }

    public override void Shoot()
    {
        if (!IsInitialized())
        {
            Debug.LogError("초기화 되지 않았으므로, 총알을 발사하지 않습니다.");
            return;
        }

        Vector2 targetPos = Owner.detector.GetCurrentTargetTransform().position;
        BulletOfMonster bullet = BulletsObjectPool.Instance.GetGO(PrefabName).GetComponent<BulletOfMonster>();
        
        Vector2 bulletDirection = (targetPos - (Vector2)Owner.transform.position).normalized;
        float startAngle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

        bullet.transform.position = FireTf.transform.position;  
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, startAngle));
        
        bullet.Initialize((int)Owner.attributes.ATK.Value);

        PenetrateAddEvent(bullet, startAngle);

        bullet.rb.velocity = bulletDirection * Speed;
    }

    private void PenetrateAddEvent(BulletOfMonster eventTarget, float startAngle)
    {
        eventTarget.OnAttackEvent += (() =>
        {
            for (int i = 0; i < SpreadCount; i++)
            {
                float angle = startAngle + AddAngle / (SpreadCount - 1) * i;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
                BulletOfMonster bullet = BulletsObjectPool.Instance.GetGO(PrefabName).GetComponent<BulletOfMonster>();
                
                bullet.transform.position = eventTarget.transform.position;
                bullet.transform.rotation = rotation;

                bullet.Initialize((int)Owner.attributes.ATK.Value);
                bullet.OnDelayEnabled(0.5f);
                bullet.rb.velocity = bullet.transform.right * Speed;
            }
        });

    }
}
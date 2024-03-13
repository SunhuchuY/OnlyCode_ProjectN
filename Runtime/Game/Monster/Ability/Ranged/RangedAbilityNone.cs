using UnityEngine;


public class RangedAbilityNone : BaseRangedAbility
{
    float Speed;
    BulletOfMonster bullet;

    public void ParameterInitialize(float speed)
    {
        Speed = speed;
        IsParameterInitialized = true;
    }

    public override void Shoot()
    {
        if (!IsInitialized())
        {
            Debug.LogError("초기화 되지 않았으므로, 총알을 발사하지 않습니다.");
            return;
        }

        Vector2 targetPos = Owner.detector.GetCurrentTargetActor().Go.transform.position;
        Vector2 bulletDirection = (targetPos - (Vector2)FireTf.position).normalized;
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        bullet = BulletsObjectPool.Instance.GetGO(PrefabName).GetComponent<BulletOfMonster>();
        
        bullet.transform.position = FireTf.transform.position;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        bullet.Initialize((int)Owner.Stats["Attack"].CurrentValue);
        bullet.rb.velocity = bulletDirection * Speed;
    }
}
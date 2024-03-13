using UnityEngine;

public class RangedAbilityShotGun : BaseRangedAbility
{
    float Speed;
    int BulletCount;
    float SpreadAngle;

    public void ParameterInitialize(float speed, int bulletCount, float spreadAngle)
    {
        Speed = speed;
        BulletCount = bulletCount;
        SpreadAngle = spreadAngle;
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
        float startAngle = Mathf.Atan2(targetPos.y - FireTf.position.y, targetPos.x - FireTf.position.x) * Mathf.Rad2Deg - SpreadAngle / 2;

        for (int i = 0; i < BulletCount; i++)
        {
            float angle = startAngle + SpreadAngle / (BulletCount - 1) * i;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            BulletOfMonster bullet = BulletsObjectPool.Instance.GetGO(PrefabName).GetComponent<BulletOfMonster>();

            bullet.transform.position = FireTf.transform.position;
            bullet.transform.rotation = rotation;   

            bullet.Initialize((int)Owner.Stats["Attack"].CurrentValue);
            bullet.rb.velocity = bullet.transform.right * Speed;
        }
    }
}
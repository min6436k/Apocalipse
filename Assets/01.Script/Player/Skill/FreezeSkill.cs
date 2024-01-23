using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeSkill : BaseSkill
{
    public override void Activate()
    {
        base.Activate();

        // 모든 Enemy 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            if (obj != null)
            {
                if (obj.GetComponent<BossA>() && obj.GetComponent<BossB>())
                    return;

                Freeze Freeze = obj.GetComponent<Freeze>();
                Enemy enemy = obj.GetComponent<Enemy>();
                if (enemy != null && Freeze != null)
                {
                    StartCoroutine(Freeze.FreezeTime(3));
                    StartCoroutine(enemy.FreezeTime(3));
                }
            }
        }

    }
}

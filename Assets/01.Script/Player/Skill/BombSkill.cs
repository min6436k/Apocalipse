using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//폭발 스킬을 구현하는 클래스
public class BombSkill : BaseSkill
{
    //BaseSkill에 있던 Activate함수 내용을 가져오고, 추가로 코드 진행
    //Enemy 태그가 붙은 모든 오브젝트를 검색하여 코드 처리
    public override void Activate()
    {
        base.Activate();

        // 모든 Enemy 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            //if (obj != null)
            //{
            //    if (obj.GetComponent<BossA>())
            //        return;

            //    Enemy enemy = obj.GetComponent<Enemy>();
            //    if (enemy != null)
            //    {
            //        enemy.Dead();
            //    }
            //}
        }

    }
}
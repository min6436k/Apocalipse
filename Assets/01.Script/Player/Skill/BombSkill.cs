using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ��ų�� �����ϴ� Ŭ����
public class BombSkill : BaseSkill
{
    //BaseSkill�� �ִ� Activate�Լ� ������ ��������, �߰��� �ڵ� ����
    //Enemy �±װ� ���� ��� ������Ʈ�� �˻��Ͽ� �ڵ� ó��
    public override void Activate()
    {
        base.Activate();

        // ��� Enemy ã��
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
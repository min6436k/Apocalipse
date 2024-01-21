using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

//��� ��ų�� �����ϴ� Ŭ����
public class RepairSkill : BaseSkill
{
    //BaseSkill�� �ִ� Activate�Լ� ������ ��������, �߰��� �ڵ� ����
    //PlayerHPSystem�� ü���� �ҷ��� �ִ�ü���� ���� �ʵ��� ü���� ȸ����
    public override void Activate()
    {
        base.Activate();

        PlayerHPSystem system = _characterManager.Player.GetComponent<PlayerHPSystem>();
        if (system != null && system.Health < system.MaxHealth)
        {
            system.Health += 1;
            system.Health = system.MaxHealth;
        }
    }
}
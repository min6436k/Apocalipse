using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        PlayerHPSystem system = characterManager.Player.GetComponent<PlayerHPSystem>(); //PlayerHPSystem ���� �Ҵ�
        if (system != null) //���� �����Ѵٸ�
        {
            system.Health += 1; //ü��1 ����, ����� �ٸ��� ������Ʈ�� ���ϴ� ����? �� �𸣰ڴ�.
        }
    }
}
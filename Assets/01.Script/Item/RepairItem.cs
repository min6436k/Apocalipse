using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        PlayerHPSystem system = characterManager.Player.GetComponent<PlayerHPSystem>(); //PlayerHPSystem 변수 할당
        if (system != null) //값이 존재한다면
        {
            system.Health += 1; //체력1 증가, 연료와 다르게 업데이트를 안하는 이유? 잘 모르겠다.
        }
    }
}
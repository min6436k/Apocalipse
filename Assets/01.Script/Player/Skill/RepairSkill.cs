using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//재생 스킬을 구현하는 클래스
public class RepairSkill : BaseSkill
{
    //BaseSkill에 있던 Activate함수 내용을 가져오고, 추가로 코드 진행
    //PlayerHPSystem의 체력을 불러와 최대체력을 넘지 않도록 체력을 회복함
    public override void Activate()
    {
        base.Activate();

        PlayerHPSystem system = _characterManager.Player.GetComponent<PlayerHPSystem>();
        if (system != null)
        {
            system.Health += 1;

            if (system.Health >= system.MaxHealth)
            {
                system.Health = system.MaxHealth;
            }
        }
    }
}
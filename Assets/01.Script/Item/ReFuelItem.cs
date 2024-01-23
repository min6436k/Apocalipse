using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReFuelItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        PlayerFuelSystem system = characterManager.Player.GetComponent<PlayerFuelSystem>(); //PlayerFuelSystem���� �Ҵ�
        if (system != null) //���� �����Ѵٸ�
        {
            system.Fuel = system.MaxFuel; //���Ḧ �ִ�ġ�� ä��
            GameInstance.instance.CurrentPlayerFuel = system.Fuel; //GameInstance���� ���� ������Ʈ
        }
    }
}
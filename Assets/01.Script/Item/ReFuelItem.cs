using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReFuelItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        PlayerFuelSystem system = characterManager.Player.GetComponent<PlayerFuelSystem>(); //PlayerFuelSystem변수 할당
        if (system != null) //값이 존재한다면
        {
            system.Fuel = system.MaxFuel; //연료를 최대치로 채움
            GameInstance.instance.CurrentPlayerFuel = system.Fuel; //GameInstance내부 값도 업데이트
        }
    }
}
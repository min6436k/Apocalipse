using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        characterManager.Player.GetComponent<PlayerCharacter>().SetInvincibility(true); //플레이어의 SetInvincibility 함수 호출
    }
}
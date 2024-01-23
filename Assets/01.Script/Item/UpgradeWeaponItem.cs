using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWeaponItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        if (characterManager != null && characterManager.Player) //characterManager 매개변수와 플레이어의 값이 졵재한다면
        {
            PlayerCharacter playerCharacter = characterManager.Player.GetComponent<PlayerCharacter>(); //playerCharacter 변수 할당

            int currentLevel = playerCharacter.CurrentWeaponLevel; //현재 레벨 변수
            int maxLevel = playerCharacter.MaxWeaponLevel; //최대 레벨 변수

            if (currentLevel >= maxLevel) //현재 레벨이 최대 레벨보다 같거나 크다면
            {
                //GameManager.Instance.AddScore(30);
                return;//함수 종료
            }

            playerCharacter.CurrentWeaponLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel); //현재 레벨을+1을 0과 최대레벨사이로 제한
            GameInstance.instance.CurrentPlayerWeaponLevel = playerCharacter.CurrentWeaponLevel; //GameInstance에 업데이트
        }
    }
}
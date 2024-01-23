using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWeaponItem : BaseItem
{
    public override void OnGetItem(CharacterManager characterManager)
    {
        if (characterManager != null && characterManager.Player) //characterManager �Ű������� �÷��̾��� ���� �����Ѵٸ�
        {
            PlayerCharacter playerCharacter = characterManager.Player.GetComponent<PlayerCharacter>(); //playerCharacter ���� �Ҵ�

            int currentLevel = playerCharacter.CurrentWeaponLevel; //���� ���� ����
            int maxLevel = playerCharacter.MaxWeaponLevel; //�ִ� ���� ����

            if (currentLevel >= maxLevel) //���� ������ �ִ� �������� ���ų� ũ�ٸ�
            {
                //GameManager.Instance.AddScore(30);
                return;//�Լ� ����
            }

            playerCharacter.CurrentWeaponLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel); //���� ������+1�� 0�� �ִ뷹�����̷� ����
            GameInstance.instance.CurrentPlayerWeaponLevel = playerCharacter.CurrentWeaponLevel; //GameInstance�� ������Ʈ
        }
    }
}
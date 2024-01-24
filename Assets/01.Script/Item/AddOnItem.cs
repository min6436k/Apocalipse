using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnItem : BaseItem
{
    public GameObject AddOnPrefab;
    public static void SpawnAddOn(CharacterManager characterManager, GameObject AddOnPrefap, Transform targetTransform)
    {
        GameObject instance = Instantiate(AddOnPrefap, characterManager.Player.transform.position, Quaternion.identity);
        instance.GetComponent<AddOn>().TargetTransform = targetTransform;
    }


    public override void OnGetItem(CharacterManager characterManager)
    {
        int currentAddOnCount = GameInstance.instance.CurrentAddOnCount;

        PlayerCharacter playerCharacter = characterManager.Player.GetComponent<PlayerCharacter>();

        if (currentAddOnCount == playerCharacter.MaxAddOnCound) return;

        SpawnAddOn(characterManager, AddOnPrefab, playerCharacter.AddOnPos[currentAddOnCount]);

        playerCharacter.PlusAddOnCount(1);
    }
}
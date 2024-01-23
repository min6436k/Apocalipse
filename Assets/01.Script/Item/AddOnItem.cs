using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnItem : BaseItem
{
    public GameObject AddOnPrefab;
    public static void SpawnAddOn(CharacterManager characterManager, GameObject AddOnPrefap, Transform targetTransform)
    {
        GameObject instance = Instantiate(AddOnPrefap, characterManager.Player.transform.position, Quaternion.identity);
        instance.GetComponent<AddOn>().targetTransform = targetTransform;
    }


    public override void OnGetItem(CharacterManager characterManager)
    {
        int CurrentAddOnCount = GameInstance.instance.CurrentAddOnCount;

        PlayerCharacter playerCharacter = characterManager.Player.GetComponent<PlayerCharacter>();

        int AddOnCount = playerCharacter.AddOnCount;

        if (CurrentAddOnCount == AddOnCount) return;
        SpawnAddOn(characterManager, AddOnPrefab, playerCharacter.AddOnPos[AddOnCount]);
        characterManager.Player.GetComponent<PlayerCharacter>().AddOnCount++;
        GameInstance.instance.CurrentAddOnCount = AddOnCount;
    }
}
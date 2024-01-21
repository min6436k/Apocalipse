using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkill : BaseSkill
{
    public override void Activate()
    {
        base.Activate();

        PlayerHPSystem HPsystem = _characterManager.Player.GetComponent<PlayerHPSystem>();
        PlayerFuelSystem Fuelsystem = _characterManager.Player.GetComponent<PlayerFuelSystem>();
        if (HPsystem != null && Fuelsystem.Fuel >= Fuelsystem.MaxFuel*0.4 && HPsystem.Health < HPsystem.MaxHealth)
        {
            HPsystem.Health += 1;
            Fuelsystem.Fuel -= (Fuelsystem.MaxFuel / 3);
        }
    }
}

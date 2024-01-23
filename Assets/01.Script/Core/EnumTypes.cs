
//프로젝트에서 사용할 Enum들을 한곳에 모아둘 namespace
using UnityEngine;

namespace EnumTypes
{
    public enum PlayerSkill
    {
        Primary = 0, 
        Repair, 
        Bomb, 
        Freeze,
        Shield,
        Change,
    }

    public enum ItemName
    {
        UpgradeWeapon = 0,
        Invincibility,
        Repair,
        Refuel,
        AddOn,
        Last,
    }
}



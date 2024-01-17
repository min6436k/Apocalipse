using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//일반 공격 스킬을 구현하는 클래스
public class PrimarySkill : BaseSkill
{
    public float ProjectileMoveSpeed;
    public GameObject Projectile;
    public GameObject Homingtile;

    private Weapon[] weapons;
    //각 무기에 대한 정보를 담기 위한 배열

    //시작 시 스킬의 쿨타임을 정하고, 무기 배열을 초기화함
    void Start()
    {
        CooldownTime = 0.2f;

        weapons = new Weapon[6];

        weapons[0] = new Level1Weapon();
        weapons[1] = new Level2Weapon();
        weapons[2] = new Level3Weapon();
        weapons[3] = new Level4Weapon();
        weapons[4] = new Level5Weapon();
        weapons[5] = new Level6Weapon();
    }

    //BaseSkill에 있던 Activate함수 내용을 가져오고, 추가로 코드 진행
    //캐릭터가 가진 현재 무기의 레벨을 무기 배열의 인덱스로 불러와 그에 맞는 레벨의 무기 클래스가 가진 Activate 함수를 호출
    public override void Activate()
    {
        base.Activate();
        weapons[_characterManager.Player.GetComponent<PlayerCharacter>().CurrentWeaponLevel].Activate(this, _characterManager);
        //GameManager.Instance.SoundManager.PlaySFX("PrimarySkill");
    }

    //오브젝트를 생성하고 입력받은 위치와 방향으로 이동시키는 함수
    public void ShootProjectile(Vector3 position, Vector3 direction)
    {
        GameObject instance = Instantiate(Projectile, position, Quaternion.identity);
        Projectile projectile = instance.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.MoveSpeed = ProjectileMoveSpeed;
            projectile.SetDirection(direction.normalized);

            if (_characterManager.Player.GetComponent<PlayerCharacter>().CurrentWeaponLevel == 5)
            {
                projectile.Homingtile = Homingtile;
                projectile.Is5Level = true;
            }
        }
    }
}

//무기들의 정보를 담을 인터페이스를 선언
//모든 레벨의 무기들은 꼭 이 인터페이스에 있는 Activate를 구현해야 함
public interface Weapon
{
    void Activate(PrimarySkill primarySkill, CharacterManager characterManager);
}

//위치를 플레이어로 설정하고 ShootProjectile를 호출하여 총알을 발사함
public class Level1Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;
        primarySkill.ShootProjectile(position, Vector3.up);
    }
}

//추가로 총알을 한발 더 발사함
public class Level2Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;
        position.x -= 0.1f;

        for (int i = 0; i < 2; i++)
        {
            primarySkill.ShootProjectile(position, Vector3.up);
            position.x += 0.2f;
        }
    }
}

//한번에 총알을 3개 발사함
public class Level3Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;

        primarySkill.ShootProjectile(position, Vector3.up);
        primarySkill.ShootProjectile(position, new Vector3(0.3f, 1, 0));
        primarySkill.ShootProjectile(position, new Vector3(-0.3f, 1, 0));
    }
}

//총알을 두번 연속으로 발사하고, 양옆으로 하나씩 발사함
public class Level4Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;
        position.x -= 0.1f;

        for (int i = 0; i < 2; i++)
        {
            primarySkill.ShootProjectile(position, Vector3.up);
            position.x += 0.2f;
        }

        Vector3 position2 = characterManager.Player.transform.position;
        primarySkill.ShootProjectile(position2, new Vector3(0.3f, 1, 0));
        primarySkill.ShootProjectile(position2, new Vector3(-0.3f, 1, 0));
    }
}

public class Level5Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;

        for (int i = 0; i < 180; i += 10) // 360도를 10도씩 나눠서 총알 발사
        {
            /*
               180 degree = π radian
               1 degree = π / 180 radian
               x degree = x * π / 180 radian
 
               π radian = 180 degree
               1 radian = 180 / π degree
               x radian = x * 180 / π degree
             */

            // i = degree
            // Deg2Rad = 180 / π degree
            // Mathf 의 cos, sin 은 rad 를 넣어줘야 함.

            float angle = i * Mathf.Deg2Rad;
            Debug.Log(angle);
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            primarySkill.ShootProjectile(position, direction);
        }
    }
}

public class Level6Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;

        primarySkill.ShootProjectile(position, Vector3.up);

        position -= new Vector3(0.3f, 0.4f);
        for (int i = 0; i < 2; i++)
        {
            primarySkill.ShootProjectile(position, Vector3.up);
            position.x += 0.6f;
        }
    }
}

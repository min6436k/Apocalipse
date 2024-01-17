using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ϲ� ���� ��ų�� �����ϴ� Ŭ����
public class PrimarySkill : BaseSkill
{
    public float ProjectileMoveSpeed;
    public GameObject Projectile;
    public GameObject Homingtile;

    private Weapon[] weapons;
    //�� ���⿡ ���� ������ ��� ���� �迭

    //���� �� ��ų�� ��Ÿ���� ���ϰ�, ���� �迭�� �ʱ�ȭ��
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

    //BaseSkill�� �ִ� Activate�Լ� ������ ��������, �߰��� �ڵ� ����
    //ĳ���Ͱ� ���� ���� ������ ������ ���� �迭�� �ε����� �ҷ��� �׿� �´� ������ ���� Ŭ������ ���� Activate �Լ��� ȣ��
    public override void Activate()
    {
        base.Activate();
        weapons[_characterManager.Player.GetComponent<PlayerCharacter>().CurrentWeaponLevel].Activate(this, _characterManager);
        //GameManager.Instance.SoundManager.PlaySFX("PrimarySkill");
    }

    //������Ʈ�� �����ϰ� �Է¹��� ��ġ�� �������� �̵���Ű�� �Լ�
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

//������� ������ ���� �������̽��� ����
//��� ������ ������� �� �� �������̽��� �ִ� Activate�� �����ؾ� ��
public interface Weapon
{
    void Activate(PrimarySkill primarySkill, CharacterManager characterManager);
}

//��ġ�� �÷��̾�� �����ϰ� ShootProjectile�� ȣ���Ͽ� �Ѿ��� �߻���
public class Level1Weapon : Weapon
{
    public void Activate(PrimarySkill primarySkill, CharacterManager characterManager)
    {
        Vector3 position = characterManager.Player.transform.position;
        primarySkill.ShootProjectile(position, Vector3.up);
    }
}

//�߰��� �Ѿ��� �ѹ� �� �߻���
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

//�ѹ��� �Ѿ��� 3�� �߻���
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

//�Ѿ��� �ι� �������� �߻��ϰ�, �翷���� �ϳ��� �߻���
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

        for (int i = 0; i < 180; i += 10) // 360���� 10���� ������ �Ѿ� �߻�
        {
            /*
               180 degree = �� radian
               1 degree = �� / 180 radian
               x degree = x * �� / 180 radian
 
               �� radian = 180 degree
               1 radian = 180 / �� degree
               x radian = x * 180 / �� degree
             */

            // i = degree
            // Deg2Rad = 180 / �� degree
            // Mathf �� cos, sin �� rad �� �־���� ��.

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

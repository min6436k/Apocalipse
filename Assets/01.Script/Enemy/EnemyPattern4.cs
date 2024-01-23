using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyPattern4 : MonoBehaviour, Freeze
{
    public float MoveSpeed;
    public float AttackStopTime;
    public float MoveTime;
    public GameObject Projectile;
    public float ProjectileMoveSpeed;

    public float acceleration;
    public float accelTime;

    private bool _isAttack = false;

    private GameObject manager;
    private BaseCharacter character;

    void Start()
    {
        manager = GameObject.Find("Managers");
        character = manager.GetComponent<CharacterManager>().Player;
        StartCoroutine(Attack());
    }

    void Update()
    {
        if (false == _isAttack)
            Move();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 기다림

            if (character is null)
            {
                Debug.Log("Player is null");
                break;
            }

            Vector3 playerPos = character.GetComponent<Transform>().position;
            Vector3 direction = playerPos - transform.position;
            direction.Normalize();

            Vector3 position = transform.position;

            position.x -= 0.25f;

            for (int i = 0; i < 2; i++)
            {
                GameObject instance = Instantiate(Projectile, position, Quaternion.identity);
                Projectile projectile = instance.GetComponent<Projectile>();
                projectile.SetDirection(direction);
                projectile.MoveSpeed = ProjectileMoveSpeed;
                projectile.acceleration = acceleration;
                projectile.accelTime = accelTime;

                position.x += 0.5f;
            }

            _isAttack = true;

            yield return new WaitForSeconds(AttackStopTime); // 1초 기다림

            _isAttack = false;

            yield return new WaitForSeconds(MoveTime); // 3초 동안 움직임
        }
    }

    public IEnumerator FreezeTime(int stopTime)
    {
        StopAllCoroutines();
        _isAttack = true;
        yield return new WaitForSeconds(stopTime);
        StartCoroutine(Attack());
        _isAttack = false;
    }

    void Move()
    {
        transform.position -= new Vector3((character.GetComponent<Transform>().position - transform.position).normalized.x*-0.5f, MoveSpeed, 0f)*Time.deltaTime;
    }
}

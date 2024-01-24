using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOn : MonoBehaviour
{
    public GameObject HomingProjectTile;

    public Transform TargetTransform;
    public Transform TargetEnemyTransform;

    public int FollowSpeed = 20;
    public float AttackInterval = 0.5f;

    private void Start()
    {
        StartCoroutine(ShootProjectile());
    }

    private void Update()
    {
        if (TargetTransform == null) return;
        transform.position = Vector3.Lerp(transform.position, TargetTransform.position, FollowSpeed * Time.deltaTime);
    }

    IEnumerator ShootProjectile()
    {
        SearchEnemy();

        if (TargetEnemyTransform is not null)
        {
            GameObject instance = Instantiate(HomingProjectTile, transform.position, Quaternion.identity);
            instance.GetComponent<HomingProjectile>().target = TargetEnemyTransform;
        }

        yield return new WaitForSeconds(AttackInterval);
        StartCoroutine(ShootProjectile());
    }

    private void SearchEnemy()
    {
        TargetEnemyTransform = null;

        float distance = float.MaxValue;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            if (obj?.GetComponent<Projectile>() == null && obj?.GetComponent<Meteor>() == null)
            {
                float targetDistance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance >= targetDistance)
                {
                    TargetEnemyTransform = obj.transform;
                    distance = Mathf.Min(targetDistance, distance);
                }
            }
        }
    }
}
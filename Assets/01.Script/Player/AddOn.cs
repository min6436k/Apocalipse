using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOn : MonoBehaviour
{
    public Transform TargetTransform;
    public GameObject HomingTile;
    public int FollowSpeed = 20;
    public float AttackInterval = 0.5f;

    public Transform TargetEnemyTransform;

    private Coroutine _searchCoroutine;

    private void Start()
    {
        StartCoroutine(ShootProjectile());
    }
    private void Update()
    {
        if (TargetEnemyTransform == null)
        {
            if (_searchCoroutine != null) StopCoroutine(_searchCoroutine);
            _searchCoroutine = StartCoroutine(SearchEnemy());
        }

        transform.position = Vector3.Lerp(transform.position, TargetTransform.position, FollowSpeed * Time.deltaTime);
    }

    IEnumerator ShootProjectile()
    {
        if (TargetEnemyTransform != null)
        {

            GameObject instance = Instantiate(HomingTile, transform.position, Quaternion.identity);
            instance.GetComponent<Homingtile>().target = TargetEnemyTransform;
        }
        yield return new WaitForSeconds(AttackInterval);
        StartCoroutine(ShootProjectile());

    }

    IEnumerator SearchEnemy()
    {
        if (GameManager.Instance.bStageCleared) StopAllCoroutines();

        float distance = float.MaxValue;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            if (obj != null && obj.GetComponent<Projectile>() == null && obj.GetComponent<Meteor>() == null)
            {
                if (distance >= Vector3.Distance(transform.position, obj.transform.position))
                {
                    TargetEnemyTransform = obj.transform;
                    distance = Mathf.Min(Vector3.Distance(transform.position, obj.transform.position), distance);
                }
            }
        }


        yield return new WaitForSeconds(1);

        _searchCoroutine = StartCoroutine(SearchEnemy());
    }

}
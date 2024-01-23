using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AddOn : MonoBehaviour
{
    public Transform targetTransform;
    public GameObject HomingTile;
    public int followSpeed = 20;
    public float ATKInterval = 0.5f;

    public Transform targetEnemyTransform;

    private Coroutine SearchCoroutine;

    private void Start()
    {
        StartCoroutine(ShootProjectile());
    }
    private void Update()
    {
        if (targetEnemyTransform == null)
        {
            if (SearchCoroutine != null) StopCoroutine(SearchCoroutine);
            SearchCoroutine = StartCoroutine(SearchEnemy());
        }

        transform.position = Vector3.Lerp(transform.position, targetTransform.position, followSpeed * Time.deltaTime);
    }

    IEnumerator ShootProjectile()
    {
        if (targetEnemyTransform != null)
        {

            GameObject instance = Instantiate(HomingTile, transform.position, Quaternion.identity);
            instance.GetComponent<Homingtile>().target = targetEnemyTransform;
        }
        yield return new WaitForSeconds(ATKInterval);
        StartCoroutine(ShootProjectile());

    }

    IEnumerator SearchEnemy()
    {
        if (GameManager.Instance.bStageCleared) StopAllCoroutines();

        float distance = float.MaxValue;
        GameObject target;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enemies)
        {
            if (obj != null && obj.GetComponent<Projectile>() == null)
            {
                if (distance >= Vector3.Distance(transform.position, obj.transform.position))
                {
                    targetEnemyTransform = obj.transform;
                    distance = Mathf.Min(Vector3.Distance(transform.position, obj.transform.position), distance);
                }
            }
        }


        yield return new WaitForSeconds(1);

        SearchCoroutine = StartCoroutine(SearchEnemy());
    }

}
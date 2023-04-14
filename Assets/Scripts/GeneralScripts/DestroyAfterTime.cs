using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    void Start()
    {
        StartCoroutine(DestroyThis());
    }

    // Update is called once per frame
    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float timeforDestruction;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf(timeforDestruction));
    }

    private IEnumerator DestroySelf(float timeforDestruction)
    {
        yield return new WaitForSeconds(timeforDestruction);
        Destroy(gameObject);
    }

}

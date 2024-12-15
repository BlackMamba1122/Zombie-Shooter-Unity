using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottlee : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Rigidbody> allparts = new List<Rigidbody>();
    public void shatter()
    {
        foreach (Rigidbody part in allparts)
        {
            part.isKinematic = false;
        }
    }
}

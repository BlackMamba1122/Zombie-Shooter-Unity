using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 100;
    public AmmoType ammoType;
    public enum AmmoType
    {
        pistol_ammo,
        riffle_ammo
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    // The amount of damage to deal (if not specified already)
    public float meleeDamage = 10.0f;

    // Deal damage to the specified game object
    public void MeleeAttack(GameObject target, float damage = -1)
    {
        // If no damage value was specified, set it to this script's default
        if (damage == -1)
        {
            damage = meleeDamage;
        }

        Debug.Log("<color='orange'> Enemy dealt " + meleeDamage + " damage to " + target + "</color>");
    }
}

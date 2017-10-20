namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Player Weapon Type", menuName = "CreateWeapon/PlayerWeaponType", order = 1)]
    public class PlayerWeaponType : ScriptableObject
    {

        [SerializeField]
        private float fireRate = .5f;
        private float FireRate { get { return fireRate; } }

    }
}

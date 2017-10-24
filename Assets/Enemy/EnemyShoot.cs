namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyShoot : Enemy
    {
        [SerializeField]
        private int numOfShots = 2;
        [SerializeField]
        private float timeBetweenShots = 1.5f;

        public override void StartMovement()
        {
            base.StartMovement();
            StartCoroutine(ShootTwice());
        }

        IEnumerator ShootTwice()
        {
            yield return new WaitForSeconds(timeBetweenShots);
            int counter = 0;
            while (counter < numOfShots)
            {
                counter++;
                FireWeapon();
            }
        }

        public virtual void FireWeapon()
        {

        }
    }
}

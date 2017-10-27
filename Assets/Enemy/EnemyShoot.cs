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
        [SerializeField]
        private EnemyBullet_Base bulletPrefab;

        public override void StartMovement()
        {
            base.StartMovement();
            StartCoroutine(ShootTwice());
        }

        IEnumerator ShootTwice()
        {
            yield return new WaitForSeconds(timeBetweenShots);
            int counter = 0;
            if (numOfShots != 0)
            {
                while (counter < numOfShots)
                {
                    counter++;
                    FireWeapon();
                    yield return new WaitForSeconds(timeBetweenShots);
                }
            }
            else
            {
                while (counter == 0)
                {
                    FireWeapon();
                    yield return new WaitForSeconds(timeBetweenShots);
                }
            }
        }

        public virtual void FireWeapon()
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}

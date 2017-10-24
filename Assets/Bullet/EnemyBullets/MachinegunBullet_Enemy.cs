namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MachinegunBullet_Enemy : EnemyBullet_Base
    {
        private void Start()
        {
            Transform target = AquireTarget();
            transform.LookAt(target);
        }

        protected override void ExecuteMovement()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}

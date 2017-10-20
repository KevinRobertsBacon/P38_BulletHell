namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MachinegunBullet : PlayerBullet_Base
    {
        protected override void ExecuteMovement()
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }
}

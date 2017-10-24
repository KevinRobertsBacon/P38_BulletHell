namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyBullet_Base : Bullet_Base
    {
        protected virtual Transform AquireTarget()
        {
            EventGetPlayerTransform playerTransformGetter = new EventGetPlayerTransform();
            EventManager.TriggerEvent(playerTransformGetter);
            return playerTransformGetter.playerTransform;
        }

        protected override void ExecuteMovement()
        {
            throw new System.NotImplementedException();
        }
    }
}

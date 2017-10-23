namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Bullet_Base : MonoBehaviour
    {
        [SerializeField]
        protected float speed = 3f;

        [SerializeField]
        private float damage = 1f;

        [SerializeField]
        private LayerMask triggerLayersToIgnore;

        public enum DestroyCondition { Distance, Camera, Time }
        public DestroyCondition destroyCondition;

        protected abstract void ExecuteMovement();

        

        protected virtual void Update()
        {
            ExecuteMovement();

            switch (destroyCondition)
            {
                case DestroyCondition.Camera:
                    if (transform.position.y > Camera.main.orthographicSize)
                        Destroy(this.gameObject);
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & triggerLayersToIgnore) != 0)
            {
                //we hit an ignore layer
                DebugHelper.Log("Bullet hit Something, but let's ignore it.");
            }
            else
            {
                BulletCollision(collision.gameObject);
                Destroy(this.gameObject);
            }
            
        }

        protected virtual void BulletCollision(GameObject hitObj)
        {
            Enemy hitEnemy = hitObj.GetComponent<Enemy>();
            if (hitEnemy != null)
                hitEnemy.DealDamage(damage);
        }
    }
}

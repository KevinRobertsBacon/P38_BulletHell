namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth;

        private float damage = 0f;

        public virtual void StartMovement()
        {

        }
        
        public void DealDamage (float f)
        {
            damage += f;
            if (damage >= maxHealth)
            {
                Death();
            }
        }

        protected virtual void Death()
        {
            Destroy(this.gameObject);
        }
    }
}

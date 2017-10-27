namespace P38
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Level : MonoBehaviour
    {
        [SerializeField]
        private float speed = 2f;
        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(transform.position, .75f);
        }
    }
}

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
    }
}

namespace P38
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyWaveSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<Enemy> enemiesToSpawn = new List<Enemy>();

        [SerializeField]
        private float Spawnrate = 1f;

        bool spawned = false;

        [SerializeField]
        private int numOfEnemiesInWave = 5;

        private void Update()
        {
            //todo make a smarter instigator... maybe.
            if (!spawned)
            {
                if (transform.position.y < Camera.main.orthographicSize)
                    BuildWave();
            }
        }

        void BuildWave()
        {
            spawned = true;
            List<Enemy> builtWave = new List<Enemy>();
            int counter = 0;
            while (builtWave.Count < numOfEnemiesInWave)
            {
                builtWave.Add(enemiesToSpawn[counter]);
                counter = Increment(counter, enemiesToSpawn.Count);
            }
            SpawnMyWave(builtWave);
        }

        int Increment(int current, int max)
        {
            current++;
            if (current >= max)
            {
                return 0;
            }
            else
                return current;
        }

        void SpawnMyWave(List<Enemy> wave)
        {
            StartCoroutine(Spawning(wave));
        }

        IEnumerator Spawning (List<Enemy> wave)
        {
            GameObject offsetter = new GameObject("Offsetter");
            offsetter.transform.position = transform.position;
            offsetter.transform.rotation = transform.rotation;
            for (int i = 0; i < wave.Count; i++)
            {
                Enemy spawnedEnemy  = Instantiate(wave[i], transform.position, transform.rotation);
                spawnedEnemy.transform.parent = offsetter.transform;
                spawnedEnemy.StartMovement();
                yield return new WaitForSeconds(Spawnrate);
            }
        }
    }
}

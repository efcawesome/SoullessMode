using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;


namespace SoullessMode
{
    public class GruzMotherSpawner : MonoBehaviour
    {
        System.Random random = new System.Random();
        int maxHP = 0;
        private void Start()
        {
            maxHP = this.gameObject.GetComponent<HealthManager>().hp;
            StartCoroutine(SpawnGruzzers());
        }

        private IEnumerator SpawnGruzzers()
        {
            while(true)
            {
                yield return new WaitForSeconds(3f);
                if(this.gameObject.GetComponent<HealthManager>().hp < maxHP)
                {
                    GameObject gruzzer = Instantiate(SoullessMode.preloadedGameObjects["Gruzzer"]);
                    gruzzer.SetActive(true);
                    if(random.Next(6) == 0)
                    {
                        PlayMakerFSM fsm = gruzzer.LocateMyFSM("Bouncer Control");
                        fsm.FsmVariables.FindFsmFloat("Speed").Value = 15f;
                        gruzzer.transform.localScale *= 1.5f;
                        gruzzer.GetComponent<HealthManager>().hp = 20;
                    }
                    gruzzer.transform.position = this.gameObject.transform.position;
                }
            }
        }
    }
}

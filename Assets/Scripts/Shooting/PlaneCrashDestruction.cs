using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlightSim
{
    using AI;
    public class PlaneCrashDestruction : MonoBehaviour
    {
        public float health = 100;
        [SerializeField] GameObject explosion;
        private void OntriggerEnter3D(BoxCollider other)
        {
            die();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                die();
            }
        }
        public void getsDamaged(float damage)
        {
            health -= damage;
            if (planeDead())
            {
                die();
            }
        }
        private void die()
        {
            Debug.Log(gameObject.name + " just died");
            GetComponent<FlightControl>().enabled = false;
            GetComponent<ComplexAI>().enabled = false;
            explode();
            explodeParticle();
            StartCoroutine(perish());
        }
        private void explode()
        {
            float power = 200;
            float radius = 100;
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
        }
        private void explodeParticle()
        {
            GameObject expl = Instantiate(explosion, transform.position, transform.rotation);
            StartCoroutine(destroyExplosion(expl));
        }
        IEnumerator destroyExplosion(GameObject expl)
        {
            yield return new WaitForSeconds(1);
            Destroy(expl);
            Destroy(this);
        }
        private IEnumerator perish()
        {
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }
        private bool planeDead()
        {
            if (health <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
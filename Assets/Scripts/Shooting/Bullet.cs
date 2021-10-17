using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlightSim
{
    public class Bullet : MonoBehaviour
    {
        public BulletData data;
        private Rigidbody rb;
        [SerializeField] GameObject explosion;
        private float delay = 2f;

        private void FixedUpdate()
        {
            checkIfHitsPlane();
        }

        public void shootThis()
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(explodeDelay());
            rb.AddForce(transform.forward * data.speed);
        }
        private void checkIfHitsPlane()
        {
            PlaneCrashDestruction Hit = hitPlane();
            if (Hit != null)
            {
                Hit.getsDamaged(data.damage);
            }
        }
        private PlaneCrashDestruction hitPlane()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.CompareTag("plane") && hit.collider.gameObject.GetComponent<PlaneCrashDestruction>() != null)
                {
                    Debug.LogWarning("Other plane hit!!!!");
                    return hit.collider.gameObject.GetComponent<PlaneCrashDestruction>();
                }
            }
            return null;
        }
        private void explode()
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
        IEnumerator explodeDelay()
        {
            yield return new WaitForSeconds(delay);
            explode();
        }
        
    }
}

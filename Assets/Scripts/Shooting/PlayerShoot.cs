using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FlightSim
{
    public class PlayerShoot : ShootingMechanic
    {
        [SerializeField] private TextMeshProUGUI ammoTmpro;
        private void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                shootTurret();
            }
        }
        public override void updateAmmoText()
        {
            ammoTmpro.text = "Ammo: " +  currentAmmoInTurret.ToString();
        }
        public override void noAmmoText()
        {
            ammoTmpro.text = "Out of Ammo";
        }
        public override void reloadText()
        {
            ammoTmpro.text = "Reloading... " ;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace FlightSim
{
    public class ShootingMechanic : MonoBehaviour
    {
        [SerializeField] private int maxAmmo = 90;
        [SerializeField] private int ammoInTurret = 30;
        [SerializeField] public float reloadWhenTurretIsEmpty;
        private int currentAmmo;
        protected int currentAmmoInTurret;
        public GameObject currentBullet;
        [SerializeField] GameObject turret;
        private ShootState shootState = new ShootState();
        private void Start()
        {
            currentAmmo = maxAmmo;
            currentAmmoInTurret = ammoInTurret;
            updateAmmoText();
        }
        private void Update()
        {
            shootState = shootState.HandleInput();
            shootState.shootMechanic = this;
        }
        protected void shootTurret()
        {
            shootState.signToShoot();
        }
        public void shootBullet()
        {
            GameObject Instance = Instantiate(currentBullet, turret.transform.position, transform.rotation);
            Bullet bullet = Instance.GetComponent<Bullet>();
            bullet.shootThis();
            currentAmmoInTurret--;
        }
        public void resetTurret()
        {
            if (currentAmmo < ammoInTurret)
            {
                currentAmmoInTurret = currentAmmo;
            }
            else
            {
                currentAmmoInTurret = ammoInTurret;
            }
            currentAmmo -= ammoInTurret;
            print(currentAmmoInTurret.ToString());
            updateAmmoText();
        }
        public bool noAmmo()
        {
            if (currentAmmo <= 0)
            {
                return true;
            }
            return false;
        }
        public bool noAmmoInTurret()
        {
            if(currentAmmoInTurret <= 0)
            {
                return true;
            }
            return false;
        }
        
        public void _reloadTurret()
        {
            StartCoroutine(shootState.reloadTurret());
        }
        public virtual void noAmmoText()
        {

        }
        public virtual void reloadText()
        {

        }

        public virtual void updateAmmoText()
        {

        }

    }
    class ShootState
    {
        public ShootingMechanic shootMechanic;
        private bool shoot = false;
        public void signToShoot()
        {
            shoot = true;
            HandleInput();
        }
        public virtual ShootState HandleInput()
        {
            if (shoot)
            {
                return new shoot();
            }
            return this;
        }
        public virtual IEnumerator reloadTurret()
        {
            yield return new WaitForSeconds(shootMechanic.reloadWhenTurretIsEmpty);
        }
    }


    class reloadingState : ShootState
    {
        private bool isReloading = false;
        private bool reloaded = false;
        public override ShootState HandleInput()
        {
            if (shootMechanic.noAmmo())
            {
                return new outOfAmmo();
            }
            if (!isReloading)
            {
                shootMechanic._reloadTurret();
                isReloading = true;
                shootMechanic.reloadText();
            }
            else
            {
                if (!reloaded) return this;
                reloaded = false;
                isReloading = false;
                if (shootMechanic.noAmmo())
                {
                    return new outOfAmmo();
                }
                else
                {
                    return new ShootState();
                }
            }
            return this;
        }
        public override IEnumerator reloadTurret()
        {
            yield return new WaitForSeconds(shootMechanic.reloadWhenTurretIsEmpty);
            shootMechanic.resetTurret();
            reloaded = true;
        }

    }
    class shoot : ShootState
    {
        public override ShootState HandleInput()
        {
            shootMechanic.shootBullet();
            shootMechanic.updateAmmoText();
            if (shootMechanic.noAmmoInTurret())
            {
                return new reloadingState();
            }
            return new ShootState();
        }
    }
    class outOfAmmo : ShootState
    {
        public override ShootState HandleInput()
        {
            shootMechanic.noAmmoText();
            return this;
        }
    }
}


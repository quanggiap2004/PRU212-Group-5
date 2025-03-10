using Assets.Scripts.Gameplay.Tower.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Tower.BasicTower
{
    public class BasicTower : FrostTower
    {
        [Header("Basic Tower Attributes")]
        [SerializeField] private float basicTargetingRange = 7f;
        [SerializeField] private float basicRotationSpeed = 150f;
        [SerializeField] private float basicFireRate = 1.5f;
        [SerializeField] private int basicBaseUpgradeCost = 15;
        [SerializeField] private bool isUpgraded = false;

        protected override void Start()
        {
            // Override the values for the Basic Tower
            if(!isUpgraded)
            {
                targetingRange = basicTargetingRange;
                rotationSpeed = basicRotationSpeed;
                fireRate = basicFireRate;
                baseUpgradeCost = basicBaseUpgradeCost;
            }
            Debug.Log("Basic Tower Start" + basicTargetingRange);
            // Call the base class Start method
            base.Start();
        }

        protected override void Shoot()
        {
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            BasicTowerBullet bulletScript = bulletObj.GetComponent<BasicTowerBullet>();
            bulletScript.SetTarget(target);
        }

        public override void UpgradeAppearance(int index)
        {
            Debug.Log("Haha: " + towerPlot);

            GameObject upgradedTower = Instantiate(upgradedPrefab[index], transform.position, transform.rotation);
            upgradedTower.GetComponent<BasicTower>().level = this.level;
            upgradedTower.GetComponent<BasicTower>().fireRate = this.fireRate;
            upgradedTower.GetComponent<BasicTower>().targetingRange = this.targetingRange;
            upgradedTower.GetComponent<BasicTower>().baseUpgradeCost = this.baseUpgradeCost;
            upgradedTower.GetComponent<BasicTower>().isUpgraded = true;
            base.towerPlot.SetTower(upgradedTower);
            
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }

        public override void OpenUpgradeUI()
        {
            upgradeUI.SetActive(true);
        }

        public override void SetTowerPlot(TowerPlot plot)
        {
            base.SetTowerPlot(plot);

            Debug.Log("Tower plot basic" + towerPlot);
        }
    }
}

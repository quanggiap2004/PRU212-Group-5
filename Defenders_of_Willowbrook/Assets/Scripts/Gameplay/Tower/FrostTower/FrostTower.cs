using Assets.Scripts.Gameplay.Tower.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FrostTower : MonoBehaviour, ITower
{
    [Header("References")]
    [SerializeField] private Transform weaponRotation;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firingPoint;
    [SerializeField] public GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton;
    [SerializeField] public TextMeshProUGUI upgradeCostText;
    [SerializeField] public TextMeshProUGUI sellCostText;
    [SerializeField] public GameObject[] upgradedPrefab;

    [Header("Attributes")]
    [SerializeField] public float targetingRange = 5f;
    [SerializeField] public float rotationSpeed = 200f;
    [SerializeField] public float fireRate = 0.6f;
    [SerializeField] public int baseUpgradeCost = 20;

    protected Transform target;
    public float timeUntilFire;
    public float fireRateBase;
    public float targetingRangeBase;
    public int level = 1;
    public TowerPlot towerPlot;
    protected virtual void Start()
    {
        fireRateBase = fireRate;
        targetingRangeBase = targetingRange;
        upgradeCostText.text = baseUpgradeCost.ToString();
        sellCostText.text = (baseUpgradeCost / 2).ToString();
        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(SellTower);
    }
    protected virtual void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetIsInRange()) { 
            target = null;
        } else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / fireRate)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    protected virtual void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        FrostTowerBullet bulletScript = bulletObj.GetComponent<FrostTowerBullet>();
        bulletScript.SetTarget(target);
        //Debug.Log("Shoot");
    }
    protected virtual void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    protected bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    protected void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        weaponRotation.rotation = Quaternion.RotateTowards(weaponRotation.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    protected void OnDrawGizmosSelected()
    {
       Handles.color = Color.blue;
       Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    public virtual void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public virtual void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UpgradeAndSoldUIManager.main.SetHoveringState(false);
    }

    protected virtual void Upgrade()
    {
        if(baseUpgradeCost > Level1Manager.main.CurrentMoney)
        {
            return;
        }
        Level1Manager.main.SpendMoney(baseUpgradeCost);
        level++;
        fireRate = CalculateFireRate();
        targetingRange = CalculateTargetingRange();
        baseUpgradeCost = CalculateUpgradeCost();

        upgradeCostText.text = baseUpgradeCost.ToString();
        sellCostText.text = (baseUpgradeCost / 2).ToString();
        if(level == 2)
        {
            UpgradeAppearance(0);
        } else if (level == 3)
        {
            UpgradeAppearance(1);
        }
        CloseUpgradeUI();
        Debug.Log("New fire rate:" + fireRate);
        Debug.Log("New targeting range:" + targetingRange);
        Debug.Log("New upgrade cost:" + CalculateUpgradeCost());
    }

    public int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.6f));
    }

    public float CalculateFireRate()
    {
        return fireRate * Mathf.Pow(level, 0.3f);
    }

    public float CalculateTargetingRange()
    {
        return targetingRange * Mathf.Pow(level, 0.3f);
    }

    protected virtual void SellTower()
    {
        Level1Manager.main.IncreaseMoney(baseUpgradeCost/2);
        UpgradeAndSoldUIManager.main.SetHoveringState(false);
        Destroy(gameObject);
    }

    public virtual void UpgradeAppearance(int index)
    {
        GameObject upgradedTower = Instantiate(upgradedPrefab[index], transform.position, transform.rotation);
        upgradedTower.GetComponent<FrostTower>().level = this.level;
        upgradedTower.GetComponent<FrostTower>().fireRate = this.fireRate;
        upgradedTower.GetComponent<FrostTower>().targetingRange = this.targetingRange;
        upgradedTower.GetComponent<FrostTower>().baseUpgradeCost = this.baseUpgradeCost;
        towerPlot.SetTower(upgradedTower);
        
        Destroy(gameObject);
    }

    public virtual void SetTowerPlot(TowerPlot plot)
    {
        towerPlot = plot;
    }
}

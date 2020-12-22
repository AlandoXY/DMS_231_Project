using Alando.Features.Weapon;
using Assets.Features.BattleField;
using Assets.Features.BehaviorTrees.Nodes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] avaliableCovers;
    public PlayerInfoData playerInfo;

    public WeaponData WeaponController => weaponController;


    private Material material;
    private Transform bestCoverSpot;
    private BattlePoint curBattlePoint;
    private Transform curShootTarget;
    private NavMeshAgent agent;
    private WeaponData weaponController;

    private Node topNode;

    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponentInChildren<MeshRenderer>().material;
        weaponController = GetComponent<WeaponData>();
    }

    private void Start()
    {
        _currentHealth = startingHealth;
        ConstructBehahaviourTree();
    }

    private void ConstructBehahaviourTree()
    {
        /*
        IsCovereAvaliableNode coverAvaliableNode = new IsCovereAvaliableNode(avaliableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this, playerTransform);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });
        */


        IsPointAvailableNode pointAvaliableNode = new IsPointAvailableNode(curBattlePoint, this);
        GoToPointNode goToPointNode = new GoToPointNode(agent, this);
        IsOccupyPoint isOccupyPoint = new IsOccupyPoint(curBattlePoint.transform, transform);
        RangeNode shootingRangeNode = new RangeNode(weaponController.weapon.fireRange, curShootTarget, transform);
        ShootNode shootNode = new ShootNode(agent, this, curShootTarget);

        Sequence occupyPoint = new Sequence(new List<Node> { pointAvaliableNode, goToPointNode });


    }

    private void Update()
    {
        topNode.Evaluate();
        if (topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }
        currentHealth += Time.deltaTime * healthRestoreRate;
        //Debug.DrawRay(transform.position, playerTransform.position - transform.position);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }

        healthText.text = currentHealth.ToString();
        if (bestCoverSpot != null)
        {
            Debug.Log($"{bestCoverSpot.parent.name}.{bestCoverSpot.name}");
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }

    public void SetBestBattlePoint(BattlePoint battlePoint) => curBattlePoint = battlePoint;

    public BattlePoint GetCurBattlePoint() => curBattlePoint;
}

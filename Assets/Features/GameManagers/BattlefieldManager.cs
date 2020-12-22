using Assets.Features.BattleField;
using Assets.Features.Enemy;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.GameManagers
{
    public class BattlefieldManager : MonoBehaviour
    {
        public static BattlefieldManager instance;
        private void Awake() => instance = this;

        public int botNum = 20;

        [SerializeField] private Transform blueBotsParent;
        [SerializeField] private Transform redBotsParent;
        [SerializeField] private EnemyAi2 botObject;
        [SerializeField] private Transform blueTeamSpawnPoint;
        [SerializeField] private Transform redTeamSpawnPoint;

        private List<int> teamOwenPoints = new List<int>() { 0, 0 }; // red - blue
        private List<BattlePoint> battlePoints = new List<BattlePoint>();
        private List<PlayerInfoData> players = new List<PlayerInfoData>();
        private List<Transform> buildingCoverSpot = new List<Transform>();

        [Title("DEBUG")]
        [ReadOnly, SerializeField] private int battlepointCount;
        [ReadOnly, SerializeField] private int playerCount;
        [ReadOnly, SerializeField] private int coverSpotCount;

        private void Start()
        {
            SpawnAllBots();
        }

        private void Update()
        {
            battlepointCount = battlePoints.Count;
            playerCount = players.Count;
            coverSpotCount = buildingCoverSpot.Count;
        }


        public void UpdateBattlePoint(BattlePoint battlePoint)
        {
            BattlefieldUI.instance.UpdatePointsProgress(battlePoint);
            CheckBattlePoint(battlePoint);
            BattlefieldUI.instance.UpdateTeamCount(teamOwenPoints[0], teamOwenPoints[1]);

        }

        /// <summary>
        /// 检查本地是否有缓存
        /// </summary>
        /// <param name="point"></param>
        private void CheckBattlePoint(BattlePoint point)
        {
            for (int i = 0; i < teamOwenPoints.Count; i++)
            {
                teamOwenPoints[i] = 0;
            }
            if (!battlePoints.Contains(point))
            {
                battlePoints.Add(point);
            }
            foreach (var p in battlePoints)
            {
                if (p.CurBattlePointPercent == 1)
                {
                    switch (p.CurBattleSide)
                    {
                        case BattleSide.Red:
                            teamOwenPoints[0]++;
                            break;
                        case BattleSide.Blue:
                            teamOwenPoints[1]++;
                            break;
                    }
                }
            }
            for (int i = 0; i < teamOwenPoints.Count; i++)
            {
                teamOwenPoints[i] = Mathf.Clamp(teamOwenPoints[i], 0, BattlefieldUI.instance.pointNum);
            }
        }

        /// <summary>
        /// 查找范围内的玩家
        /// </summary>
        /// <param name="me">发出者</param>
        /// <param name="range">范围</param>
        /// <returns>输出范围内所有玩家</returns>
        public List<PlayerInfoData> GetPlayerNearMe(PlayerInfoData me, float range)
        {
            List<PlayerInfoData> playersNearMe = new List<PlayerInfoData>();
            if (players.Count == 0) return null;

            foreach (var player in players)
            {
                var distance = Vector3.Distance(me.transform.position, player.transform.position);
                if (distance <= range)
                {
                    if (player.BattleSide != me.BattleSide)
                    {
                        playersNearMe.Add(player);
                    }
                }
            }
            return playersNearMe;
        }



        public void AddPlayer(PlayerInfoData player)
        {
            if (players.Contains(player)) return;
            players.Add(player);
        }

        public void AddBattlePoint(BattlePoint bp)
        {
            if (battlePoints.Contains(bp)) return;
            battlePoints.Add(bp);
        }


        /// <summary>
        /// 寻找距离最近的点
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public BattlePoint GetBattlePoint(Transform player, PlayerInfoData playerInfo)
        {
            List<BattlePoint> checkPoints = new List<BattlePoint>();
            BattlePoint bestPoint = null;

            foreach (BattlePoint point in battlePoints)
            {
                if (point.CurBattleSide != playerInfo.BattleSide)
                {
                    checkPoints.Add(point);
                }
            }

            if (checkPoints.Count > 0)
            {
                bestPoint = checkPoints[Random.Range(0, checkPoints.Count - 1)];
            }
            else
            {
                bestPoint = battlePoints[Random.Range(0, battlePoints.Count - 1)];
            }

            return bestPoint;
        }

        /// <summary>
        /// 获取最近的建筑掩体点
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public Transform GetBuildingCoverSpot(Transform player)
        {
            float minDistance = Mathf.Infinity;
            Transform nearestSpot = buildingCoverSpot[0];
            foreach (Transform spot in buildingCoverSpot)
            {
                var distance = Vector3.Distance(player.position, spot.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestSpot = spot;
                }
            }

            return nearestSpot;
        }

        public void SignCoverSpot(Transform spot)
        {
            if (buildingCoverSpot.Contains(spot)) return;
            buildingCoverSpot.Add(spot);
        }


        private void SpawnAllBots()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < botNum; j++)
                {
                    EnemyAi2 newBot = Instantiate(botObject);
                    BattleSide side = BattleSide.Blue;
                    Vector3 spawnPos = blueTeamSpawnPoint.position;
                    Transform spawnParent = blueBotsParent;


                    if (i == 1)
                    {
                        spawnPos = redTeamSpawnPoint.position;
                        spawnParent = redBotsParent;
                        side = BattleSide.Red;
                    }

                    Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
                    newPos += new Vector3(Random.Range(0, 5f), 0f, Random.Range(0, 5f));
                    newPos.y = 2f;

                    newBot.transform.parent = spawnParent;
                    newBot.transform.position = newPos;
                    newBot.aiInfo.SetBattleSide(side);
                }
            }
        }
    }
}
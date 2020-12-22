using Assets.Features.GameManagers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Features.BattleField
{
    public enum BattleSide
    {
        Red = 0,
        Blue = 1
    }

    public class BattlePoint : MonoBehaviour
    {
        [SerializeField] private bool DEBUG_Button;
        public int Id;
        public BattleSide CurBattleSide { get; private set; }

        private float increasePoint = 0.025f;
        private float maxPoint = 100;

        /// <summary>
        /// (0,1)
        /// </summary>
        public float CurBattlePointPercent
        {
            get
            {
                float num, afterNum;
                if (battleProgress[CurBattleSide.GetHashCode()] == 200f)
                {
                    afterNum = 1f;
                }
                else
                {
                    num = battleProgress[CurBattleSide.GetHashCode()] / maxPoint;
                    afterNum = Mathf.Clamp(num, 0f, 1f);
                    //Debug.Log($"Pre: {num} | After: {afterNum}");
                }

                return afterNum;
            }
        }


        private Transform m_transform;
        private List<PlayerInfoData> players = new List<PlayerInfoData>(); // 当前在点内的玩家
        private List<float> battleProgress = new List<float>() { 0f, 0f }; // red - blue
        private int curSideNum; // 如果是蓝队则++，如果是红队则--。判断最后是正数还是负数。如果是正数则蓝队增加，如果是负数则红队增加

        private int redNum, blueNum;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            CurBattleSide = BattleSide.Red;
            BattlefieldManager.instance.AddBattlePoint(this);
        }

        /// <summary>
        /// 占点，并且判断当前点的阵营状态，如果跟在点内的人阵营一样，则增加点数， 如果不一样则减少当前点数，如果当前点数为0，则转换阵营。并向Manager请求更新UI
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public bool OccupiedPoint(BattleSide bs)
        {
            float curBattlePointFloat = battleProgress[CurBattleSide.GetHashCode()];
            if (CurBattleSide == bs)
            {
                /*
                              
                                if (curBattlePointFloat <= 200f) curBattlePointFloat += 1f;
                                else curBattlePointFloat = 200f;
                                */
                curBattlePointFloat = curBattlePointFloat <= maxPoint ? curBattlePointFloat += increasePoint : maxPoint;
            }
            else
            {
                curBattlePointFloat = curBattlePointFloat >= 0 ? curBattlePointFloat -= increasePoint : 0;
                if (curBattlePointFloat == 0)
                {
                    switch (CurBattleSide)
                    {
                        case BattleSide.Red:
                            CurBattleSide = BattleSide.Blue;
                            break;
                        case BattleSide.Blue:
                            CurBattleSide = BattleSide.Red;
                            break;
                    }
                }
            }

            battleProgress[CurBattleSide.GetHashCode()] = curBattlePointFloat;
            BattlefieldManager.instance.UpdateBattlePoint(this);
            return true;
        }


        // 检测点内的人
        private void OnTriggerStay(Collider other)
        {
            CheckPoint();
        }

        // 检测进入点的人
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player") || other.transform.CompareTag("AI"))
            {
                PlayerInfoData pInfoData = other.transform.GetComponent<PlayerInfoData>();
                if (pInfoData.health.CurHealth > 0)
                {
                    players.Add(pInfoData);
                }

                //Debug.Log($"Enter: {other.transform.tag} | {name}");
                //Debug.Log($"CurPlayerList count: {players.Count}");
            }
        }

        // 检测从点离开的人
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player") || other.transform.CompareTag("AI"))
            {
                PlayerInfoData pInfoData = other.transform.GetComponent<PlayerInfoData>();
                players.Remove(pInfoData);
                //Debug.Log($"Exit: {other.transform.tag} | {name}");
                //Debug.Log($"CurPlayerList count: {players.Count}");
            }
        }

        /// <summary>
        /// 检查点内是否有存活的人，并且判断存活的人里是否有不同阵营的。
        /// </summary>
        /// <param name="player"></param>
        private void CheckPoint()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].health.CurHealth <= 0)
                {
                    players.Remove(players[i]);
                    Debug.Log($"Remove {players[i].name} (dead)");
                    return;
                }

                if (players[i].BattleSide == BattleSide.Blue) blueNum++;
                if (players[i].BattleSide == BattleSide.Red) redNum++;
            }

            if (redNum == 0 && blueNum == 0) return;
            if (redNum == 0 || blueNum == 0)
            {
                //Debug.Log($"这个点位阵营： {players[0].BattleSide}");
                OccupiedPoint(players[0].BattleSide);
            }

            redNum = 0;
            blueNum = 0;
        }


        private void OnDrawGizmos()
        {
            if (DEBUG_Button)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(transform.localPosition, transform.localScale);
            }
        }
    }
}
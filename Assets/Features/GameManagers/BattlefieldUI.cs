using Assets.Features.BattleField;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Features.GameManagers
{
    public class BattlefieldUI : MonoBehaviour
    {
        [SerializeField] private List<BattlePointUIStatus> filledImgs = new List<BattlePointUIStatus>();
        [SerializeField] private TextMeshProUGUI blueTeamCountText, redTeamCountText;
        public int pointNum => filledImgs.Count;
        public static BattlefieldUI instance;

        private void Awake() => instance = this;

        /// <summary>
        /// 更新占点UI的状态
        /// </summary>
        /// <param name="bpData"></param>
        public void UpdatePointsProgress(BattlePoint battlePoint)
        {
            foreach (BattlePointUIStatus pointUi in filledImgs)
            {
                if (pointUi.id == battlePoint.Id)
                {
                    switch (battlePoint.CurBattleSide)
                    {
                        case BattleSide.Red:
                            pointUi.FillImage.color = new Color(255f / 255f, 0f / 255f, 31f / 255f, 255f / 255f);
                            break;
                        case BattleSide.Blue:
                            pointUi.FillImage.color = new Color(0f / 255f, 153f / 255f, 255f / 255f, 255f / 255f);
                            break;
                    }

                    pointUi.FillImage.fillAmount = battlePoint.CurBattlePointPercent;
                    //Debug.Log($"更新[{pointUi.displayName}]点位，预计进度: {battlePoint.CurBattlePointPercent}，实际：{pointUi.FillImage.fillAmount}");
                }
            }
        }

        public void UpdateTeamCount(int red, int blue)
        {
            blueTeamCountText.text = blue.ToString();
            redTeamCountText.text = red.ToString();
        }
    }
}
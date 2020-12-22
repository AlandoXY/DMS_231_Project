using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Features.BattleField
{
    public class BattlePointUIStatus : MonoBehaviour
    {
        public string displayName;
        public Image FillImage;
        public TextMeshProUGUI text;
        public int id;

        private void Start()
        {
            text.text = displayName;
        }
    }
}
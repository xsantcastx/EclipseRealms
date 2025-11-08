using EclipseRealms.Core.Player;
using TMPro;
using UnityEngine;

namespace EclipseRealms.UI.HUD
{
    public class PlayerHudController : MonoBehaviour
    {
        [SerializeField] private PlayerStatsComponent player;
        [SerializeField] private TMP_Text statsLabel;

        private void Update()
        {
            if (player == null || statsLabel == null)
            {
                return;
            }

            var stats = player.CurrentStats;
            statsLabel.text =
                $"STR {stats.Strength}\nINT {stats.Intelligence}\nDEX {stats.Dexterity}\n" +
                $"VIT {stats.Vitality}\nWIS {stats.Wisdom}\nLCK {stats.Luck}";
        }
    }
}

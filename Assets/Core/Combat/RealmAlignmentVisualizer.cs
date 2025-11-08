using EclipseRealms.Core.Player;
using UnityEngine;

namespace EclipseRealms.Core.Combat
{
    /// <summary>
    /// Applies simple color grading feedback based on the current realm alignment.
    /// </summary>
    public class RealmAlignmentVisualizer : MonoBehaviour
    {
        [SerializeField] private PlayerStatsComponent statsComponent;
        [SerializeField] private Renderer[] tintTargets;
        [SerializeField] private Color lightColor = new(0.85f, 0.9f, 1f);
        [SerializeField] private Color shadowColor = new(0.5f, 0.2f, 0.6f);
        [SerializeField] private Color neutralColor = Color.white;
        [SerializeField] private float lerpSpeed = 3f;

        private Color currentColor;

        private void Awake()
        {
            if (statsComponent == null)
            {
                statsComponent = GetComponent<PlayerStatsComponent>();
            }

            currentColor = neutralColor;
        }

        private void Update()
        {
            if (statsComponent == null || tintTargets == null || tintTargets.Length == 0)
            {
                return;
            }

            Color targetColor = statsComponent.AlignmentState.Alignment switch
            {
                RealmAlignment.Light => lightColor,
                RealmAlignment.Shadow => shadowColor,
                _ => neutralColor
            };

            currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpSpeed);
            foreach (Renderer renderer in tintTargets)
            {
                if (renderer != null && renderer.material.HasProperty("_Color"))
                {
                    renderer.material.color = currentColor;
                }
            }
        }
    }
}

using EclipseRealms.Core.Combat;
using UnityEngine;

namespace EclipseRealms.Items.Charms
{
    public enum CharmStability
    {
        Permanent,
        Ethereal
    }

    [CreateAssetMenu(menuName = "Eclipse Realms/Items/Charm")]
    public class CharmDefinition : ScriptableObject
    {
        [SerializeField] private string charmId;
        [SerializeField] private RealmAlignment alignmentBias = RealmAlignment.Neutral;
        [SerializeField] private CharmStability stability = CharmStability.Permanent;
        [SerializeField] private float realmShiftModifier = 5f;
        [SerializeField, TextArea] private string lightEffect;
        [SerializeField, TextArea] private string shadowEffect;

        public string CharmId => charmId;
        public RealmAlignment AlignmentBias => alignmentBias;
        public CharmStability Stability => stability;
        public float RealmShiftModifier => realmShiftModifier;
        public string LightEffect => lightEffect;
        public string ShadowEffect => shadowEffect;
    }
}

using UnityEngine;

namespace FlightSim
{
    [CreateAssetMenu(menuName = "Flight/Special Rank")]
    public class SpecialRank : ScriptableObject
    {

        public string Title => title;
        [SerializeField] string title;

        public Sprite TargetIndicator => targetIndicator;
        [SerializeField] Sprite targetIndicator;

        public Sprite OffScreenTargetIndicator => offScreenTargetIndicator;
        [SerializeField] Sprite offScreenTargetIndicator;

        public Material IndicatorMaterial => indicatorMaterial;
        [SerializeField] Material indicatorMaterial;

    }
}
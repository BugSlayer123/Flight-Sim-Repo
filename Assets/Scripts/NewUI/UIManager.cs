using System;
using UnityEngine;

namespace FlightSim.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] PaperPlane player;
        public Canvas Canvas => canvas;
        [SerializeField] Canvas canvas;
        public IndicatorImage IndicatorPrefab => indicatorPrefab;
        [SerializeField] IndicatorImage indicatorPrefab;
        public float MaximumIndicatorDistance => maximumIndicatorDistance;
        [SerializeField] float maximumIndicatorDistance = 1000;

        public Color EnemyColor => enemyColor;
        [SerializeField] Color enemyColor = Color.red;

        public Color AllyColor => allyColor;
        [SerializeField] Color allyColor = Color.cyan;

        public Color NeutralColor => neutralColor;
        [SerializeField] Color neutralColor = Color.yellow;

        public float ScreenEdgeOffset => screenEdgeOffset;
        [SerializeField] float screenEdgeOffset = 50;

        public static UIManager Instance;

        void Awake() => Instance = this;

        public Color GetColor(PaperPlane plane)
        {
            if (player.Squadron == plane.Squadron) return allyColor;
            if (player.Squadron.FriendlySquadrons.Contains(plane.Squadron)
                || plane.Squadron.FriendlySquadrons.Contains(player.Squadron)) return neutralColor;
            return enemyColor;
        }
        
    }
}
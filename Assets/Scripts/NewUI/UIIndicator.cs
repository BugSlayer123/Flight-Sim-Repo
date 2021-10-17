using UnityEngine;

namespace FlightSim.UI
{
    public class UIIndicator : MonoBehaviour
    {
        public PaperPlane Plane => plane;
        PaperPlane plane;

        Camera mainCamera;
        Canvas canvas;

        IndicatorImage indicator;

        float screenEdgeOffset;

        void Awake() => plane = GetComponent<PaperPlane>();

        void Start()
        {
            canvas = UIManager.Instance.Canvas;
            mainCamera = Camera.main;
            indicator = Instantiate(UIManager.Instance.IndicatorPrefab, canvas.transform);
            indicator.SetSprites(plane.SpecialRank, UIManager.Instance.GetColor(plane));
            screenEdgeOffset = UIManager.Instance.ScreenEdgeOffset;
        }

        void Update()
        {
            Vector3 indicatorPosition = mainCamera.WorldToScreenPoint(transform.position);

            if (InFrontOfCamera(indicatorPosition) && WithinViewport())
            {
                indicatorPosition.z = 0f;
                indicator.SetOnScreen();
            }
            else if (InFrontOfCamera(indicatorPosition))
            {
                indicatorPosition.z = 0f;
                indicatorPosition = GetScreenEdgePointToTarget(indicatorPosition);

                indicator.SetOffScreen();
            }
            else
            {
                indicatorPosition *= -1f;
                indicatorPosition.z = 0f;
                indicatorPosition = GetScreenEdgePointToTarget(indicatorPosition);

                indicator.SetOffScreen();
            }

            indicator.SetPosition(indicatorPosition);
        }

        bool InFrontOfCamera(Vector3 position) => position.z >= 0;

        bool WithinViewport()
        {
            Vector3 screenPos = mainCamera.WorldToViewportPoint(transform.position);
            return screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1;
        }

        Vector2 ScreenEdge() => new Vector2(
            Screen.width * 0.5f - screenEdgeOffset,
            Screen.height * 0.5f - screenEdgeOffset);


        Vector3 GetScreenEdgePointToTarget(Vector3 indicatorPosition)
        {
            Vector3 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            indicatorPosition -= screenCenter;

            float divX = ScreenEdge().x / Mathf.Abs(indicatorPosition.x);
            float divY = ScreenEdge().y / Mathf.Abs(indicatorPosition.y);

            indicatorPosition = divX < divY
                ? GetHorizontalEdgePosition(indicatorPosition)
                : GetVerticalEdgePosition(indicatorPosition);
            indicatorPosition += screenCenter;
            return indicatorPosition;
        }

        Vector3 GetVerticalEdgePosition(Vector3 indicatorPosition)
        {
            float angle = Vector3.SignedAngle(Vector3.up, indicatorPosition, Vector3.forward);

            indicatorPosition.y = Mathf.Sign(indicatorPosition.y) *
                                  ScreenEdge().y;
            indicatorPosition.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.y;
            return indicatorPosition;
        }

        Vector3 GetHorizontalEdgePosition(Vector3 indicatorPosition)
        {
            float angle = Vector3.SignedAngle(Vector3.right, indicatorPosition, Vector3.forward);
            indicatorPosition.x = Mathf.Sign(indicatorPosition.x) *
                                  ScreenEdge().x;
            indicatorPosition.y = Mathf.Tan(Mathf.Deg2Rad * angle) * indicatorPosition.x;
            return indicatorPosition;
        }
    }
}
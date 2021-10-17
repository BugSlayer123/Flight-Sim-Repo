using UnityEngine;

namespace FlightSim
{
    [RequireComponent(typeof(FlightControl), typeof(IController), typeof(Rigidbody))]
    public class PaperPlane : MonoBehaviour
    {
        public Squadron Squadron => squadron;
        [SerializeField] Squadron squadron;
        public int Rank => rank;
        [SerializeField] int rank;
        public SpecialRank SpecialRank => specialRank;
        [SerializeField] SpecialRank specialRank;

        public FlightControl FlightControl => flightControl;
        FlightControl flightControl;
        public IController IController => iController;
        IController iController;
        public Rigidbody Rigidbody => rigidbody;
        Rigidbody rigidbody;
        

        void Awake()
        {
            flightControl = GetComponent<FlightControl>();
            iController = GetComponent<IController>();
            rigidbody = GetComponent<Rigidbody>();
        }

        public static KDTree<PaperPlane> AllPlanes = new KDTree<PaperPlane>();

        void OnEnable() => AllPlanes.Add(this);
        void OnDisable() => AllPlanes.RemoveAll(p => p == this);


    }
}
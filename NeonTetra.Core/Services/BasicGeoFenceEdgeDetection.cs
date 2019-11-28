using NeonTetra.Contracts.Services;

namespace NeonTetra.Core.Services
{
    public class BasicGeoFenceEdgeDetection : IGeoFenceEdgeDetection
    {
        public Edges DetectedEdge(bool currentState, bool nextState)
        {
            if (currentState == false && nextState == true) return Edges.Leading;
            if (currentState == true && nextState == false) return Edges.Trailing;
            return Edges.None;
        }
    }
}
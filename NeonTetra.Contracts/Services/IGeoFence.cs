using System;
using System.Collections.Generic;
using System.Text;
using NeonTetra.Contracts.Logging;

namespace NeonTetra.Contracts.Services
{
    public enum Edges
    {
        Leading,   //The leading edge (or front edge) is the first edge of the pulse.
        Trailing,   //The trailing edge (or back edge) is the second edge of the pulse.
        None
    }

    public interface IGeoFenceEdgeDetection
    {
        Edges DetectedEdge(bool currentState, bool nextState);
    }

    public interface IGeoFenceDetection
    {
        bool IsPointInFence(ILocation point, IGeoFence fence);
    }

    public interface IGeoFence
    {
        IList<ILocation> Fence { get; set; }
    }

    public interface ILocation
    {
        IGeoPoint Lat { get; set; }
        IGeoPoint Lon { get; set; }
    }

    public interface IGeoPoint
    {
        double Point { get; set; }
    }
}
using System;
using System.Linq.Expressions;

namespace NeonTetra.Contracts.Services
{
    public interface IScheduler
    {
        string Enqueue(Expression<Action> methodCall);
    }
}
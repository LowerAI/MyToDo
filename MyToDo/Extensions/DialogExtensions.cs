using MyToDo.Common.Events;

using Prism.Events;

using System;

namespace MyToDo.Extensions;

public static class DialogExtensions
{
    public static void UpdateLoading(this IEventAggregator aggregator, UpdateModel model)
    {
        aggregator.GetEvent<UpdateLoadingEvent>().Publish(model);
    }

    public static void Register(this IEventAggregator aggregator, Action<UpdateModel> action)
    {
        aggregator.GetEvent<UpdateLoadingEvent>().Subscribe(action);
    }
}
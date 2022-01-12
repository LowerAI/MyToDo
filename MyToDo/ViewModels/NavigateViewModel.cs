﻿using MyToDo.Extensions;

using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MyToDo.ViewModels;

public class NavigateViewModel : BindableBase, INavigationAware
{
    private readonly IContainerProvider _containerProvider;
    private readonly IEventAggregator _aggregator;

    public NavigateViewModel(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
        _aggregator = containerProvider.Resolve<IEventAggregator>();
    }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
        
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
        
    }

    public virtual void UpdateLoading(bool IsOpen)
    {
        _aggregator.UpdateLoading(new Common.Events.UpdateModel
        {
            IsOpen = IsOpen
        });
    }
}
using Caliburn.Micro;
using DesktopUI.EventModels;
using DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly IEventAggregator _events;
        private readonly SalesViewModel _salesViewModel;
        private ILoggedInUserModel _user;

        public bool IsLoggedIn {  
            get
            {
                return !string.IsNullOrEmpty(_user.Token);
            }
        }

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel, ILoggedInUserModel user)
        {
            _events = events;
            _salesViewModel = salesViewModel;
            _user = user;
            _events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
 
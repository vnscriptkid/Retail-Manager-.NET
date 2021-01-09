using Caliburn.Micro;
using DesktopUI.Library.Api;
using DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private BindingList<UserModel> _users;
        private IWindowManager _window;
        private IUserEndpoint _userEndpoint;
        private StatusInfoViewModel _statusViewModel;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set { 
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserDisplayViewModel(IWindowManager window, IUserEndpoint userEndpoint, StatusInfoViewModel statusViewModel)
        {
            _window = window;
            _userEndpoint = userEndpoint;
            _statusViewModel = statusViewModel;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    _statusViewModel.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales Form.");
                    _window.ShowDialog(_statusViewModel, null, settings);
                }
            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }
    }
}

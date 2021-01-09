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

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { 
                _selectedUser = value;
                SelectedUsername = value.Email;
                CurrentRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                AvailableRoles = new BindingList<string>(GetAvailableRolesToAdd());
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private IList<string> GetAvailableRolesToAdd()
        {
            List<string> output = new List<string>();

            foreach (var role in AllRoles)
            {
                if (!CurrentRoles.Contains(role))
                {
                    output.Add(role);
                }
            }

            return output;
        }

        private string _selectedUsername;

        public string SelectedUsername
        {
            get { return _selectedUsername; }
            set { 
                _selectedUsername = value;
                NotifyOfPropertyChange(() => SelectedUsername);
            }
        }

        private BindingList<string> _currentRoles = new BindingList<string>();

        public BindingList<string> CurrentRoles
        {
            get { return _currentRoles; }
            set
            {
                _currentRoles = value;
                NotifyOfPropertyChange(() => CurrentRoles);
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
                await LoadRoles();
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

        private BindingList<string> _allRoles = new BindingList<string>();

        public BindingList<string> AllRoles
        {
            get { return _allRoles; }
            set { _allRoles = value; }
        }

        private async Task LoadRoles()
        {
            var roleList = await _userEndpoint.GetAllRoles();
            AllRoles = new BindingList<string>(roleList.Select(role => role.Value).ToList());
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }

        private BindingList<string> _availableRoles;

        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set { 
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        private string _selectedAvailableRole;

        public string SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set { 
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }

        private string _selectedCurrentRole;

        public string SelectedCurrentRole
        {
            get { return _selectedCurrentRole; }
            set { 
                _selectedCurrentRole = value;
                NotifyOfPropertyChange(() => SelectedCurrentRole);
            }
        }

        public async Task AddSelectedRole()
        {
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

            CurrentRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
        }

        public async Task RemoveSelectedRole()
        {
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedCurrentRole);

            CurrentRoles.Remove(SelectedCurrentRole);
            AvailableRoles.Add(SelectedCurrentRole);
        }
    }
}

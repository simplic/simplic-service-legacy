using CommonServiceLocator;
using Simplic.Framework.UI;
using Simplic.Group;
using Simplic.Icon;
using Simplic.User;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace Simplic.Authorization.UI
{
    public class AccessRightsEditorViewModel : ExtendableViewModel
    {
        #region Private Readonly Fields
        private readonly IIconService iconService;
        private readonly IGroupService groupService;
        private readonly IUserService userService;
        private readonly IAuthorizationService authorizationService;
        private readonly string tableName;
        private readonly string idColName;
        private readonly object rowId;
        #endregion

        #region Private Fields
        private string allowedSearchText;
        private string availableSearchText;
        private int? ownerId;

        private CollectionViewSource availableViewSource;
        private ObservableCollection<AccessRightItemViewModel> availableCollection;

        private CollectionViewSource allowedViewSource;
        private ObservableCollection<AccessRightItemViewModel> allowedCollection;
        #endregion

        #region Constructor
        public AccessRightsEditorViewModel(string tableName, string idColName, object rowId)
        {
            availableCollection = new ObservableCollection<AccessRightItemViewModel>();
            allowedCollection = new ObservableCollection<AccessRightItemViewModel>();

            this.tableName = tableName;
            this.idColName = idColName;
            this.rowId = rowId;

            iconService = ServiceLocator.Current.GetInstance<IIconService>();
            groupService = ServiceLocator.Current.GetInstance<IGroupService>();
            userService = ServiceLocator.Current.GetInstance<IUserService>();
            authorizationService = ServiceLocator.Current.GetInstance<IAuthorizationService>();

            var groups = groupService.GetAllSortedByName();
            var users = userService.GetAllSorted();
            var accessRights = authorizationService.GetAccessRights(tableName, idColName, rowId);

            availableViewSource = new CollectionViewSource();
            availableViewSource.Source = availableCollection;
            availableViewSource.Filter += AvailableViewSource_Filter;

            allowedViewSource = new CollectionViewSource();
            allowedViewSource.Source = allowedCollection;
            allowedViewSource.Filter += AllowedViewSource_Filter;

            GenerateAvailableCollection(groups, users);
            GenerateAllowedCollection(groups, users, accessRights);

            OwnerId = accessRights.OwnerId;

            IsDirty = false;
        }
        #endregion

        #region Private Methods

        #region [GenerateAllowedCollection]
        /// <summary>
        /// Creates viewmodels for allowed collection
        /// </summary>
        /// <param name="groups">Available groups</param>
        /// <param name="users">Available users</param>
        /// <param name="accessRights">AccessRights object for given row</param>
        private void GenerateAllowedCollection(IEnumerable<Group.Group> groups, IEnumerable<User.User> users, RowAccess accessRights)
        {
            // add full access groups
            foreach (var id in accessRights.GroupFullAccess)
            {
                var item = groups.Where(x => x.Ident == id).FirstOrDefault();
                if (item == null) continue;

                allowedCollection.Add(new AccessRightItemViewModel(GetAccessRightItemForGroup(item, AccessRightType.Full), iconService));

                // remove from available collection
                availableCollection.Remove(availableCollection.Where(x => x.ItemType == AccessRightItemType.Group && x.Id == id).FirstOrDefault());
            }

            // add write groups
            foreach (var id in accessRights.GroupWriteAccess.Except(accessRights.GroupFullAccess))
            {
                var item = groups.Where(x => x.Ident == id).FirstOrDefault();
                if (item == null) continue;

                allowedCollection.Add(new AccessRightItemViewModel(GetAccessRightItemForGroup(item, AccessRightType.Write), iconService));

                // remove from available collection
                availableCollection.Remove(availableCollection.Where(x => x.ItemType == AccessRightItemType.Group && x.Id == id).FirstOrDefault());
            }

            // add read groups
            foreach (var id in accessRights.GroupReadAccess.Except(accessRights.GroupWriteAccess))
            {
                var item = groups.Where(x => x.Ident == id).FirstOrDefault();
                if (item == null) continue;

                allowedCollection.Add(new AccessRightItemViewModel(GetAccessRightItemForGroup(item, AccessRightType.Read), iconService));

                // remove from available collection
                availableCollection.Remove(availableCollection.Where(x => x.ItemType == AccessRightItemType.Group && x.Id == id).FirstOrDefault());
            }

            // add full access users
            foreach (var id in accessRights.UserFullAccess)
            {
                var item = users.Where(x => x.Ident == id).FirstOrDefault();
                if (item == null) continue;

                allowedCollection.Add(new AccessRightItemViewModel(GetAccessRightItemForUser(item, AccessRightType.Full), iconService));

                // remove from available collection
                availableCollection.Remove(availableCollection.Where(x => x.ItemType == AccessRightItemType.User && x.Id == id).FirstOrDefault());
            }

            // add write users
            foreach (var id in accessRights.UserWriteAccess.Except(accessRights.UserFullAccess))
            {
                var item = users.Where(x => x.Ident == id).FirstOrDefault();
                if (item == null) continue;

                allowedCollection.Add(new AccessRightItemViewModel(GetAccessRightItemForUser(item, AccessRightType.Write), iconService));

                // remove from available collection
                availableCollection.Remove(availableCollection.Where(x => x.ItemType == AccessRightItemType.User && x.Id == id).FirstOrDefault());
            }

            // add read users
            foreach (var id in accessRights.UserReadAccess.Except(accessRights.UserWriteAccess))
            {
                var item = users.Where(x => x.Ident == id).FirstOrDefault();
                if (item == null) continue;

                allowedCollection.Add(new AccessRightItemViewModel(GetAccessRightItemForUser(item, AccessRightType.Read), iconService));

                // remove from available collection
                availableCollection.Remove(availableCollection.Where(x => x.ItemType == AccessRightItemType.User && x.Id == id).FirstOrDefault());
            }
        }
        #endregion

        #region [GenerateAvailableCollection]
        /// <summary>
        /// Creates viewmodels for the available collection
        /// </summary>
        /// <param name="groups">Available groups</param>
        /// <param name="users">Available users</param>
        private void GenerateAvailableCollection(IEnumerable<Group.Group> groups, IEnumerable<User.User> users)
        {
            foreach (var group in groups)
            {
                availableCollection.Add(new AccessRightItemViewModel(
                    new AccessRightItem
                    {
                        Id = group.Ident,
                        DisplayText = group.Name,
                        ItemType = AccessRightItemType.Group,
                        RightType = AccessRightType.Read
                    }, iconService));
            }

            foreach (var user in users)
            {
                availableCollection.Add(new AccessRightItemViewModel(
                    GetAccessRightItemForUser(user, AccessRightType.Read),
                    iconService));
            }
        }
        #endregion

        #region [GetAccessRightItemForUser]
        /// <summary>
        /// Creates a new <see cref="AccessRightItem"/> object
        /// </summary>
        /// <param name="user">User model</param>
        /// <param name="type">Access right type</param>
        /// <returns><see cref="AccessRightItem"/> object created</returns>
        private AccessRightItem GetAccessRightItemForUser(User.User user, AccessRightType type)
        {
            return new AccessRightItem
            {
                Id = user.Ident,
                DisplayText = $"{user.FirstName} {user.LastName} ({user.UserName})",
                ItemType = AccessRightItemType.User,
                RightType = type
            };
        }
        #endregion

        #region [GetAccessRightItemForGroup]
        /// <summary>
        /// Creates a new <see cref="AccessRightItem"/> for a group
        /// </summary>
        /// <param name="group">Group model</param>
        /// <param name="type">Access right type</param>
        /// <returns><see cref="AccessRightItem"/> object created</returns>
        private AccessRightItem GetAccessRightItemForGroup(Group.Group group, AccessRightType type)
        {
            return new AccessRightItem
            {
                Id = group.Ident,
                DisplayText = group.Name,
                ItemType = AccessRightItemType.Group,
                RightType = type
            };
        }
        #endregion

        #region [AvailableViewSource_Filter]
        /// <summary>
        /// Event handle for available view source filter event. Called when the search text is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AvailableViewSource_Filter(object sender, FilterEventArgs e)
        {
            var access = e.Item as AccessRightItemViewModel;

            if (access != null &&
                access.DisplayText != null &&
                AvailableSearchText != null)
            {
                e.Accepted = access.DisplayText.ToLower().Contains(AvailableSearchText.ToLower());
            }
        }
        #endregion

        #region [AllowedViewSource_Filter]
        /// <summary>
        /// Event handle for allowed view source filter event. Called when the search text is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllowedViewSource_Filter(object sender, FilterEventArgs e)
        {
            var access = e.Item as AccessRightItemViewModel;

            if (access != null &&
                access.DisplayText != null &&
                AllowedSearchText != null)
            {
                e.Accepted = access.DisplayText.ToLower().Contains(AllowedSearchText.ToLower());
            }
        }
        #endregion

        #endregion

        #region Public Methods

        #region [Save]
        /// <summary>
        /// Saves the access rights
        /// </summary>
        public void Save()
        {
            var groupReadIds = allowedCollection.Where(x => x.ItemType == AccessRightItemType.Group && x.RightType == AccessRightType.Read).Select(x => x.Id).ToList();
            var userReadIds = allowedCollection.Where(x => x.ItemType == AccessRightItemType.User && x.RightType == AccessRightType.Read).Select(x => x.Id).ToList();

            var groupWriteIds = allowedCollection.Where(x => x.ItemType == AccessRightItemType.Group && x.RightType == AccessRightType.Write).Select(x => x.Id).ToList();
            var userWriteIds = allowedCollection.Where(x => x.ItemType == AccessRightItemType.User && x.RightType == AccessRightType.Write).Select(x => x.Id).ToList();

            var groupFullIds = allowedCollection.Where(x => x.ItemType == AccessRightItemType.Group && x.RightType == AccessRightType.Full).Select(x => x.Id).ToList();
            var userFullIds = allowedCollection.Where(x => x.ItemType == AccessRightItemType.User && x.RightType == AccessRightType.Full).Select(x => x.Id).ToList();

            // if full access is given, give read and write access manually
            if (groupFullIds.Count() > 0)
            {
                groupReadIds.AddRange(groupFullIds.Except(groupReadIds));
                groupWriteIds.AddRange(groupFullIds.Except(groupWriteIds));
            }

            // if write access is given, give read access manually
            if (groupWriteIds.Count() > 0)
            {
                groupReadIds.AddRange(groupWriteIds.Except(groupReadIds));
            }

            // if full access is given, give read and write access manually
            if (userFullIds.Count() > 0)
            {
                userReadIds.AddRange(userFullIds.Except(userReadIds));
                userWriteIds.AddRange(userFullIds.Except(userWriteIds));
            }

            // if write access is given, give read access manually
            if (userWriteIds.Count() > 0)
            {
                userReadIds.AddRange(userWriteIds.Except(userReadIds));
            }

            var newAccess = new RowAccess
            {
                OwnerId = OwnerId,
                GroupFullAccess = groupFullIds,
                GroupReadAccess = groupReadIds,
                GroupWriteAccess = groupWriteIds,
                UserFullAccess = userFullIds,
                UserReadAccess = userReadIds,
                UserWriteAccess = userWriteIds
            };

            var result = authorizationService.SetAccess(tableName, idColName, rowId, newAccess);
            IsDirty = false;
        }  
        #endregion

        #endregion

        #region Public Properties

        #region [OwnerId]
        /// <summary>
        /// Gets or sets the owner id
        /// </summary>
        public int? OwnerId
        {
            get { return ownerId; }
            set { PropertySetter(value, (newValue) => { ownerId = newValue; }); }
        }
        #endregion

        #region [AvailableSearchText]
        /// <summary>
        /// Gets or sets the search text for filtering the available list
        /// </summary>
        public string AvailableSearchText
        {
            get { return availableSearchText; }
            set
            {
                PropertySetter(value, (newValue) => { availableSearchText = newValue; });
                this.AvailableViewSource.View.Refresh();
            }
        }
        #endregion

        #region [AllowedSearchText]
        /// <summary>
        /// Gets or sets the search text for filtering the allowed (permissions) list
        /// </summary>
        public string AllowedSearchText
        {
            get { return allowedSearchText; }
            set
            {
                PropertySetter(value, (newValue) => { allowedSearchText = newValue; });
                this.AllowedViewSource.View.Refresh();
            }
        }
        #endregion

        #region [AvailableViewSource]
        /// <summary>
        /// Gets or sets the available list view source for filtering
        /// </summary>
        public CollectionViewSource AvailableViewSource
        {
            get { return availableViewSource; }
            set { availableViewSource = value; }
        }
        #endregion

        #region [AllowedViewSource]
        /// <summary>
        /// Gets or sets the allowed list view source for filtering
        /// </summary>
        public CollectionViewSource AllowedViewSource
        {
            get { return allowedViewSource; }
            set { allowedViewSource = value; }
        }  
        #endregion

        #endregion
    }
}
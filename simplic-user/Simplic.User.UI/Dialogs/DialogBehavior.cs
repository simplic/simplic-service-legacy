using Simplic.Framework.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Simplic.User.UI
{
    public static class DialogBehavior
    {
        private static Dictionary<IDialogViewModel, Window> DialogBoxes = new Dictionary<IDialogViewModel, Window>();
        private static Dictionary<Window, NotifyCollectionChangedEventHandler> ChangeNotificationHandlers = new Dictionary<Window, NotifyCollectionChangedEventHandler>();
        private static Dictionary<ObservableCollection<IDialogViewModel>, List<IDialogViewModel>> DialogBoxViewModels = new Dictionary<ObservableCollection<IDialogViewModel>, List<IDialogViewModel>>();

        public static readonly DependencyProperty ClosingProperty = DependencyProperty.RegisterAttached(
            "Closing",
            typeof(bool),
            typeof(DialogBehavior),
            new PropertyMetadata(false));

        public static readonly DependencyProperty ClosedProperty = DependencyProperty.RegisterAttached(
            "Closed",
            typeof(bool),
            typeof(DialogBehavior),
            new PropertyMetadata(false));

        public static readonly DependencyProperty DialogViewModelsProperty = DependencyProperty.RegisterAttached(
            "DialogViewModels",
            typeof(object),
            typeof(DialogBehavior),
            new PropertyMetadata(null, OnDialogViewModelsChange));

        public static void SetDialogViewModels(DependencyObject source, object value)
        {
            source.SetValue(DialogViewModelsProperty, value);
        }

        public static object GetDialogViewModels(DependencyObject source)
        {
            return source.GetValue(DialogViewModelsProperty);
        }

        private static void OnDialogViewModelsChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parent = d as Window;
            if (parent == null)
                return;
            parent.Closed += (s, a) => ChangeNotificationHandlers.Remove(parent);

            if (!ChangeNotificationHandlers.ContainsKey(parent))
                ChangeNotificationHandlers[parent] = (sender, args) =>
                {
                    var collection = sender as ObservableCollection<IDialogViewModel>;
                    if (collection != null)
                    {
                        if (args.Action == NotifyCollectionChangedAction.Add ||
                            args.Action == NotifyCollectionChangedAction.Remove ||
                            args.Action == NotifyCollectionChangedAction.Replace)
                        {
                            if (args.NewItems != null)
                                foreach (IDialogViewModel viewModel in args.NewItems)
                                {
                                    if (!DialogBoxViewModels.ContainsKey(collection))
                                        DialogBoxViewModels[collection] = new List<IDialogViewModel>();
                                    DialogBoxViewModels[collection].Add(viewModel);
                                    AddDialog(viewModel, collection, d as Window);
                                }
                            if (args.OldItems != null)
                                foreach (IDialogViewModel viewModel in args.OldItems)
                                {
                                    RemoveDialog(viewModel);
                                    DialogBoxViewModels[collection].Remove(viewModel);
                                    if (DialogBoxViewModels[collection].Count == 0)
                                        DialogBoxViewModels.Remove(collection);
                                }
                        }
                        else if (args.Action == NotifyCollectionChangedAction.Reset)
                        {
                            if (DialogBoxViewModels.ContainsKey(collection))
                            {
                                var viewModels = DialogBoxViewModels[collection];
                                foreach (var viewModel in DialogBoxViewModels[collection])
                                    RemoveDialog(viewModel);
                                DialogBoxViewModels.Remove(collection);
                            }
                        }
                    }
                };

            var newCollection = e.NewValue as ObservableCollection<IDialogViewModel>;
            if (newCollection != null)
            {
                newCollection.CollectionChanged += ChangeNotificationHandlers[parent];
                foreach (IDialogViewModel viewModel in newCollection.ToList())
                    AddDialog(viewModel, newCollection, d as Window);
            }

            var oldCollection = e.OldValue as ObservableCollection<IDialogViewModel>;
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= ChangeNotificationHandlers[parent];
                foreach (IDialogViewModel viewModel in oldCollection.ToList())
                    RemoveDialog(viewModel);
            }
        }

        private static void AddDialog(IDialogViewModel viewModel, ObservableCollection<IDialogViewModel> collection, Window owner)
        {
            var resource = owner.TryFindResource(viewModel.GetType());
            if (resource == null)
                return;

            if (resource is DefaultRibbonWindow dialog)
            {
                var userViewModel = viewModel as IUserDialogViewModel;
                if (userViewModel == null)
                    return;
                dialog.DataContext = userViewModel;
                DialogBoxes[userViewModel] = dialog;
                userViewModel.DialogClosing += (sender, args) =>
                    collection.Remove(sender as IUserDialogViewModel);
                dialog.Closing += (sender, args) =>
                {
                    if (!(bool)dialog.GetValue(ClosingProperty))
                    {
                        dialog.SetValue(ClosingProperty, true);
                        userViewModel.RequestClose();
                        if (!(bool)dialog.GetValue(ClosedProperty))
                        {
                            args.Cancel = true;
                            dialog.SetValue(ClosingProperty, false);
                        }
                    }
                };
                dialog.Closed += (sender, args) =>
                {
                    Debug.Assert(DialogBoxes.ContainsKey(userViewModel));
                    DialogBoxes.Remove(userViewModel);
                    return;
                };
                dialog.Owner = owner;
                if (userViewModel.IsModal)
                    dialog.ShowDialog();
                else
                    dialog.Show();
            }
        }

        private static void RemoveDialog(IDialogViewModel viewModel)
        {
            if (DialogBoxes.ContainsKey(viewModel))
            {
                var dialog = DialogBoxes[viewModel];
                if (!(bool)dialog.GetValue(ClosingProperty))
                {
                    dialog.SetValue(ClosingProperty, true);
                    DialogBoxes[viewModel].Close();
                }
                dialog.SetValue(ClosedProperty, true);
            }
        }


        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }
    }
}

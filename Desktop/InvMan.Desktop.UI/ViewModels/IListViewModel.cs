using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InvMan.Desktop.UI.ViewModels
{
    public interface IListViewModel<TModel>
    {
        ObservableCollection<TModel> Items { get; }

        public IEnumerable<TModel> ItemsCache { get; set; }

        public abstract TModel SelectedItem { get; set; }

        public bool AreItemsLoaded { get; set; }

        public bool AreThereItems { get; set; }

        public string NoItemsMessage { get; set; }

        void LoadItemsFromList(IEnumerable<TModel> items);

        void InitializeList();
    }
}

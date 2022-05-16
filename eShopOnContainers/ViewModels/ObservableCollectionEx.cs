using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace eShopOnContainers.ViewModels
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        public ObservableCollectionEx() : base()
        {
        }

        public ObservableCollectionEx(IEnumerable<T> collection) : base(collection)
        {
        }

        public ObservableCollectionEx(List<T> list) : base(list)
        {
        }

        public void ReloadData(IEnumerable<T> items)
        {
            ReloadData(
                innerList =>
                {
                    foreach (var item in items)
                    {
                        innerList.Add(item);
                    }
                });
        }

        // TODO: This will fix data reloading
        //public void ReloadData(Action<IList<T>> innerListAction)
        //{
        //    Items.Clear();

        //    innerListAction(Items);

        //    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        //    this.OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
        //    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        //}

        //public async Task ReloadDataAsync(Func<IList<T>, Task> innerListAction)
        //{
        //    Items.Clear();

        //    await innerListAction(Items);

        //    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        //    this.OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
        //    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        //}

        //These are broken...
        public void ReloadData(Action<IList<T>> innerListAction)
        {
            this.Clear();

            innerListAction(this);
        }

        public async Task ReloadDataAsync(Func<IList<T>, Task> innerListAction)
        {
            this.Clear();

            await innerListAction(this);
        }
    }
}


using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfApp2
{
    public class Node : INotifyPropertyChanged
    {
        public Node()
        {
            isItemExpanded = true;
        }
        private string treeValue;
        public string TreeValue
        {
            get { return treeValue; }
            set
            {
                treeValue = value;
                OnPropertyChanged();
            }
        }

        private bool isItemExpanded;
        public bool IsItemExpanded
        {
            get { return isItemExpanded; }
            set
            {
                isItemExpanded = value;
                OnPropertyChanged();
            }
        }

        private Visibility itemVisibility;
        public Visibility ItemVisibility
        {
            get { return itemVisibility; }
            set
            {
                itemVisibility = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Node> _nodeList = new ObservableCollection<Node>();
        public ObservableCollection<Node> NodeList
        {
            get { return _nodeList; }
            set
            {
                if (_nodeList == value) return;
                _nodeList = value;
                if (this.PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NodeList"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

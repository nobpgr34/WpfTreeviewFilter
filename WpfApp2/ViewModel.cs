using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class ViewModel
    {
        public ObservableCollection<Node> BinaryTree { get; set; }
        public Node root { get; set; }

        public ViewModel()
        {
            BinaryTree = new ObservableCollection<Node>();
            root = new Node();
            root.TreeValue = "worker";
            var n = new Node() { TreeValue = "john smith" };
            root.NodeList.Add(n);
            var nn = new Node() { TreeValue = "emily brown" };
            root.NodeList.Add(nn);
            var root1 = new Node() { TreeValue = "client" };
            var nnnn = new Node() { TreeValue = "peter johanson" };
            root1.NodeList.Add(nnnn);
            BinaryTree.Add(root);
            BinaryTree.Add(root1);
        }
    }
}

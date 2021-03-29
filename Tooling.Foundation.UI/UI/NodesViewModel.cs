using System.Collections.Generic;
using Tooling.UI;

namespace Tooling.Foundation.UI
{
    public abstract class NodesViewModel<T, TV> : NodesViewModel<TV> 
        where TV : ViewModel

    {
        public T Model { get; protected set; }

        protected NodesViewModel()
        { }

        protected NodesViewModel(T model)
        {
            Model = model;
        }
    }

    public abstract class NodesViewModel<TV> : NodeViewModel
        where TV : ViewModel
    {
        public abstract IEnumerable<TV> Nodes { get; }
    }
}
using System.Threading.Tasks;

namespace Tooling.Foundation.UI
{
    public delegate Task CommandTaskDelegate();


    public delegate Task CommandTaskDelegate<in T>(T parameter);
}
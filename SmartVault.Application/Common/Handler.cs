using System.Threading.Tasks;

namespace SmartVault.Application.Common
{
    public abstract class Handler<T>
    {
        protected Handler<T>? Successor;

        public Handler<T> SetSuccessor(Handler<T> successor)
        {
            Successor = successor;
            return successor;
        }

        public abstract Task Process(T request);
    }

}

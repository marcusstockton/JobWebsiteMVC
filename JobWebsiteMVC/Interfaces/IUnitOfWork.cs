using System.Threading.Tasks;

namespace JobWebsiteMVC.Interfaces
{
    public interface IUnitOfWork
    {
        IJobRepository Jobs { get; }

        Task CompleteAsync();
    }
}

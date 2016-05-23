using System;
using System.Threading.Tasks;
using RevStack.Notification;
using RevStack.Mvc;

namespace RevStack.Admin
{
    public interface IAdminAlertMessageService<TKey>
    {
        AlertMessage<TKey> Get(NotifyAlert<TKey> entity, UriUtility uri);
        Task<AlertMessage<TKey>> GetAsync(NotifyAlert<TKey> entity, UriUtility uri);
    }
}

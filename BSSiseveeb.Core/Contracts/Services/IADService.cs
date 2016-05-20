using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace BSSiseveeb.Core.Contracts.Services
{
    public interface IADService : IApplicationService
    {
        Task<IPagedCollection<IUser>> GetUsers();
        Task<string> GetTokenForApplication();
        Task<AuthenticationResult> ADStartUp(AuthorizationCodeReceivedNotification context, string authority);
    }
}
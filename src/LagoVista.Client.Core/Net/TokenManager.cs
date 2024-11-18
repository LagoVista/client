using LagoVista.Core.Interfaces;
using LagoVista.Core.Networking.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Net
{
    public class TokenManager : ITokenManager
    {
        public Task<bool> ValidateTokenAsync(IAuthManager authManager, CancellationTokenSource cancellationTokenSource = null)
        {


            return Task.FromResult(true);
        }
    }
}

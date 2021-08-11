using System.Threading.Tasks;

namespace TradeReports.UI.Contracts.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle();

        Task HandleAsync();
    }
}

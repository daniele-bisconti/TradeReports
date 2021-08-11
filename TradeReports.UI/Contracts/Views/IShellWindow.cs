using System.Windows.Controls;

namespace TradeReports.UI.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}

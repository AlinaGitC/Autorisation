using Autorisation.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Autorisation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var blockTime = BlockState.GetBlockTime();
            if (blockTime.HasValue && blockTime.Value <= DateTime.Now)
            {
                BlockState.ClearBlockTime();
            }

        }
    }

}

using System.Globalization;
using System.Threading;
using System.Windows;

namespace NetCoreTetris
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetCulture("bg-BG");
        }

        private void SetCulture(string cultureString)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureString);
        }
    }
}

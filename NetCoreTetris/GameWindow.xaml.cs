using System.Windows;

namespace NetCoreTetris
{
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            DataContext = new GameViewModel();
        }
    }
}

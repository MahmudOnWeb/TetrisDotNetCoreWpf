using System.Windows.Media;

namespace NetCoreTetris.Model
{
    internal class Cell
    {
        internal Brush Brush { get; set; }
        internal Brush BorderBrush { get; set; }
        internal double Opacity { get; set; }
        internal bool Shown { get; set; }
    }
}

using NetCoreTetris.Model;
using System.Windows.Media;

namespace NetCoreTetris
{
    public class CellViewModel : ViewModelBase
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private Cell cell;
#pragma warning restore IDE0044 // Add readonly modifier

        internal CellViewModel(Cell cell)
        {
            this.cell = cell;
        }

        public Brush Brush
        {
            get { return cell.Brush; }
            set
            {
                cell.Brush = value;
                OnPropertyChanged("Brush");
            }
        }

        public Brush BorderBrush
        {
            get { return cell.BorderBrush; }
            set
            {
                cell.BorderBrush = value;
                OnPropertyChanged("BorderBrush");
            }
        }

        public double Opacity
        {
            get { return cell.Opacity; }
            set
            {
                cell.Opacity = value;
                OnPropertyChanged("Opacity");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace NetCoreTetris.Model
{
    internal class Board
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private Random random = new Random();
#pragma warning restore IDE0044 // Add readonly modifier
        private Figure figure, previewFigure;
        private const int ROWS = 20, COLUMNS = 10;

        internal Board()
        {
            ClearBrush = Brushes.White;
            ClearBorderBrush = Brushes.White;
            Opacity = 1.0;
            BorderBrush = Brushes.Black;
            Cells = new Cell[ROWS * COLUMNS];
            PreviewCells = new Cell[4 * 4];

            for(int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new Cell() { Shown = false, Brush = ClearBrush, BorderBrush = ClearBorderBrush, Opacity = Opacity };
                if(i < 16)
                {
                    PreviewCells[i] = new Cell() { Shown = false, Brush = ClearBrush, BorderBrush = ClearBorderBrush, Opacity = Opacity };
                }
            }

            MakePreview();
            RunNewFigure();
        }

        internal int[] Current { get; private set; }
        internal int[] Old { get; private set; }
        internal int[] Animated { get; private set; }

        internal Cell[] Cells { get; private set;}
        internal Cell[] PreviewCells { get; private set; }

        internal Brush Brush { get; private set; }
        internal Brush PreviewBrush { get; private set; }
        internal Brush BorderBrush { get; private set; }
        internal double Opacity { get; private set; }
        internal Brush ClearBrush { get; private set; }
        internal Brush ClearBorderBrush { get; private set; }
        internal GameState State { get; set; }

        internal void RunNewFigure()
        {
            figure = previewFigure;
            Brush = PreviewBrush;

            Current = figure.Current;
            Old = figure.Old;

            foreach(int index in figure.Current)
            {
                if(Cells[index].Shown)
                {
                    State = GameState.GameOverAnimation;
                    return;
                }
            }

            State = GameState.Move;
            MakePreview();
        }

        internal void MoveFigureLeft()
        {
            SimpleFigureTransform(() => figure.MoveLeft());
        }

        internal void MoveFigureRight()
        {
            SimpleFigureTransform(() => figure.MoveRight());
        }

        internal void MoveFigureDown()
        {
            int[] temp = new int[figure.Current.Length];
            figure.Current.CopyTo(temp, 0);
            figure.MoveDown();
            bool cancel = VerifyMovement();

            if(cancel)
            {
                figure.Current = temp;
                foreach(int figureIndex in figure.Current)
                {
                    Cells[figureIndex].Shown = true;
                    Cells[figureIndex].Brush = Brush;
                    Cells[figureIndex].Opacity = Opacity;
                    Cells[figureIndex].BorderBrush = BorderBrush;
                }

                Current = figure.Current;
                State = GameState.Placed;
            }
            else
            {
                Current = figure.Current;
                Old = figure.Old;
                State = GameState.Move;
            }
        }

        internal void MoveFigureFullDown()
        {
            MoveFigureDown();
            if(State == GameState.Move)
            {
                State = GameState.MoveFullDown;
            }
        }

        internal void RotateFigure(bool counterClockwise = true)
        {
            SimpleFigureTransform(() => figure.Rotate(counterClockwise));
        }

        private void SimpleFigureTransform(Action a)
        {
            int[] temp = new int[figure.Current.Length];
            FigureStates tempState = figure.State;

            figure.Current.CopyTo(temp, 0);

            a();
            bool cancel = VerifyMovement();

            if (cancel)
            {
                figure.Current = temp;
                figure.State = tempState;
            }
            else
            {
                Current = figure.Current;
                Old = figure.Old;
                State = GameState.Move;
            }
        }

        internal void AnimateFullRows()
        {
            List<int> rowsToAnimate = new List<int>();
            for (int row = 0; row < ROWS; row++)
            {
                int[] fullRow = new int[COLUMNS];
                bool rowFull = true;
                int fullRowIndex = 0;
                for (int col = row * COLUMNS; col < (row + 1) * COLUMNS; col++)
                {
#pragma warning disable IDE0054 // Use compound assignment
                    rowFull = rowFull & Cells[col].Shown;
#pragma warning restore IDE0054 // Use compound assignment
                    fullRow[fullRowIndex] = col;
                    fullRowIndex++;
                }

                if (rowFull)
                {
                    rowsToAnimate.AddRange(fullRow);
                }
            }

            Animated = rowsToAnimate.ToArray();
        }

        internal void ClearRows()
        {
            int rows = Animated.Length / COLUMNS;
            for(int i = 0; i < rows; i++)
            {
                MoveRowDown(Animated[i * COLUMNS]);
            }

            State = GameState.RowsRemoved;
        }

        private void MakePreview()
        {
            foreach(Cell cell in PreviewCells)
            {
                cell.Shown = false;
            }

            int flip = random.Next(0, 22);
            switch(flip)
            {
                case 0:
                case 8:
                case 15:
                    previewFigure = new Line();
                    PreviewBrush = Brushes.Red;
                    break;
                case 1:
                case 9:
                case 16:
                    previewFigure = new Square();
                    PreviewBrush = Brushes.Blue;
                    break;
                case 2:
                case 10:
                case 17:
                    previewFigure = new Triangle();
                    PreviewBrush = Brushes.Orange;
                    break;
                case 3:
                case 11:
                case 18:
                    previewFigure = new LeftWinkel();
                    PreviewBrush = Brushes.Magenta;
                    break;
                case 4:
                case 12:
                case 19:
                    previewFigure = new RightWinkel();
                    PreviewBrush = Brushes.LightSlateGray;
                    break;
                case 5:
                case 13:
                case 20:
                    previewFigure = new LeftSkew();
                    PreviewBrush = Brushes.LightBlue;
                    break;
                case 6:
                case 14:
                case 21:
                    previewFigure = new RightSkew();
                    PreviewBrush = Brushes.Green;
                    break;
                case 7:
                    previewFigure = new DotSquare();
                    PreviewBrush = Brushes.Purple;
                    break;
                default:
                    break;
            }

            foreach(int index in previewFigure.Current)
            {
                Tuple<int, int> bigBoardrc = GetRowAndColumn(index);
                int previewIndex = bigBoardrc.Item1 * 4 + (bigBoardrc.Item2 - 3);
                PreviewCells[previewIndex].Brush = PreviewBrush;
                PreviewCells[previewIndex].BorderBrush = BorderBrush;
                PreviewCells[previewIndex].Shown = true;
            }
        }

        private bool VerifyMovement()
        {
            bool cancel = false;
            foreach(int figureIndex in figure.Current)
            {
                if(figureIndex < 0)
                {
                    cancel = true;
                    break;
                }
            }

            if(cancel == false)
            {
                foreach(int figureIndex in figure.Current)
                {
                    if(Cells[figureIndex].Shown)
                    {
                        cancel = true;
                        break;
                    }
                }
            }

            return cancel;
        }

        private void MoveRowDown(int firstRowIndex)
        {
            for(int i = (firstRowIndex + COLUMNS -1); i > (COLUMNS -1); i--)
            {
                Cells[i] = Cells[i - COLUMNS];
            }
            MakeFirstRowClear();
        }

        private void MakeFirstRowClear()
        {
            for(int i = 0; i < COLUMNS;i++)
            {
                Cells[i] = new Cell() { Shown = false, Brush = ClearBrush, BorderBrush = ClearBorderBrush, Opacity = Opacity };
            }
        }

        internal static int MoveCellLeft(int index)
        {
            Tuple<int, int> rc = GetRowAndColumn(index);
            if(rc.Item2 == 0)
            {
                return -1;
            }
            else
            {
                return GetIndex(rc.Item1, rc.Item2 - 1);
            }
        }

        internal static int MoveCellRight(int index)
        {
            Tuple<int, int> rc = GetRowAndColumn(index);
            if(rc.Item2 == (COLUMNS -1))
            {
                return -1;
            }
            else
            {
                return GetIndex(rc.Item1, rc.Item2 + 1);
            }
        }

        internal static int MoveCellDown(int index)
        {
            Tuple<int, int> rc = GetRowAndColumn(index);
            if(rc.Item1 == (ROWS - 1))
            {
                return -1;
            }
            else
            {
                return GetIndex(rc.Item1 + 1, rc.Item2);
            }
        }

        internal static int MoveCellUp(int index)
        {
            Tuple<int, int> rc = GetRowAndColumn(index);
            return GetIndex(rc.Item1 - 1, rc.Item2);
        }

        internal static Tuple<int,int> GetRowAndColumn(int index)
        {
            if(index < 0)
            {
                return new Tuple<int, int>(-1, -1);
            }
            else
            {
                int row = index / COLUMNS;
                int column = index % COLUMNS;
                return new Tuple<int, int>(row, column);
            }
        }

        internal static int GetIndex(int row, int column)
        {
            if(row < 0 || column < 0)
            {
                return -1;
            }
            else
            {
                return row * COLUMNS + column;
            }
        }
    }
}

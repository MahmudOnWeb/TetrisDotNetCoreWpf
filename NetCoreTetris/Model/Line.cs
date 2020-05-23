using System;

namespace NetCoreTetris.Model
{
    internal class Line : Figure
    {
        internal Line()
        {
            Old = new int[0];
            Current = new int[4];
            State = FigureStates.A;
            Current[0] = Board.GetIndex(0, 5);
            Current[1] = Board.GetIndex(1, 5);
            Current[2] = Board.GetIndex(2, 5);
            Current[3] = Board.GetIndex(3, 5);
        }

        internal override void MoveLeft()
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            switch (State)
            {
                case FigureStates.A:
                    Current[0] = Board.MoveCellLeft(temp[0]);
                    Current[1] = Board.MoveCellLeft(temp[1]);
                    Current[2] = Board.MoveCellLeft(temp[2]);
                    Current[3] = Board.MoveCellLeft(temp[3]);
                    Old = new int[4];
                    Old[0] = temp[0];
                    Old[1] = temp[1];
                    Old[2] = temp[2];
                    Old[3] = temp[3];
                    break;
                case FigureStates.B:
                    Current[0] = Board.MoveCellLeft(temp[0]);
                    Current[1] = temp[0];
                    Current[2] = temp[1];
                    Current[3] = temp[2];
                    Old = new int[1];
                    Old[0] = temp[3];
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal override void MoveRight()
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            switch (State)
            {
                case FigureStates.A:
                    Current[0] = Board.MoveCellRight(temp[0]);
                    Current[1] = Board.MoveCellRight(temp[1]);
                    Current[2] = Board.MoveCellRight(temp[2]);
                    Current[3] = Board.MoveCellRight(temp[3]);
                    Old = new int[4];
                    Old[0] = temp[0];
                    Old[1] = temp[1];
                    Old[2] = temp[2];
                    Old[3] = temp[3];
                    break;
                case FigureStates.B:
                    Current[0] = temp[1];
                    Current[1] = temp[2];
                    Current[2] = temp[3];
                    Current[3] = Board.MoveCellRight(temp[3]);
                    Old = new int[1];
                    Old[0] = temp[0];
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal override void MoveDown()
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            switch (State)
            {
                case FigureStates.A:
                    Current[0] = temp[1];
                    Current[1] = temp[2];
                    Current[2] = temp[3];
                    Current[3] = Board.MoveCellDown(temp[3]);
                    Old = new int[1];
                    Old[0] = temp[0];
                    break;
                case FigureStates.B:
                    Current[0] = Board.MoveCellDown(temp[0]);
                    Current[1] = Board.MoveCellDown(temp[1]);
                    Current[2] = Board.MoveCellDown(temp[2]);
                    Current[3] = Board.MoveCellDown(temp[3]);
                    Old = new int[4];
                    Old[0] = temp[0];
                    Old[1] = temp[1];
                    Old[2] = temp[2];
                    Old[3] = temp[3];
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal override void MoveUp()
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            switch (State)
            {
                case FigureStates.A:
                    Current[0] = Board.MoveCellUp(temp[0]);
                    Current[1] = temp[0];
                    Current[2] = temp[1];
                    Current[3] = temp[2];
                    Old = new int[1];
                    Old[0] = temp[3];
                    break;
                case FigureStates.B:
                    Current[0] = Board.MoveCellUp(temp[0]);
                    Current[1] = Board.MoveCellUp(temp[1]);
                    Current[2] = Board.MoveCellUp(temp[2]);
                    Current[3] = Board.MoveCellUp(temp[3]);
                    Old = new int[4];
                    Old[0] = temp[0];
                    Old[1] = temp[1];
                    Old[2] = temp[2];
                    Old[3] = temp[3];
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal override void Rotate(bool counterClockWise = true)
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            switch (State)
            {
                case FigureStates.A:
                    Current[0] = Board.MoveCellDown(Board.MoveCellLeft(temp[0]));
                    Current[1] = temp[1];
                    Current[2] = Board.MoveCellUp(Board.MoveCellRight(temp[2]));
                    Current[3] = Board.MoveCellUp(Board.MoveCellUp(Board.MoveCellRight(Board.MoveCellRight(temp[3]))));
                    Old = new int[3];
                    Old[0] = temp[0];
                    Old[1] = temp[2];
                    Old[2] = temp[3];
                    State = FigureStates.B;
                    break;
                case FigureStates.B:
                    Current[0] = Board.MoveCellRight(Board.MoveCellUp(temp[0]));
                    Current[1] = temp[1];
                    Current[2] = Board.MoveCellLeft(Board.MoveCellDown(temp[2]));
                    Current[3] = Board.MoveCellLeft(Board.MoveCellLeft(Board.MoveCellDown(Board.MoveCellDown(temp[3]))));
                    Old = new int[3];
                    Old[0] = temp[0];
                    Old[1] = temp[2];
                    Old[2] = temp[3];
                    State = FigureStates.A;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

using System;

namespace NetCoreTetris.Model
{
    internal class LeftSkew : Figure
    {
        internal LeftSkew()
        {
            Old = new int[0];
            Current = new int[4];
            State = FigureStates.A;
            Current[0] = Board.GetIndex(1, 4);
            Current[1] = Board.GetIndex(2, 4);
            Current[2] = Board.GetIndex(0, 5);
            Current[3] = Board.GetIndex(1, 5);
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
                    Current[3] = temp[0];
                    Old = new int[3];
                    Old[0] = temp[1];
                    Old[1] = temp[2];
                    Old[2] = temp[3];
                    break;
                case FigureStates.B:
                    Current[0] = Board.MoveCellLeft(temp[0]);
                    Current[1] = temp[0];
                    Current[2] = Board.MoveCellLeft(temp[2]);
                    Current[3] = temp[2];
                    Old = new int[2];
                    Old[0] = temp[1];
                    Old[1] = temp[3];
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
                    Current[0] = temp[3];
                    Current[1] = Board.MoveCellRight(temp[1]);
                    Current[2] = Board.MoveCellRight(temp[2]);
                    Current[3] = Board.MoveCellRight(temp[3]);
                    Old = new int[3];
                    Old[0] = temp[0];
                    Old[1] = temp[1];
                    Old[2] = temp[2];
                    break;
                case FigureStates.B:
                    Current[0] = temp[1];
                    Current[1] = Board.MoveCellRight(temp[1]);
                    Current[2] = temp[3];
                    Current[3] = Board.MoveCellRight(temp[3]);
                    Old = new int[2];
                    Old[0] = temp[0];
                    Old[1] = temp[2];
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
                    Current[1] = Board.MoveCellDown(temp[1]);
                    Current[2] = temp[3];
                    Current[3] = Board.MoveCellDown(temp[3]);
                    Old = new int[2];
                    Old[0] = temp[0];
                    Old[1] = temp[2];
                    break;
                case FigureStates.B:
                    Current[0] = Board.MoveCellDown(temp[0]);
                    Current[1] = Board.MoveCellDown(temp[1]);
                    Current[2] = Board.MoveCellDown(temp[2]);
                    Current[3] = temp[0];
                    Old = new int[3];
                    Old[0] = temp[1];
                    Old[1] = temp[2];
                    Old[2] = temp[3];
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal override void MoveUp()
        {
            throw new NotImplementedException();
        }

        internal override void Rotate(bool counterClockWise)
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);

            switch (State)
            {
                case FigureStates.A:
                    Current[0] = Board.MoveCellDown(temp[3]);
                    Current[1] = Board.MoveCellDown(Board.MoveCellRight(temp[3]));
                    Current[2] = temp[0];
                    Current[3] = temp[3];
                    Old = new int[2];
                    Old[0] = temp[1];
                    Old[1] = temp[2];
                    State = FigureStates.B;
                    break;
                case FigureStates.B:
                    Current[0] = temp[2];
                    Current[1] = Board.MoveCellDown(temp[2]);
                    Current[2] = Board.MoveCellUp(temp[3]);
                    Current[3] = temp[3];
                    Old = new int[2];
                    Old[0] = temp[0];
                    Old[1] = temp[1];
                    State = FigureStates.A;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

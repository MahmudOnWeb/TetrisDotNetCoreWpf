using System;

namespace NetCoreTetris.Model
{
    internal class Square : Figure
    {
        internal Square()
        {
            Old = new int[0];
            Current = new int[4];
            Current[0] = Board.GetIndex(0, 5);
            Current[1] = Board.GetIndex(1, 5);
            Current[2] = Board.GetIndex(0, 6);
            Current[3] = Board.GetIndex(1, 6);

            State = FigureStates.A;
        }

        internal override void MoveLeft()
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            Current[0] = Board.MoveCellLeft(temp[0]);
            Current[1] = Board.MoveCellLeft(temp[1]);
            Current[2] = temp[0];
            Current[3] = temp[1];
            Old = new int[2];
            Old[0] = temp[2];
            Old[1] = temp[3];
        }

        internal override void MoveRight()
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            Current[0] = temp[2];
            Current[1] = temp[3];
            Current[2] = Board.MoveCellRight(temp[2]);
            Current[3] = Board.MoveCellRight(temp[3]);
            Old = new int[2];
            Old[0] = temp[0];
            Old[1] = temp[1];
        }

        internal override void MoveDown()
        {
            int[] temp = new int[4];
            Current.CopyTo(temp, 0);
            Current[0] = temp[1];
            Current[1] = Board.MoveCellDown(temp[1]);
            Current[2] = temp[3];
            Current[3] = Board.MoveCellDown(temp[3]);
            Old = new int[2];
            Old[0] = temp[0];
            Old[1] = temp[2];
        }

        internal override void MoveUp()
        {
            //int[] temp = new int[4];
            //Current.CopyTo(temp, 0);
            //Current[0] = Board.MoveCellUp(temp[0]);
            //Current[1] = temp[0];
            //Current[2] = Board.MoveCellUp(temp[2]);
            //Current[3] = temp[2];
            //Old = new int[2];
            //Old[0] = temp[1];
            //Old[1] = temp[3];
            throw new NotImplementedException();
        }

        internal override void Rotate(bool counterClockWise)
        {

        }
    }
}

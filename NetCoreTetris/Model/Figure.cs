namespace NetCoreTetris.Model
{
    internal abstract class Figure
    {
        internal int[] Old { get; set; }
        internal int[] Current { get; set; }

        internal abstract void MoveLeft();
        internal abstract void MoveRight();
        internal abstract void MoveDown();
        internal abstract void MoveUp();
        internal abstract void Rotate(bool counterClockwise);

        internal FigureStates State { get; set; }
    }
}

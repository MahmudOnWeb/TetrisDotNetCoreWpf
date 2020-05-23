namespace NetCoreTetris
{
    internal enum GameState
    {
        Idle = 0,
        Move = 1,
        MoveFullDown = 2,
        Placed = 3,
        Animation = 4,
        RowsRemoved = 5,
        GameOverAnimation = 6,
        GameOverClearAnimation = 7,
        GameOver = 8
    }
}
using NetCoreTetris.Model;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace NetCoreTetris
{
    public class GameViewModel : ViewModelBase
    {
        private string message, totalScoreMessage, instantScoreMessage, levelMessage, startGameButtonLabel, soundOnOffButtonLabel;
        private CellViewModel[] cells, previewCells;
        private Board board;
#pragma warning disable IDE0044 // Add readonly modifier
        private DispatcherTimer timer;
#pragma warning restore IDE0044 // Add readonly modifier
        private Moves requestedMove;
        private FlowState flowState = FlowState.Running;
        private int rowsAnimationTicks = 0, speedTicks = 0, gameOverAnimationTicks = 0;
        private Brush animationBrush = Brushes.Black, instantScoreBrush = Brushes.DarkOrange;
        private Score score;

        public GameViewModel()
        {
            Message = "Service message";
            GameInputCommand = new SimpleCommand(GameMoveInput, CanUseGame);
            PauseResumeGameCommand = new SimpleCommand(PauseResumeGame);
            StartGameCommand = new SimpleCommand(StartGame, CanStartGame);
            MuteCommand = new SimpleCommand(Mute, CanMute);
            Cells = new CellViewModel[200];
            PreviewCells = new CellViewModel[16];
            StartGameButtonLabel = Localization.StartGame;

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new CellViewModel(new Cell());
                if(i < 16)
                {
                    PreviewCells[i] = new CellViewModel(new Cell());
                }
            }

            timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(10) };
            timer.Tick += OnTimerTick;
        }

        public ICommand GameInputCommand { get; private set; }
        public ICommand PauseResumeGameCommand { get; private set; }
        public ICommand StartGameCommand { get; private set; }
        public ICommand MuteCommand { get; private set; }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public string StartGameButtonLabel
        {
            get { return startGameButtonLabel; }
            set
            {
                startGameButtonLabel = value;
                OnPropertyChanged("StartGameButtonLabel");
            }
        }

        public CellViewModel[] Cells
        {
            get { return cells; }
            set
            {
                cells = value;
                OnPropertyChanged("Cells");
            }
        }

        public CellViewModel[] PreviewCells
        {
            get { return previewCells; }
            set
            {
                previewCells = value;
                OnPropertyChanged("PreviewCells");
            }
        }

        public string TotalScoreMessage
        {
            get { return totalScoreMessage; }
            set
            {
                totalScoreMessage = value;
                OnPropertyChanged("TotalScoreMessage");
            }
        }

        public string InstantScoreMessage
        {
            get { return instantScoreMessage; }
            set
            {
                instantScoreMessage = value;
                OnPropertyChanged("InstantScoreMessage");
            }
        }

        public string LevelMessage
        {
            get { return levelMessage; }
            set
            {
                levelMessage = value;
                OnPropertyChanged("LevelMessage");
            }
        }

        public Brush InstantScoreBrush
        {
            get { return instantScoreBrush; }
            set
            {
                instantScoreBrush = value;
                OnPropertyChanged("InstantScoreBrush");
            }
        }

        public string SoundOnOffButtonLabel
        {
            get { return soundOnOffButtonLabel; }
            set
            {
                soundOnOffButtonLabel = value;
                OnPropertyChanged("SoundOnOffButtonLabel");
            }
        }

        private bool CanStartGame(object parameter = null)
        {
            return (board == null);
        }

        private void StartGame(object parameter = null)
        {
            board = new Board();
            board.RunNewFigure();
            timer.Start();

            DisplayPreview();
            score = new Score();
            requestedMove = Moves.None;
            SoundOnOffButtonLabel = Localization.Mute;
        }

        private bool CanUseGame(object parameter = null)
        {
            return (board != null);
        }

        private bool CanMute(object paramter = null)
        {
            return (board != null);
        }

        private void Mute(object parameter = null)
        {
            if(score.SoundState == SoundState.On)
            {
                score.SoundState = SoundState.Off;
                SoundOnOffButtonLabel = Localization.Unmute;
            }
            else
            {
                score.SoundState = SoundState.On;
                SoundOnOffButtonLabel = Localization.Mute;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            switch(board.State)
            {
                case GameState.GameOverAnimation:
                    StartGameButtonLabel = Localization.GameOver;
                    AnimateBoardCellByCellDescending(Brushes.AliceBlue, Brushes.Purple, GameState.GameOverClearAnimation);
                    return;
                case GameState.GameOverClearAnimation:
                    AnimateBoardCellByCellDescending(board.ClearBrush, board.ClearBorderBrush, GameState.GameOver);
                    return;
                case GameState.GameOver:
                    timer.Stop();
                    Message = Localization.GameOver;
                    board = null;
                    TotalScoreMessage = string.Empty;
                    LevelMessage = Localization.PlayGame;
                    StartGameButtonLabel = Localization.PlayGame;
                    return;
                case GameState.RowsRemoved:
                    for (int index = 0; index < board.Cells.Length; index++)
                    {
                        if (board.Cells[index].Shown)
                        {
                            Cells[index].Brush = board.Cells[index].Brush;
                            Cells[index].Opacity = board.Cells[index].Opacity;
                            Cells[index].BorderBrush = board.Cells[index].BorderBrush;
                        }
                        else
                        {
                            Cells[index].Brush = board.ClearBrush;
                            Cells[index].Opacity = board.Opacity;
                            Cells[index].BorderBrush = board.ClearBorderBrush;
                        }
                    }

                    board.RunNewFigure();
                    DisplayPreview();
                    return;
                case GameState.Animation:
                    AnimateRowsRemoval();
                    return;
                case GameState.Move:
                    DrawFigure();
                    break;
                case GameState.MoveFullDown:
                    DrawFigure();
                    board.MoveFigureFullDown();
                    return;
                case GameState.Placed:
                    board.AnimateFullRows();
                    if (board.Animated.Length > 0)
                    {
                        score.CalculateRows(board.Animated.Length);
                        board.State = GameState.Animation;
                    }
                    else
                    {
                        board.RunNewFigure();
                        DisplayPreview();
                        score.PlaceFigure();
                        TotalScoreMessage = string.Format("{0:### ### #00} ", score.Total);
                        InstantScoreMessage = string.Empty;
                        LevelMessage = string.Format(Localization.Level, score.Level);
                        return;
                    }

                    break;
                default:
                    break;
            }

            ProcessUserInput();
        }

        private void ProcessUserInput()
        {
            Message = string.Format("User: {0} State: {1}", requestedMove.ToString(), board.State.ToString());
            switch (requestedMove)
            {
                case Moves.Down:
                    board.MoveFigureDown();
                    break;
                case Moves.Rotate:
                    board.RotateFigure(true);
                    break;
                case Moves.Left:
                    board.MoveFigureLeft();
                    break;
                case Moves.Right:
                    board.MoveFigureRight();
                    break;
                case Moves.FullDown:
                    board.MoveFigureFullDown();
                    break;
                case Moves.None:
                    if (speedTicks > Math.Max(2, 100 - score.Level * 2))
                    {
                        board.MoveFigureDown();
                        speedTicks = -1;
                    }
                    break;
                default:
                    break;
            }
            speedTicks++;

            requestedMove = Moves.None;
        }

        private void DrawFigure()
        {
            foreach (int index in board.Old)
            {
                Cells[index].Brush = board.ClearBrush;
                Cells[index].Opacity = board.Opacity;
                Cells[index].BorderBrush = board.ClearBorderBrush;
            }

            foreach (int index in board.Current)
            {
                Cells[index].Brush = board.Brush;
                Cells[index].Opacity = board.Opacity;
                Cells[index].BorderBrush = board.BorderBrush;
            }

            board.State = GameState.Idle;
        }

        private void AnimateRowsRemoval()
        {
            if (rowsAnimationTicks > 80)
            {
                rowsAnimationTicks = 0;
                board.ClearRows();
                TotalScoreMessage = string.Format("{0:### ### #00} ", score.Total);
                LevelMessage = string.Format(Localization.Level, score.Level);
            }
            else
            {
                InstantScoreMessage = string.Format("{0:### 000}", score.Instant);

                if (rowsAnimationTicks % 5 == 0)
                {
                    if (animationBrush.Equals(Brushes.Red))
                    {
                        animationBrush = Brushes.Blue;
                        InstantScoreBrush = Brushes.LightBlue;
                    }
                    else
                    {
                        animationBrush = Brushes.Red;
                        InstantScoreBrush = Brushes.OrangeRed;
                    }
                }

                foreach (int index in board.Animated)
                {
                    Cells[index].Brush = animationBrush;
                }
            }

            rowsAnimationTicks++;
        }

        private void AnimateBoardCellByCellDescending(Brush brush, Brush border, GameState exitState)
        {
            if(gameOverAnimationTicks > (Cells.Length-1))
            {
                gameOverAnimationTicks = 0;
                board.State = exitState;
            }
            else
            {
                Cells[199 - gameOverAnimationTicks].Brush = brush;
                Cells[199 - gameOverAnimationTicks].BorderBrush = border;
                gameOverAnimationTicks++;
            }
        }

        private void DisplayPreview()
        {
            for (int index = 0; index < board.PreviewCells.Length; index++)
            {
                if (board.PreviewCells[index].Shown)
                {
                    PreviewCells[index].Brush = board.PreviewCells[index].Brush;
                    PreviewCells[index].BorderBrush = board.PreviewCells[index].BorderBrush;
                }
                else
                {
                    PreviewCells[index].Brush = board.ClearBrush;
                    PreviewCells[index].BorderBrush = board.ClearBorderBrush;
                }
            }
        }

        private void GameMoveInput(object parameter)
        {
            requestedMove = (Moves)parameter;
        }

        private void PauseResumeGame(object parameter = null)
        {
            switch (flowState)
            {
                case FlowState.Running:
                    timer.Stop();
                    flowState = FlowState.Pause;
                    Message = Localization.GamePaused;
                    break;
                case FlowState.Pause:
                default:
                    timer.Start();
                    flowState = FlowState.Running;
                    Message = Localization.GameResumed;
                    break;
            }
        }
    }
}

using System;
using System.IO;
using System.Media;

namespace NetCoreTetris.Model
{
    internal class Score
    {
#pragma warning disable IDE0052 // Remove unread private members
        private int placedFigures, pointsToLevel;
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning disable IDE0044 // Add readonly modifier
        private Random random;
#pragma warning restore IDE0044 // Add readonly modifier

        internal Score()
        {
            random = new Random();
            placedFigures = 0;
            pointsToLevel = 0;
            Total = 0;
            Level = 0;
            Instant = 0;
        }

        internal void LevelUp()
        {
            Level++;
            PlaySound(0);
        }

        internal void PlaceFigure()
        {
            placedFigures++;
            Total += 10 + Level;
            PlaySound(random.Next(1, 5));
        }

        internal void CalculateRows(int rows)
        {
            int basis = 10 + Level;
            switch (rows)
            {
                case 10:
                    Instant = basis * 10;
                    pointsToLevel += 1; // 10/1 = 10
                    PlaySound(5);
                    break;
                case 20:
                    Instant = basis * 25;
                    pointsToLevel += 2; // 25/2 = 12,5
                    PlaySound(5);
                    break;
                case 30:
                    Instant = basis * 45;
                    pointsToLevel += 3; // 45/3 = 15
                    PlaySound(5);
                    break;
                case 40:
                    Instant = basis * 100;
                    pointsToLevel += 4; // 100/4 = 25
                    PlaySound(6);
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (pointsToLevel > 40)
            {
                pointsToLevel = 0;
                LevelUp();
            }

            Total += Instant;
        }

        internal int Total { get; private set; }
        internal int Instant { get; private set; }
        internal int Level { get; private set; }
        internal SoundState SoundState { get; set; }


        internal void PlaySound(int choice)
        {
            if(SoundState == SoundState.Off)
            {
                return;
            }

            Stream stream;
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (choice)
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
                case 0:
                    stream = GameResources.levelup;
                    break;
                case 1:
                    stream = GameResources.put01;
                    break;
                case 2:
                    stream = GameResources.put02;
                    break;
                case 3:
                    stream = GameResources.put03;
                    break;
                case 4:
                    stream = GameResources.put04;
                    break;
                case 5:
                    stream = GameResources.row01;
                    break;
                case 6:
                    stream = GameResources.row02;
                    break;
                default:
                    stream = null;
                    break;
            }

            SoundPlayer sound = new SoundPlayer(stream);
            sound.Play();
        }

    }
}

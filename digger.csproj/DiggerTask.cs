using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    //Напишите здесь классы Player, Terrain и другие.
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public int GetDrawingPriority() => 0;

        public string GetImageFileName() => "Terrain.png";
    }

    public class Player : ICreature
    {
        public static int XPosition = 0;
        public static int YPosition = 0;

        private bool CanMove(int x, int y)
        {
            return Game.Map[x, y] == null || !(Game.Map[x, y] is Sack);
        }

        public CreatureCommand Act(int x, int y)
        {
            XPosition = x;
            YPosition = y;

            Keys key = Game.KeyPressed;

            if (y < Game.MapHeight - 1 && CanMove(x, y + 1) && key == Keys.Down)
                return new CreatureCommand { DeltaY = 1 };
            if (y >= 1 && CanMove(x, y - 1) && key == Keys.Up)
                return new CreatureCommand { DeltaY = -1 };
            if (x < Game.MapWidth - 1 && CanMove(x + 1, y) && key == Keys.Right)
                return new CreatureCommand { DeltaX = 1 };
            if (x >= 1 && CanMove(x - 1, y) && key == Keys.Left)
                return new CreatureCommand { DeltaX = -1 };

            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Gold)
                Game.Scores += 10;

            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority() => 1;

        public string GetImageFileName() => "Digger.png";
    }

    public class Sack : ICreature
    {
        private int countOfFall = 0;

        public CreatureCommand Act(int x, int y)
        {
            int below = Game.MapHeight - 1;

            while (y != below)
            {
                if (Game.Map[x, y + 1] == null || ((Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster) && countOfFall > 0))
                {
                    countOfFall++;
                    return new CreatureCommand { DeltaY = 1 };
                }
                else if (countOfFall > 1)
                    return new CreatureCommand { TransformTo = new Gold() };
                countOfFall = 0;
                return new CreatureCommand();
            }

            if (countOfFall > 1)
                return new CreatureCommand { TransformTo = new Gold() };
            countOfFall = 0;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public int GetDrawingPriority() => 2;

        public string GetImageFileName() => "Sack.png";
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject) => true;

        public int GetDrawingPriority() => 3;

        public string GetImageFileName() => "Gold.png";
    }

    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            int xTo = 0;
            int yTo = 0;

            if (FindPlayer())
            {
                if (Player.XPosition == x)
                {
                    if (Player.YPosition < y) yTo = -1;
                    else if (Player.YPosition > y) yTo = 1;
                }
                else if (Player.YPosition == y)
                {
                    if (Player.XPosition < x) xTo = -1;
                    else if (Player.XPosition > x) xTo = 1;
                }
                else
                {
                    if (Player.XPosition < x) xTo = -1;
                    else if (Player.XPosition > x) xTo = 1;
                }
            }
            else
                return new CreatureCommand();

            if (!(x + xTo >= 0 && x + xTo < Game.MapWidth && y + yTo >= 0 && y + yTo < Game.MapHeight))
                return new CreatureCommand();

            var map = Game.Map[x + xTo, y + yTo];
            if (map != null && (map is Terrain || map is Sack || map is Monster))
                return new CreatureCommand();

            return new CreatureCommand() { DeltaX = xTo, DeltaY = yTo };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority() => 0;

        public string GetImageFileName() => "Monster.png";

        static private bool FindPlayer()
        {
            for (int i = 0; i < Game.MapWidth; i++)
            {
                for (int j = 0; j < Game.MapHeight; j++)
                {
                    if (Game.Map[i, j] != null && Game.Map[i, j] is Player)
                    {
                        Player.XPosition = i;
                        Player.YPosition = j;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
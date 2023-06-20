using System;

namespace func_rocket
{
	public class ControlTask
	{
		private static double angle = 0.0;
		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			Calculate(rocket, target);
			if (angle > 0)
				return Turn.Right;
			else if (angle < 0)
				return Turn.Left;
			return Turn.None;
		}

		private static void Calculate(Rocket rocket, Vector target)
		{
			var distance = target - rocket.Location;
			if (Check(distance, rocket))
				angle = (distance.Angle * 2 - rocket.Velocity.Angle - rocket.Direction) / 2;
			else angle = distance.Angle - rocket.Direction;
		}

		private static bool Check(Vector distance, Rocket rocket)
		{
			var a = Math.Abs(distance.Angle - rocket.Velocity.Angle) < 0.5;
			var b = Math.Abs(distance.Angle - rocket.Direction) < 0.5;
			return a || b;
		}
	}

}
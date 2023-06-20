using System;
using System.Collections.Generic;

namespace func_rocket
{
	public class LevelsTask
	{
		static readonly Physics standardPhysics = new Physics();

		public static IEnumerable<Level> CreateLevels()
		{
			Rocket rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
			yield return new Level("Zero", 
				rocket,new Vector(600, 200), 
				(size, v) => Vector.Zero, standardPhysics);
			yield return new Level("Heavy",
				rocket, new Vector(600, 200),
				(size,v) => new Vector(0,0.9), standardPhysics);
			Vector endVector = new Vector(700, 500);
			yield return new Level("Up",
				rocket, endVector,
				(size, v) => new Vector(0,
				-300 / (300 + (size.Height - v.Y))), standardPhysics);
			yield return new Level("WhiteHole", rocket, endVector,
			(size, v) => WhiteHoleVector(v, endVector), standardPhysics);
			yield return new Level("BlackHole", rocket, endVector,
				(size, v) => BlackHoleVector(v, endVector, rocket.Location), standardPhysics);
			yield return new Level("BlackAndWhite", rocket, endVector,
				(size, v) =>
				{
					var vectorWhite = WhiteHoleVector(v, endVector);
					var vectorBlack = BlackHoleVector(v, endVector, rocket.Location);
					return (vectorWhite + vectorBlack) / 2;
				}, standardPhysics);
		}

		public static Vector WhiteHoleVector(Vector v, Vector end)
		{
			var d = (v - end).Length;
			return (v - end).Normalize() * 140 * d / (d * d + 1);
		}

		public static Vector BlackHoleVector(Vector v, Vector end, Vector rocketLocation)
		{
			var blackHoleLocation = (end + rocketLocation) / 2;
			var d = (blackHoleLocation - v).Length;
			return (blackHoleLocation - v).Normalize() * 300 * d / (d * d + 1);
		}
	}
}
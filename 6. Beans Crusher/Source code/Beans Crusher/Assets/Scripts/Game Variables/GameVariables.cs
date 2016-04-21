using System;


public static class GameVariables {

	//Number of rows and columns
	public static int Rows = 12;
	public static int Columns = 8;

	//Time for animatin
	public static float AnimationDuration = 0.2f;
	public static float MoveAnimationDuration = 0.05f;

	public static float ExplosionAnimatonDuration = 0.3f;

	public static float WaitBeforePotentialMatchesCheck = 2f;
	public static float OpacityAnimationDelay= 0.05f;

	//Matches beans
	public static int MinimumMatches = 3;
	public static int MinimumMatchesForBonus = 4;

	//For score
	public static int Matches3Score = 100;
	public static int SubsequelMatchesScore = 1000;


}

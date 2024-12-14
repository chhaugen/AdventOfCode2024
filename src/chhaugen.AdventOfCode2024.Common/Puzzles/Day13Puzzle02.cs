using chhaugen.AdventOfCode2024.Common.Structures;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day13Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day13Puzzle02 : Puzzle
{
    public Day13Puzzle02(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        ArcadeMachine[] arcadeMachines = ParseInput(input, add10To13ToPrize: true);
        long[] leastAmoutOfTokensArray = new long[arcadeMachines.Length];

        for (int i = 0; i < arcadeMachines.Length; i++)
        {
            var arcadeMachine = arcadeMachines[i];

            long? leastAmoutOfTokens = null;
            GameState boostedStartState = CreateBoostedGameState(arcadeMachine.Buttons);
            HashSet<GameState> gameStates = [boostedStartState];
            while (!leastAmoutOfTokens.HasValue)
            {
                HashSet<GameState> newGameStates = [];
                foreach (var gameState in gameStates)
                {
                    if (arcadeMachine.Prize == gameState.ClawPosition)
                    {
                        leastAmoutOfTokens = gameState.TokensUsed;
                        break;
                    }

                    foreach (Button button in arcadeMachine.Buttons)
                    {
                        if (gameState.MovesPlayed.TryGetValue(button.Symbol, out long buttonMoveCount))
                        {
                            if (!boostedStartState.MovesPlayed.TryGetValue(button.Symbol, out long boostedStartStateCount))
                                boostedStartStateCount = 0;

                            if (buttonMoveCount - boostedStartStateCount > 200)
                                continue;
                        }

                        var movesPlayed = gameState.MovesPlayed.ToDictionary(x => x.Key, x => x.Value);
                        movesPlayed.TryAdd(button.Symbol, 0);
                        movesPlayed[button.Symbol]++;
                        var newGripper = gameState.ClawPosition.Add(button.Vector);
                        var newTokenCount = gameState.TokensUsed + button.TokenCost;
                        var newGameState = new GameState(movesPlayed, newGripper, newTokenCount);
                        newGameStates.Add(newGameState);
                    }
                }
                if (newGameStates.Count == 0)
                    leastAmoutOfTokens = -1;
                gameStates = newGameStates;
            }
            leastAmoutOfTokensArray[i] = leastAmoutOfTokens.Value;
        }
        long totalSum = leastAmoutOfTokensArray.Where(x => x != -1).Sum();

        return Task.FromResult(totalSum.ToString());
    }

    public static GameState CreateBoostedGameState(Button[] arcadeMachineButtons)
    {
        //var cheapestButton = arcadeMachineButtons.Max(x => x.Vector.ManhattanLength / (decimal)x.TokenCost);
        long goal = 10000000000000;
        Point2D goalPoint = new(goal, goal);
        Line2D straightLine = new(a: -1, b: 1, c: 0);
        GameState gameState = new();
        while (gameState.ClawPosition.X < goal && gameState.ClawPosition.Y < goal)
        {
            Vector2D towardsGoal = new(gameState.ClawPosition, goalPoint);
            double LengthToCover = towardsGoal.Length * 0.1;
            GameState[] possibleGameStates = new GameState[arcadeMachineButtons.Length];
            for (int i = 0;  i < possibleGameStates.Length; i++)
            {
                Button button = arcadeMachineButtons[i];
                long scale = Convert.ToInt64(LengthToCover / button.Vector.Length);
                if (scale < 1)
                    scale = 1;
                var movesPlayed = gameState.MovesPlayed.ToDictionary(x => x.Key, x => x.Value);
                movesPlayed.TryAdd(button.Symbol, 0);
                movesPlayed[button.Symbol] += scale;
                var newGripper = gameState.ClawPosition.Add(button.Vector.Scale(scale));
                var newTokenCount = gameState.TokensUsed + (button.TokenCost * scale);
                possibleGameStates[i] = new GameState(movesPlayed, newGripper, newTokenCount);
            }
            gameState = possibleGameStates.MinBy(x => x.TokensUsed * straightLine.GetDistanceTo(x.ClawPosition));
        }
        return gameState;
    }
}

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

            GameState boostedStartState = CreateBoostedGameState(arcadeMachine.Buttons, arcadeMachine.Prize);
            leastAmoutOfTokensArray[i] = boostedStartState.ClawPosition == arcadeMachine.Prize
                ? MovesPlayedToTokensSpent(boostedStartState.MovesPlayed, arcadeMachine.Buttons)
                : -1;
        }
        long totalSum = leastAmoutOfTokensArray.Where(x => x != -1).Sum();

        return Task.FromResult(totalSum.ToString());
    }

    public static long MovesPlayedToTokensSpent(Dictionary<char, long> movesPlayed, Button[] buttons)
        => movesPlayed
        .Select(x => new { Button = buttons.First(y => y.Symbol == x.Key), Pressed = x.Value })
        .Select(x => x.Button.TokenCost * x.Pressed)
        .Sum();

    public static GameState CreateBoostedGameState(Button[] arcadeMachineButtons, Point2D pize)
    {
        Line2D straightLine = new(pize);
        GameState gameState = new();
        while (gameState.ClawPosition.X < pize.X && gameState.ClawPosition.Y < pize.Y)
        {
            Vector2D towardsGoal = new(gameState.ClawPosition, pize);
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

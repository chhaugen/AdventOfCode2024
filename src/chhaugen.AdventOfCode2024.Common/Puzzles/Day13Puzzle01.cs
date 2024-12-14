using System.Linq;
using System.Reflection.PortableExecutable;
using chhaugen.AdventOfCode2024.Common.Extentions;
using chhaugen.AdventOfCode2024.Common.Structures;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day13Puzzle01 : Puzzle
{
    public Day13Puzzle01(Action<string>? progressOutput = null) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        ArcadeMachine[] arcadeMachines = ParseInput(input);
        long[] leastAmoutOfTokensArray = new long[arcadeMachines.Length];
        for (int i = 0; i < arcadeMachines.Length; i++)
        {
            var machine = arcadeMachines[i];
            var firstButton = machine.Buttons[0];
            var secondButton = machine.Buttons[1];
            var matrix = ButtonsToMatrix(firstButton, secondButton);
            var invertedMatrix = matrix.Cast(x => (double)x).GetInverse();
            var resultVector = machine.Prize.AsVector().MultiplyWith(invertedMatrix);
        }

        return Task.FromResult("".ToString());
    }

    public static Matrix2x2<long> ButtonsToMatrix(Button first, Button second)
        => new(first.Vector.X, first.Vector.Y, second.Vector.X, second.Vector.Y);


    //public override Task<string> SolveAsync(string input)
    //{

    //    ArcadeMachine[] arcadeMachines = ParseInput(input);
    //    long[] leastAmoutOfTokensArray = new long[arcadeMachines.Length];

    //    for (int i = 0; i < arcadeMachines.Length; i++)
    //    {
    //        var arcadeMachine = arcadeMachines[i];

    //        long? leastAmoutOfTokens = null;
    //        HashSet<GameState> gameStates = [new()];
    //        while (!leastAmoutOfTokens.HasValue)
    //        {
    //            HashSet<GameState> newGameStates = [];
    //            foreach(var gameState in gameStates)
    //            {
    //                if (arcadeMachine.Prize == gameState.ClawPosition)
    //                {
    //                    leastAmoutOfTokens = gameState.TokensUsed;
    //                    break;
    //                }

    //                foreach(Button button in arcadeMachine.Buttons)
    //                {
    //                    if (gameState.MovesPlayed.TryGetValue(button.Symbol, out long buttonMoveCount))
    //                    {
    //                        if (buttonMoveCount > 100)
    //                            continue;
    //                    }

    //                    var movesPlayed = gameState.MovesPlayed.ToDictionary(x => x.Key, x => x.Value);
    //                    movesPlayed.TryAdd(button.Symbol, 0);
    //                    movesPlayed[button.Symbol]++;
    //                    var newGripper = gameState.ClawPosition.Add(button.Vector);
    //                    var newTokenCount = gameState.TokensUsed + button.TokenCost;
    //                    var newGameState = new GameState(movesPlayed, newGripper, newTokenCount);
    //                    newGameStates.Add(newGameState);
    //                }
    //            }
    //            if (newGameStates.Count == 0)
    //                leastAmoutOfTokens = -1;
    //            gameStates = newGameStates;
    //        }
    //        leastAmoutOfTokensArray[i] = leastAmoutOfTokens.Value;
    //    }
    //    long totalSum = leastAmoutOfTokensArray.Where(x => x != -1).Sum();

    //    return Task.FromResult(totalSum.ToString());
    //}

    public static ArcadeMachine[] ParseInput(string input, bool add10To13ToPrize = false)
    {
        string[] sections = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        ArcadeMachine[] arcadeMachines = new ArcadeMachine[sections.Length];
        for (int i = 0; i < sections.Length; i++)
        {
            arcadeMachines[i] = ArcadeMachine.Parse(sections[i], add10To13ToPrize);
        }
        return arcadeMachines;
    }


    public static int SymbolToTokenCost(char symbol)
        => symbol switch
        {
            'A' => 3,
            'B' => 1,
            _ => throw new NotImplementedException(),
        };


    public readonly struct ArcadeMachine
    {
        public ArcadeMachine(Button[] buttons, Point2D prize)
        {
            Buttons = buttons;
            Prize = prize;
        }
        public Button[] Buttons { get; }
        public Point2D Prize { get; }

        public static ArcadeMachine Parse(string oneArcadeMachineinput, bool add10To13ToPrize = false)
        {
            string[] lines = oneArcadeMachineinput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Button[] buttons = new Button[lines.Length - 1];
            Point2D? prize = null;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(' ');
                switch (parts[0])
                {
                    case "Button":
                        {
                            var symbol = parts[1][0];
                            var xString = parts[2].Where(char.IsDigit).ToArray();
                            var yString = parts[3].Where(char.IsDigit).ToArray();
                            long x = long.Parse(xString);
                            long y = long.Parse(yString);
                            int tokenCost = SymbolToTokenCost(symbol);
                            buttons[i] = new(symbol, tokenCost, new(x, y));
                        }
                        break;
                    case "Prize:":
                        {
                            var xString = parts[1].Where(char.IsDigit).ToArray();
                            var yString = parts[2].Where(char.IsDigit).ToArray();
                            long x = long.Parse(xString);
                            long y = long.Parse(yString);
                            if (add10To13ToPrize)
                            {
                                x += 10000000000000;
                                y += 10000000000000;
                            }
                            prize = new Point2D(x, y);
                        }
                        break;
                }
            }
            if (!prize.HasValue)
                throw new InvalidOperationException("Prize not parsed!");
            return new(buttons, prize.Value);
        }

    }

    public readonly struct Button
    {
        public Button(char symbol, int tokenCost, Vector2D vector)
        {
            Symbol = symbol;
            TokenCost = tokenCost;
            Vector = vector;
        }

        public char Symbol { get; }
        public int TokenCost { get; }
        public Vector2D Vector { get; }

        public override string ToString()
            => $"Button {Symbol}: X+{Vector.X}, Y+{Vector.Y}";
    }

    public readonly struct GameState : IEquatable<GameState>
    {
        public GameState()
        {
            MovesPlayed = [];
            ClawPosition = new(0, 0);
            TokensUsed = 0;
        }

        public GameState(Dictionary<char, long> movesPlayed, Point2D clawPosition, long tokensUsed)
        {
            MovesPlayed = movesPlayed;
            ClawPosition = clawPosition;
            TokensUsed = tokensUsed;
        }

        public Dictionary<char, long> MovesPlayed { get; }
        public Point2D ClawPosition { get; }
        public long TokensUsed { get; }

        public bool Equals(GameState other)
        {
            if (TokensUsed != other.TokensUsed)
                return false;

            if (ClawPosition != other.ClawPosition)
                return false;

            return MovesPlayed.ContentEquals(other.MovesPlayed);
        }

        public override int GetHashCode()
        {
            var hashcode = HashCode.Combine(TokensUsed, ClawPosition);
            foreach (var move in MovesPlayed)
                hashcode = HashCode.Combine(hashcode, move);
            return hashcode;
        }
    }
}

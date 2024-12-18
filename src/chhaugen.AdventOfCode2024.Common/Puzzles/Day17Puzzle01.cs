using System.Text;
using static chhaugen.AdventOfCode2024.Common.Puzzles.Day17Puzzle01;

namespace chhaugen.AdventOfCode2024.Common.Puzzles;
public class Day17Puzzle01 : Puzzle
{
    public Day17Puzzle01(Action<string>? progressOutput) : base(progressOutput)
    {
    }

    public override Task<string> SolveAsync(string input)
    {
        Computer computer = new();

        computer.LoadProgram(input);
        computer.RunUntilHalt();

        return Task.FromResult(computer.OutputString);
    }


    public class Computer : ICloneable
    {
        public Computer()
        {
            InitializeStandardInstructionSet();
        }

        public int RegisterA { get; set; }
        public int RegisterB { get; set; }
        public int RegisterC { get; set; }

        public int InstructionPointer { get; set; } = 0;

        public int[] Program { get; set; } = [];

        public List<int> Output { get; private set; } = [];

        public string OutputString => string.Join(',', Output);

        public Dictionary<int, IInstruction> Instructions { get; set; } = [];

        public void RunUntilHalt()
        {
            bool halted = false;
            while (!halted)
            {
                halted = RunNextInstruction();
            }
        }

        public bool RunNextInstruction()
        {
            if (!(0 <= InstructionPointer && InstructionPointer < Program.Length))
                return true;

            var opcode = Program[InstructionPointer++];
            var operand = Program[InstructionPointer++];
            Instructions[opcode].Run(operand, this);
            return false;
        }

        public int GetComboOperand(int operand)
            => operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                _ => throw new NotImplementedException(),
            };

        public void LoadProgram(string input)
        {
            var lines = input.Split('\n', options: StringSplitOptions.RemoveEmptyEntries);
            var registerALine = lines[0];
            var registerBLine = lines[1];
            var registerCLine = lines[2];
            var programLine   = lines[3];

            var registerAString = registerALine.Replace("Register A: ", string.Empty);
            var registerBString = registerBLine.Replace("Register B: ", string.Empty);
            var registerCString = registerCLine.Replace("Register C: ", string.Empty);

            var programStrings = programLine.Replace("Program: ", string.Empty).Split(',');

            RegisterA = int.Parse(registerAString);
            RegisterB = int.Parse(registerBString);
            RegisterC = int.Parse(registerCString);

            Program = programStrings.Select(int.Parse).ToArray();
        }

        public void InitializeStandardInstructionSet()
        {
            List<IInstruction> instructions =
            [
                new DivisionInstruction(),
                new BitwiseXORInstruction(),
                new WriteBRegisterInstruction(),
                new JumpInstruction(),
                new BitwiseXORBCInstruction(),
                new OutputInstruction(),
                new DivisionInstructionB(),
                new DivisionInstructionC(),
            ];

            Instructions = instructions.ToDictionary(x => x.Opcode, x => x);
        }

        public Computer Clone()
        {
            var result = new Computer()
            {
                RegisterA = RegisterA,
                RegisterB = RegisterB,
                RegisterC = RegisterC,
                InstructionPointer = InstructionPointer,
                Instructions = Instructions,
                Output = [.. Output],
                Program = (int[])Program.Clone(),
            };
            return  result;
        }

        object ICloneable.Clone()
            => Clone();
    }

    public interface IInstruction
    {
        string Name { get; }

        int Opcode { get; }

        void Run(int operand, Computer computer);
    }

    public class DivisionInstruction : IInstruction
    {
        public string Name => "adv";

        public int Opcode => 0;
            
        public void Run(int operand, Computer computer)
        {
            var comboOperand = computer.GetComboOperand(operand);
            var numerator = computer.RegisterA;
            var denominator = Math.Pow(2, comboOperand);
            var quotent = numerator / denominator;
            computer.RegisterA = (int)quotent;
        }
    }

    public class BitwiseXORInstruction : IInstruction
    {
        public string Name => "bxl";

        public int Opcode => 1;

        public void Run(int operand, Computer computer)
        {
            computer.RegisterB ^= operand;
        }
    }

    public class WriteBRegisterInstruction : IInstruction
    {
        public string Name => "bst";

        public int Opcode => 2;

        public void Run(int operand, Computer computer)
        {
            var comboOperand = computer.GetComboOperand(operand);
            computer.RegisterB = comboOperand % 8;
        }
    }

    public class JumpInstruction : IInstruction
    {
        public string Name => "jnz";

        public int Opcode => 3;

        public void Run(int operand, Computer computer)
        {
            if (computer.RegisterA == 0)
                return;
            computer.InstructionPointer = operand;
        }
    }

    public class BitwiseXORBCInstruction : IInstruction
    {
        public string Name => "bxc";

        public int Opcode => 4;

        public void Run(int operand, Computer computer)
        {
            computer.RegisterB ^= computer.RegisterC;
        }
    }

    public class OutputInstruction : IInstruction
    {
        public string Name => "out";

        public int Opcode => 5;

        public void Run(int operand, Computer computer)
        {
            var comboOperand = computer.GetComboOperand(operand);
            computer.Output.Add(comboOperand % 8);
        }
    }

    public class DivisionInstructionB : IInstruction
    {
        public string Name => "bdv";

        public int Opcode => 6;

        public void Run(int operand, Computer computer)
        {
            var comboOperand = computer.GetComboOperand(operand);
            var numerator = computer.RegisterA;
            var denominator = Math.Pow(2, comboOperand);
            var quotent = numerator / denominator;
            computer.RegisterB = (int)quotent;
        }
    }

    public class DivisionInstructionC : IInstruction
    {
        public string Name => "bdv";

        public int Opcode => 7;

        public void Run(int operand, Computer computer)
        {
            var comboOperand = computer.GetComboOperand(operand);
            var numerator = computer.RegisterA;
            var denominator = Math.Pow(2, comboOperand);
            var quotent = numerator / denominator;
            computer.RegisterC = (int)quotent;
        }
    }
}

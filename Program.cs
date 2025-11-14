using Raylib_cs;

namespace MyFiveApp;

public class Core
{
    private readonly List<Player> _playersList = [];

    private const int NumberOfPlayers = 10;

    private const int WindowWidth = 1200;
    private const int WindowHeight = 700;

    private readonly Color _backgroundColor = Color.Black;
    private readonly Color _fontColor = Color.White;
    private readonly Color _targetColor = Color.Yellow;
    private readonly Color _lateralBarColor = Color.White;

    private const int FrameRate = 0;

    private const int TargetPositionX = 1100;
    private const int TargetPositionY = 500;


    private void SpawnPlayers()
    {
        foreach (var _ in Enumerable.Range(0, NumberOfPlayers))
        {
            var newPlayer = new Player();
            newPlayer.Init();
            newPlayer.SetTargetPosition(TargetPositionX, TargetPositionY);
            _playersList.Add(newPlayer);
        }
    }
    
    public static void Main(string[] args)
    {
        Console.WriteLine("HW");

        var myCore = new Core();
        myCore.SpawnPlayers();
        myCore.Loop();
    }

    private void Loop()
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "MyGamingAi");
        Raylib.SetTargetFPS(FrameRate);
        while (!Raylib.WindowShouldClose())
        {
            Render();
            foreach (var p in _playersList)
            {
                p.Process();
                p.PhysicsProcess();
                p.Render();
            }
        }
        Raylib.CloseWindow();
    }

    private void Render()
    {
        Raylib.BeginDrawing();
        
        Raylib.ClearBackground(_backgroundColor);

        DrawInterface();

        Raylib.DrawRectangle(TargetPositionX, TargetPositionY, 30, 30, _targetColor);
        
        Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 10, 10, 20, _fontColor);
        Raylib.DrawText($"Frame Time: {Raylib.GetFrameTime()}", 10, 40, 20, _fontColor);
        
        Raylib.EndDrawing();
    }

    private void DrawInterface()
    {
        Raylib.DrawRectangle(0, 0, WindowWidth/5, WindowHeight, _lateralBarColor);
    }

}

public class Player
{
    private float _positionX = 300f;
    private float _positionY = 300f;
    
    private float _velocityX;
    private float _velocityY;

    private const float Speed = 2f;

    private readonly List<int> _neuralWeights = [];
    private const int NumberOfNeuralWeights = 55 + 1;

    private int _targetPositionX = 1100;
    private int _targetPositionY = 500;

    private void ApplyLeftMoveForce()
    {
        _velocityX -= Speed * Raylib.GetFrameTime();
    }

    private void ApplyRightMoveForce()
    {
        _velocityX += Speed * Raylib.GetFrameTime();
    }

    private void ApplyUpMoveForce()
    {
        _velocityY -= Speed * Raylib.GetFrameTime();
    }

    private void ApplyBottomMoveForce()
    {
        _velocityY += Speed * Raylib.GetFrameTime();
    }

    private void ApplyVelocity()
    {
        _positionX += _velocityX;
        _positionY += _velocityY;

        if (_velocityX < 0)
        {
            _velocityX += Raylib.GetFrameTime();
        }
        if (_velocityX > 0)
        {
            _velocityX -= Raylib.GetFrameTime();
        }
        if (_velocityY < 0)
        {
            _velocityY += Raylib.GetFrameTime();
        }
        if (_velocityY > 0)
        {
            _velocityY -= Raylib.GetFrameTime();
        }
    }

    public void SetTargetPosition(int newPositionX, int newPositionY)
    {
        _targetPositionX = newPositionX;
        _targetPositionY = newPositionY;
    }

    private void SetNeuralWeights()
    {
        _neuralWeights.Clear();
        
        Random rng = new Random();
        foreach (var _ in Enumerable.Range(0, NumberOfNeuralWeights))
        {
            _neuralWeights.Add(rng.Next(-1000, 1000+1));
        }
    }


    public void Init()
    {
        SetNeuralWeights();
    }

    public void Process()
    {
        var input0 = _positionX;
        var input1 = _positionY;
        var input2 = _velocityX;
        var input3 = _velocityY;
        var input4 = Speed;
        var input5 = _targetPositionX;
        var input6 = _targetPositionY;

        var partialResult0 = input0 * _neuralWeights[0] +
                                  input1 * _neuralWeights[1] +
                                  input2 * _neuralWeights[2] +
                                  input3 * _neuralWeights[3] +
                                  input4 * _neuralWeights[4] +
                                  input5 * _neuralWeights[5] +
                                  input6 * _neuralWeights[6];
        var partialResult1 = input0 * _neuralWeights[7] +
                                  input1 * _neuralWeights[8] +
                                  input2 * _neuralWeights[9] +
                                  input3 * _neuralWeights[10] +
                                  input4 * _neuralWeights[11] +
                                  input5 * _neuralWeights[12] +
                                  input6 * _neuralWeights[13];
        var partialResult2 = input0 * _neuralWeights[14] +
                                  input1 * _neuralWeights[15] +
                                  input2 * _neuralWeights[16] +
                                  input3 * _neuralWeights[17] +
                                  input4 * _neuralWeights[18] +
                                  input5 * _neuralWeights[19] +
                                  input6 * _neuralWeights[20];
        var partialResult3 = input0 * _neuralWeights[21] +
                                  input1 * _neuralWeights[22] +
                                  input2 * _neuralWeights[23] +
                                  input3 * _neuralWeights[24] +
                                  input4 * _neuralWeights[25] +
                                  input5 * _neuralWeights[26] +
                                  input6 * _neuralWeights[27];

        var result0 = partialResult0 * _neuralWeights[28] +
                           partialResult1 * _neuralWeights[29] +
                           partialResult2 * _neuralWeights[30] +
                           partialResult3 * _neuralWeights[31];
        var result1 = partialResult0 * _neuralWeights[32] +
                           partialResult1 * _neuralWeights[33] +
                           partialResult2 * _neuralWeights[34] +
                           partialResult3 * _neuralWeights[35];
        var result2 = partialResult0 * _neuralWeights[36] +
                           partialResult1 * _neuralWeights[37] +
                           partialResult2 * _neuralWeights[38] +
                           partialResult3 * _neuralWeights[39];
        var result3 = partialResult0 * _neuralWeights[40] +
                           partialResult1 * _neuralWeights[41] +
                           partialResult2 * _neuralWeights[42] +
                           partialResult3 * _neuralWeights[43];

        if (result0 > 0)
        {
            ApplyLeftMoveForce();
        }
        if (result1 > 0)
        {
            ApplyRightMoveForce();
        }
        if (result2 > 0)
        {
            ApplyUpMoveForce();
        }
        if (result3 > 0)
        {
            ApplyBottomMoveForce();
        }
        
    }

    public void PhysicsProcess()
    {
        ApplyVelocity();
    }

    public void Render()
    {
        Raylib.DrawRectangle((int) _positionX, (int) _positionY, 30, 30, Color.Red);
    }
}
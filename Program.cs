using Raylib_cs;

namespace MyFiveApp;

public class Core
{
    private readonly List<Player> _playersList = [];

    private const int NumberOfPlayers = 3000;

    private const int WindowWidth = 1200;
    private const int WindowHeight = 700;

    private readonly Color _backgroundColor = Color.Black;
    private readonly Color _fontColor = Color.Black;
    private readonly Color _targetColor = Color.Yellow;
    private readonly Color _lateralBarColor = Color.White;

    private const int FrameRate = 0;

    private const int TargetPositionX = 1100;
    private const int TargetPositionY = 500;

    private int _playersAlive;

    private float _nextGenerationTime;
    private float _simulationTime;

    private float _currentGeneration;


    private void SpawnPlayers()
    {
        _currentGeneration += 1;
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
        myCore.UpdatePlayersCount();
        myCore.Loop();
    }

    private void Loop()
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "MyGamingAi");
        Raylib.SetTargetFPS(FrameRate);
        while (!Raylib.WindowShouldClose())
        {
            _simulationTime += Raylib.GetFrameTime();
            _nextGenerationTime -= Raylib.GetFrameTime();
            
            foreach (var p in _playersList.ToList())
            {
                if (p.VerifyCollision())
                {
                    _playersList.Remove(p);
                }
                else
                {
                    p.Process();
                    p.PhysicsProcess();
                    p.Render();
                }
            }
            Render();

            UpdatePlayersCount();
        }
        Raylib.CloseWindow();
    }

    private void UpdatePlayersCount()
    {
        _playersAlive = _playersList.Count;
    }

    private void Render()
    {
        Raylib.BeginDrawing();
        
        Raylib.ClearBackground(_backgroundColor);

        DrawInterface();

        Raylib.DrawRectangle(TargetPositionX, TargetPositionY, 30, 30, _targetColor);
        
        Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 10, 10, 20, _fontColor);
        Raylib.DrawText($"Frame Time: {Raylib.GetFrameTime()}", 10, 40, 20, _fontColor);
        Raylib.DrawText($"Players Alive: {_playersAlive}", 10, 70, 20, _fontColor);
        Raylib.DrawText($"Next Gen Time: {_nextGenerationTime}", 10, 100, 20, _fontColor);
        Raylib.DrawText($"Simulation Time: {_simulationTime}", 10, 130, 20, _fontColor);
        Raylib.DrawText($"Generation: {_currentGeneration}", 10, 160, 20, _fontColor);
        
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

    private const int WindowLeftLimit = 1200/5;
    private const int WindowRightLimit = 1200;
    private const int WindowTopLimit = 0;
    private const int WindowBottomLimit = 700;

    private const float Speed = 1f;

    private readonly List<int> _neuralWeights = [];
    private const int NumberOfNeuralWeights = 55 + 1;

    private int _targetPositionX = 1100;
    private int _targetPositionY = 500;
    
    private float _input0;
    private float _input1;
    private float _input2;
    private float _input3;
    private float _input4;
    private float _input5;
    private float _input6;
    private float _input7;
    private float _input8;
    private float _input9;
    private float _input10;
    
    private float _partialResult0;
    private float _partialResult1;
    private float _partialResult2;
    private float _partialResult3;
    
    private float _result0;
    private float _result1;
    private float _result2;
    private float _result3;

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
        
        
        //Reduces the applied velocity to zero
        //
        //if (_velocityX < 0)
        //{
        //    _velocityX += Raylib.GetFrameTime();
        //}
        //if (_velocityX > 0)
        //{
        //    _velocityX -= Raylib.GetFrameTime();
        //}
        //if (_velocityY < 0)
        //{
        //    _velocityY += Raylib.GetFrameTime();
        //}
        //if (_velocityY > 0)
        //{
        //    _velocityY -= Raylib.GetFrameTime();
        //}
    }

    public void SetTargetPosition(int newPositionX, int newPositionY)
    {
        _targetPositionX = newPositionX;
        _targetPositionY = newPositionY;
    }

    private void SetNeuralWeights()
    {
        _neuralWeights.Clear();
        
        var rng = new Random();
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
        _input0 = _positionX;
        _input1 = _positionY;
        _input2 = _velocityX;
        _input3 = _velocityY;
        _input4 = Speed;
        _input5 = _targetPositionX;
        _input6 = _targetPositionY;
        _input7 = WindowLeftLimit;
        _input8 = WindowRightLimit;
        _input9 = WindowTopLimit;
        _input10 = WindowBottomLimit;

        _partialResult0 = _input0 * _neuralWeights[0] +
                         _input1 * _neuralWeights[1] +
                         _input2 * _neuralWeights[2] +
                         _input3 * _neuralWeights[3] +
                         _input4 * _neuralWeights[4] +
                         _input5 * _neuralWeights[5] +
                         _input6 * _neuralWeights[6] +
                         _input7 * _neuralWeights[7] +
                         _input8 * _neuralWeights[8] +
                         _input9 * _neuralWeights[9] +
                         _input10 * _neuralWeights[10];
        _partialResult1 = _input0 * _neuralWeights[11] +
                         _input1 * _neuralWeights[12] +
                         _input2 * _neuralWeights[13] +
                         _input3 * _neuralWeights[14] +
                         _input4 * _neuralWeights[15] +
                         _input5 * _neuralWeights[16] +
                         _input6 * _neuralWeights[17] +
                         _input7 * _neuralWeights[18] +
                         _input8 * _neuralWeights[19] +
                         _input9 * _neuralWeights[20] +
                         _input10 * _neuralWeights[21];
        _partialResult2 = _input0 * _neuralWeights[22] +
                         _input1 * _neuralWeights[23] +
                         _input2 * _neuralWeights[24] +
                         _input3 * _neuralWeights[25] +
                         _input4 * _neuralWeights[26] +
                         _input5 * _neuralWeights[27] +
                         _input6 * _neuralWeights[28] +
                         _input7 * _neuralWeights[29] +
                         _input8 * _neuralWeights[30] +
                         _input9 * _neuralWeights[31] +
                         _input10 * _neuralWeights[32];
        _partialResult3 = _input0 * _neuralWeights[33] +
                         _input1 * _neuralWeights[34] +
                         _input2 * _neuralWeights[35] +
                         _input3 * _neuralWeights[36] +
                         _input4 * _neuralWeights[37] +
                         _input5 * _neuralWeights[38] +
                         _input6 * _neuralWeights[39] +
                         _input7 * _neuralWeights[40] +
                         _input8 * _neuralWeights[41] +
                         _input9 * _neuralWeights[42] +
                         _input10 * _neuralWeights[43];

        _result0 = _partialResult0 * _neuralWeights[28] +
                  _partialResult1 * _neuralWeights[29] +
                  _partialResult2 * _neuralWeights[30] +
                  _partialResult3 * _neuralWeights[31];
        _result1 = _partialResult0 * _neuralWeights[32] +
                  _partialResult1 * _neuralWeights[33] +
                  _partialResult2 * _neuralWeights[34] +
                  _partialResult3 * _neuralWeights[35];
        _result2 = _partialResult0 * _neuralWeights[36] +
                  _partialResult1 * _neuralWeights[37] +
                  _partialResult2 * _neuralWeights[38] +
                  _partialResult3 * _neuralWeights[39];
        _result3 = _partialResult0 * _neuralWeights[40] +
                  _partialResult1 * _neuralWeights[41] +
                  _partialResult2 * _neuralWeights[42] +
                  _partialResult3 * _neuralWeights[43];

        if (_result0 > 0)
        {
            ApplyLeftMoveForce();
        }
        if (_result1 > 0)
        {
            ApplyRightMoveForce();
        }
        if (_result2 > 0)
        {
            ApplyUpMoveForce();
        }
        if (_result3 > 0)
        {
            ApplyBottomMoveForce();
        }
        
    }

    public bool VerifyCollision()
    {
        return (int) _positionX < WindowLeftLimit || (int) _positionX > WindowRightLimit ||
               (int) _positionY < WindowTopLimit || (int) _positionY > WindowBottomLimit;
    }

    public void PhysicsProcess()
    {
        ApplyVelocity();
        VerifyCollision();
    }

    public void Render()
    {
        Raylib.DrawRectangle((int) _positionX, (int) _positionY, 30, 30, Color.Red);
    }
}
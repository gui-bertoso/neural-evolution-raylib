using Raylib_cs;

namespace MyFiveApp;

public class Core
{
    private readonly List<Player> _playersList = [];

    private const int NumberOfPlayers = 1000;

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

    private float _bestPlayerDistanceToTarget = 10000;

    private Player _bestPlayerReference = new Player();
    private Player _bestAlivePlayerReference = new Player();

    private bool _applyMoreNeuralMutation;


    private void SpawnPlayers()
    {
        _currentGeneration += 1;
        _nextGenerationTime = 10f;
        foreach (var _ in Enumerable.Range(0, NumberOfPlayers))
        {
            var newPlayer = new Player();
            newPlayer.Init();
            newPlayer.SetTargetPosition(TargetPositionX, TargetPositionY);
            if (_currentGeneration > 1)
            {
                newPlayer.SetNeuralWeights(_bestPlayerReference.GetNeuralWeights());
                if (_applyMoreNeuralMutation)
                {
                    newPlayer.ApplyMoreNeuralWeightsMutation();
                }
            }
            _playersList.Add(newPlayer);
        }

        _applyMoreNeuralMutation = false;
    }

    private void UpdateBestAlivePlayer()
    {
        var bestAlivePlayerDistanceToTarget = 10000f;
        foreach (var i in Enumerable.Range(0, _playersList.Count))
        {
            if (_playersList[i].GetDistanceToTarget() < 10)
            {
                _bestAlivePlayerReference = _playersList[i];
                return;
            }
            if (!(_playersList[i].GetDistanceToTarget() < bestAlivePlayerDistanceToTarget)) continue;
            _bestAlivePlayerReference = _playersList[i];
            bestAlivePlayerDistanceToTarget = _bestAlivePlayerReference.GetDistanceToTarget();
        }
    }

    public static void Main(string[] args)
    {
        var myCore = new Core();
        myCore.SpawnPlayers();
        myCore.UpdatePlayersCount();
        myCore.Loop();
    }

    private void Loop()
    {
        UpdateBestAlivePlayer();
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


            UpdatePlayersRank();
            UpdatePlayersCount();
            UpdateSimulationReset();
            UpdateBestAlivePlayer();
            Render();
        }

        Raylib.CloseWindow();
    }

    private void UpdatePlayersCount()
    {
        _playersAlive = _playersList.Count;
    }

    private void UpdatePlayersRank()
    {
        foreach (var i in Enumerable.Range(0, _playersList.Count))
        {
            if (!(_playersList[i].GetDistanceToTarget() < _bestPlayerDistanceToTarget)) continue;
            _bestPlayerDistanceToTarget = _playersList[i].GetDistanceToTarget();
            _bestPlayerReference = _playersList[i];
        }
    }

    private void Render()
    {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(_backgroundColor);

        DrawInterface();

        Raylib.DrawRectangle(TargetPositionX, TargetPositionY, 30, 30, _targetColor);

        Raylib.EndDrawing();
    }

    private void DrawInterface()
    {
        Raylib.DrawRectangle(0, 0, WindowWidth / 5, WindowHeight, _lateralBarColor);

        Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 10, 10, 18, _fontColor);
        Raylib.DrawText($"Frame Time: {Raylib.GetFrameTime()}", 10, 40, 18, _fontColor);
        Raylib.DrawText($"Players Alive: {_playersAlive}", 10, 70, 18, _fontColor);
        Raylib.DrawText($"Next Gen Time: {_nextGenerationTime}", 10, 100, 18, _fontColor);
        Raylib.DrawText($"Simulation Time: {_simulationTime}", 10, 130, 18, _fontColor);
        Raylib.DrawText($"Generation: {_currentGeneration}", 10, 160, 18, _fontColor);
        Raylib.DrawText($"B.P.D.T.T: {_bestPlayerDistanceToTarget}", 10, 190, 18, _fontColor);
        Raylib.DrawText($"B.P.ID: {_bestPlayerReference.GetId()}", 10, 220, 18, _fontColor);
        Raylib.DrawText($"B.A.P.ID: {_bestAlivePlayerReference.GetId()}", 10, 250, 18, _fontColor);

        RenderNeuralWeb();
    }

    private void UpdateSimulationReset()
    {
        if (!(_nextGenerationTime < 0) && _playersAlive != 0) return;
        if (_playersAlive == NumberOfPlayers)
        {
            _applyMoreNeuralMutation = true;
            _bestPlayerDistanceToTarget += 100;
        }
        foreach (var p in _playersList.ToArray())
        {
            _playersList.Remove(p);
        }
        SpawnPlayers();
    }

    private void RenderNeuralWeb()
    {
        const int baseX = 45;
        var baseY = 300;

        var pointsList0 = Enumerable.Range(0, 12).Select(i => new Tuple<int, int>(baseX + 20, baseY + (i * 30))).ToList();

        baseY = 405;
        var pointsList1 = Enumerable.Range(0, 4).Select(i => new Tuple<int, int>(baseX + 70, baseY + (i * 30))).ToList();

        baseY = 405;

        var pointArray1 = pointsList1.ToArray();
        var pointArray2 = Enumerable.Range(0, 4).Select(i => new Tuple<int, int>(baseX + 120, baseY + (i * 30))).ToArray();
        foreach (var i in pointsList0)
        {
            Raylib.DrawLine(i.Item1, i.Item2, pointArray1[0].Item1, pointArray1[0].Item2,
                _bestAlivePlayerReference.GetNeuralLine0Color(0, 0));
            Raylib.DrawLine(i.Item1, i.Item2, pointArray1[1].Item1, pointArray1[1].Item2,
                _bestAlivePlayerReference.GetNeuralLine0Color(0, 1));
            Raylib.DrawLine(i.Item1, i.Item2, pointArray1[2].Item1, pointArray1[2].Item2,
                _bestAlivePlayerReference.GetNeuralLine0Color(0, 2));
            Raylib.DrawLine(i.Item1, i.Item2, pointArray1[3].Item1, pointArray1[3].Item2,
                _bestAlivePlayerReference.GetNeuralLine0Color(0, 3));
        }

        foreach (var i in pointsList1)
        {
            Raylib.DrawLine(i.Item1, i.Item2, pointArray2[0].Item1, pointArray2[0].Item2,
                _bestAlivePlayerReference.GetNeuralLine1Color(0, 0));
            Raylib.DrawLine(i.Item1, i.Item2, pointArray2[1].Item1, pointArray2[1].Item2,
                _bestAlivePlayerReference.GetNeuralLine1Color(0, 0));
            Raylib.DrawLine(i.Item1, i.Item2, pointArray2[2].Item1, pointArray2[2].Item2,
                _bestAlivePlayerReference.GetNeuralLine1Color(0, 0));
            Raylib.DrawLine(i.Item1, i.Item2, pointArray2[3].Item1, pointArray2[3].Item2,
                _bestAlivePlayerReference.GetNeuralLine1Color(0, 0));
        }
        
        baseY = 300;
        foreach (var i in Enumerable.Range(0, 12))
        {
            Raylib.DrawCircle(baseX + 20, baseY + (i * 30), 10, _bestAlivePlayerReference.GetNeuralPointColor(0, i));
        }

        baseY = 405;
        foreach (var i in Enumerable.Range(0, 4))
        {
            Raylib.DrawCircle(baseX + 70, baseY + (i * 30), 10, _bestAlivePlayerReference.GetNeuralPointColor(1, i));
        }

        baseY = 405;
        foreach (var i in Enumerable.Range(0, 4))
        {
            Raylib.DrawCircle(baseX + 120, baseY + (i * 30), 10, _bestAlivePlayerReference.GetNeuralPointColor(2, i));
        }
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

    private List<int> _neuralWeights = [];
    private const int NumberOfNeuralWeights = 67 + 1;

    private int _targetPositionX = 1100;
    private int _targetPositionY = 500;

    private float _distanceToTargetX;
    private float _distanceToTargetY;
    
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
    private float _input11;
    private float _input12;
    
    private float _partialResult0;
    private float _partialResult1;
    private float _partialResult2;
    private float _partialResult3;
    
    private float _result0;
    private float _result1;
    private float _result2;
    private float _result3;

    private int _id;

    public int GetId()
    {
        return _id;
    }

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

        UpdateDistanceToTarget();
    }

    public float GetDistanceToTarget()
    {
        return _distanceToTargetX + _distanceToTargetY;
    }

    public List<int> GetNeuralWeights()
    {
        return _neuralWeights;
    }

    public void SetTargetPosition(int newPositionX, int newPositionY)
    {
        _targetPositionX = newPositionX;
        _targetPositionY = newPositionY;
    }

    public void SetNeuralWeights(List<int> newWeights)
    {
        _neuralWeights = newWeights;
        foreach (var i in Enumerable.Range(0, _neuralWeights.Count))
        {
            _neuralWeights[i] += new Random().Next(-100, 100);
        }
    }

    public void ApplyMoreNeuralWeightsMutation()
    {
        foreach (var i in Enumerable.Range(0, _neuralWeights.Count))
        {
            _neuralWeights[i] += new Random().Next(-500, 500);
        }
    }

    private void SetRandomNeuralWeights()
    {
        _neuralWeights.Clear();
        
        var rng = new Random();
        foreach (var _ in Enumerable.Range(0, NumberOfNeuralWeights))
        {
            _neuralWeights.Add(rng.Next(-1000, 1000+1));
        }
    }

    private void UpdateDistanceToTarget()
    {
        var distanceX = _targetPositionX - _positionX;
        var distanceY = _targetPositionY - _positionY;

        if (distanceX < 0)
        {
            distanceX *= -1;
        }

        if (distanceY < 0)
        {
            distanceY *= -1;
        }

        _distanceToTargetX = distanceX;
        _distanceToTargetY = distanceY;
    }


    public void Init()
    {
        _id = new Random().Next(0, 9999999);
        SetRandomNeuralWeights();
    }

    public void Process()
    {
        _input0 = _positionX;
        _input1 = _positionY;
        _input2 = _velocityX;
        _input3 = _velocityY;
        _input4 = _distanceToTargetX;
        _input5 = _distanceToTargetY;
        _input6 = Speed;
        _input7 = _targetPositionX;
        _input8 = _targetPositionY;
        _input9 = WindowLeftLimit;
        _input10 = WindowRightLimit;
        _input11 = WindowTopLimit;
        _input12 = WindowBottomLimit;

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
                         _input10 * _neuralWeights[10] +
                         _input11 * _neuralWeights[11] +
                         _input12 * _neuralWeights[12];
        _partialResult1 = _input0 * _neuralWeights[13] +
                          _input1 * _neuralWeights[14] +
                          _input2 * _neuralWeights[15] +
                          _input3 * _neuralWeights[16] +
                          _input4 * _neuralWeights[17] +
                          _input5 * _neuralWeights[18] +
                          _input6 * _neuralWeights[19] +
                          _input7 * _neuralWeights[20] +
                          _input8 * _neuralWeights[21] +
                          _input9 * _neuralWeights[22] +
                          _input10 * _neuralWeights[23] +
                          _input11 * _neuralWeights[24] +
                          _input12 * _neuralWeights[25];
        _partialResult2 = _input0 * _neuralWeights[26] +
                          _input1 * _neuralWeights[27] +
                          _input2 * _neuralWeights[28] +
                          _input3 * _neuralWeights[29] +
                          _input4 * _neuralWeights[30] +
                          _input5 * _neuralWeights[31] +
                          _input6 * _neuralWeights[32] +
                          _input7 * _neuralWeights[33] +
                          _input8 * _neuralWeights[34] +
                          _input9 * _neuralWeights[35] +
                          _input10 * _neuralWeights[36] +
                          _input11 * _neuralWeights[37] +
                          _input12 * _neuralWeights[38];
        _partialResult3 = _input0 * _neuralWeights[39] +
                          _input1 * _neuralWeights[40] +
                          _input2 * _neuralWeights[41] +
                          _input3 * _neuralWeights[42] +
                          _input4 * _neuralWeights[43] +
                          _input5 * _neuralWeights[44] +
                          _input6 * _neuralWeights[45] +
                          _input7 * _neuralWeights[46] +
                          _input8 * _neuralWeights[47] +
                          _input9 * _neuralWeights[48] +
                          _input10 * _neuralWeights[49] +
                          _input11 * _neuralWeights[50] +
                          _input12 * _neuralWeights[51];

        _result0 = _partialResult0 * _neuralWeights[52] +
                   _partialResult1 * _neuralWeights[53] +
                   _partialResult2 * _neuralWeights[54] +
                   _partialResult3 * _neuralWeights[55];
        _result1 = _partialResult0 * _neuralWeights[56] +
                   _partialResult1 * _neuralWeights[57] +
                   _partialResult2 * _neuralWeights[58] +
                   _partialResult3 * _neuralWeights[59];
        _result2 = _partialResult0 * _neuralWeights[60] +
                   _partialResult1 * _neuralWeights[61] +
                   _partialResult2 * _neuralWeights[62] +
                   _partialResult3 * _neuralWeights[63];
        _result3 = _partialResult0 * _neuralWeights[64] +
                   _partialResult1 * _neuralWeights[65] +
                   _partialResult2 * _neuralWeights[66] +
                   _partialResult3 * _neuralWeights[67];

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

    public Color GetNeuralPointColor(int layer, int index)
    {
        switch (layer)
        {
            case 0:
                switch (index)
                {
                    case 0:
                        return _input0 > 0 ? Color.Red : Color.Black;
                    case 1:
                        return _input1 > 0 ? Color.Red : Color.Black;
                    case 2:
                        return _input2 > 0 ? Color.Red : Color.Black;
                    case 3:
                        return _input3 > 0 ? Color.Red : Color.Black;
                    case 4:
                        return _input4 > 0 ? Color.Red : Color.Black;
                    case 5:
                        return _input5 > 0 ? Color.Red : Color.Black;
                    case 6:
                        return _input6 > 0 ? Color.Red : Color.Black;
                    case 7:
                        return _input7 > 0 ? Color.Red : Color.Black;
                    case 8:
                        return _input8 > 0 ? Color.Red : Color.Black;
                    case 9:
                        return _input9 > 0 ? Color.Red : Color.Black;
                    case 10:
                        return _input10 > 0 ? Color.Red : Color.Black;
                    case 11:
                        return _input11 > 0 ? Color.Red : Color.Black;
                    case 12:
                        return _input12 > 0 ? Color.Red : Color.Black;
                }

                break;
            case 1:
                switch (index)
                {
                    case 0:
                        return _partialResult0 > 0 ? Color.Red : Color.Black;
                    case 1:
                        return _partialResult1 > 0 ? Color.Red : Color.Black;
                    case 2:
                        return _partialResult2 > 0 ? Color.Red : Color.Black;
                    case 3:
                        return _partialResult3 > 0 ? Color.Red : Color.Black;
                }

                break;
            case 2:
                switch (index)
                {
                    case 0:
                        return _result0 > 0 ? Color.Red : Color.Black;
                    case 1:
                        return _result1 > 0 ? Color.Red : Color.Black;
                    case 2:
                        return _result2 > 0 ? Color.Red : Color.Black;
                    case 3:
                        return _result3 > 0 ? Color.Red : Color.Black;
                }

                break;
        }

        return Color.Green;
    }

    private bool IsRedInNeuralWeb(int layer, int index, Color color)
    {
        return GetNeuralPointColor(layer, index).Equals(color);
    }

    public Color GetNeuralLine0Color(int index0, int index1)
    {
        if (IsRedInNeuralWeb(0, index0, Color.Red) && IsRedInNeuralWeb(1, index1, Color.Red))
        {
            return Color.Red;
        }
        else
        {
            return Color.Black;
        }
    }

    public Color GetNeuralLine1Color(int index0, int index1)
    {
        if (IsRedInNeuralWeb(0, index0, Color.Red) && IsRedInNeuralWeb(1, index1, Color.Red))
        {
            return Color.Red;
        }
        else
        {
            return Color.Black;
        }
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
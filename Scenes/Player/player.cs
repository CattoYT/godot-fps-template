using Godot;
using System;

public partial class player : CharacterBody3D {
    [Export] public int Speed { get; set; } = 14;
    [Export] public float JUMPVELOCITY { get; set; } = 4.5F;


    float gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");


    private Vector3 _targetVelocity = Vector3.Zero;

    // from the source file, velocity = _targetVelocity
    
    
    public override void _Ready() {
        var gunRay = GetNode<RayCast3D>("Head/Camera3d/RayCast3D");
        var Camera = GetNode<Camera3D>("Head/Camera3d");

        gunRay.AddException(this);

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    
    

    public override void _PhysicsProcess(double delta) {
        if (!IsOnFloor()) {
            _targetVelocity.Y -= gravity * (float)delta;
        }

        if (Input.IsActionJustPressed("Jump") && IsOnFloor()) {
            _targetVelocity.Y = JUMPVELOCITY;
        }

        if (Input.IsActionJustPressed("Shoot")) {
            //Shoot();
        }

        Vector2 inputDirection = Input.GetVector("moveLeft", "moveRight", "moveUp", "moveDown");
        var direction = (Transform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();
        if (direction != Vector3.Zero) {
            _targetVelocity.X = direction.X * Speed;
            _targetVelocity.Y = direction.Y * Speed;
        }
        else {
            //unsure of this being necessary
            // source: line 37 and 38 in https://github.com/Shidoengie/FPS-template-gd4/blob/main/Scenes/Player/player.gd
            _targetVelocity = _targetVelocity.MoveToward(_targetVelocity, (float)delta);
        }

        MoveAndSlide();





    }
}
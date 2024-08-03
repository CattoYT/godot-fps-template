using Godot;
using System;

public partial class player : CharacterBody3D {
    [Export] public int Speed { get; set; } = 5;
    [Export] public float JUMPVELOCITY { get; set; } = 4.5F;

    [Export] public float MouseSensitivity { get; set; } = 1200;


    float gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

    private Camera3D Camera;
    private Vector3 _targetVelocity = Vector3.Zero;
    private Vector3 Rotation = Vector3.Zero;



    // from the source file, velocity = _targetVelocity


    public override void _Ready() {
        var gunRay = GetNode<RayCast3D>("Head/Camera3d/RayCast3D");
        Camera = GetNode<Camera3D>("Head/Camera3d");

        gunRay.AddException(this);

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    
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
            _targetVelocity.Z = direction.Z * Speed;
        }
        else {
            //unsure of this being necessary
            // source: line 37 and 38 in https://github.com/Shidoengie/FPS-template-gd4/blob/main/Scenes/Player/player.gd
            _targetVelocity = Vector3.Zero;
        }
        Velocity = _targetVelocity;
        MoveAndSlide();





    }

    // public override void _Input(InputEvent @event) {
    //     if (@event is InputEventMouseMotion MMEvent) {
    //         // unrelated, but c# syntax is really nice, i didnt have to google this one
    //         Rotation.Y -= MMEvent.Relative.X / MouseSensitivity;
    //         //Camera.Rotation.X = MMEvent.Relative.Y / MouseSensitivity;
    //
    //     }
    // }
}
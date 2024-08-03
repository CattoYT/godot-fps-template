using Godot;
using System;
using static Godot.Mathf; 

public partial class player : CharacterBody3D {
    [Export] public int Speed { get; set; } = 5;
    [Export] public float JUMPVELOCITY { get; set; } = 4.5F;

    [Export] public PackedScene Bullet;

    [Export] public float MouseSensitivity { get; set; } = 1200;
    
    float gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    
    private Vector3 _targetVelocity = Vector3.Zero;
    private Vector3 Rotation = Vector3.Zero;
    
    private Camera3D Camera;
    private RayCast3D gunRay;
    private MeshInstance3D playerMesh;
    
    



    // from the source file, velocity = _targetVelocity


    public override void _Ready() {
        gunRay = GetNode<RayCast3D>("Head/Camera3d/RayCast3d");
        Camera = GetNode<Camera3D>("Head/Camera3d");
        playerMesh = GetNode<MeshInstance3D>("MeshInstance3d");
        gunRay.AddException(this);
        Bullet = ResourceLoader.Load<PackedScene>("res://Scenes/Bullet/Bullet.tscn");
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
            shoot();
        }

        Vector2 inputDirection = Input.GetVector("moveLeft", "moveRight", "moveUp", "moveDown");
        var direction = (Camera.Transform.Basis * new Vector3(inputDirection.X, 0, inputDirection.Y)).Normalized();
        if (direction != Vector3.Zero) {
            _targetVelocity.X = direction.X * Speed;
            _targetVelocity.Z = direction.Z * Speed;
        }
        else {
            //unsure of this being necessary
            // source: line 37 and 38 in https://github.com/Shidoengie/FPS-template-gd4/blob/main/Scenes/Player/player.gd
            _targetVelocity.X = 0;
            _targetVelocity.Z = 0;
        }
        
        Velocity = _targetVelocity;
        MoveAndSlide();





    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion MMEvent) {
            // unrelated, but c# syntax is really nice, i didnt have to google this one
            Rotation.Y -=MMEvent.Relative.X / MouseSensitivity;
            Rotation.X -= MMEvent.Relative.Y / MouseSensitivity;
            Rotation.X = Clamp(Rotation.X, DegToRad(-90), DegToRad(90));
            Camera.Rotation = Rotation;
            playerMesh.Rotation = new Vector3(Rotation.X, 0, 0);
            
    
        }
    }

    private void shoot() {
        if (!gunRay.IsColliding()) {
            return;
        }

        var BulletInstance = (Node3D)Bullet.Instantiate();

        BulletInstance.TopLevel = true;
        GetParent().AddChild(BulletInstance);
        BulletInstance.Position = gunRay.GetCollisionPoint(); //test, unsure
        BulletInstance.LookAt((gunRay.GetCollisionPoint()+gunRay.GetCollisionPoint()), Vector3.Back);
        GD.Print(gunRay.GetCollisionPoint());
        GD.Print(gunRay.GetCollisionPoint()+gunRay.GetCollisionNormal());

    }
}
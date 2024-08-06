using Godot;
using System;


[Tool]
public partial class ColoredBlocka : StaticBody3D {
// this literally didnt work with inheritance, so enjoy the copypaste

    [Signal]
    public delegate void TransformChangedEventHandler();
    

    // someone pls make a pr to fix these im 90% sure its bad practise
    [Export]
    public Color BlockColorSetter {
        get => BlockColor;
        set {
            BlockColor = value;
            _UpdateMesh();
        }
    }
    protected Color BlockColor = Colors.White;

    private MeshInstance3D _mesh;

    public override void _Ready() {
        _mesh = GetNode<MeshInstance3D>("Mesh");
        
        _mesh.SetSurfaceOverrideMaterial(0, (BaseMaterial3D)GD.Load("res://addons/devblocks/blocks/block_material.tres").Duplicate(true));
        _UpdateMesh();
        
        
        _mesh.SetNotifyLocalTransform(true);
        Connect(nameof(TransformChangedEventHandler), new Callable(this, nameof(_UpdateMesh)));
    }

    private void _notification(int reason) {
        if (reason == NotificationTransformChanged) {
            EmitSignal(nameof(TransformChangedEventHandler));
        }
            
    }
    
    //TODO: Live updating in the editor
    protected void _UpdateMesh() {
        if (_mesh == null) {
            return;
        }

        Material mat = _mesh.GetSurfaceOverrideMaterial(0);
        if (mat == null) {
            return;
        }
        
        
        MeshInstance3D mesh = GetNode<MeshInstance3D>("Mesh");
        if (mesh != null) {
            mesh.GetSurfaceOverrideMaterial(0).Set("albedo_color", BlockColor);
            GD.Print("Set");
        }

    }







    

}
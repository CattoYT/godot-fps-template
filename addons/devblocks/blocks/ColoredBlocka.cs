using Godot;
using System;


[Tool]
public partial class ColoredBlocka : StaticBody3D {
// this literally didnt work with inheritance, so enjoy the copypaste

    

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
    }

    private void _notification(int reason) {
        if (reason == NotificationTransformChanged) {
        }
            
    }
    
    //TODO: Live updating in the editor
    protected void _UpdateMesh() {

        
        
        MeshInstance3D mesh = GetNode<MeshInstance3D>("Mesh");
        if (mesh != null) {
            mesh.GetSurfaceOverrideMaterial(0).Set("albedo_color", BlockColor);
            GD.Print("Set");
        }

    }







    

}
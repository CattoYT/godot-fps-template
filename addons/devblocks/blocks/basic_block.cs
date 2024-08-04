using System;
using Godot;
public enum DEVBLOCK_COLOR_GROUP {DARK, GREEN, LIGHT, ORANGE, PURPLE, RED}
// _devblock_color_to_foldername will be aquired from .ToLower ig
    
public enum DEVBLOCK_STYLE {
    DEFAULT,
    CROSS,
    CONTRAST,
    DIAGONAL,
    DIAGONAL_FADED,
    GROUPED_CROSS,
    GROUPED_CHECKERS,
    CHECKERS,
    CROSS_CHECKERS,
    STAIRS,
    DOOR,
    WINDOW,
    INFO
}

[Tool]
public partial class BasicBlock : StaticBody3D {
    
    [Signal]
    public delegate void TransformChanged();
    
    const String TextureDirectory = "res://addons/devblocks/textures/";
    


    [Export]
    public DEVBLOCK_COLOR_GROUP BlockColorSetter {
        get => BlockColor;
        set {
            BlockColor = value;
            _UpdateMesh();
        }
    }
    private DEVBLOCK_COLOR_GROUP BlockColor = DEVBLOCK_COLOR_GROUP.DARK;
    
    [Export]
    public DEVBLOCK_STYLE BlockStyleSetter {
        get => BlockStyle;
        set {
            BlockStyle = value;
            _UpdateMesh();
        }
    }
    private DEVBLOCK_STYLE BlockStyle = DEVBLOCK_STYLE.DEFAULT;

    private MeshInstance3D _mesh;

    public override void _Ready() {
        _mesh = GetNode<MeshInstance3D>("Mesh");
        
        _mesh.SetSurfaceOverrideMaterial(0, (Material)GD.Load("res://addons/devblocks/blocks/block_material.tres").Duplicate(true));
        _UpdateMesh();
        _UpdateUvs();
        _mesh.SetNotifyLocalTransform(true);
        Connect(nameof(TransformChanged), new Callable(this, nameof(_UpdateUvs)));
    }

    private void _notification(int reason) {
        if (reason == NotificationTransformChanged) {
            EmitSignal(nameof(TransformChanged));
        }
            
    }
    

    private void _UpdateMesh() {
        if (_mesh == null) {
            return;
        }

        Material mat = _mesh.GetSurfaceOverrideMaterial(0);
        if (mat == null) {
            return;
        }

        //int TextureI = BlockStyleSetter + 1;
    }
    


    private void _UpdateUvs() { }


}
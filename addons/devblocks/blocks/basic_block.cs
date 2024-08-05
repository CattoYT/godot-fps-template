using System;
using Godot;
using static Godot.Mathf;


public partial class BasicBlock : StaticBody3D {
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
    
    [Signal]
    public delegate void TransformChangedEventHandler();
    
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
        GD.Print("A");
        _mesh = GetNode<MeshInstance3D>("Mesh");
        
        _mesh.SetSurfaceOverrideMaterial(0, (Material)GD.Load("res://addons/devblocks/blocks/block_material.tres").Duplicate(true));
        _UpdateMesh();
        _UpdateUvs();
        _mesh.SetNotifyLocalTransform(true);
        Connect(nameof(TransformChangedEventHandler), new Callable(this, nameof(_UpdateUvs)));
    }

    private void _notification(int reason) {
        if (reason == NotificationTransformChanged) {
            EmitSignal(nameof(TransformChangedEventHandler));
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

        int TextureI = (int)BlockStyle + 1; //line from gpt because I had no idea you could cast enums to ints lol
        string textureString;
        if (TextureI < 10) {
            textureString = "0";
        }
        else {
            textureString = "";
        }
        textureString += TextureI.ToString();
        string texturePath = TextureDirectory + 
                             BlockStyle.ToString().ToLower() + "/" + 
                             "texture_" + 
                             textureString + 
                             ".png";
        
        Resource Texture = GD.Load(texturePath);
        if (Texture == null) {
            return; // might want to error this actually, may change
        }
        
        _mesh.GetSurfaceOverrideMaterial(0).Set("albedo_texture", Texture);

    }





    private void _UpdateUvs() {
        Material material = _mesh.GetSurfaceOverrideMaterial(0);
        Vector3 offset = Vector3.Zero;
        Vector3 scale = new Vector3(Scale.X, Scale.Y, Scale.Z);
        GD.Print("PLEASE");
        for (int i = 0; i == 3; i++) {
            bool diffOffset1 = Scale[i] % 2.0 >=0.99;
            bool diffOffset2 = Scale[i] % 1.0 >=0.49;
            
            // I don't know how to do funky 1 liners in c# sooooooo
            if (diffOffset1) offset[i] = 0.5F;
            else offset[i] = 1F;
            //do these all suffice as funky 1 liners, past me?
            if (diffOffset2) offset[i] -= 0.25F;
            

        }
        GetNode<MeshInstance3D>("Mesh").GetSurfaceOverrideMaterial(0).Set("uv2_triplanar", true); //not working
        GetNode<MeshInstance3D>("Mesh").MaterialOverride.Set("uv1_offset", offset); // not working
    }
        

}
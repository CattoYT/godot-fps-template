using System;
using Godot;
using static Godot.Mathf;

[Tool]
public partial class basic_block : StaticBody3D { //Reminder to self to make sure the class name matches so i don't go insane again
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
    
    protected const String TextureDirectory = "res://addons/devblocks/textures/";
    

    // someone pls make a pr to fix these im 90% sure its bad practise
    [Export]
    public DEVBLOCK_COLOR_GROUP BlockColorSetter {
        get => BlockColor;
        set {
            BlockColor = value;
            _UpdateMesh();
        }
    }
    protected DEVBLOCK_COLOR_GROUP BlockColor = DEVBLOCK_COLOR_GROUP.DARK;
    
    [Export]
    public DEVBLOCK_STYLE BlockStyleSetter {
        get => BlockStyle;
        set {
            BlockStyle = value;
            _UpdateMesh();
        }
    }
    protected DEVBLOCK_STYLE BlockStyle = DEVBLOCK_STYLE.DEFAULT;

    private MeshInstance3D _mesh;

    public override void _Ready() {
        _mesh = GetNode<MeshInstance3D>("Mesh");
        
        _mesh.SetSurfaceOverrideMaterial(0, (BaseMaterial3D)GD.Load("res://addons/devblocks/blocks/block_material.tres").Duplicate(true));
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
    

    protected void _UpdateMesh() {
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
                             BlockColor.ToString().ToLower() + "/" + 
                             "texture_" + 
                             textureString + 
                             ".png";
        GD.Print(texturePath);

        
        Resource Texture = GD.Load(texturePath);
        if (Texture == null) {
            return; // might want to error this actually, may change
        }
        
        _mesh.GetSurfaceOverrideMaterial(0).Set("albedo_texture", Texture);

    }





    protected void _UpdateUvs() {
        Material material = _mesh.GetSurfaceOverrideMaterial(0);
        Vector3 offset = Vector3.Zero;
        Vector3 scale = new Vector3(Scale.X, Scale.Y, Scale.Z);

        for (int i = 0; i == 3; i++) {
            bool diffOffset1 = Scale[i] % 2.0 >=0.99;
            bool diffOffset2 = Scale[i] % 1.0 >=0.49;
            
            // I don't know how to do funky 1 liners in c# sooooooo
            if (diffOffset1) offset[i] = 0.5F;
            else offset[i] = 1F;
            //do these all suffice as funky 1 liners, past me?
            if (diffOffset2) offset[i] -= 0.25F;
            

        }
        
        material.Set("uv1_scale", scale);
        material.Set("uv1_offset", offset);
    }
        

}
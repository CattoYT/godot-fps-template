using Godot;

[Tool]
public partial class colored_block : basic_block {
    [Export]
    public Color BlockFlatColorSetter {
        get => BlockFlatColor;
        set {
            BlockFlatColor = value;
            _UpdateMesh();
        }
    }
    private Color BlockFlatColor = Colors.White;

    protected void _UpdateMesh() {
        base._UpdateMesh();
        MeshInstance3D mesh = GetNode<MeshInstance3D>("Mesh");
        if (mesh != null) {
            mesh.Set("albedo_color", BlockFlatColor);
        }
    }

}
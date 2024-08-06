
using Godot;

[Tool]
public partial class emission_block : ColoredBlocka {

    [Export(PropertyHint.Range, "0,4.0,")] 
    public float EmissionEnergySetter {
        get => EmissionEnergy;
        set {
            EmissionEnergy = value;
            _UpdateMesh();
        }
    }
    protected float EmissionEnergy = 1.0F;
    
    public override void _Ready() {
        base._Ready();
        GetNode<MeshInstance3D>("Mesh").GetSurfaceOverrideMaterial(0).Set("emission_enabled", true);
        _UpdateMesh();
    }



    private void _UpdateMesh() {
        base._UpdateMesh();
            MeshInstance3D mesh = GetNode<MeshInstance3D>("Mesh");
            mesh.GetSurfaceOverrideMaterial(0).Set("emission", base.BlockColorSetter);
            mesh.GetSurfaceOverrideMaterial(0).Set("emission_energy", EmissionEnergy);
    }
}
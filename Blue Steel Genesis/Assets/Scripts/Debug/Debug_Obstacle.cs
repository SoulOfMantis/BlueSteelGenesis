using System.Threading.Tasks;

public class Debug_Obstacle : Obstacle
{
    Debug_Obstacle() : base(2) {
        Name = "Default Obstacle";
        Description = "Breakable!";
    }

    protected override Task die() {
        base.die();
        tracker.RemoveObstacle(this);
        Destroy(gameObject);
        return Task.CompletedTask;
    }
}

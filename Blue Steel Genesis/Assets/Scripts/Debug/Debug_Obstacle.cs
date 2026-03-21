using System.Threading.Tasks;

public class Debug_Obstacle : Obstacle
{
    Debug_Obstacle() {
        currentHealth = new(2, 2);
    }

    protected override Task die() {
        tracker.RemoveObstacle(this);
        Destroy(gameObject);
        return Task.CompletedTask;
    }
}

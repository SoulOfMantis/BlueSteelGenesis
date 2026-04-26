public static class SuccessChecker
{
    private static System.Random random = new System.Random();

    
    public static bool RollSuccess(int chancePercent)
    {
        int roll = random.Next(1, 101);
        return roll <= chancePercent;
    }
}
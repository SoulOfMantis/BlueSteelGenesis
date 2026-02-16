public class Expedition
{
    static Expedition instance;
    static void CreateInstance()
    {
        instance = new Expedition();
    }
    static GameModule DrawNextModule()
    {
        instance.moduleGeneration.DrawNextModule();
    }
    internal class ModuleGeneration
    {
        private Random gen;
        public ModuleGeneration(int Seed)
        {
            seed = Seed;
            gen = new Random(seed);
        }
        public GameModule DrawNextModule()
        {
            //TODO
            return new GameModule();
        }
    }
    ModuleGeneration moduleGeneration;
}
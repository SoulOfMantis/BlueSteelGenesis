public class Expedition
{
    internal class ModuleGeneration
    {
        private Random gen;
        private int seed;
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
using System;
using System.Collections.Generic;

    public interface IResourceHandler
    {
        public event Action ResourceAmountChanged;
        public event Action ResourcesCleared;
        
        /*public List<Resource> CurrentIronAmount { get; }
        public List<Resource> CurrentCrystalAmount { get; }
        public List<Resource> CurrentPlantAmount { get; }
        public List<Resource> CurrentAlienArtifactAmount { get; }*/
        public IReadOnlyDictionary<Type, int> CurrentResourceCatched { get; }
        public Dictionary<Type, int> GetAllResources();
        public void SetResourceAmount(Dictionary<Type, int> resources);
    }

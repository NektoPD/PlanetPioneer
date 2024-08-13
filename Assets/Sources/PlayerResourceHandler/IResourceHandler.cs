using System;
using System.Collections.Generic;

    public interface IResourceHandler
    {
        public event Action ResourceAdded;
        public event Action ResourcesCleared;
        public event Action<int> IronAmountChanged;
        public event Action<int> CrystalAmountChanged;
        public event Action<int> PlantAmountChanged;
        public event Action<int> AlienArtifactAmountChanged;
        
        public List<Resource> CurrentIronAmount { get; }
        public List<Resource> CurrentCrystalAmount { get; }
        public List<Resource> CurrentPlantAmount { get; }
        public List<Resource> CurrentAlienArtifactAmount { get; }

        public Dictionary<Type, List<Resource>> GetAllResources();
        public void SetResourceAmount(List<Resource> resourcesToAdd);
    }

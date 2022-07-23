namespace CardGrid
{
    //Add new maps
    public enum Maps
    {
        Row3,
        Cross3,
        Cross5,
        Diagonals,
        One,
        NoImpact,
        AllNeighboring,
        DirectNeighboring
    }
    
    //For a valid center map can be 1X1 or 3X3 or 5X5 only
    public  class ImpactMaps
    {
        
        
        public static int[,] Row3 =
        {
            {0, 0, 0},
            {1, 1, 1},
            {0, 0, 0}
        };
        
        public static int[,] Cross3 =
        {
            {0, 1, 0},
            {1, 1, 1},
            {0, 1, 0}
        };

        public static int[,] Cross5 =
        {
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0},
            {1, 1, 1, 1, 1},
            {0, 0, 1, 0, 0},
            {0, 0, 1, 0, 0}
        };

        public static int[,] Diagonals =
        {
            {1, 0, 1},
            {0, 0, 0},
            {1, 0, 1}
        };
        
        public static int[,] One =
        {
            {1}
        };

        public static int[,] NoImpact =
        {
            {0}
        };
        
        public static int[,] AllNeighboring =
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
        
        public static int[,] DirectNeighboring =
        {
            {0, 1, 0},
            {1, 0, 1},
            {0, 1, 0}
        };
    }
}
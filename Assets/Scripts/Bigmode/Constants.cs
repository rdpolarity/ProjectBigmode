using System;
using System.Collections.Generic;

namespace Bigmode
{
    public static class Constants
    {
	    // Lists

        public static readonly Dictionary<Type, string> DefTypes = new () 
        {
            { typeof(TiledStructureDef), DefTiledStructure },
            { typeof(GameObjectStructureDef), DefGameObjectStructure },
        };

        public static readonly List<RotationAction> RotationActions = new()
        {
            RotationAction.Normal,
            RotationAction.Left,
            RotationAction.Right,
            RotationAction.Back
        };
		
        // Defs
		
        public const string DefTiledStructure = "TiledStructure";
        
        public const string DefGameObjectStructure = "GameObjectStructure";
        
    }
}
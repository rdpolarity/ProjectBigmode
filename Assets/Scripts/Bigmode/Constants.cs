using System;
using System.Collections.Generic;
using Unity.Transforms;

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

        public static class Tags
        {
            public const string Health = "Attributes.Health";
            public const string MaxHealth = "Attributes.MaxHealth";
            public const string Speed = "Attributes.Speed";
            public const string Damage = "Attributes.Damage";
        }
        
    }
}
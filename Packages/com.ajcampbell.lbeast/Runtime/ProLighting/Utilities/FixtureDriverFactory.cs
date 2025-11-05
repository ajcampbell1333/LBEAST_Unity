// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

namespace LBEAST.ProLighting
{
    /// <summary>Factory for creating fixture drivers based on fixture type</summary>
    public static class FixtureDriverFactory
    {
        public static IFixtureDriver Create(LBEASTDMXFixtureType type)
        {
            return type switch
            {
                LBEASTDMXFixtureType.Dimmable => new FixtureDriverDimmable(),
                LBEASTDMXFixtureType.RGB => new FixtureDriverRGB(),
                LBEASTDMXFixtureType.RGBW => new FixtureDriverRGBW(),
                LBEASTDMXFixtureType.MovingHead => new FixtureDriverMovingHead(),
                LBEASTDMXFixtureType.Custom => new FixtureDriverCustom(),
                _ => new FixtureDriverDimmable()
            };
        }
    }
}


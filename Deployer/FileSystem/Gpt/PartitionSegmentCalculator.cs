﻿using System;

namespace Deployer.FileSystem.Gpt
{
    public class PartitionSegmentCalculator
    {
        private readonly uint chunkSize;
        private readonly ulong totalSectors;

        public PartitionSegmentCalculator(uint chunkSize, ulong totalSectors)
        {
            this.chunkSize = chunkSize;
            this.totalSectors = totalSectors;
        }

        public GptSegment Constraint(GptSegment desired)
        {
            var size = SmallerDivisible(desired.Length, chunkSize);

            var finalFirst = SmallerDivisible(desired.Start, chunkSize);
            var finalLast = Math.Min(finalFirst + size, totalSectors - 1 * chunkSize);
            var finalLength = finalLast - finalFirst;
            return new GptSegment(finalFirst, finalLength);
        }

        private static ulong SmallerDivisible(ulong x, ulong y)
        {
            return x / y * y;
        }

        public ulong GetNextSector(ulong lastSector)
        {
            if (lastSector == 0)
            {
                return 0;
            }

            var boundary = SmallerDivisible(lastSector + 1, chunkSize);
            return boundary + chunkSize;
        }
    }
}
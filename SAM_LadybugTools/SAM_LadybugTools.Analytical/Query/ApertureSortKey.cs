// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors
using SAM.Geometry.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.LadybugTools
{
    public static partial class Query
    {
        /// <summary>
        /// Deterministic geometry sort key used to pair SAM apertures with their Honeybee
        /// counterparts after SAM-side aperture regeneration (lexicographic centroid order).
        /// </summary>
        public static Tuple<double, double, double, int> ApertureSortKey(this Aperture aperture)
        {
            Face3D face3D = aperture?.GetFace3D();
            Point3D centroid = face3D?.GetCentroid();
            int count = face3D?.GetExternalEdge3D() is ISegmentable3D segmentable3D ? segmentable3D.GetPoints()?.Count ?? 0 : 0;

            return new Tuple<double, double, double, int>(
                centroid == null ? double.NaN : System.Math.Round(centroid.X, 6),
                centroid == null ? double.NaN : System.Math.Round(centroid.Y, 6),
                centroid == null ? double.NaN : System.Math.Round(centroid.Z, 6),
                count);
        }

        /// <summary>
        /// Deterministic geometry sort key for a Honeybee aperture or door
        /// (lexicographic centroid order, matching the SAM overload).
        /// </summary>
        public static Tuple<double, double, double, int> ApertureSortKey(this HoneybeeSchema.IDdBaseModel dDBaseModel)
        {
            HoneybeeSchema.Face3D geometry = (dDBaseModel as HoneybeeSchema.Aperture)?.Geometry ?? (dDBaseModel as HoneybeeSchema.Door)?.Geometry;
            List<List<double>> boundary = geometry?.Boundary;
            if (boundary == null || boundary.Count == 0)
            {
                return new Tuple<double, double, double, int>(double.NaN, double.NaN, double.NaN, 0);
            }

            double x = boundary.Average(point => point[0]);
            double y = boundary.Average(point => point[1]);
            double z = boundary.Average(point => point[2]);

            return new Tuple<double, double, double, int>(
                System.Math.Round(x, 6),
                System.Math.Round(y, 6),
                System.Math.Round(z, 6),
                boundary.Count);
        }
    }
}

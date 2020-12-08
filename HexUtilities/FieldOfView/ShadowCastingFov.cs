﻿#region Copyright (c) 2012-2019 Pieter Geerkens (email: pgeerkens@users.noreply.github.com)
///////////////////////////////////////////////////////////////////////////////////////////
// THis software may be used under the terms of attached file License.md (The MIT License).
///////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using PGNapoleonics.HexUtilities.Common;

namespace PGNapoleonics.HexUtilities.FieldOfView {
    /// <summary>Credit: Eric Lippert</summary>
    /// <remarks>
    /// It is important for realiism that observerHeight > board[observerCoords].ElevationASL, 
    /// or no parallax over the current ground elevation will be observed. TerrainHeight is the 
    /// ElevationASL of the hex, plus the height of any blocking in the hex, usually due to terrain 
    /// but sometimes occuppying units will block vision as well. Control this in the implementation 
    /// of IFovBoard {IHex} with the definition of the observerHeight, targetHeight, and 
    /// terrainHeight delegates.
    /// 
    /// See: <a href="http://blogs.msdn.com/b/ericlippert/archive/2011/12/29/shadowcasting-in-c-part-six.aspx">
    /// Shadow Casting in C# Part Six</a>
    /// </remarks>
    public static partial class ShadowCasting {
        /// <summary>Get or set whether to force serial execution of FOV calculation.</summary>
        /// <remarks>Defaults true when DEBUG defined; otherwise false.</remarks>
        public static bool InSerial   { get; set; }

        /// <summary>Set to use decimeters as height unit instead of feet.</summary>
        public static bool UseMetric  { get; set; }

        /// <summary>Calculate Field-of-View from a specified TargetMode, assuming a flat earth.</summary>
        /// <param name="board">A reference to an IFovBoard {IHex} instance.</param>
        /// <param name="coordsObserver">Cordinates of observer;s hex.</param>
        /// <param name="fovRadius">Radius of Field-of-view desired.</param>
        /// <param name="targetMode">TargetMode value for determining target visibility.</param>
        /// <param name="setFieldOfView">Sets a hex as visible in the Field-of-View.</param>
        public static void ComputeFieldOfView(
            IFovBoard         board,
            HexCoords         coordsObserver,
            int               fovRadius,
            FovTargetMode     targetMode, 
            Action<HexCoords> setFieldOfView
        )
        => ComputeFieldOfView(board, coordsObserver, fovRadius, targetMode, setFieldOfView, 1, 0);

        /// <summary>Calculate Field-of-View from a specified TargetMode, assuming a flat earth.</summary>
        /// <param name="board">A reference to an IFovBoard {IHex} instance.</param>
        /// <param name="coordsObserver">Cordinates of observer;s hex.</param>
        /// <param name="fovRadius">Radius of Field-of-view desired.</param>
        /// <param name="targetMode">TargetMode value for determining target visibility.</param>
        /// <param name="setFieldOfView">Sets a hex as visible in the Field-of-View.</param>
        /// <param name="defaultHeight">Height used for observer and target when targetMode = EqualHeights/</param>
        public static void ComputeFieldOfView(
            IFovBoard         board,
            HexCoords         coordsObserver,
            int               fovRadius,
            FovTargetMode     targetMode, 
            Action<HexCoords> setFieldOfView,
            int               defaultHeight
        )
        => ComputeFieldOfView(board, coordsObserver, fovRadius, targetMode, setFieldOfView, defaultHeight, 0);

        /// <summary>Calculate Field-of-View from a specified TargetMode, assuming a spherical earth
        /// and height measured in feet if <code>hexesPerMile</code> is not equal 0.</summary>
        /// <param name="board">A reference to an IFovBoard {IHex} instance.</param>
        /// <param name="coordsObserver">Cordinates of observer;s hex.</param>
        /// <param name="fovRadius">Radius of Field-of-view desired.</param>
        /// <param name="targetMode">TargetMode value for determining target visibility.</param>
        /// <param name="setFieldOfView">Sets a hex as visible in the Field-of-View.</param>
        /// <param name="defaultHeight">Height used for observer and target when targetMode = EqualHeights/</param>
        /// <param name="hexesPerMile">Number of hexes per mile (ie 1/4000 of planet radius).</param>
        /// <remarks>Adjusts visibility for curvature of the Earth. This is the only version of 
        /// ComputeFieldOfView that is <b>not</b> scale invariant for height, and assumes that height
        /// is measured in feet.
        /// </remarks>
        /// <a href="http://mathcentral.uregina.ca/qq/database/QQ.09.02/shirley3.html">Hidden by the Curvature of the Earth</a>
        public static void ComputeFieldOfView(
            IFovBoard         board, 
            HexCoords         coordsObserver, 
            int               fovRadius,
            FovTargetMode     targetMode, 
            Action<HexCoords> setFieldOfView, 
            int               defaultHeight, 
            int               hexesPerMile
        ) {
            if (fovRadius > int.MinValue) fovRadius = fovRadius - 1;
            int CalculationHeightUnits = UseMetric ?   5  // convert metres to  cm  for greater precision
                                                   :  12; // convert feet to inches for greater precision

            Func<HexCoords,int> elevationTarget;
            int                 elevationObserver;
            switch (targetMode) {
                case FovTargetMode.TargetHeightEqualZero:
                    elevationTarget   = coords => board.ElevationGroundASL(coords);
                    elevationObserver = board.ElevationObserverASL(coordsObserver);
                    break;

                default:
                case FovTargetMode.TargetHeightEqualActual:
                    elevationTarget   = coords => board.ElevationTargetASL(coords);
                    elevationObserver = board.ElevationObserverASL(coordsObserver);
                    break;

                case FovTargetMode.EqualHeights: 
                    elevationTarget   = coords => board.ElevationGroundASL(coords) + defaultHeight;
                    elevationObserver = elevationTarget(coordsObserver);
                    break;
            }

            var deltaHeight = GetDeltaHeight(coordsObserver,hexesPerMile,CalculationHeightUnits);

            ComputeFieldOfView(
                coordsObserver, 
                fovRadius, 
                elevationObserver * CalculationHeightUnits,
                hc => board.MapSizeHexes.IsOnboard(hc),
                coords => elevationTarget(coords) * CalculationHeightUnits - deltaHeight(coords),
                (coords,hexside) => board.ElevationHexsideASL(coords,hexside) * CalculationHeightUnits - deltaHeight(coords),
                setFieldOfView
            );
        }

        private static Func<HexCoords,int> GetDeltaHeight(
            HexCoords coordsObserver,
            int       hexesPerMile,
            int       calculationHeightUnits
        ) {
            if (hexesPerMile == 0) {
                return coords => 0;
            } else if(UseMetric) {
                return coords => coordsObserver.Range(coords) * coordsObserver.Range(coords)
                               * calculationHeightUnits * 2 / (9 * hexesPerMile * hexesPerMile);
            } else {
                return coords => coordsObserver.Range(coords) * coordsObserver.Range(coords)
                               * calculationHeightUnits * 2 / (3 * hexesPerMile * hexesPerMile);
            }
        }

        /// <summary>Calculate Field-of-View from a detailed prescription.</summary>
        /// <param name="coordsObserver">Cordinates of observer;s hex.</param>
        /// <param name="radius">Maximum radius for Field-of-View.</param>
        /// <param name="heightObserver">Height (ASL) of the observer's eyes.</param>
        /// <param name="isOnboard">Returns whether this hex on the baoard.</param>
        /// <param name="heightTarget">Returns ground level (ASL) of supplied hex.</param>
        /// <param name="heightTerrain">Returns height (ASL) of terrain in supplied hex.</param>
        /// <param name="setFieldOfView">Sets a hex as visible in the Field-of-View.</param>
        private static void ComputeFieldOfView(
            HexCoords                   coordsObserver, 
            int                         radius, 
            int                         heightObserver,
            Func<HexCoords,bool>        isOnboard,
            Func<HexCoords,int>         heightTarget, 
            Func<HexCoords,Hexside,int> heightTerrain,
            Action<HexCoords>           setFieldOfView
        ) {
            FieldOfViewTrace(true, $" - Coords = {coordsObserver.User.ToString()}");
            var matrixOrigin = new IntMatrix2D(coordsObserver.Canon);

            if (radius >= 0) setFieldOfView(coordsObserver);    // Always visible to self!

            void dodecantFov(Dodecant d) {
                var dodecant = new Dodecant(d,matrixOrigin);
                _mapCoordsDodecant = hex => dodecant.TranslateDodecant<HexCoords>(v => v)(hex);

                ComputeFieldOfViewInDodecantZero(
                    radius,
                    heightObserver,
                    dodecant.TranslateDodecant(isOnboard),
                    dodecant.TranslateDodecant(heightTarget),
                    dodecant.TranslateDodecant(heightTerrain),
                    dodecant.TranslateDodecant(setFieldOfView)
                );
            }
            #if DEBUG
                InSerial = true;
            #endif
            if (InSerial) {
                Dodecant.Dodecants.ForEach(dodecantFov);
            } else {
                Parallel.ForEach(Dodecant.Dodecants, dodecantFov);
            }
        }

        /// <summary>TODO</summary>
        /// <param name="radius"></param>
        /// <param name="heightObserver"></param>
        /// <param name="isOnboard"></param>
        /// <param name="heightTarget"></param>
        /// <param name="heightTerrain"></param>
        /// <param name="setFieldOfView"></param>
        private static void ComputeFieldOfViewInDodecantZero(
            int                         radius,
            int                         heightObserver,
            Func<HexCoords,bool>        isOnboard,
            Func<HexCoords,int>         heightTarget,
            Func<HexCoords,Hexside,int> heightTerrain,
            Action<HexCoords>           setFieldOfView
        ) {
            var currentCoords = HexCoords.NewCanonCoords(0,1);
            if ( ! isOnboard(currentCoords) ) return;

            if (radius > 0) setFieldOfView(currentCoords);

            var queue   = new FovConeQueue();
            var current = new FovCone(
                    2,
                    new IntVector2D(1,2), 
                    new IntVector2D(0,1), 
                    new RiseRun(2 * (heightTerrain(currentCoords,Hexside.North) - heightObserver), 1) );
            while (current.Range <= radius) {
                current = ComputeFoVForRange(heightObserver, isOnboard, heightTarget, heightTerrain, setFieldOfView, queue, current);
            }
        }

        /// <summary>Processes the supplied FovCone and returns the next FovCone to process.</summary>
        /// <param name="heightObserver"></param>
        /// <param name="isOnboard"></param>
        /// <param name="heightTarget"></param>
        /// <param name="heightTerrain"></param>
        /// <param name="setFieldOfView"></param>
        /// <param name="queue"></param>
        /// <param name="cone"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method: 
        /// (1) marks points inside the cone-arc that are within the radius of the field 
        ///     of view; and 
        /// (2) computes which portions of the following column are in the field of view, 
        ///     queueing them for later processing. 
        ///
        /// This algorithm is "center-to-center"; a more sophisticated algorithm 
        /// would say that a cell is visible if neighbour is *any* straight line segment that 
        /// passes through *any* portion of the origin cell and any portion of the target 
        /// cell, passing through only transparent cells along the way. This is the 
        /// "Permissive Field Of View" algorithm, and it is much harder to implement.
        ///
        /// Search for transitions from opaque to transparent or transparent to opaque and 
        /// use those to determine what portions of the *next* column are visible from the 
        /// origin.
        /// </remarks>
        private static FovCone ComputeFoVForRange(
            int                         heightObserver, 
            Func<HexCoords,bool>        isOnboard, 
            Func<HexCoords,int>         heightTarget, 
            Func<HexCoords,Hexside,int> heightTerrain, 
            Action<HexCoords>           setFieldOfView, 
            FovConeQueue                queue, 
            FovCone                     cone
        ) {
            Action<FovCone> enqueue = queue.Enqueue;

            var range         = cone.Range;
            var topVector     = cone.VectorTop;
            var topRiseRun    = cone.RiseRun;
            var bottomVector  = cone.VectorBottom;

            // track the overlap-cone between adjacent hexes as we move down.
            var overlapVector = cone.VectorTop;
            var hexX          = XFromVector(range, topVector);
            FieldOfViewTrace(false, $"DQ:   ({cone}) from {hexX}");

            do {
                while (overlapVector.GT(bottomVector)) {
                    var coordsCurrent   = HexCoords.NewCanonCoords(hexX,range);
                    var hexVectorBottom = VectorHexBottom(coordsCurrent);
                    if (isOnboard(coordsCurrent)) { 
                        #region Set current hex parameters
                        var hexVectorTop  = VectorHexTop(coordsCurrent);
                        var hexElevation  = heightTarget(coordsCurrent);
                        var hexHeight     = heightTerrain(coordsCurrent,Hexside.North);
                        var hexRiseRun    = new RiseRun(hexHeight-heightObserver, range);
                        #endregion

                        #region Check visibility of current hex
                        var riseRun = new RiseRun(hexElevation-heightObserver, coordsCurrent.RangeFromOrigin);
                        if ( riseRun >= cone.RiseRun  
                        && bottomVector.LE(coordsCurrent.Canon) && coordsCurrent.Canon.LE(topVector)  
                        ) {
                            setFieldOfView(coordsCurrent);
                            FieldOfViewTrace(false,
                                $"    Set visible: {_mapCoordsDodecant(coordsCurrent)} / {coordsCurrent.ToString()}; {riseRun} >= {cone.RiseRun}");
                        }
                        #endregion

                        #region Check hex transition
                        if (hexRiseRun > topRiseRun) {
                            topVector  = LogAndEnqueue(enqueue, range, topVector, hexVectorTop, topRiseRun, 0);
                            topRiseRun = hexRiseRun;
                        } else if (hexRiseRun > cone.RiseRun) {
                            topVector  = LogAndEnqueue(enqueue, range, topVector, overlapVector, topRiseRun, 1);
                            topRiseRun = hexRiseRun;
                        } else if (hexRiseRun < cone.RiseRun) {
                            topVector  = LogAndEnqueue(enqueue, range, topVector, overlapVector, topRiseRun, 2);
                            topRiseRun = cone.RiseRun;
                        }
                        #endregion
                    }

                    overlapVector = VectorMax(hexVectorBottom, bottomVector); 
                    if (hexVectorBottom.GT(bottomVector))      -- hexX;
                }

                #region Dequeue next cone
                if (queue.Count == 0) {
                    topVector   = LogAndEnqueue(enqueue, range, topVector, bottomVector, topRiseRun, 3);
                    cone        = queue.Dequeue();
                    break;
                } else {
                    cone        = queue.Dequeue();
                    if(cone.Range != range) {
                        topVector = LogAndEnqueue(enqueue, range, topVector, bottomVector, topRiseRun, 4);
                        break;
                    }
                    FieldOfViewTrace(false, $"DQ:   ({cone}) from {hexX}");
                }
                #endregion

                #region Check cone transition
                if (cone.RiseRun > topRiseRun) {
                    topVector   = LogAndEnqueue(enqueue, range, topVector, cone.VectorTop, topRiseRun, 5);
                    topRiseRun  = cone.RiseRun;
                } else if (cone.RiseRun < topRiseRun) {
                    topVector   = LogAndEnqueue(enqueue, range, topVector, overlapVector, topRiseRun, 6);
                    topRiseRun  = cone.RiseRun;
                }
                #endregion

                overlapVector = topVector;
                bottomVector  = cone.VectorBottom;
            } while(true);

            // Pick-up any cone portion at bottom of range still unprocessed
            if (topVector.GT(bottomVector)) {
                LogAndEnqueue(enqueue, range, topVector, bottomVector, cone.RiseRun, 7);
            }
            return cone;
        }

        #region Tracing partial methods
        /// <summary>For tracing: maps HexCoords back to original hex.</summary>
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        static Func<HexCoords, HexCoords> _mapCoordsDodecant;

        static partial void FieldOfViewTrace(string format, params object[] paramArgs);

        static partial void FieldOfViewTrace(bool newline, string format, params object[] paramArgs);

        [Conditional("TRACE")]
        static partial void FieldOfViewTrace(string format, params object[] paramArgs)
        =>  Tracing.FieldOfView.Trace(format, paramArgs);
        
        [Conditional("TRACE")]
        static partial void FieldOfViewTrace(bool newline, string format, params object[] paramArgs)
        =>  Tracing.FieldOfView.Trace(newline, format, paramArgs);
        #endregion
    }
}

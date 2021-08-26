﻿using System.Collections;
using System.Collections.Generic;
using Sandbox;
using System;

namespace Saandy {

    public static class Math2d {

        public const float PI = 3.14159265358979f;

        public const float Deg2Rad = PI / 180f;
        public const float Rad2Deg = 180f / PI;

        public static Vector2 RotateByAngle(Vector2 vector, float angle) {
            float a = (float)Math.Atan2(vector.y, vector.x);
            a -= angle * Deg2Rad;
            return new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
        }

        public static float Angle(Vector2 vector) {
            float a = (float)Math.Atan2(vector.y, vector.x);
            a = (180 * a) / PI - 90f; // deg
            return (360 - (float)Math.Round(a)) % 360;
        }

		public static float Angle3D( Vector3 v1, Vector3 v2, Vector3 up ) {
			var cross = Vector3.Cross( v1, v2 );
			var dot = Vector3.Dot( v1, v2 );
			var angle = Math.Atan2( cross.Length, dot );

			var test = Vector3.Dot( up, cross );
			if ( test < 0.0 ) angle = -angle;
			return (float)angle * Rad2Deg;
		}

		/// <summary>
		/// Get x and y coordinates from index.
		/// </summary>
		public static void FlattenedArrayIndex(int i, int w, out int x, out int y) {
            y = i / w;
            x = i % w;
        }

        public static void DrawCircle(Vector2 p, float r, Color c, float duration = 5f) {
            Vector2 dO = new Vector2(0, 1);
            Vector2 dC = new Vector2();

            float step = 360.0f / 45;
            for (int i = 0; i <= 45; i++) {
                float a = step * i * Deg2Rad;
                dC = new Vector2((float)Math.Sin(a), (float)Math.Cos(a));

				DebugOverlay.Line(p + (dO * r), p + (dC * r), c, duration);
                dO = dC;
            }
        }

		public static void DrawPoint( Vector3 p, Color c, float duration = 5f, float size = 0.025f )
		{
			BBox b = new BBox( p - (Vector3.One * size) / 2, p + (Vector3.One * size) / 2 );
			DebugOverlay.Line( new Vector3(b.Mins.x, b.Mins.y, b.Center.z), new Vector3( b.Maxs.x, b.Maxs.y, b.Center.z ), c, duration );
			DebugOverlay.Line( new Vector3(b.Mins.x, b.Maxs.y, b.Center.z), new Vector3( b.Maxs.x, b.Mins.y, b.Center.z ), c, duration );
		}

		public static bool PointIsOnLine(Vector2 point, Vector2 l_start, Vector2 l_end, float precision = .05f) {
            float id = GetLineIntersectionDistance(point, l_start, l_end);

            if (id <= precision)
                return true;

            return false;
        }

        public static float GetLineIntersectionDistance(Vector2 point, Vector2 l_start, Vector2 l_end) {
            Vector2 pointCross = (l_start - l_end).Normal;

            Vector2 cross = (Vector2)Vector3.Cross(pointCross, Vector3.Forward);
            Vector2 l2_start = point - cross;
            Vector2 l2_end = point + cross;

            Vector2 intersectionPoint;
            LineSegmentsIntersection(l_start, l_end, l2_start, l2_end, out intersectionPoint);

            return new Vector2(point.x - intersectionPoint.x, point.y - intersectionPoint.y).Length;
        }

        public static bool LineSegmentsIntersection(Line a, Line b) {
            return LineSegmentsIntersection(a.pointA, a.pointB, b.pointA, b.pointB, out Vector2 i);
        }

        public static bool LineSegmentsIntersection(Vector2 l1_start, Vector2 l1_end, Vector2 l2_start, Vector3 l2_end, out Vector2 intersectionPoint) {
            intersectionPoint = Vector2.Zero;

            var d = (l1_end.x - l1_start.x) * (l2_end.y - l2_start.y) - (l1_end.y - l1_start.y) * (l2_end.x - l2_start.x);

            if (d == 0.0f) { return false; }

            var u = ((l2_start.x - l1_start.x) * (l2_end.y - l2_start.y) - (l2_start.y - l1_start.y) * (l2_end.x - l2_start.x)) / d;
            var v = ((l2_start.x - l1_start.x) * (l1_end.y - l1_start.y) - (l2_start.y - l1_start.y) * (l1_end.x - l1_start.x)) / d;

            if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f) { return false; }

            intersectionPoint.x = l1_start.x + u * (l1_end.x - l1_start.x);
            intersectionPoint.y = l1_start.y + u * (l1_end.y - l1_start.y);

            return true;
        }

        public static int ClampListIndex(int index, int listSize) {
            index = ((index % listSize) + listSize) % listSize;

            return index;
        }

        public static int GetGreatestCommonFactor(int a, int b) {
            while (b != 0) {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static int GetLeastCommonMultiple(int a, int b) {
            return (a / GetGreatestCommonFactor(a, b)) * b;
        }

        // Euclidian distance between A and B
        public static float Distance(Vector2 A, Vector2 B) {
            return (float)Math.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y));
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t) {
            return a + (b - a) * t;
        }

        public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t) {
            Vector2 p0 = Lerp(a, b, t);
            Vector2 p1 = Lerp(b, c, t);
            return Lerp(p0, p1, t);
        }

        public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t) {
            Vector2 p0 = QuadraticCurve(a, b, c, t);
            Vector2 p1 = QuadraticCurve(b, c, d, t);
            return Lerp(p0, p1, t);
        }

		public static float Map( float input, float inputMin, float inputMax, float min, float max ) {
			return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
		}

        public class Line {
            public Vector2 pointA;
            public Vector2 pointB;

            public Line(Vector2 a, Vector2 b) {
                pointA = a;
                pointB = b;
            }

            /// <summary>
            /// Outline direction on specified side.
            /// </summary>
            public Vector2 Direction { get { return GetDir(); } }
            Vector2 GetDir() { return (pointB - pointA).Normal; }

            public float Magnitude { get { return GetMagnitude(); } }
            float GetMagnitude() {
                return (pointB - pointA).Length;
            }
            public Line Shrink(float distance = 1) {
                Vector2 p1 = pointA + Direction * distance;
                Vector2 p2 = pointB - Direction * distance;

                return new Line(p1, p2);
            }

            public void Draw(Color c = default(Color), float duration = 0) {
				DebugOverlay.Line(pointA, pointB, c, duration);
            }

            public bool Equals(Line other) {
                if (pointA == other.pointA && pointB == other.pointB ||
                   pointA == other.pointB && pointB == other.pointA) {
                    return true;
                }

                return false;
            }

        }
    }

}

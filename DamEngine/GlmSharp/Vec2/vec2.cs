using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Numerics;
using System.Linq;
using DamEngine;

// ReSharper disable InconsistentNaming

namespace DamEngine
{
    
    /// <summary>
    /// A vector of type float with 2 components.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "vec")]
    [StructLayout(LayoutKind.Sequential)]
    public struct vec2 : IReadOnlyList<float>, IEquatable<vec2>
    {

        #region Fields
        
        /// <summary>
        /// x-component
        /// </summary>
        [DataMember]
        public float x;
        
        /// <summary>
        /// y-component
        /// </summary>
        [DataMember]
        public float y;

        #endregion


        #region Constructors
        
        /// <summary>
        /// Component-wise constructor
        /// </summary>
        public vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        
        /// <summary>
        /// all-same-value constructor
        /// </summary>
        public vec2(float v)
        {
            this.x = v;
            this.y = v;
        }
        
        /// <summary>
        /// from-vector constructor
        /// </summary>
        public vec2(vec2 v)
        {
            this.x = v.x;
            this.y = v.y;
        }
        
        
        /// <summary>
        /// From-array/list constructor (superfluous values are ignored, missing values are zero-filled).
        /// </summary>
        public vec2(IReadOnlyList<float> v)
        {
            var c = v.Count;
            this.x = c < 0 ? 0f : v[0];
            this.y = c < 1 ? 0f : v[1];
        }
        
        /// <summary>
        /// Generic from-array constructor (superfluous values are ignored, missing values are zero-filled).
        /// </summary>
        public vec2(Object[] v)
        {
            var c = v.Length;
            this.x = c < 0 ? 0f : (float)v[0];
            this.y = c < 1 ? 0f : (float)v[1];
        }
        
        /// <summary>
        /// From-array constructor (superfluous values are ignored, missing values are zero-filled).
        /// </summary>
        public vec2(float[] v)
        {
            var c = v.Length;
            this.x = c < 0 ? 0f : v[0];
            this.y = c < 1 ? 0f : v[1];
        }
        
        /// <summary>
        /// From-array constructor with base index (superfluous values are ignored, missing values are zero-filled).
        /// </summary>
        public vec2(float[] v, int startIndex)
        {
            var c = v.Length;
            this.x = c + startIndex < 0 ? 0f : v[0 + startIndex];
            this.y = c + startIndex < 1 ? 0f : v[1 + startIndex];
        }
        
        /// <summary>
        /// From-IEnumerable constructor (superfluous values are ignored, missing values are zero-filled).
        /// </summary>
        public vec2(IEnumerable<float> v)
            : this(v.ToArray())
        {
        }

        #endregion



        #region Explicit Operators
        
        /// <summary>
        /// Explicitly converts this to a float array.
        /// </summary>
        public static explicit operator float[](vec2 v) => new [] { v.x, v.y };
        
        /// <summary>
        /// Explicitly converts this to a generic object array.
        /// </summary>
        public static explicit operator Object[](vec2 v) => new Object[] { v.x, v.y };

        #endregion


        #region Indexer
        
        /// <summary>
        /// Gets/Sets a specific indexed component (a bit slower than direct access).
        /// </summary>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        #endregion


        #region Properties
        
        /// <summary>
        /// Returns an object that can be used for arbitrary swizzling (e.g. swizzle.zy)
        /// </summary>
        public swizzle_vec2 swizzle => new swizzle_vec2(x, y);
        
        /// <summary>
        /// Gets or sets the specified subset of components. For more advanced (read-only) swizzling, use the .swizzle property.
        /// </summary>
        public vec2 xy
        {
            get
            {
                return new vec2(x, y);
            }
            set
            {
                x = value.x;
                y = value.y;
            }
        }
        
        /// <summary>
        /// Gets or sets the specified subset of components. For more advanced (read-only) swizzling, use the .swizzle property.
        /// </summary>
        public vec2 rg
        {
            get
            {
                return new vec2(x, y);
            }
            set
            {
                x = value.x;
                y = value.y;
            }
        }
        
        /// <summary>
        /// Gets or sets the specified RGBA component. For more advanced (read-only) swizzling, use the .swizzle property.
        /// </summary>
        public float r
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the specified RGBA component. For more advanced (read-only) swizzling, use the .swizzle property.
        /// </summary>
        public float g
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        
        /// <summary>
        /// Returns an array with all values
        /// </summary>
        public float[] Values => new[] { x, y };
        
        /// <summary>
        /// Returns the number of components (2).
        /// </summary>
        public int Count => 2;
        
        /// <summary>
        /// Returns the minimal component of this vector.
        /// </summary>
        public float MinElement => Math.Min(x, y);
        
        /// <summary>
        /// Returns the maximal component of this vector.
        /// </summary>
        public float MaxElement => Math.Max(x, y);
        
        /// <summary>
        /// Returns the euclidean length of this vector.
        /// </summary>
        public float Length => (float)Math.Sqrt((x*x + y*y));
        
        /// <summary>
        /// Returns the squared euclidean length of this vector.
        /// </summary>
        public float LengthSqr => (x*x + y*y);
        
        /// <summary>
        /// Returns the sum of all components.
        /// </summary>
        public float Sum => (x + y);
        
        /// <summary>
        /// Returns the euclidean norm of this vector.
        /// </summary>
        public float Norm => (float)Math.Sqrt((x*x + y*y));
        
        /// <summary>
        /// Returns the one-norm of this vector.
        /// </summary>
        public float Norm1 => (Math.Abs(x) + Math.Abs(y));
        
        /// <summary>
        /// Returns the two-norm (euclidean length) of this vector.
        /// </summary>
        public float Norm2 => (float)Math.Sqrt((x*x + y*y));
        
        /// <summary>
        /// Returns the max-norm of this vector.
        /// </summary>
        public float NormMax => Math.Max(Math.Abs(x), Math.Abs(y));
        
        /// <summary>
        /// Returns a copy of this vector with length one (undefined if this has zero length).
        /// </summary>
        public vec2 Normalized => this / (float)Length;
        
        /// <summary>
        /// Returns a copy of this vector with length one (returns zero if length is zero).
        /// </summary>
        public vec2 NormalizedSafe => this == Zero ? Zero : this / (float)Length;
        
        /// <summary>
        /// Returns the vector angle (atan2(y, x)) in radians.
        /// </summary>
        public double Angle => Math.Atan2((double)y, (double)x);

        #endregion


        #region Static Properties
        
        /// <summary>
        /// Predefined all-zero vector
        /// </summary>
        public static vec2 Zero { get; } = new vec2(0f, 0f);
        
        /// <summary>
        /// Predefined all-ones vector
        /// </summary>
        public static vec2 Ones { get; } = new vec2(1f, 1f);
        
        /// <summary>
        /// Predefined unit-X vector
        /// </summary>
        public static vec2 UnitX { get; } = new vec2(1f, 0f);
        
        /// <summary>
        /// Predefined unit-Y vector
        /// </summary>
        public static vec2 UnitY { get; } = new vec2(0f, 1f);
        
        /// <summary>
        /// Predefined all-MaxValue vector
        /// </summary>
        public static vec2 MaxValue { get; } = new vec2(float.MaxValue, float.MaxValue);
        
        /// <summary>
        /// Predefined all-MinValue vector
        /// </summary>
        public static vec2 MinValue { get; } = new vec2(float.MinValue, float.MinValue);
        
        /// <summary>
        /// Predefined all-Epsilon vector
        /// </summary>
        public static vec2 Epsilon { get; } = new vec2(float.Epsilon, float.Epsilon);
        
        /// <summary>
        /// Predefined all-NaN vector
        /// </summary>
        public static vec2 NaN { get; } = new vec2(float.NaN, float.NaN);
        
        /// <summary>
        /// Predefined all-NegativeInfinity vector
        /// </summary>
        public static vec2 NegativeInfinity { get; } = new vec2(float.NegativeInfinity, float.NegativeInfinity);
        
        /// <summary>
        /// Predefined all-PositiveInfinity vector
        /// </summary>
        public static vec2 PositiveInfinity { get; } = new vec2(float.PositiveInfinity, float.PositiveInfinity);

        #endregion


        #region Operators
        
        /// <summary>
        /// Returns true iff this equals rhs component-wise.
        /// </summary>
        public static bool operator==(vec2 lhs, vec2 rhs) => lhs.Equals(rhs);
        
        /// <summary>
        /// Returns true iff this does not equal rhs (component-wise).
        /// </summary>
        public static bool operator!=(vec2 lhs, vec2 rhs) => !lhs.Equals(rhs);

        #endregion


        #region Functions
        
        /// <summary>
        /// Returns an enumerator that iterates through all components.
        /// </summary>
        public IEnumerator<float> GetEnumerator()
        {
            yield return x;
            yield return y;
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through all components.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        /// <summary>
        /// Returns a string representation of this vector using ', ' as a seperator.
        /// </summary>
        public override string ToString() => ToString(", ");
        
        /// <summary>
        /// Returns a string representation of this vector using a provided seperator.
        /// </summary>
        public string ToString(string sep) => (x + sep + y);
        
        /// <summary>
        /// Returns a string representation of this vector using a provided seperator and a format provider for each component.
        /// </summary>
        public string ToString(string sep, IFormatProvider provider) => (x.ToString(provider) + sep + y.ToString(provider));
        
        /// <summary>
        /// Returns a string representation of this vector using a provided seperator and a format for each component.
        /// </summary>
        public string ToString(string sep, string format) => (x.ToString(format) + sep + y.ToString(format));
        
        /// <summary>
        /// Returns a string representation of this vector using a provided seperator and a format and format provider for each component.
        /// </summary>
        public string ToString(string sep, string format, IFormatProvider provider) => (x.ToString(format, provider) + sep + y.ToString(format, provider));
        
        /// <summary>
        /// Returns true iff this equals rhs component-wise.
        /// </summary>
        public bool Equals(vec2 rhs) => (x.Equals(rhs.x) && y.Equals(rhs.y));
        
        /// <summary>
        /// Returns true iff this equals rhs type- and component-wise.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is vec2 && Equals((vec2) obj);
        }
        
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((x.GetHashCode()) * 397) ^ y.GetHashCode();
            }
        }
        
        /// <summary>
        /// Returns the p-norm of this vector.
        /// </summary>
        public double NormP(double p) => Math.Pow((Math.Pow((double)Math.Abs(x), p) + Math.Pow((double)Math.Abs(y), p)), 1 / p);
        
        #endregion


        #region Static Functions
        
        /// <summary>
        /// Converts the string representation of the vector into a vector representation (using ', ' as a separator).
        /// </summary>
        public static vec2 Parse(string s) => Parse(s, ", ");
        
        /// <summary>
        /// Converts the string representation of the vector into a vector representation (using a designated separator).
        /// </summary>
        public static vec2 Parse(string s, string sep)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 2) throw new FormatException("input has not exactly 2 parts");
            return new vec2(float.Parse(kvp[0].Trim()), float.Parse(kvp[1].Trim()));
        }
        
        /// <summary>
        /// Converts the string representation of the vector into a vector representation (using a designated separator and a type provider).
        /// </summary>
        public static vec2 Parse(string s, string sep, IFormatProvider provider)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 2) throw new FormatException("input has not exactly 2 parts");
            return new vec2(float.Parse(kvp[0].Trim(), provider), float.Parse(kvp[1].Trim(), provider));
        }
        
        /// <summary>
        /// Converts the string representation of the vector into a vector representation (using a designated separator and a number style).
        /// </summary>
        public static vec2 Parse(string s, string sep, NumberStyles style)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 2) throw new FormatException("input has not exactly 2 parts");
            return new vec2(float.Parse(kvp[0].Trim(), style), float.Parse(kvp[1].Trim(), style));
        }
        
        /// <summary>
        /// Converts the string representation of the vector into a vector representation (using a designated separator and a number style and a format provider).
        /// </summary>
        public static vec2 Parse(string s, string sep, NumberStyles style, IFormatProvider provider)
        {
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 2) throw new FormatException("input has not exactly 2 parts");
            return new vec2(float.Parse(kvp[0].Trim(), style, provider), float.Parse(kvp[1].Trim(), style, provider));
        }
        
        /// <summary>
        /// Tries to convert the string representation of the vector into a vector representation (using ', ' as a separator), returns false if string was invalid.
        /// </summary>
        public static bool TryParse(string s, out vec2 result) => TryParse(s, ", ", out result);
        
        /// <summary>
        /// Tries to convert the string representation of the vector into a vector representation (using a designated separator), returns false if string was invalid.
        /// </summary>
        public static bool TryParse(string s, string sep, out vec2 result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(s)) return false;
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 2) return false;
            float x = 0f, y = 0f;
            var ok = (float.TryParse(kvp[0].Trim(), out x) && float.TryParse(kvp[1].Trim(), out y));
            result = ok ? new vec2(x, y) : Zero;
            return ok;
        }
        
        /// <summary>
        /// Tries to convert the string representation of the vector into a vector representation (using a designated separator and a number style and a format provider), returns false if string was invalid.
        /// </summary>
        public static bool TryParse(string s, string sep, NumberStyles style, IFormatProvider provider, out vec2 result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(s)) return false;
            var kvp = s.Split(new[] { sep }, StringSplitOptions.None);
            if (kvp.Length != 2) return false;
            float x = 0f, y = 0f;
            var ok = (float.TryParse(kvp[0].Trim(), style, provider, out x) && float.TryParse(kvp[1].Trim(), style, provider, out y));
            result = ok ? new vec2(x, y) : Zero;
            return ok;
        }
        
        /// <summary>
        /// Returns true iff distance between lhs and rhs is less than or equal to epsilon
        /// </summary>
        public static bool ApproxEqual(vec2 lhs, vec2 rhs, float eps = 0.1f) => Distance(lhs, rhs) <= eps;
       
        /// <summary>
        /// Returns a unit 2D vector with a given angle in radians (CAUTION: result may be truncated for integer types).
        /// </summary>
        public static vec2 FromAngle(double angleInRad) => new vec2((float)Math.Cos(angleInRad), (float)Math.Sin(angleInRad));
        
        /// <summary>
        /// Returns the inner product (dot product, scalar product) of the two vectors.
        /// </summary>
        public static float Dot(vec2 lhs, vec2 rhs) => (lhs.x * rhs.x + lhs.y * rhs.y);
        
        /// <summary>
        /// Returns the euclidean distance between the two vectors.
        /// </summary>
        public static float Distance(vec2 lhs, vec2 rhs) => (lhs - rhs).Length;
        
        /// <summary>
        /// Returns the squared euclidean distance between the two vectors.
        /// </summary>
        public static float DistanceSqr(vec2 lhs, vec2 rhs) => (lhs - rhs).LengthSqr;
        
        /// <summary>
        /// Calculate the reflection direction for an incident vector (N should be normalized in order to achieve the desired result).
        /// </summary>
        public static vec2 Reflect(vec2 I, vec2 N) => I - 2 * Dot(N, I) * N;
        
        /// <summary>
        /// Calculate the refraction direction for an incident vector (The input parameters I and N should be normalized in order to achieve the desired result).
        /// </summary>
        public static vec2 Refract(vec2 I, vec2 N, float eta)
        {
            var dNI = Dot(N, I);
            var k = 1 - eta * eta * (1 - dNI * dNI);
            if (k < 0) return Zero;
            return eta * I - (eta * dNI + (float)Math.Sqrt(k)) * N;
        }
        
        /// <summary>
        /// Returns a vector pointing in the same direction as another (faceforward orients a vector to point away from a surface as defined by its normal. If dot(Nref, I) is negative faceforward returns N, otherwise it returns -N).
        /// </summary>
        public static vec2 FaceForward(vec2 N, vec2 I, vec2 Nref) => Dot(Nref, I) < 0 ? N : -N;
        
        /// <summary>
        /// Returns the length of the outer product (cross product, vector product) of the two vectors.
        /// </summary>
        public static float Cross(vec2 l, vec2 r) => l.x * r.y - l.y * r.x;
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 0.0 and 1.0.
        /// </summary>
        public static vec2 Random(Random random) => new vec2((float)random.NextDouble(), (float)random.NextDouble());
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between -1.0 and 1.0.
        /// </summary>
        public static vec2 RandomSigned(Random random) => new vec2((float)(random.NextDouble() * 2.0 - 1.0), (float)(random.NextDouble() * 2.0 - 1.0));
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal distribution (zero mean, unit variance).
        /// </summary>
        public static vec2 RandomNormal(Random random) => new vec2((float)(Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))), (float)(Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))));

        #endregion


        #region Component-Wise Static Functions
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Abs (Math.Abs(v)).
        /// </summary>
        public static vec2 Abs(vec2 v) => new vec2(Math.Abs(v.x), Math.Abs(v.y));
        
        /// <summary>
        /// Returns a vec from the application of Abs (Math.Abs(v)).
        /// </summary>
        public static vec2 Abs(float v) => new vec2(Math.Abs(v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of HermiteInterpolationOrder3 ((3 - 2 * v) * v * v).
        /// </summary>
        public static vec2 HermiteInterpolationOrder3(vec2 v) => new vec2((3 - 2 * v.x) * v.x * v.x, (3 - 2 * v.y) * v.y * v.y);
        
        /// <summary>
        /// Returns a vec from the application of HermiteInterpolationOrder3 ((3 - 2 * v) * v * v).
        /// </summary>
        public static vec2 HermiteInterpolationOrder3(float v) => new vec2((3 - 2 * v) * v * v);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of HermiteInterpolationOrder5 (((6 * v - 15) * v + 10) * v * v * v).
        /// </summary>
        public static vec2 HermiteInterpolationOrder5(vec2 v) => new vec2(((6 * v.x - 15) * v.x + 10) * v.x * v.x * v.x, ((6 * v.y - 15) * v.y + 10) * v.y * v.y * v.y);
        
        /// <summary>
        /// Returns a vec from the application of HermiteInterpolationOrder5 (((6 * v - 15) * v + 10) * v * v * v).
        /// </summary>
        public static vec2 HermiteInterpolationOrder5(float v) => new vec2(((6 * v - 15) * v + 10) * v * v * v);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Sqr (v * v).
        /// </summary>
        public static vec2 Sqr(vec2 v) => new vec2(v.x * v.x, v.y * v.y);
        
        /// <summary>
        /// Returns a vec from the application of Sqr (v * v).
        /// </summary>
        public static vec2 Sqr(float v) => new vec2(v * v);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Pow2 (v * v).
        /// </summary>
        public static vec2 Pow2(vec2 v) => new vec2(v.x * v.x, v.y * v.y);
        
        /// <summary>
        /// Returns a vec from the application of Pow2 (v * v).
        /// </summary>
        public static vec2 Pow2(float v) => new vec2(v * v);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Pow3 (v * v * v).
        /// </summary>
        public static vec2 Pow3(vec2 v) => new vec2(v.x * v.x * v.x, v.y * v.y * v.y);
        
        /// <summary>
        /// Returns a vec from the application of Pow3 (v * v * v).
        /// </summary>
        public static vec2 Pow3(float v) => new vec2(v * v * v);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Step (v &gt;= 0f ? 1f : 0f).
        /// </summary>
        public static vec2 Step(vec2 v) => new vec2(v.x >= 0f ? 1f : 0f, v.y >= 0f ? 1f : 0f);
        
        /// <summary>
        /// Returns a vec from the application of Step (v &gt;= 0f ? 1f : 0f).
        /// </summary>
        public static vec2 Step(float v) => new vec2(v >= 0f ? 1f : 0f);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Sqrt ((float)Math.Sqrt((double)v)).
        /// </summary>
        public static vec2 Sqrt(vec2 v) => new vec2((float)Math.Sqrt((double)v.x), (float)Math.Sqrt((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Sqrt ((float)Math.Sqrt((double)v)).
        /// </summary>
        public static vec2 Sqrt(float v) => new vec2((float)Math.Sqrt((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of InverseSqrt ((float)(1.0 / Math.Sqrt((double)v))).
        /// </summary>
        public static vec2 InverseSqrt(vec2 v) => new vec2((float)(1.0 / Math.Sqrt((double)v.x)), (float)(1.0 / Math.Sqrt((double)v.y)));
        
        /// <summary>
        /// Returns a vec from the application of InverseSqrt ((float)(1.0 / Math.Sqrt((double)v))).
        /// </summary>
        public static vec2 InverseSqrt(float v) => new vec2((float)(1.0 / Math.Sqrt((double)v)));
        
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Max (Math.Max(lhs, rhs)).
        /// </summary>
        public static vec2 Max(vec2 lhs, vec2 rhs) => new vec2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Max (Math.Max(lhs, rhs)).
        /// </summary>
        public static vec2 Max(vec2 lhs, float rhs) => new vec2(Math.Max(lhs.x, rhs), Math.Max(lhs.y, rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Max (Math.Max(lhs, rhs)).
        /// </summary>
        public static vec2 Max(float lhs, vec2 rhs) => new vec2(Math.Max(lhs, rhs.x), Math.Max(lhs, rhs.y));
        
        /// <summary>
        /// Returns a vec from the application of Max (Math.Max(lhs, rhs)).
        /// </summary>
        public static vec2 Max(float lhs, float rhs) => new vec2(Math.Max(lhs, rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Min (Math.Min(lhs, rhs)).
        /// </summary>
        public static vec2 Min(vec2 lhs, vec2 rhs) => new vec2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Min (Math.Min(lhs, rhs)).
        /// </summary>
        public static vec2 Min(vec2 lhs, float rhs) => new vec2(Math.Min(lhs.x, rhs), Math.Min(lhs.y, rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Min (Math.Min(lhs, rhs)).
        /// </summary>
        public static vec2 Min(float lhs, vec2 rhs) => new vec2(Math.Min(lhs, rhs.x), Math.Min(lhs, rhs.y));
        
        /// <summary>
        /// Returns a vec from the application of Min (Math.Min(lhs, rhs)).
        /// </summary>
        public static vec2 Min(float lhs, float rhs) => new vec2(Math.Min(lhs, rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Pow ((float)Math.Pow((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Pow(vec2 lhs, vec2 rhs) => new vec2((float)Math.Pow((double)lhs.x, (double)rhs.x), (float)Math.Pow((double)lhs.y, (double)rhs.y));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Pow ((float)Math.Pow((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Pow(vec2 lhs, float rhs) => new vec2((float)Math.Pow((double)lhs.x, (double)rhs), (float)Math.Pow((double)lhs.y, (double)rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Pow ((float)Math.Pow((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Pow(float lhs, vec2 rhs) => new vec2((float)Math.Pow((double)lhs, (double)rhs.x), (float)Math.Pow((double)lhs, (double)rhs.y));
        
        /// <summary>
        /// Returns a vec from the application of Pow ((float)Math.Pow((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Pow(float lhs, float rhs) => new vec2((float)Math.Pow((double)lhs, (double)rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Log ((float)Math.Log((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Log(vec2 lhs, vec2 rhs) => new vec2((float)Math.Log((double)lhs.x, (double)rhs.x), (float)Math.Log((double)lhs.y, (double)rhs.y));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Log ((float)Math.Log((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Log(vec2 lhs, float rhs) => new vec2((float)Math.Log((double)lhs.x, (double)rhs), (float)Math.Log((double)lhs.y, (double)rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Log ((float)Math.Log((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Log(float lhs, vec2 rhs) => new vec2((float)Math.Log((double)lhs, (double)rhs.x), (float)Math.Log((double)lhs, (double)rhs.y));
        
        /// <summary>
        /// Returns a vec from the application of Log ((float)Math.Log((double)lhs, (double)rhs)).
        /// </summary>
        public static vec2 Log(float lhs, float rhs) => new vec2((float)Math.Log((double)lhs, (double)rhs));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(vec2 v, vec2 min, vec2 max) => new vec2(Math.Min(Math.Max(v.x, min.x), max.x), Math.Min(Math.Max(v.y, min.y), max.y));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(vec2 v, vec2 min, float max) => new vec2(Math.Min(Math.Max(v.x, min.x), max), Math.Min(Math.Max(v.y, min.y), max));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(vec2 v, float min, vec2 max) => new vec2(Math.Min(Math.Max(v.x, min), max.x), Math.Min(Math.Max(v.y, min), max.y));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(vec2 v, float min, float max) => new vec2(Math.Min(Math.Max(v.x, min), max), Math.Min(Math.Max(v.y, min), max));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(float v, vec2 min, vec2 max) => new vec2(Math.Min(Math.Max(v, min.x), max.x), Math.Min(Math.Max(v, min.y), max.y));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(float v, vec2 min, float max) => new vec2(Math.Min(Math.Max(v, min.x), max), Math.Min(Math.Max(v, min.y), max));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(float v, float min, vec2 max) => new vec2(Math.Min(Math.Max(v, min), max.x), Math.Min(Math.Max(v, min), max.y));
        
        /// <summary>
        /// Returns a vec from the application of Clamp (Math.Min(Math.Max(v, min), max)).
        /// </summary>
        public static vec2 Clamp(float v, float min, float max) => new vec2(Math.Min(Math.Max(v, min), max));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(vec2 min, vec2 max, vec2 a) => new vec2(min.x * (1-a.x) + max.x * a.x, min.y * (1-a.y) + max.y * a.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(vec2 min, vec2 max, float a) => new vec2(min.x * (1-a) + max.x * a, min.y * (1-a) + max.y * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(vec2 min, float max, vec2 a) => new vec2(min.x * (1-a.x) + max * a.x, min.y * (1-a.y) + max * a.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(vec2 min, float max, float a) => new vec2(min.x * (1-a) + max * a, min.y * (1-a) + max * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(float min, vec2 max, vec2 a) => new vec2(min * (1-a.x) + max.x * a.x, min * (1-a.y) + max.y * a.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(float min, vec2 max, float a) => new vec2(min * (1-a) + max.x * a, min * (1-a) + max.y * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(float min, float max, vec2 a) => new vec2(min * (1-a.x) + max * a.x, min * (1-a.y) + max * a.y);
        
        /// <summary>
        /// Returns a vec from the application of Mix (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Mix(float min, float max, float a) => new vec2(min * (1-a) + max * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(vec2 min, vec2 max, vec2 a) => new vec2(min.x * (1-a.x) + max.x * a.x, min.y * (1-a.y) + max.y * a.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(vec2 min, vec2 max, float a) => new vec2(min.x * (1-a) + max.x * a, min.y * (1-a) + max.y * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(vec2 min, float max, vec2 a) => new vec2(min.x * (1-a.x) + max * a.x, min.y * (1-a.y) + max * a.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(vec2 min, float max, float a) => new vec2(min.x * (1-a) + max * a, min.y * (1-a) + max * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(float min, vec2 max, vec2 a) => new vec2(min * (1-a.x) + max.x * a.x, min * (1-a.y) + max.y * a.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(float min, vec2 max, float a) => new vec2(min * (1-a) + max.x * a, min * (1-a) + max.y * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(float min, float max, vec2 a) => new vec2(min * (1-a.x) + max * a.x, min * (1-a.y) + max * a.y);
        
        /// <summary>
        /// Returns a vec from the application of Lerp (min * (1-a) + max * a).
        /// </summary>
        public static vec2 Lerp(float min, float max, float a) => new vec2(min * (1-a) + max * a);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(vec2 edge0, vec2 edge1, vec2 v) => new vec2(((v.x - edge0.x) / (edge1.x - edge0.x)).Clamp().HermiteInterpolationOrder3(), ((v.y - edge0.y) / (edge1.y - edge0.y)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(vec2 edge0, vec2 edge1, float v) => new vec2(((v - edge0.x) / (edge1.x - edge0.x)).Clamp().HermiteInterpolationOrder3(), ((v - edge0.y) / (edge1.y - edge0.y)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(vec2 edge0, float edge1, vec2 v) => new vec2(((v.x - edge0.x) / (edge1 - edge0.x)).Clamp().HermiteInterpolationOrder3(), ((v.y - edge0.y) / (edge1 - edge0.y)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(vec2 edge0, float edge1, float v) => new vec2(((v - edge0.x) / (edge1 - edge0.x)).Clamp().HermiteInterpolationOrder3(), ((v - edge0.y) / (edge1 - edge0.y)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(float edge0, vec2 edge1, vec2 v) => new vec2(((v.x - edge0) / (edge1.x - edge0)).Clamp().HermiteInterpolationOrder3(), ((v.y - edge0) / (edge1.y - edge0)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(float edge0, vec2 edge1, float v) => new vec2(((v - edge0) / (edge1.x - edge0)).Clamp().HermiteInterpolationOrder3(), ((v - edge0) / (edge1.y - edge0)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(float edge0, float edge1, vec2 v) => new vec2(((v.x - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3(), ((v.y - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec from the application of Smoothstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3()).
        /// </summary>
        public static vec2 Smoothstep(float edge0, float edge1, float v) => new vec2(((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder3());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(vec2 edge0, vec2 edge1, vec2 v) => new vec2(((v.x - edge0.x) / (edge1.x - edge0.x)).Clamp().HermiteInterpolationOrder5(), ((v.y - edge0.y) / (edge1.y - edge0.y)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(vec2 edge0, vec2 edge1, float v) => new vec2(((v - edge0.x) / (edge1.x - edge0.x)).Clamp().HermiteInterpolationOrder5(), ((v - edge0.y) / (edge1.y - edge0.y)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(vec2 edge0, float edge1, vec2 v) => new vec2(((v.x - edge0.x) / (edge1 - edge0.x)).Clamp().HermiteInterpolationOrder5(), ((v.y - edge0.y) / (edge1 - edge0.y)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(vec2 edge0, float edge1, float v) => new vec2(((v - edge0.x) / (edge1 - edge0.x)).Clamp().HermiteInterpolationOrder5(), ((v - edge0.y) / (edge1 - edge0.y)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(float edge0, vec2 edge1, vec2 v) => new vec2(((v.x - edge0) / (edge1.x - edge0)).Clamp().HermiteInterpolationOrder5(), ((v.y - edge0) / (edge1.y - edge0)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(float edge0, vec2 edge1, float v) => new vec2(((v - edge0) / (edge1.x - edge0)).Clamp().HermiteInterpolationOrder5(), ((v - edge0) / (edge1.y - edge0)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(float edge0, float edge1, vec2 v) => new vec2(((v.x - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5(), ((v.y - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec from the application of Smootherstep (((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5()).
        /// </summary>
        public static vec2 Smootherstep(float edge0, float edge1, float v) => new vec2(((v - edge0) / (edge1 - edge0)).Clamp().HermiteInterpolationOrder5());
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(vec2 a, vec2 b, vec2 c) => new vec2(a.x * b.x + c.x, a.y * b.y + c.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(vec2 a, vec2 b, float c) => new vec2(a.x * b.x + c, a.y * b.y + c);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(vec2 a, float b, vec2 c) => new vec2(a.x * b + c.x, a.y * b + c.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(vec2 a, float b, float c) => new vec2(a.x * b + c, a.y * b + c);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(float a, vec2 b, vec2 c) => new vec2(a * b.x + c.x, a * b.y + c.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(float a, vec2 b, float c) => new vec2(a * b.x + c, a * b.y + c);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(float a, float b, vec2 c) => new vec2(a * b + c.x, a * b + c.y);
        
        /// <summary>
        /// Returns a vec from the application of Fma (a * b + c).
        /// </summary>
        public static vec2 Fma(float a, float b, float c) => new vec2(a * b + c);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Add (lhs + rhs).
        /// </summary>
        public static vec2 Add(vec2 lhs, vec2 rhs) => new vec2(lhs.x + rhs.x, lhs.y + rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Add (lhs + rhs).
        /// </summary>
        public static vec2 Add(vec2 lhs, float rhs) => new vec2(lhs.x + rhs, lhs.y + rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Add (lhs + rhs).
        /// </summary>
        public static vec2 Add(float lhs, vec2 rhs) => new vec2(lhs + rhs.x, lhs + rhs.y);
        
        /// <summary>
        /// Returns a vec from the application of Add (lhs + rhs).
        /// </summary>
        public static vec2 Add(float lhs, float rhs) => new vec2(lhs + rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Sub (lhs - rhs).
        /// </summary>
        public static vec2 Sub(vec2 lhs, vec2 rhs) => new vec2(lhs.x - rhs.x, lhs.y - rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Sub (lhs - rhs).
        /// </summary>
        public static vec2 Sub(vec2 lhs, float rhs) => new vec2(lhs.x - rhs, lhs.y - rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Sub (lhs - rhs).
        /// </summary>
        public static vec2 Sub(float lhs, vec2 rhs) => new vec2(lhs - rhs.x, lhs - rhs.y);
        
        /// <summary>
        /// Returns a vec from the application of Sub (lhs - rhs).
        /// </summary>
        public static vec2 Sub(float lhs, float rhs) => new vec2(lhs - rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mul (lhs * rhs).
        /// </summary>
        public static vec2 Mul(vec2 lhs, vec2 rhs) => new vec2(lhs.x * rhs.x, lhs.y * rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mul (lhs * rhs).
        /// </summary>
        public static vec2 Mul(vec2 lhs, float rhs) => new vec2(lhs.x * rhs, lhs.y * rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Mul (lhs * rhs).
        /// </summary>
        public static vec2 Mul(float lhs, vec2 rhs) => new vec2(lhs * rhs.x, lhs * rhs.y);
        
        /// <summary>
        /// Returns a vec from the application of Mul (lhs * rhs).
        /// </summary>
        public static vec2 Mul(float lhs, float rhs) => new vec2(lhs * rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Div (lhs / rhs).
        /// </summary>
        public static vec2 Div(vec2 lhs, vec2 rhs) => new vec2(lhs.x / rhs.x, lhs.y / rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Div (lhs / rhs).
        /// </summary>
        public static vec2 Div(vec2 lhs, float rhs) => new vec2(lhs.x / rhs, lhs.y / rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Div (lhs / rhs).
        /// </summary>
        public static vec2 Div(float lhs, vec2 rhs) => new vec2(lhs / rhs.x, lhs / rhs.y);
        
        /// <summary>
        /// Returns a vec from the application of Div (lhs / rhs).
        /// </summary>
        public static vec2 Div(float lhs, float rhs) => new vec2(lhs / rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Modulo (lhs % rhs).
        /// </summary>
        public static vec2 Modulo(vec2 lhs, vec2 rhs) => new vec2(lhs.x % rhs.x, lhs.y % rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Modulo (lhs % rhs).
        /// </summary>
        public static vec2 Modulo(vec2 lhs, float rhs) => new vec2(lhs.x % rhs, lhs.y % rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Modulo (lhs % rhs).
        /// </summary>
        public static vec2 Modulo(float lhs, vec2 rhs) => new vec2(lhs % rhs.x, lhs % rhs.y);
        
        /// <summary>
        /// Returns a vec from the application of Modulo (lhs % rhs).
        /// </summary>
        public static vec2 Modulo(float lhs, float rhs) => new vec2(lhs % rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Degrees (Radians-To-Degrees Conversion).
        /// </summary>
        public static vec2 Degrees(vec2 v) => new vec2((float)(v.x * 57.295779513082320876798154814105170332405472466564321f), (float)(v.y * 57.295779513082320876798154814105170332405472466564321f));
        
        /// <summary>
        /// Returns a vec from the application of Degrees (Radians-To-Degrees Conversion).
        /// </summary>
        public static vec2 Degrees(float v) => new vec2((float)(v * 57.295779513082320876798154814105170332405472466564321f));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Radians (Degrees-To-Radians Conversion).
        /// </summary>
        public static vec2 Radians(vec2 v) => new vec2((float)(v.x * 0.0174532925199432957692369076848861271344287188854172f), (float)(v.y * 0.0174532925199432957692369076848861271344287188854172f));
        
        /// <summary>
        /// Returns a vec from the application of Radians (Degrees-To-Radians Conversion).
        /// </summary>
        public static vec2 Radians(float v) => new vec2((float)(v * 0.0174532925199432957692369076848861271344287188854172f));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Acos ((float)Math.Acos((double)v)).
        /// </summary>
        public static vec2 Acos(vec2 v) => new vec2((float)Math.Acos((double)v.x), (float)Math.Acos((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Acos ((float)Math.Acos((double)v)).
        /// </summary>
        public static vec2 Acos(float v) => new vec2((float)Math.Acos((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Asin ((float)Math.Asin((double)v)).
        /// </summary>
        public static vec2 Asin(vec2 v) => new vec2((float)Math.Asin((double)v.x), (float)Math.Asin((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Asin ((float)Math.Asin((double)v)).
        /// </summary>
        public static vec2 Asin(float v) => new vec2((float)Math.Asin((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Atan ((float)Math.Atan((double)v)).
        /// </summary>
        public static vec2 Atan(vec2 v) => new vec2((float)Math.Atan((double)v.x), (float)Math.Atan((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Atan ((float)Math.Atan((double)v)).
        /// </summary>
        public static vec2 Atan(float v) => new vec2((float)Math.Atan((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Cos ((float)Math.Cos((double)v)).
        /// </summary>
        public static vec2 Cos(vec2 v) => new vec2((float)Math.Cos((double)v.x), (float)Math.Cos((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Cos ((float)Math.Cos((double)v)).
        /// </summary>
        public static vec2 Cos(float v) => new vec2((float)Math.Cos((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Cosh ((float)Math.Cosh((double)v)).
        /// </summary>
        public static vec2 Cosh(vec2 v) => new vec2((float)Math.Cosh((double)v.x), (float)Math.Cosh((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Cosh ((float)Math.Cosh((double)v)).
        /// </summary>
        public static vec2 Cosh(float v) => new vec2((float)Math.Cosh((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Exp ((float)Math.Exp((double)v)).
        /// </summary>
        public static vec2 Exp(vec2 v) => new vec2((float)Math.Exp((double)v.x), (float)Math.Exp((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Exp ((float)Math.Exp((double)v)).
        /// </summary>
        public static vec2 Exp(float v) => new vec2((float)Math.Exp((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Log ((float)Math.Log((double)v)).
        /// </summary>
        public static vec2 Log(vec2 v) => new vec2((float)Math.Log((double)v.x), (float)Math.Log((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Log ((float)Math.Log((double)v)).
        /// </summary>
        public static vec2 Log(float v) => new vec2((float)Math.Log((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Log2 ((float)Math.Log((double)v, 2)).
        /// </summary>
        public static vec2 Log2(vec2 v) => new vec2((float)Math.Log((double)v.x, 2), (float)Math.Log((double)v.y, 2));
        
        /// <summary>
        /// Returns a vec from the application of Log2 ((float)Math.Log((double)v, 2)).
        /// </summary>
        public static vec2 Log2(float v) => new vec2((float)Math.Log((double)v, 2));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Log10 ((float)Math.Log10((double)v)).
        /// </summary>
        public static vec2 Log10(vec2 v) => new vec2((float)Math.Log10((double)v.x), (float)Math.Log10((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Log10 ((float)Math.Log10((double)v)).
        /// </summary>
        public static vec2 Log10(float v) => new vec2((float)Math.Log10((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Floor ((float)Math.Floor(v)).
        /// </summary>
        public static vec2 Floor(vec2 v) => new vec2((float)Math.Floor(v.x), (float)Math.Floor(v.y));
        
        /// <summary>
        /// Returns a vec from the application of Floor ((float)Math.Floor(v)).
        /// </summary>
        public static vec2 Floor(float v) => new vec2((float)Math.Floor(v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Ceiling ((float)Math.Ceiling(v)).
        /// </summary>
        public static vec2 Ceiling(vec2 v) => new vec2((float)Math.Ceiling(v.x), (float)Math.Ceiling(v.y));
        
        /// <summary>
        /// Returns a vec from the application of Ceiling ((float)Math.Ceiling(v)).
        /// </summary>
        public static vec2 Ceiling(float v) => new vec2((float)Math.Ceiling(v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Round ((float)Math.Round(v)).
        /// </summary>
        public static vec2 Round(vec2 v) => new vec2((float)Math.Round(v.x), (float)Math.Round(v.y));
        
        /// <summary>
        /// Returns a vec from the application of Round ((float)Math.Round(v)).
        /// </summary>
        public static vec2 Round(float v) => new vec2((float)Math.Round(v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Sin ((float)Math.Sin((double)v)).
        /// </summary>
        public static vec2 Sin(vec2 v) => new vec2((float)Math.Sin((double)v.x), (float)Math.Sin((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Sin ((float)Math.Sin((double)v)).
        /// </summary>
        public static vec2 Sin(float v) => new vec2((float)Math.Sin((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Sinh ((float)Math.Sinh((double)v)).
        /// </summary>
        public static vec2 Sinh(vec2 v) => new vec2((float)Math.Sinh((double)v.x), (float)Math.Sinh((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Sinh ((float)Math.Sinh((double)v)).
        /// </summary>
        public static vec2 Sinh(float v) => new vec2((float)Math.Sinh((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Tan ((float)Math.Tan((double)v)).
        /// </summary>
        public static vec2 Tan(vec2 v) => new vec2((float)Math.Tan((double)v.x), (float)Math.Tan((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Tan ((float)Math.Tan((double)v)).
        /// </summary>
        public static vec2 Tan(float v) => new vec2((float)Math.Tan((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Tanh ((float)Math.Tanh((double)v)).
        /// </summary>
        public static vec2 Tanh(vec2 v) => new vec2((float)Math.Tanh((double)v.x), (float)Math.Tanh((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Tanh ((float)Math.Tanh((double)v)).
        /// </summary>
        public static vec2 Tanh(float v) => new vec2((float)Math.Tanh((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Truncate ((float)Math.Truncate((double)v)).
        /// </summary>
        public static vec2 Truncate(vec2 v) => new vec2((float)Math.Truncate((double)v.x), (float)Math.Truncate((double)v.y));
        
        /// <summary>
        /// Returns a vec from the application of Truncate ((float)Math.Truncate((double)v)).
        /// </summary>
        public static vec2 Truncate(float v) => new vec2((float)Math.Truncate((double)v));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Fract ((float)(v - Math.Floor(v))).
        /// </summary>
        public static vec2 Fract(vec2 v) => new vec2((float)(v.x - Math.Floor(v.x)), (float)(v.y - Math.Floor(v.y)));
        
        /// <summary>
        /// Returns a vec from the application of Fract ((float)(v - Math.Floor(v))).
        /// </summary>
        public static vec2 Fract(float v) => new vec2((float)(v - Math.Floor(v)));
        
        /// <summary>
        /// Returns a vec2 from component-wise application of Trunc ((long)(v)).
        /// </summary>
        public static vec2 Trunc(vec2 v) => new vec2((long)(v.x), (long)(v.y));
        
        /// <summary>
        /// Returns a vec from the application of Trunc ((long)(v)).
        /// </summary>
        public static vec2 Trunc(float v) => new vec2((long)(v));
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 Random(Random random, vec2 minValue, vec2 maxValue) => new vec2((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 Random(Random random, vec2 minValue, float maxValue) => new vec2((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 Random(Random random, float minValue, vec2 maxValue) => new vec2((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 Random(Random random, float minValue, float maxValue) => new vec2((float)random.NextDouble() * (maxValue - minValue) + minValue);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 RandomUniform(Random random, vec2 minValue, vec2 maxValue) => new vec2((float)random.NextDouble() * (maxValue.x - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue.y - minValue.y) + minValue.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 RandomUniform(Random random, vec2 minValue, float maxValue) => new vec2((float)random.NextDouble() * (maxValue - minValue.x) + minValue.x, (float)random.NextDouble() * (maxValue - minValue.y) + minValue.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 RandomUniform(Random random, float minValue, vec2 maxValue) => new vec2((float)random.NextDouble() * (maxValue.x - minValue) + minValue, (float)random.NextDouble() * (maxValue.y - minValue) + minValue);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed uniform values between 'minValue' and 'maxValue'.
        /// </summary>
        public static vec2 RandomUniform(Random random, float minValue, float maxValue) => new vec2((float)random.NextDouble() * (maxValue - minValue) + minValue);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomNormal(Random random, vec2 mean, vec2 variance) => new vec2((float)(Math.Sqrt((double)variance.x) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt((double)variance.y) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomNormal(Random random, vec2 mean, float variance) => new vec2((float)(Math.Sqrt((double)variance) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt((double)variance) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomNormal(Random random, float mean, vec2 variance) => new vec2((float)(Math.Sqrt((double)variance.x) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt((double)variance.y) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomNormal(Random random, float mean, float variance) => new vec2((float)(Math.Sqrt((double)variance) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomGaussian(Random random, vec2 mean, vec2 variance) => new vec2((float)(Math.Sqrt((double)variance.x) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt((double)variance.y) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomGaussian(Random random, vec2 mean, float variance) => new vec2((float)(Math.Sqrt((double)variance) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.x, (float)(Math.Sqrt((double)variance) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean.y);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomGaussian(Random random, float mean, vec2 variance) => new vec2((float)(Math.Sqrt((double)variance.x) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean, (float)(Math.Sqrt((double)variance.y) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);
        
        /// <summary>
        /// Returns a vec2 with independent and identically distributed values according to a normal/Gaussian distribution with specified mean and variance.
        /// </summary>
        public static vec2 RandomGaussian(Random random, float mean, float variance) => new vec2((float)(Math.Sqrt((double)variance) * Math.Cos(2 * Math.PI * random.NextDouble()) * Math.Sqrt(-2.0 * Math.Log(random.NextDouble()))) + mean);

        #endregion


        #region Component-Wise Operator Overloads
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator+ (lhs + rhs).
        /// </summary>
        public static vec2 operator+(vec2 lhs, vec2 rhs) => new vec2(lhs.x + rhs.x, lhs.y + rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator+ (lhs + rhs).
        /// </summary>
        public static vec2 operator+(vec2 lhs, float rhs) => new vec2(lhs.x + rhs, lhs.y + rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator+ (lhs + rhs).
        /// </summary>
        public static vec2 operator+(float lhs, vec2 rhs) => new vec2(lhs + rhs.x, lhs + rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator- (lhs - rhs).
        /// </summary>
        public static vec2 operator-(vec2 lhs, vec2 rhs) => new vec2(lhs.x - rhs.x, lhs.y - rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator- (lhs - rhs).
        /// </summary>
        public static vec2 operator-(vec2 lhs, float rhs) => new vec2(lhs.x - rhs, lhs.y - rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator- (lhs - rhs).
        /// </summary>
        public static vec2 operator-(float lhs, vec2 rhs) => new vec2(lhs - rhs.x, lhs - rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator* (lhs * rhs).
        /// </summary>
        public static vec2 operator*(vec2 lhs, vec2 rhs) => new vec2(lhs.x * rhs.x, lhs.y * rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator* (lhs * rhs).
        /// </summary>
        public static vec2 operator*(vec2 lhs, float rhs) => new vec2(lhs.x * rhs, lhs.y * rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator* (lhs * rhs).
        /// </summary>
        public static vec2 operator*(float lhs, vec2 rhs) => new vec2(lhs * rhs.x, lhs * rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator/ (lhs / rhs).
        /// </summary>
        public static vec2 operator/(vec2 lhs, vec2 rhs) => new vec2(lhs.x / rhs.x, lhs.y / rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator/ (lhs / rhs).
        /// </summary>
        public static vec2 operator/(vec2 lhs, float rhs) => new vec2(lhs.x / rhs, lhs.y / rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator/ (lhs / rhs).
        /// </summary>
        public static vec2 operator/(float lhs, vec2 rhs) => new vec2(lhs / rhs.x, lhs / rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator+ (identity).
        /// </summary>
        public static vec2 operator+(vec2 v) => v;
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator- (-v).
        /// </summary>
        public static vec2 operator-(vec2 v) => new vec2(-v.x, -v.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator% (lhs % rhs).
        /// </summary>
        public static vec2 operator%(vec2 lhs, vec2 rhs) => new vec2(lhs.x % rhs.x, lhs.y % rhs.y);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator% (lhs % rhs).
        /// </summary>
        public static vec2 operator%(vec2 lhs, float rhs) => new vec2(lhs.x % rhs, lhs.y % rhs);
        
        /// <summary>
        /// Returns a vec2 from component-wise application of operator% (lhs % rhs).
        /// </summary>
        public static vec2 operator%(float lhs, vec2 rhs) => new vec2(lhs % rhs.x, lhs % rhs.y);

        #endregion

    }
}

using System;

namespace WindowsFormsApp1
{
    internal class Fraction
    {
        private long num;
        private long denom;

        public Fraction(long num, long denom = 1)
        {
            this.num = num;
            this.denom = denom;
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.num * b.denom + b.num * a.denom, a.denom * b.denom);
        }

        public static Fraction operator +(Fraction a, long b)
        {
            return a + new Fraction(b);
        }

        public static Fraction operator +(long b, Fraction a)
        {
            return new Fraction(b) + a;
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a.num * b.denom - b.num * a.denom, a.denom * b.denom);
        }

        public static Fraction operator -(Fraction a, long b)
        {
            return a - new Fraction(b);
        }

        public static Fraction operator -(long b, Fraction a)
        {
            return new Fraction(b) - a;
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.num * b.num, a.denom * b.denom);
        }

        public static Fraction operator *(Fraction a, long b)
        {
            return a * new Fraction(b);
        }

        public static Fraction operator *(long b, Fraction a)
        {
            return new Fraction(b) * a;
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new Fraction(a.num * b.denom, a.denom * b.num);
        }

        public static Fraction operator /(Fraction a, long b)
        {
            return a / new Fraction(b);
        }

        public static Fraction operator /(long b, Fraction a)
        {
            return new Fraction(b) / a;
        }

        public double GetValue()
        {
            return 1.0 * num / denom;
        }

        public static bool operator >(Fraction a, Fraction b)
        {
            if (a.GetValue() > b.GetValue())
                return true;

            return false;
        }

        public static bool operator <(Fraction a, Fraction b)
        {
            if (a.GetValue() < b.GetValue())
                return true;

            return false;
        }

        public static explicit operator double(Fraction a)
        {
            return a.GetValue();
        }

        public static Fraction operator +(Fraction a)
        {
            return new Fraction(+a.num, a.denom);
        }

        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a.num, a.denom);
        }

        public static Fraction operator ++(Fraction a)
        {
            a.num += a.denom;
            return a;
        }

        public static Fraction operator --(Fraction a)
        {
            a.num -= a.denom;
            return a;
        }

        public static bool operator >=(Fraction a, Fraction b)
        {
            return !(a < b);
        }

        public static bool operator <=(Fraction a, Fraction b)
        {
            return !(a > b);
        }

        public static bool operator ==(Fraction a, Fraction b)
        {
            if (a.GetValue() == b.GetValue())
                return true;
            else return false;
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }

        public Fraction Abbreviation()
        {
            for (long i = Math.Abs(num); i > 0; i--)
            {
                if (Math.Abs(num) % i == 0 && Math.Abs(denom) % i == 0)
                {
                    num /= i;
                    denom /= i;
                }
            }
            return this;
        }

        public bool IsInt()
        {
            if (num % denom == 0)
                return true;

            return false;
        }

        public override string ToString()
        {
            if (num < 0 && denom < 0)
                return $"{-num}/{-denom}";
            else if (num > 0 && denom < 0)
                return $"{-num}/{-denom}";
            else if (num == 0)
                return 0.ToString();
            else if (IsInt())
                return ((double)this).ToString();
            else return $"{num}/{denom}";
        }

        public override bool Equals(object obj)
        {
            return obj is Fraction fraction &&
                   num == fraction.num &&
                   denom == fraction.denom;
        }

        public override int GetHashCode()
        {
            return num.GetHashCode() + denom.GetHashCode();
        }
    }
}

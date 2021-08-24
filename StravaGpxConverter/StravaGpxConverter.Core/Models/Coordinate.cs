namespace StravaGpxConverter.Core.Models
{
    public sealed class Coordinate : ValueObject<Coordinate>
    {
        public double Value { get; }
        public string StringValue { get; }

        public Coordinate(string value)
        {
            StringValue = value;
            Value = double.Parse(value);
        }

        protected override bool EqualCore(Coordinate other)
        {
            return this.StringValue == other.StringValue;
        }

        public override string ToString()
        {
            return StringValue;
        }
    }
}

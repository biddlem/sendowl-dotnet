using Shouldly;
using Xunit;

namespace SendOwl.Test
{
    public class LowecaseJsonSerializerTest
    {
        public enum TestEnum
        {
            SomeValue = 0
        }

        [Fact]
        public void SerializeObject_Serializes_properties_and_enums_to_lowercase()
        {
            var someObject = new
            {
                MyEnum = TestEnum.SomeValue
            };
            var json = LowercaseJsonSerializer.SerializeObject(someObject);

            json.ShouldBe("{\"myEnum\":\"someValue\"}");
        }
    }
}

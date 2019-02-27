using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Helpers.Test
{

    [TestFixture]
    public class NotifyPropertyChangedHelperTest
    {
        public int MyProperty { get; set; }

        [Test]
        public void GetPropertyNameFromExpression()
        {
            var name = NotifyPropertyChangedHelper.GetPropertyNameFromExpression(() => MyProperty);
            Assert.That(name, Is.EqualTo("MyProperty"));
        }

    }

}

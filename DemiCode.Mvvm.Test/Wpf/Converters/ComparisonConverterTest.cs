using System;
using System.Globalization;
using System.Windows.Data;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace DemiCode.Mvvm.Wpf.Converters.Test
{

    [TestFixture]
    public class ComparisonConverterTest
    {
        private ComparisonConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _converter = new ComparisonConverter();
        }

        [Test]
        public void Convert_WithIllegalTargetType_Throws()
        {
            Assert.Throws<ArgumentException>(() => _converter.Convert("1", typeof(string), "1", CultureInfo.InvariantCulture));
        } 

        [Test]
        public void Convert_WithNullableBooleanTargetType()
        {
            Assert.That(_converter.Convert("1", typeof(bool?), "1", CultureInfo.InvariantCulture), Is.True);
        } 

        [Test]
        public void Convert_WithMatchingSourceAndParameterTypes()
        {
            Assert.That(_converter.Convert("str", typeof(bool), "str", CultureInfo.InvariantCulture), Is.True);
            Assert.That(_converter.Convert("str", typeof(bool), "some", CultureInfo.InvariantCulture), Is.False);
        } 

        [Test]
        public void Convert_WithDifferentSourceAndParameterTypes_Throws()
        {
            Assert.Throws<ArgumentException>(() => _converter.Convert("1", typeof(bool), 1, CultureInfo.InvariantCulture));
        }

        [Test]
        public void Convert_WithNullSource()
        {
            Assert.That(_converter.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture), Is.True);
            Assert.That(_converter.Convert(null, typeof(bool), "some", CultureInfo.InvariantCulture), Is.False);
        }

        [Test]
        public void Convert_WithNullParameter()
        {
            Assert.That(_converter.Convert("str", typeof(bool), null, CultureInfo.InvariantCulture), Is.False);
        }

        [Test]
        public void ConvertBack_WithNullValue_SameAsFalse()
        {
            Assert.That(_converter.ConvertBack(null, typeof(string), "1", CultureInfo.InvariantCulture), Is.EqualTo(Binding.DoNothing));
        }

        [Test]
        public void ConvertBack_WithNullParameter()
        {
            Assert.That(_converter.ConvertBack(true, typeof(string), null, CultureInfo.InvariantCulture), Is.EqualTo(null));
            Assert.That(_converter.ConvertBack(false, typeof(string), null, CultureInfo.InvariantCulture), Is.EqualTo(Binding.DoNothing));
        }

        [Test]
        public void ConvertBack_WithNullParameterAndValueTargetType_Throws()
        {
            Assert.Throws<ArgumentException>(() => _converter.ConvertBack(true, typeof(int), null, CultureInfo.InvariantCulture));
        }

        [Test]
        public void ConvertBack_WithIllegalSourceType_Throws()
        {
            Assert.Throws<ArgumentException>(() => _converter.ConvertBack("tru'", typeof(string), "1", CultureInfo.InvariantCulture));
        }

        [Test]
        public void ConvertBack_WithMatchingSourceAndParameterTypes()
        {
            Assert.That(_converter.ConvertBack(true, typeof(string), "str", CultureInfo.InvariantCulture), Is.EqualTo("str"));
            Assert.That(_converter.ConvertBack(false, typeof(string), "some", CultureInfo.InvariantCulture), Is.EqualTo(Binding.DoNothing));
        }

        [Test]
        public void ConvertBack_WithDifferentSourceAndParameterTypes_Throws()
        {
            Assert.Throws<ArgumentException>(() => _converter.ConvertBack(true, typeof(string), 1, CultureInfo.InvariantCulture));
        }

        [Test]
        public void ConvertBack_WithNullableBoolParameter()
        {
            var flag = (bool?) true;
            var notflag = (bool?) false;
            Assert.That(_converter.ConvertBack(flag, typeof(string), "test", CultureInfo.InvariantCulture), Is.EqualTo("test"));
            Assert.That(_converter.ConvertBack(notflag, typeof(string), "test", CultureInfo.InvariantCulture), Is.EqualTo(Binding.DoNothing));
        }

    }

}

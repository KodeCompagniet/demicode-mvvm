using System;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable EqualExpressionComparison
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace DemiCode.Mvvm.Helpers.Test
{
    [TestFixture]
    public class WeakPredicateTest
    {
        [Test]
        public void Construct_WithNullTarget_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WeakPredicate<int>(null, i => true));
        }

        [Test]
        public void Construct_WithNullPredicate_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WeakPredicate<int>(this, null));
        }

        [Test]
        public void Equals_SameInstance_IsTrue()
        {
            var left = new WeakPredicate<int>(this, i => true);

            Assert.That(left.Equals(left), Is.True);
        } 

        [Test]
        public void Equals_WithSameTargetAndPredicate_IsTrue()
        {
            Predicate<int> p = i => i == 42;

            var left = new WeakPredicate<int>(this, p);
            var right = new WeakPredicate<int>(this, p);

            Assert.That(left.Equals(right), Is.True);
        } 

        [Test]
        public void Equals_WithSameTargetAndDifferentPredicate_IsFalse()
        {
            Predicate<int> p0 = i => i == 42;
            Predicate<int> p1 = i => i == 43;

            var left = new WeakPredicate<int>(this, p0);
            var right = new WeakPredicate<int>(this, p1);

            Assert.That(left.Equals(right), Is.False);
        } 

        [Test]
        public void Equals_WithNull_IsFalse()
        {
            Predicate<int> p0 = i => i == 42;

            var left = new WeakPredicate<int>(this, p0);

            Assert.That(left.Equals(null), Is.False);
        }

        [Test]
        public void EqualsOperator_WithNulls_IsTrue()
        {
            WeakPredicate<int> left = null;
            WeakPredicate<int> right = null;

            Assert.That(left == right, Is.True);
        }

        [Test]
        public void EqualsOperator_WithLeftNull_IsFalse()
        {
            WeakPredicate<int> left = null;
            WeakPredicate<int> right = new WeakPredicate<int>(this, i => true);

            Assert.That(left == right, Is.False);
        }

        [Test]
        public void EqualsOperator_WithRightNull_IsFalse()
        {
            WeakPredicate<int> left = new WeakPredicate<int>(this, i => true);
            WeakPredicate<int> right = null;

            Assert.That(left == right, Is.False);
        }

        [Test]
        public void EqualsOperator_WithSameInstance_IsTrue()
        {
            WeakPredicate<int> left = new WeakPredicate<int>(this, i => true);

            Assert.That(left == left, Is.True);
        }

        [Test]
        public void NotEqualsOperator_WithNulls_IsFalse()
        {
            WeakPredicate<int> left = null;
            WeakPredicate<int> right = null;

            Assert.That(left != right, Is.False);
        }
    }
}
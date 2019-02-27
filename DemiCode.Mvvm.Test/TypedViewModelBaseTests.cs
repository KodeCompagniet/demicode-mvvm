using System;
using NUnit.Framework;
using System.Diagnostics;

// ReSharper disable InconsistentNaming

namespace DemiCode.Mvvm.Test
{
    [TestFixture]
    public class TypedViewModelBaseTests
    {
        [Test]
        public void Construct_InitializeTyped_Null()
        {
            TestableModel model = null;
            var viewModel = new TestableViewModel();
            viewModel.Initialize(model);

            Assert.That(viewModel.TypedInitializerCalled, Is.True);
            Assert.That(viewModel.TestGetModel(), Is.Null);
        }

        [Test]
        public void Construct_InitializeTyped()
        {
            var model = new TestableModel();
            var viewModel = new TestableViewModel();
            viewModel.Initialize(model);

            Assert.That(viewModel.TypedInitializerCalled, Is.True);
            Assert.That(viewModel.TestGetModel(), Is.SameAs(model));
        }

        [Test]
        public void InitializeUntyped_ValueTypeWithNull_UsesDefaultValueType()
        {
            object model = null;
            var viewModel = new IntViewModel();
            
            viewModel.Initialize(model);

            Assert.That(viewModel.TestGetModel(), Is.EqualTo(0));
        }

        [Test]
        public void InitializeUntyped_WithValue_UsesTheValue()
        {
            object model = 20;
            var viewModel = new IntViewModel();
            
            viewModel.Initialize(model);

            Assert.That(viewModel.TestGetModel(), Is.EqualTo(20));
        }

        [Test]
        public void InitializeUntyped_Null()
        {
            object model = null;
            var viewModel = new TestableViewModel();
            viewModel.Initialize(model);

            Assert.That(viewModel.TypedInitializerCalled, Is.False);
            Assert.That(viewModel.TestGetModel(), Is.Null);
        }

        [Test]
        public void InitializeUntyped_ValidType()
        {
            object model = new TestableModel();
            var viewModel = new TestableViewModel();
            viewModel.Initialize(model);

            Assert.That(viewModel.TypedInitializerCalled, Is.False);
            Assert.That(viewModel.TestGetModel(), Is.SameAs(model));
        }

        [Test]
        public void InitializeUntyped_InvalidType_Throws()
        {
            object model = "Nisselue";
            var viewModel = new TestableViewModel();

            Assert.Throws<ArgumentException>(() => viewModel.Initialize(model));
        }

        #region Testable classes

        private class IntViewModel : TypedViewModelBase<int>
        {
            public int TestGetModel()
            {
                return Model;
            }
        }

        private class TestableViewModel : TypedViewModelBase<TestableModel>
        {
            public bool TypedInitializerCalled;

            protected override void OnInitialize()
            {
#if !DEBUG
                throw new InvalidOperationException("Release build. This method will only work in Debug builds");
#else

                // Determine if the typed initialize method was called directly, and not from TypedViewModelBase.Initialize(object)
                var stackTrace = new StackTrace();
                var stackFrames = stackTrace.GetFrames();
                if (stackFrames == null)
                    throw new InvalidOperationException("Could not retrieve stackframes");

                var caller = stackFrames[2].GetMethod();

                Console.WriteLine("Caller trace: {0}", stackTrace);

                var callerParams = caller.GetParameters();
                if (caller.Name.Equals("Initialize") && 
                    callerParams.Length == 1 && 
                    callerParams[0].ParameterType == typeof(object))
                {
                    TypedInitializerCalled = false;
                }
                else
                {
                    TypedInitializerCalled = true;
                }
#endif
            }

            public TestableModel TestGetModel()
            {
                return Model;
            }
        }

        private class TestableModel
        {
        }

        #endregion
    }
}

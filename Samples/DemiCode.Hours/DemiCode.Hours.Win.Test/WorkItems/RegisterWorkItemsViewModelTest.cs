using System;
using DemiCode.Hours.Shared.Services;
using DemiCode.Hours.Win.WorkItems;
using NUnit.Framework;
using Rhino.Mocks;

namespace DemiCode.Hours.Win.WorkItems.Test
{
    [TestFixture]
    public class RegisterWorkItemsViewModelTest
    {
        [Test]
        public void Construct_WithoutCurrentEmployee_Throws()
        {
            var ds = MockRepository.GenerateStub<IHoursDataService>();
            ds.Stub(x => x.GetCurrentEmployee()).Return(null);

            Assert.Throws<InvalidOperationException>(() => new RegisterWorkItemsViewModel(ds));
        }
    }
}

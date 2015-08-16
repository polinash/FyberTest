using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
	public static class ObjectAssertExtension
	{
        public static void ShouldBeNullOrEmpty(this string value, string message)
        {
            Assert.IsTrue(string.IsNullOrEmpty(value), message);
        }

		public static void ShouldBeTrue(this bool value, string message)
		{
			Assert.IsTrue(value, message);
		}
	}
}

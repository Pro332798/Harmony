using HarmonyLib;
using HarmonyLibTests.Assets;
using NUnit.Framework;
using System;

namespace HarmonyLibTests
{
	[TestFixture]
	public class Arguments
	{
		[Test]
		public void Test_Method6()
		{
			var originalClass = typeof(Class6);
			Assert.IsNotNull(originalClass);
			var originalMethod = originalClass.GetMethod("Method6");
			Assert.IsNotNull(originalMethod);

			var patchClass = typeof(Class6Patch);
			var prefix = patchClass.GetMethod("Prefix");
			Assert.IsNotNull(prefix);

			var instance = new Harmony("test");
			Assert.IsNotNull(instance);

			var patcher = instance.CreateProcessor(originalMethod);
			Assert.IsNotNull(patcher);
			_ = patcher.AddPrefix(prefix);
			Assert.IsNotNull(patcher);

			_ = patcher.Patch();

			var instance6 = new Class6
			{
				someFloat = 999,
				someString = "original",
				someStruct = new Class6Struct() { d1 = 1, d2 = 2, d3 = 3 }
			};
			var res = instance6.Method6();
			Assert.AreEqual(res[0], 123);
			Assert.AreEqual(res[1], "patched");
			Assert.AreEqual(((Class6Struct)res[2]).d1, 10.0);
		}

		[Test]
		public void Test_Method7()
		{
			var originalClass = typeof(Class7);
			Assert.IsNotNull(originalClass);
			var originalMethod = originalClass.GetMethod("Method7");
			Assert.IsNotNull(originalMethod);

			var patchClass = typeof(Class7Patch);
			var postfix = patchClass.GetMethod("Postfix");
			Assert.IsNotNull(postfix);

			var instance = new Harmony("test");
			Assert.IsNotNull(instance);

			var patcher = instance.CreateProcessor(originalMethod);
			Assert.IsNotNull(patcher);
			_ = patcher.AddPostfix(postfix);

			_ = patcher.Patch();

			Class7.state2 = "before";
			var instance7 = new Class7();
			var result = instance7.Method7("parameter");
			Console.WriteLine(Class7.state2);

			Assert.AreEqual("parameter", instance7.state1);
			Assert.AreEqual(10, result.a);
			Assert.AreEqual(20, result.b);
		}

		[Test]
		public void Test_Method8()
		{
			var originalClass = typeof(Class8);
			Assert.IsNotNull(originalClass);
			var originalMethod = originalClass.GetMethod("Method8");
			Assert.IsNotNull(originalMethod);

			var patchClass = typeof(Class8Patch);
			var postfix = patchClass.GetMethod("Postfix");
			Assert.IsNotNull(postfix);

			var instance = new Harmony("test");
			Assert.IsNotNull(instance);

			var patcher = instance.CreateProcessor(originalMethod);
			Assert.IsNotNull(patcher);
			_ = patcher.AddPostfix(postfix);
			Assert.IsNotNull(patcher);

			_ = patcher.Patch();

			var result = Class8.Method8("patched");

			Assert.IsTrue(Class8.mainRun);
			Assert.AreEqual(10, result.a);
			Assert.AreEqual(20, result.b);
		}

		[Test]
		public void Test_InjectingBaseClassField()
		{
			var testInstance = new InjectFieldSubClass();
			testInstance.Method("foo");
			Assert.AreEqual("foo", testInstance.TestValue);

			var originalClass = testInstance.GetType();
			Assert.IsNotNull(originalClass);
			var originalMethod = originalClass.GetMethod("Method");
			Assert.IsNotNull(originalMethod);

			var patchClass = typeof(InjectFieldSubClass_Patch);
			var postfix = patchClass.GetMethod("Postfix");
			Assert.IsNotNull(postfix);

			var instance = new Harmony("test");
			Assert.IsNotNull(instance);

			var patcher = instance.CreateProcessor(originalMethod);
			Assert.IsNotNull(patcher);
			_ = patcher.AddPostfix(postfix);
			Assert.IsNotNull(patcher);

			_ = patcher.Patch();

			testInstance.Method("bar");
			Assert.AreEqual("patched", testInstance.TestValue);
		}
	}
}
namespace Occhitta.Libraries.Stream.String;

using System.Text.RegularExpressions;
using Occhitta.Libraries.Stream.Binary;
using static System.Reflection.BindingFlags;

/// <summary>
/// <see cref="BinaryStringLightBuffer" />検証クラスです。
/// </summary>
[TestFixture]
public partial class BinaryStringLightBufferTest {
	#region 検証メソッド定義
	/// <summary>
	/// 正常処理を検証します。
	/// </summary>
	[Test]
	public void Success1() {
		using var memory = new MemoryBinaryLightReader([]);
		using var source = new BinaryStringLightBuffer(memory, 1);
		Assert.That(source.Import(), Is.False);
		Assert.That(source.Import(), Is.False);
	}
	/// <summary>
	/// 正常処理を検証します。
	/// </summary>
	[Test]
	public void Success2() {
		using var memory = new MemoryBinaryLightReader([0x01]);
		using var source = new BinaryStringLightBuffer(memory, 1);
		Assert.That(source.Import(), Is.True );
		Assert.That(source.Import(), Is.False);
	}
	/// <summary>
	/// 失敗処理を検証します。
	/// </summary>
	[TestCase(null, ExpectedResult = """
System.ArgumentNullException {
	Message : "Value cannot be null. (Parameter 'source')"
	ParamName : "source"
	TargetSite : (System.Reflection.RuntimeConstructorInfo)Void .ctor(Occhitta.Libraries.Stream.BinaryLightReader, Int32)
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2147467261
	StackTrace : ...
}
""")]
	public string Failure1(BinaryLightReader source) {
		var result = Assert.Throws<ArgumentNullException>(() => new BinaryStringLightBuffer(source, 0));
		return ExceptionUtilities.ToString(result, "\t");
	}
	/// <summary>
	/// 失敗処理を検証します。
	/// </summary>
	[TestCase(0, ExpectedResult = """
System.ArgumentOutOfRangeException {
	Message : "Value must be positive. (Parameter 'length')\r\nActual value was 0."
	ActualValue : 0
	ParamName : "length"
	TargetSite : (System.Reflection.RuntimeConstructorInfo)Void .ctor(Occhitta.Libraries.Stream.BinaryLightReader, Int32)
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2146233086
	StackTrace : ...
}
""")]
	public string Failure2(int length) {
		using var reader = new MemoryBinaryLightReader([]);
		var result = Assert.Throws<ArgumentOutOfRangeException>(() => new BinaryStringLightBuffer(reader, length));
		return ExceptionUtilities.ToString(result, "\t");
	}
	#endregion 検証メソッド定義

	#region 検証メソッド定義:IDisposable
	/// <summary>
	/// <see cref="IDisposable" />検証クラスです。
	/// </summary>
	[TestFixture]
	internal class IDisposableTest : IDisposableTest<BinaryStringLightBuffer> {
		/// <summary>
		/// 要素情報を生成します。
		/// </summary>
		/// <returns>要素情報</returns>
		protected override BinaryStringLightBuffer CreateData() =>
			new(new MemoryBinaryLightReader([]), 1);
		/// <summary>
		/// 要素集合を取得します。
		/// </summary>
		/// <returns>要素集合</returns>
		protected override IEnumerable<string> MemberList() {
			yield return "Source";
			yield return "Buffer";
		}
		/// <summary>
		/// 要素内容へ変換します。
		/// </summary>
		/// <param name="divide">分類名称</param>
		/// <param name="member">要素名称</param>
		/// <returns>要素内容</returns>
		protected override string SwitchText(string divide, string member) => divide switch {
			"TargetSite" => member switch {
				"Source" => "Occhitta.Libraries.Stream.BinaryLightReader get_Source()",
				"Buffer" => "Byte[] get_Buffer()",
				_ => divide
			},
			_ => divide
		};
	}

	/// <summary>
	/// <see cref="IDisposable" />検証クラスです。
	/// </summary>
	/// <typeparam name="TSource">検証種別</typeparam>
	public abstract partial class IDisposableTest<TSource> where TSource : IDisposable {
		#region メンバー定数定義
		/// <summary>内容書式</summary>
		private const string SourceText = """
System.Reflection.TargetInvocationException {
	TargetSite : (System.Reflection.RuntimeMethodInfo)System.Object InvokeWithNoArgs(System.Object, System.Reflection.BindingFlags)
	Message : "Exception has been thrown by the target of an invocation."
	Data : {
	}
	InnerException : System.ObjectDisposedException {
		Message : "Cannot access a disposed object.\r\nObject name: 'Occhitta.Libraries.Stream.String.BinaryStringLightBuffer'."
		ObjectName : "Occhitta.Libraries.Stream.String.BinaryStringLightBuffer"
		TargetSite : (System.Reflection.RuntimeMethodInfo)${TargetSite}
		Data : {
		}
		InnerException : Null
		HelpLink : Null
		Source : "Occhitta.Libraries.Struct"
		HResult : -2146232798
		StackTrace : ...
	}
	HelpLink : Null
	Source : "System.Private.CoreLib"
	HResult : -2146232828
	StackTrace : ...
}
""";
		#endregion メンバー定数定義

		#region 内部メソッド定義:CreateData/MemberList/SwitchText
		/// <summary>
		/// 要素情報を生成します。
		/// </summary>
		/// <returns>要素情報</returns>
		protected abstract TSource CreateData();
		/// <summary>
		/// 要素集合を取得します。
		/// </summary>
		/// <returns>要素集合</returns>
		protected abstract IEnumerable<string> MemberList();
		/// <summary>
		/// 要素内容へ変換します。
		/// </summary>
		/// <param name="divide">分類名称</param>
		/// <param name="member">要素名称</param>
		/// <returns>要素内容</returns>
		protected abstract string SwitchText(string divide, string member);
		#endregion 内部メソッド定義:CreateData/MemberList/SwitchText

		#region 内部メソッド定義:CreateCode
		/// <summary>
		/// 正規表現を生成します。
		/// </summary>
		/// <returns>正規表現</returns>
		[GeneratedRegex(@"\$\{([A-Za-z0-9]+)}")]
		private static partial Regex CreateCode();
		/// <summary>
		/// 要素内容へ変換します。
		/// </summary>
		/// <param name="divide">分類情報</param>
		/// <param name="member">要素名称</param>
		/// <returns>要素内容</returns>
		private string SwitchText(Match divide, string member) =>
			SwitchText(divide.Groups[1].Value, member);
		/// <summary>
		/// 想定内容へ変換します。
		/// </summary>
		/// <param name="source">内容書式</param>
		/// <param name="member">要素名称</param>
		/// <returns>想定内容</returns>
		private string CreateText(string source, string member) {
			var format = CreateCode();
			var result = format.Replace(source, choose => SwitchText(choose, member));
			return result;
		}
		#endregion 内部メソッド定義:CreateCode

		#region 内部メソッド定義:InvokeTest
		/// <summary>
		/// 引数情報を検証します。
		/// </summary>
		/// <param name="source">要素情報</param>
		/// <param name="member">要素名称</param>
		private void InvokeTest(TSource source, string member) {
			var choose = typeof(TSource);
			var invoke = choose.GetProperty(member, Instance | Public | NonPublic);
			Assert.That(invoke, Is.Not.Null, "member is not found.({0})", member);
			var result = Assert.Catch<Exception>(() => invoke.GetValue(source), "Does ot throw ObjectDisposedException.(member={0})", member);
			var expect = CreateText(SourceText, member);
			var actual = ExceptionUtilities.ToString(result, "\t");
			Assert.That(actual, Is.EqualTo(expect), "Different Exception.(member={0})", member);
		}
		#endregion 内部メソッド定義:InvokeTest

		#region 検証メソッド定義:InvokeTest
		/// <summary>
		/// <see cref="IDisposable" />を検証します。
		/// </summary>
		[Test]
		public void InvokeTest() {
			using var source = CreateData();
			source.Dispose();
			Assert.Multiple(() => {
				foreach (var member in MemberList()) {
					InvokeTest(source, member);
				}
			});
		}
		#endregion 検証メソッド定義:InvokeTest
	}
	#endregion 検証メソッド定義:IDisposable
}

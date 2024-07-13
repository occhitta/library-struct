using Occhitta.Libraries.Common;
using Occhitta.Libraries.Common.UnitTest;

namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// <see cref="DsvParserData{TValue}" />検証クラスです。
/// </summary>
[TestFixture]
public sealed class DsvParserDataTest {
	#region 検証メソッド定義:Instance～Test
	#region 検証メソッド定義:InstanceBaseTest
	/// <summary>以下のメソッドを検証します。
	///   <para><see cref="DsvParserData{TValue}.DsvParserData(IEnumerable{TValue})" /></para>
	///   <para><see cref="DsvParserData{TValue}[int]" /></para>
	///   <para><see cref="DsvParserData{TValue}.Count" /></para>
	/// </summary>
	/// <typeparam name="TValue">項目種別</typeparam>
	internal abstract class InstanceBaseTest<TValue> {
		#region 検証メソッド定義:SuccessCode
		/// <summary>正常処理を検証します。
		///   <para><see cref="DsvParserData{TValue}.Count" /></para>
		///   <para><see cref="DsvParserData{TValue}[int]" /></para>
		/// </summary>
		/// <param name="actual">実質情報</param>
		/// <param name="expect">想定情報</param>
		/// <returns>表現文字列</returns>
		protected static string SuccessCode(DsvParserData<TValue> actual, TValue[] expect) {
			Assert.That(actual, Has.Count.EqualTo(expect.Length));
			Assert.Multiple(() => { // 件数が同一の場合：検証継続
				for (var index = 0; index < expect.Length; index ++) {
					Assert.That(actual[index], Is.EqualTo(expect[index]), "要素情報が異なります(要素番号={0})", index);
				}
			});
			return actual.ToString();
		}
		#endregion 検証メソッド定義:SuccessCode

		#region 検証メソッド定義:FailureCode
		/// <summary>
		/// 異常処理を検証します。
		/// </summary>
		/// <param name="source">項目集合</param>
		[TestCase(null)]
		public void FailureCode(IEnumerable<TValue> source) {
			var actual = Assert.Throws<ArgumentNullException>(() => new DsvParserData<TValue>(source));
			Assert.That(ExceptionUtilities.ToString(actual, "\t"), Is.EqualTo("""
System.ArgumentNullException {
	Message : "Value cannot be null. (Parameter 'source')"
	ParamName : "source"
	TargetSite : (System.Reflection.RuntimeConstructorInfo)Void .ctor(System.Collections.Generic.IEnumerable`1[TValue])
	Data : {
	}
	InnerException : Null
	HelpLink : Null
	Source : "Occhitta.Libraries.Struct"
	HResult : -2147467261
	StackTrace : ...
}
"""));
		}
		#endregion 検証メソッド定義:FailureCode
	}
	#endregion 検証メソッド定義:InstanceBaseTest

	#region 検証メソッド定義:InstanceInt4Test
	/// <summary>以下のメソッドを検証します。
	///   <para><see cref="DsvParserData{TValue}.DsvParserData(IEnumerable{TValue})" /></para>
	///   <para><see cref="DsvParserData{TValue}[int]" /></para>
	///   <para><see cref="DsvParserData{TValue}.Count" /></para>
	/// </summary>
	[TestFixture]
	internal sealed class InstanceInt4Test : InstanceBaseTest<int> {
		/// <summary>
		/// 正常処理を検証します。
		/// </summary>
		/// <param name="source">項目配列</param>
		/// <returns>表現文字列</returns>
		[TestCase(      ExpectedResult = "[]")]
		[TestCase(1,    ExpectedResult = "[\"1\"]")]
		[TestCase(1, 2, ExpectedResult = "[\"1\",\"2\"]")]
		public string SuccessList(params int[] source) =>
			SuccessCode(new DsvParserData<int>(source), source);
		/// <summary>
		/// 正常処理を検証します。
		/// </summary>
		/// <param name="source">項目配列</param>
		/// <returns>表現文字列</returns>
		[TestCase(      ExpectedResult = "[]")]
		[TestCase(1,    ExpectedResult = "[\"1\"]")]
		[TestCase(1, 2, ExpectedResult = "[\"1\",\"2\"]")]
		public string SuccessCode(params int[] source) =>
			SuccessCode(DsvParserData<int>.Create(source), source);
	}
	#endregion 検証メソッド定義:InstanceInt4Test

	#region 検証メソッド定義:InstanceTextTest
	/// <summary>以下のメソッドを検証します。
	///   <para><see cref="DsvParserData{TValue}.DsvParserData(IEnumerable{TValue})" /></para>
	///   <para><see cref="DsvParserData{TValue}[int]" /></para>
	///   <para><see cref="DsvParserData{TValue}.Count" /></para>
	/// </summary>
	[TestFixture]
	internal sealed class InstanceTextTest : InstanceBaseTest<string> {
		/// <summary>
		/// 正常処理を検証します。
		/// </summary>
		/// <param name="source">項目配列</param>
		/// <returns>表現文字列</returns>
		[TestCase(            ExpectedResult = "[]"             )]
		[TestCase("",         ExpectedResult = "[\"\"]"         )]
		[TestCase("AA", "BB", ExpectedResult = "[\"AA\",\"BB\"]")]
		[TestCase("AA", null, ExpectedResult = "[\"AA\",\"\"]"  )]
		public string SuccessList(params string[] source) =>
			SuccessCode(new DsvParserData<string>(source), source);
		/// <summary>
		/// 正常処理を検証します。
		/// </summary>
		/// <param name="source">項目配列</param>
		/// <returns>表現文字列</returns>
		[TestCase(            ExpectedResult = "[]"             )]
		[TestCase("",         ExpectedResult = "[\"\"]"         )]
		[TestCase("AA", "BB", ExpectedResult = "[\"AA\",\"BB\"]")]
		[TestCase("AA", null, ExpectedResult = "[\"AA\",\"\"]"  )]
		public string SuccessCode(params string[] source) =>
			SuccessCode(DsvParserData<string>.Create(source), source);
	}
	#endregion 検証メソッド定義:InstanceTextTest
	#endregion 検証メソッド定義:Instance～Test

	#region 検証メソッド定義:DsvSourceData
	/// <summary>
	/// <see cref="DsvSourceData" />を検証します。
	/// </summary>
	[TestFixture]
	public sealed class SourceTest : IEnumerableTest<string> {
		/// <summary>
		/// <see cref="DsvSourceData" />を検証します。
		/// </summary>
		/// <param name="source">引数情報</param>
		[TestCase()]
		[TestCase("")]
		[TestCase("", "")]
		[TestCase("A", "B")]
		[TestCase("B", "A")]
		public void Test(params string[] source) =>
			Test(new DsvParserData<string>(source), source);
	}
	#endregion 検証メソッド定義:DsvSourceData
}

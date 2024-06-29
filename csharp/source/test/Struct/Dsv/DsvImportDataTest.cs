using Occhitta.Libraries.Common.UnitTest;

namespace Occhitta.Libraries.Struct.Dsv;

/// <summary>
/// <see cref="DsvImportData" />検証クラスです。
/// <para>当該クラスはコンストラクタ(<see cref="DsvImportData(IEnumerable{string})" /></see>)の検証を行います。</para>
/// </summary>
[TestFixture]
public sealed class DsvImportDataTest {
	#region 検証メソッド定義:SuccessCode
	/// <summary>
	/// 正常処理を検証します。
	/// </summary>
	/// <param name="source">要素配列</param>
	[TestCase()]
	[TestCase("A")]
	[TestCase("1")]
	[TestCase("A", "B")]
	[TestCase("B", "A")]
	public void SuccessCode(params string[] source) {
		var actual = new DsvImportData(source);
		Assert.That(actual, Has.Count.EqualTo(source.Length)); // 件数が異なる場合：検証終了
		Assert.Multiple(() => {                                // 件数が同一の場合：検証継続
			for (var index = 0; index < source.Length; index ++) {
				Assert.That(actual[index], Is.EqualTo(source[index]), "要素情報が異なります(要素番号={0})", index);
			}
		});
	}
	#endregion 検証メソッド定義:SuccessCode

	#region 検証メソッド定義:FailureCode
	/// <summary>
	/// 失敗処理を検証します。
	/// </summary>
	/// <param name="actual">例外情報</param>
	/// <param name="source">引数名称</param>
	private static void FailureCode(ArgumentNullException actual, string source) {
		Assert.Multiple(() => {
			Assert.That(actual.Data,           Is.Empty, $"{nameof(ArgumentException)}#{nameof(ArgumentException.Data)}");
			Assert.That(actual.HelpLink,       Is.Null,  $"{nameof(ArgumentException)}#{nameof(ArgumentException.HelpLink)}");
			Assert.That(actual.HResult,        Is.EqualTo(-2147467261));
			Assert.That(actual.InnerException, Is.Null);
			Assert.That(actual.Message,        Is.EqualTo($"Value cannot be null. (Parameter '{source}')"));
			Assert.That(actual.ParamName,      Is.EqualTo(source));
		});
	}
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	[TestCase(null)]
	public void FailureCode(IEnumerable<string> source) {
		FailureCode(Assert.Throws<ArgumentNullException>(() => new DsvImportData(source)), "source");
	}
	/// <summary>
	/// 異常処理を検証します。
	/// </summary>
	/// <param name="expect">想定名称</param>
	/// <param name="source">要素配列</param>
	[TestCase("source[1]", "", null)]
	public void FailureCode(string expect, params string[] source) {
		FailureCode(Assert.Throws<ArgumentNullException>(() => new DsvImportData(source)), expect);
	}
	#endregion 検証メソッド定義:FailureCode

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
			Test(new DsvImportData(source), source);
	}
	#endregion 検証メソッド定義:DsvSourceData

	#region 検証メソッド定義:Object継承
	/// <summary>
	/// <see cref="Object" />継承検証クラスです。
	/// </summary>
	[TestFixture]
	public sealed class ObjectTest {
		#region 検証メソッド定義:Equals
		/// <summary>
		/// <see cref="Object.Equals(object?)" />を実行します。
		/// </summary>
		private static bool Case11(object value1, object? value2) =>
			value1.Equals(value2);
		/// <summary>
		/// <see cref="Object.Equals(object?)" />を検証します。
		/// </summary>
		[Test]
		public void Case11() {
			var value1 = new DsvImportData(["A", "B"]);
			var value2 = new DsvImportData(["A", "B"]);
			var value3 = new DsvImportData(["B", "A"]);
			var value4 = new DsvImportData(["A", "B", "C"]);
			Assert.Multiple(() => {
				Assert.That(Case11(value1, value1), Is.True );
				Assert.That(Case11(value1, value2), Is.True );
				Assert.That(Case11(value1, value3), Is.False);
				Assert.That(Case11(value1, value4), Is.False);
				Assert.That(Case11(value1, "A, B"), Is.False);
				Assert.That(Case11(value1, null),   Is.False);

				Assert.That(Case11(value2, value1), Is.True );
				Assert.That(Case11(value2, value2), Is.True );
				Assert.That(Case11(value2, value3), Is.False);
				Assert.That(Case11(value2, value4), Is.False);
				Assert.That(Case11(value2, "A, B"), Is.False);
				Assert.That(Case11(value2, null),   Is.False);

				Assert.That(Case11(value3, value1), Is.False);
				Assert.That(Case11(value3, value2), Is.False);
				Assert.That(Case11(value3, value3), Is.True );
				Assert.That(Case11(value3, value4), Is.False);
				Assert.That(Case11(value3, "A, B"), Is.False);
				Assert.That(Case11(value3, null),   Is.False);

				Assert.That(Case11(value4, value1), Is.False);
				Assert.That(Case11(value4, value2), Is.False);
				Assert.That(Case11(value4, value3), Is.False);
				Assert.That(Case11(value4, value4), Is.True );
				Assert.That(Case11(value4, "A, B"), Is.False);
				Assert.That(Case11(value4, null),   Is.False);
			});
		}
		#endregion 検証メソッド定義:Equals

		#region 検証メソッド定義:GetHashCode
		/// <summary>
		/// <see cref="Object.GetHashCode" />を検証します。
		/// </summary>
		/// <param name="source">項目配列</param>
		/// <returns>ハッシュ値</returns>
		[TestCase(        ExpectedResult =       0)]
		[TestCase("",     ExpectedResult =       0)]
		[TestCase("A",    ExpectedResult = 4259905)]
		[TestCase("B",    ExpectedResult = 4325442)]
		[TestCase("", "", ExpectedResult =      33)]
		public int TestCase21(params string[] source) {
			var value1 = new DsvImportData(source);
			var value2 = new DsvImportData(source);
			Assert.That(value1.GetHashCode(), Is.EqualTo(value2.GetHashCode()));
			return value1.GetHashCode();
		}
		#endregion 検証メソッド定義:GetHashCode

		#region 検証メソッド定義:ToString
		/// <summary>
		/// <see cref="Object.ToString" />を検証します。
		/// </summary>
		/// <param name="source">項目配列</param>
		/// <returns>表現文字列</returns>
		[TestCase(        ExpectedResult = "[]")]
		[TestCase("",     ExpectedResult = "[\"\"]")]
		public string TestCase31(params string[] source) {
			var value1 = new DsvImportData(source);
			var value2 = new DsvImportData(source);
			Assert.That(value1.ToString(), Is.EqualTo(value2.ToString()));
			return value1.ToString();
		}
		#endregion 検証メソッド定義:ToString
	}
	#endregion 検証メソッド定義:Object継承
}
